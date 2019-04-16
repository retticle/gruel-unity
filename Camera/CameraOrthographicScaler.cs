using UnityEngine;

namespace Gruel.Camera {
	public class CameraOrthographicScaler : CameraTrait {

#region Init
		protected override void Start() {
			base.Start();
			
			SetOrtho();
		}

#if UNITY_EDITOR
		private void Update() {
			SetOrtho();
		}
#endif
#endregion Init
		
#region OrthographicScaler
		[Header("CameraOrthographicScaler")]
		[SerializeField] private UnityEngine.Camera _camera;
		
		public float _ppu = 100.0f;
		public float _ppuScale = 1.0f;

		private void SetOrtho() {
			var screenHeight = Screen.height;
			var actualPpu = _ppu * _ppuScale;
			
			_camera.orthographicSize = (screenHeight / actualPpu) * 0.5f;
		}
#endregion OrthographicScaler

	}
}