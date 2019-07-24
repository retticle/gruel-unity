using UnityEngine;

namespace Gruel.Camera {
	public class CameraOrthographicScalar : CameraTrait {

#region Properties
		public float Ppu {
			get => _ppu;
			set {
				_ppu = value;
				SetOrtho();
			}
		}

		public float PpuScale {
			get => _ppuScale;
			set {
				_ppuScale = value;
				SetOrtho();
			}
		}
#endregion Properties

#region Fields
		[Header("CameraOrthographicScaler")]
		[SerializeField] private UnityEngine.Camera _camera;
		
		[SerializeField] private float _ppu = 100.0f;
		[SerializeField] private float _ppuScale = 1.0f;
#endregion Fields

#region Private Methods
		private void Start() {
			
			SetOrtho();
		}

#if UNITY_EDITOR
		private void Update() {
			SetOrtho();
		}
#endif
		
		private void SetOrtho() {
			var screenHeight = Screen.height;
			var actualPpu = _ppu * _ppuScale;
			
			_camera.orthographicSize = (screenHeight / actualPpu) * 0.5f;
		}
#endregion Private Methods

	}
}