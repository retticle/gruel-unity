using System.Collections;
using Gruel.CoroutineSystem;
using UnityEngine;

namespace Gruel.Camera {
	public class CameraShake : CameraTrait {
		
#region Init
		protected override void OnDestroy() {
			base.OnDestroy();
			
			ShakeOnDestroy();
		}
#endregion Init
		
#region CameraShake
		[Header("CameraShake")]
		[SerializeField] private Transform _shakeTransform;
	
		private ManagedCoroutine _shakeCor = null;

		private void ShakeOnDestroy() {
			_shakeCor?.Stop();
		}

		public void Shake(CameraShakeData _shakeData) {
			_shakeCor?.Stop();
			_shakeCor = CoroutineRunner.StartManagedCoroutine(ShakeCor(_shakeData));
		}

		private IEnumerator ShakeCor(CameraShakeData _shakeData) {
			// Create keyframe arrays.
			var shakeKeysX = new Keyframe[_shakeData._points + 2];
			var shakeKeysY = new Keyframe[_shakeData._points + 2];
			var shakeKeysZ = new Keyframe[_shakeData._points + 2];
			var keyDelay = _shakeData._duration / (_shakeData._points + 1);
		
			// Zero out first and last keyframes.
			shakeKeysX[0] = new Keyframe(0.0f, 0.0f);
			shakeKeysY[0] = new Keyframe(0.0f, 0.0f);
			shakeKeysZ[0] = new Keyframe(0.0f, 0.0f);
			shakeKeysX[_shakeData._points + 1] = new Keyframe((_shakeData._points + 1) * keyDelay, 0.0f);
			shakeKeysY[_shakeData._points + 1] = new Keyframe((_shakeData._points + 1) * keyDelay, 0.0f);
			shakeKeysZ[_shakeData._points + 1] = new Keyframe((_shakeData._points + 1) * keyDelay, 0.0f);

			// Cache strength because we're going to modify it.
			var strength = _shakeData._strength;
		
			// Generate middle keyframes.
			for (int i = 1; i < _shakeData._points; i++) {
				shakeKeysX[i] = new Keyframe(
					i * keyDelay, 
					UnityEngine.Random.Range(-_shakeData._strength, _shakeData._strength)
				);
			
				shakeKeysY[i] = new Keyframe(
					i * keyDelay, 
					UnityEngine.Random.Range(-_shakeData._strength, _shakeData._strength)
				);
			
				shakeKeysZ[i] = new Keyframe(
					i * keyDelay, 
					UnityEngine.Random.Range(-_shakeData._strength, _shakeData._strength)
				);

				// Decrease strength each frame by our decreaseFactor.
				strength = Mathf.Clamp(strength - _shakeData._decreaseFactor, 0.0f, _shakeData._strength);
			}
		
			// Create animation curves using the keyframes we generated.
			var curveX = new AnimationCurve(shakeKeysX);
			var curveY = new AnimationCurve(shakeKeysY);
			var curveZ = new AnimationCurve(shakeKeysZ);

			// Animate shakeTransform using the generated animation curves.
			var time = 0.0f;
			while (time < _shakeData._duration) {
				_shakeTransform.localPosition = new Vector3(
					curveX.Evaluate(time),
					curveY.Evaluate(time),
					curveZ.Evaluate(time)
				);

				time += Time.deltaTime;
				yield return null;
			}

			// Reset shakeTransform back to resting position.
			_shakeTransform.localPosition = Vector3.zero;
		}
#endregion CameraShake
		
	}
}