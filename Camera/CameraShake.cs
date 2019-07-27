using System.Collections;
using Gruel.CoroutineUtils;
using UnityEngine;

namespace Gruel.Camera {
	public class CameraShake : CameraTrait {
		
#region Properties
#endregion Properties

#region Fields
		[Header("CameraShake")]
		[SerializeField] private Transform _shakeTransform;
	
		private ManagedCoroutine _shakeCor;
#endregion Fields

#region Public Methods
		public void Shake(CameraShakeData shakeData) {
			_shakeCor?.Stop();
			_shakeCor = CoroutineRunner.StartManagedCoroutine(ShakeCor(shakeData));
		}
#endregion Public Methods

#region Private Methods
		private void OnDestroy() {
			_shakeCor?.Stop();
		}
		
		private IEnumerator ShakeCor(CameraShakeData _shakeData) {
			// Create keyframe arrays.
			var shakeKeysX = new Keyframe[_shakeData.Points + 2];
			var shakeKeysY = new Keyframe[_shakeData.Points + 2];
			var shakeKeysZ = new Keyframe[_shakeData.Points + 2];
			var keyDelay = _shakeData.Duration / (_shakeData.Points + 1);
		
			// Zero out first and last keyframes.
			shakeKeysX[0] = new Keyframe(0.0f, 0.0f);
			shakeKeysY[0] = new Keyframe(0.0f, 0.0f);
			shakeKeysZ[0] = new Keyframe(0.0f, 0.0f);
			shakeKeysX[_shakeData.Points + 1] = new Keyframe((_shakeData.Points + 1) * keyDelay, 0.0f);
			shakeKeysY[_shakeData.Points + 1] = new Keyframe((_shakeData.Points + 1) * keyDelay, 0.0f);
			shakeKeysZ[_shakeData.Points + 1] = new Keyframe((_shakeData.Points + 1) * keyDelay, 0.0f);

			// Cache strength because we're going to modify it.
			var strength = _shakeData.Strength;
		
			// Generate middle keyframes.
			for (int i = 1; i < _shakeData.Points; i++) {
				shakeKeysX[i] = new Keyframe(
					i * keyDelay, 
					UnityEngine.Random.Range(-_shakeData.Strength, _shakeData.Strength)
				);
			
				shakeKeysY[i] = new Keyframe(
					i * keyDelay, 
					UnityEngine.Random.Range(-_shakeData.Strength, _shakeData.Strength)
				);
			
				shakeKeysZ[i] = new Keyframe(
					i * keyDelay, 
					UnityEngine.Random.Range(-_shakeData.Strength, _shakeData.Strength)
				);

				// Decrease strength each frame by our decreaseFactor.
				strength = Mathf.Clamp(strength - _shakeData.DecreaseFactor, 0.0f, _shakeData.Strength);
			}
		
			// Create animation curves using the keyframes we generated.
			var curveX = new AnimationCurve(shakeKeysX);
			var curveY = new AnimationCurve(shakeKeysY);
			var curveZ = new AnimationCurve(shakeKeysZ);

			// Animate shakeTransform using the generated animation curves.
			var time = 0.0f;
			while (time < _shakeData.Duration) {
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
#endregion Private Methods
		
	}
}