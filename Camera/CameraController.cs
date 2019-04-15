using System.Linq;
using DG.Tweening;
using UnityEngine;

namespace Gruel.Camera {
	public class CameraController : MonoBehaviour {
	
#region Init
		private void Awake() {
			CameraControllerInit();
		}
#endregion Init
	
#region CameraController
		public static CameraController _instance { get; private set; }

		private void CameraControllerInit() {
			// Setup instance.
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

		public UnityEngine.Camera GetCamera() {
			return _camera;
		}
	
		public Vector3 GetPosition() {
			return transform.position;
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
		[SerializeField] private CameraTrait[] _cameraTraits = new CameraTrait[0];
		
		public T GetTrait<T>() where T : CameraTrait {
			return _cameraTraits.OfType<T>().Select(trait => trait as T).FirstOrDefault();
		}

		public bool TryGetTrait<T>(out T outTrait) where T : CameraTrait {
			var type = typeof(T);
			outTrait = null;

			foreach (var trait in _cameraTraits) {
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
