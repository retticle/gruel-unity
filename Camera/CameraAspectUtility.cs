using UnityEngine;

namespace Gruel.Camera {
	[ExecuteInEditMode]
	public class CameraAspectUtility : CameraTrait {

#region Properties
		public UnityEngine.Camera Camera {
			get => _camera;
			set {
				_camera = value;
				SetAspectRatio();
			}
		}
		
		public bool Letterbox {
			get => _letterbox;
			set {
				_letterbox = value;
				SetAspectRatio();
			}
		}

		public bool Pillarbox {
			get => _pillarbox;
			set {
				_pillarbox = value;
				SetAspectRatio();
			}
		}

		public float MinimumAspectRatio {
			get => _minimumAspectRatio;
			set {
				_minimumAspectRatio = value;
				SetAspectRatio();
			}
		}
#endregion Properties

#region Fields
		[Header("CameraAspectUtility")]
		[SerializeField] private UnityEngine.Camera _camera;
		[SerializeField] private bool _letterbox = true;
		[SerializeField] private bool _pillarbox = true;
		[SerializeField] private float _minimumAspectRatio = 16.0f / 9.0f;
#endregion Fields

#region Public Methods
#endregion Public Methods
	
#region Private Methods
		private void Start() {
			SetAspectRatio();
		}
		
#if UNITY_EDITOR
		private void Update() {
			SetAspectRatio();
		}
#endif
		
		private void SetAspectRatio() {
			// Determine the game window's current aspect ratio.
			var windowAspect = (float)Screen.width / (float)Screen.height;

			// Reset camera rect.
			var rect = _camera.rect;
		
			rect.width = 1.0f;
			rect.height = 1.0f;
			rect.x = 0.0f;
			rect.y = 0.0f;

			_camera.rect = rect;
		
			// Current viewport height should be scaled by this amount.
			var scaleHeight = windowAspect / _minimumAspectRatio;
		
			// Current viewport width should be scaled by this amount.
			var scaleWidth = 1.0f / scaleHeight;
		
			if (_letterbox
			&& windowAspect < _minimumAspectRatio) {
				// If scaled height is less than current height, add letterbox.
				rect.width = 1.0f;
				rect.height = scaleHeight;
				rect.x = 0.0f;
				rect.y = (1.0f - scaleHeight) / 2.0f;

				_camera.rect = rect;
			}

			if (_pillarbox) {
				rect.width = scaleWidth;
				rect.height = 1.0f;
				rect.x = (1.0f - scaleWidth) / 2.0f;
				rect.y = 0.0f;
	
				_camera.rect = rect;
			}
		}
#endregion Private Methods

	}
}