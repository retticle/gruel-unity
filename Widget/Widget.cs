using System.Collections;
using Gruel.CoroutineSystem;
using UnityEngine;

namespace Gruel.Widget {
	public abstract class Widget : MonoBehaviour {
	
#region Widget
		[Header("Widget Base")]
		// ReSharper disable once InvalidXmlDocComment
		/// <summary>
		/// Priority used to determine which Widget is rendered ontop of others.
		/// 0 is the highest priority.
		/// </summary>
		[SerializeField] protected int _orderPriority = 1;
		public virtual int OrderPriority {
			get { return _orderPriority; }
			protected set { _orderPriority = value; }
		}
	
		/// <summary>
		/// Whether the widget is active or not.
		/// </summary>
		protected bool _active = false;
		public bool Active {
			get { return _active; }
			protected set { _active = value; }
		}
	
		/// <summary>
		/// Async initialize the widget.
		/// </summary>
		/// <returns></returns>
		public virtual Coroutine Init() {
			return CoroutineRunner.StartCoroutine(InitCor());
		}

		protected virtual IEnumerator InitCor() {
			yield return null;
		}

		/// <summary>
		/// Destroys the widget.
		/// </summary>
		public abstract void Remove();

		/// <summary>
		/// Set the widget to be active or inactive.
		/// </summary>
		/// <param name="active"></param>
		/// <returns></returns>
		public abstract Coroutine SetActive(bool active);
#endregion Widget

	}
}