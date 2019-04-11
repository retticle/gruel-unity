using System;
using System.Collections;
using System.Collections.Generic;
using Gruel.CoroutineSystem;
using UnityEngine;

namespace Gruel.Widget {
	public class WidgetController : MonoBehaviour {
	
#region Init
		public void Init() {
			UIInit();
			CoreInit();
		}
#endregion Init

#region Core
		private static WidgetController _instance = null;

		private void CoreInit() {
			// Setup instance.
			if (_instance != null) {
				Debug.LogError("WidgetController: There is already an instance of WidgetController!");
				Destroy(gameObject);
			} else {
				_instance = this;
			}
		}
#endregion Core

#region UI
		[Header("UI")]
		[SerializeField] private Canvas _canvas;

		private const float _planeDistance = 1.0f;
	
		private void UIInit() {
			_canvas.worldCamera = UnityEngine.Camera.main;
			_canvas.planeDistance = _planeDistance;
		}
#endregion UI

#region Widget Controller
		[Header("Widgets")]
		[SerializeField] private Transform _widgetContainer;
		private List<Widget> _widgets = new List<Widget>();

		public static Coroutine AddWidget(string path, Action<Widget> onWidgetAdded = null) {
			Debug.Log($"WidgetController.AddWidget: path: {path}");

			return CoroutineRunner.StartCoroutine(_instance.AddWidgetCor(path, onWidgetAdded));
		}

		public static Coroutine AddWidget(UnityEngine.Object loadedObject, Action<Widget> onWidgetAdded = null) {
			Debug.Log($"WidgetController.AddWidget: loadedObject: {loadedObject}");
		
			// _instance.InstantiateWidget(loadedObject, onWidgetAdded);
			return CoroutineRunner.StartCoroutine(_instance.InstantiateWidgetCor(loadedObject, onWidgetAdded));
		}

		public static void RemoveWidget(Widget widget) {
			Debug.Log($"WidgetController.RemoveWidget: widget: {widget}");
		
			_instance._widgets.Remove(widget);
			if (widget != null) {
				widget.Remove();
			}
		}

		private IEnumerator AddWidgetCor(string path, Action<Widget> onWidgetAdded = null) {
			// Create on loaded callback.
			UnityEngine.Object loadedWidgetObject = null;
			Action<UnityEngine.Object> onWidgetObjectLoaded = delegate(UnityEngine.Object loadedObject) {
				loadedWidgetObject = loadedObject;
			};

			// Wait for AsyncLoader to load the object.
			yield return AsyncLoader.LoadResource(path, onLoaded: onWidgetObjectLoaded);
		
			// InstantiateWidget(loadedWidgetObject, onWidgetAdded);
			yield return CoroutineRunner.StartCoroutine(InstantiateWidgetCor(loadedWidgetObject, onWidgetAdded));
		}

		private IEnumerator InstantiateWidgetCor(UnityEngine.Object loadedWidget, Action<Widget> onWidgetAdded = null) {
			var instantiatedObject = (GameObject)Instantiate(loadedWidget, _widgetContainer);
			var widget = instantiatedObject.GetComponent<Widget>();
		
			// Add to list.
			_widgets.Add(widget);
		
			// Initialize widget.
			yield return widget.Init();
		
			SortWidgets();
		
			onWidgetAdded?.Invoke(widget);
		}

		private void SortWidgets() {
			// Sort widget list.
			_widgets.Sort(delegate(Widget x, Widget y) {
				return y.OrderPriority.CompareTo(x.OrderPriority);
			});
		
			// Adjust widget position in hierarchy to match list order.
			for (int i = 0, n = _widgets.Count; i < n; i++) {
				_widgets[i].transform.SetSiblingIndex(i);
			}
		}
#endregion Widget Controller

	}
}