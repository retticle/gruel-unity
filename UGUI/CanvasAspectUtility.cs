using UnityEngine;
using UnityEngine.UI;

namespace Gruel.UGUI {
	[ExecuteInEditMode]
	public class CanvasAspectUtility : MonoBehaviour {

#region Init
		private void Start() {
			SetAspectRatio();
		}

#if UNITY_EDITOR
		private void Update() {
			SetAspectRatio();
		}
#endif
#endregion Init
		
#region CanvasAspectUtility
		[Header("CanvasAspectUtility")]
		[SerializeField] private AspectRatioFitter _aspectRatioFitter;
	
		public float _minimumAspectRatio = 16.0f / 9.0f;

		private void SetAspectRatio() {
			var windowAspect = (float)Screen.width / (float)Screen.height;
			_aspectRatioFitter.aspectRatio = (windowAspect < _minimumAspectRatio) ? _minimumAspectRatio : windowAspect;
		}
#endregion CanvasAspectUtility
	
	}
}