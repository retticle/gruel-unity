using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gruel.Camera {
	public class CameraController : MonoBehaviour {
	
#region Init
		private void Awake() {
			CameraControllerInit();
			TraitsInit();
		}
#endregion Init
	
#region CameraController
		public static CameraController _instance { get; private set; }

		private void CameraControllerInit() {
			if (_instance != null) {
				Debug.LogError("CameraController: There is already an instance of CameraController!");
				Destroy(gameObject);
			} else {
				_instance = this;
			}
		}
#endregion CameraController

#region Camera
		[Header("Camera")]
		[SerializeField] private UnityEngine.Camera _camera;

		public UnityEngine.Camera Camera() {
			return _camera;
		}
	
		public Vector3 GetPosition() {
			return _camera.transform.position;
		}
	
		public void SetPosition(Vector3 position) {
			Debug.Log($"CameraController.SetPosition: {position}");
		
			transform.position = position;
		}

		public float GetOrthoSize() {
			return _camera.orthographicSize;
		}
#endregion Camera
		
#region Traits
		[Header("Traits")]
		[FormerlySerializedAs("_cameraTraits")]
		[SerializeField] private CameraTrait[] _traitComponents;

		private List<CameraTrait> _traits;

		private void TraitsInit() {
			_traits = new List<CameraTrait>();

			for (int i = 0, n = _traitComponents.Length; i < n; i++) {
				AddTrait(_traitComponents[i]);
			}
		}

		public void AddTrait(CameraTrait trait) {
			_traits.Add(trait);
		}

		public void RemoveTrait(CameraTrait trait) {
			_traits.Remove(trait);
		}
		
		public T GetTrait<T>() where T : CameraTrait {
			return _traits.OfType<T>().Select(trait => trait).FirstOrDefault();
		}

		public bool TryGetTrait<T>(out T outTrait) where T : CameraTrait {
			var type = typeof(T);
			outTrait = null;

			foreach (var trait in _traits) {
				if (trait.GetType() == type) {
					outTrait = (T)trait;
					return true;
				}
			}

			return false;
		}
#endregion Traits
	
#region Move
		private Tweener _moveTween = null;
	
		public void Move(Vector3 position, float duration, Ease ease = Ease.OutExpo) {
			// If a previous tween is running stop it.
			if (_moveTween != null
			    && _moveTween.active) {
				_moveTween.Kill();
			}

			_moveTween = transform.DOMove(position, duration, false)
			                      .SetEase(ease);
		}
#endregion Move

	}
}