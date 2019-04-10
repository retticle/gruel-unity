using UnityEngine;
using UnityEngine.UI;

namespace Gruel.Camera {
	[ExecuteInEditMode]
	public class CanvasAspectUtility : MonoBehaviour {

		[Header("CanvasAspectUtility")]
		[SerializeField] private AspectRatioFitter _aspectRatioFitter;
	
		[SerializeField] private float _minimumAspectRatio = 16.0f / 9.0f;
	
		private void Start() {
			SetAspectRatio();
		}

#if UNITY_EDITOR
		private void Update() {
			SetAspectRatio();
		}
#endif

		private void SetAspectRatio() {
			var windowAspect = (float)Screen.width / (float)Screen.height;
			_aspectRatioFitter.aspectRatio = (windowAspect < _minimumAspectRatio) ? _minimumAspectRatio : windowAspect;
		}
	
	}
}