using UnityEngine;
using UnityEngine.UI;

namespace Gruel.UGUI {
	[ExecuteInEditMode]
	public class CanvasAspectUtility : MonoBehaviour {

#region Properties
		public AspectRatioFitter AspectRatioFitter {
			get => _aspectRatioFitter;
			set {
				_aspectRatioFitter = value;
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
		[Header("CanvasAspectUtility")]
		[SerializeField] private AspectRatioFitter _aspectRatioFitter;
		[SerializeField] private float _minimumAspectRatio = 16.0f / 9.0f;
#endregion Fields

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
			var windowAspect = (float)Screen.width / (float)Screen.height;
			_aspectRatioFitter.aspectRatio = (windowAspect < _minimumAspectRatio) ? _minimumAspectRatio : windowAspect;
		}
#endregion Private Methods
	
	}
}