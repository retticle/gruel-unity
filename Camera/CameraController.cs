using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Gruel.CoroutineSystem;
using UnityEngine;

namespace Gruel.Camera {
	public class CameraController : MonoBehaviour {
	
#region Init
		private void Awake() {
			CoreAwake();
		}

		private void LateUpdate() {
			TrackTransformLateUpdate();
		}

		private void OnDestroy() {
			ShakeOnDestroy();
		}
#endregion Init
	
#region Core
		public static CameraController _instance { get; private set; }

		private void CoreAwake() {
			// Setup instance.
			if (_instance != null) {
				Debug.LogError("CameraController: There is already an instance of CameraController!");
				Destroy(gameObject);
			} else {
				_instance = this;
			}
		}
#endregion Core

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
	
#region Shake
		[Header("Shake")]
		[SerializeField] private Transform _shakeTransform;
	
		private ManagedCoroutine _shakeCor = null;

		private void ShakeOnDestroy() {
			_shakeCor?.Stop();
		}

		public static void Shake(CameraShakeData _shakeData) {
			_instance._shakeCor?.Stop();
			_instance._shakeCor = CoroutineRunner.StartManagedCoroutine(_instance.ShakeCor(_shakeData));
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
#endregion Shake
	
#region TrackTransform
		private bool _trackTransform = false;
		private Transform _trackedTransform = null;
		private Vector3 _trackTransformOffset = Vector3.zero;

		private const float _trackSpeed = 5.0f;

		public void TrackTransform(Transform transformToTrack, Vector3 offset) {
			Debug.Log($"CameraController.TrackTransform: transformToTrack: {transformToTrack.name} | offset: {offset}");
		
			_trackTransform = true;
			_trackedTransform = transformToTrack;
			_trackTransformOffset = offset;

			transform.position = _trackedTransform.position + _trackTransformOffset;
		}

		public void StopTrackingTransform() {
			Debug.Log("CameraController.StopTrackingTransform");
		
			_trackTransform = false;
			_trackedTransform = null;
			_trackTransformOffset = Vector3.zero;
		}

		private void TrackTransformLateUpdate() {
			if (_trackTransform) {
				transform.position = (_trackedTransform.position + _trackTransformOffset);

				// var targetPosition = _trackedTransform.position + _trackTransformOffset;
				// transform.position = Vector3.Lerp(transform.position, targetPosition, Time.unscaledDeltaTime * _trackSpeed);
			}
		}
#endregion TrackTransform
	
#region Attachables
		[Header("Attachables")]
		[SerializeField] private Transform _attachablesContainer;

		private List<Transform> _attachedTransforms = new List<Transform>();

		public void Attach(Transform attachable) {
			if (_attachedTransforms.Contains(attachable)) {
				Debug.LogError("CameraController.Attach: Transform is already attached!");
				return;
			}
		
			_attachedTransforms.Add(attachable);
			attachable.SetParent(_attachablesContainer, true);
		}

		public void Detach(Transform attachable) {
			if (_attachedTransforms.Contains(attachable) == false) {
				Debug.LogError("CameraController.Detach: Transform isn't attached!");
				return;
			}

			_attachedTransforms.Remove(attachable);
			attachable.transform.parent = null;
		}
#endregion Attachables

	}
}
