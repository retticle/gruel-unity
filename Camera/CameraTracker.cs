using UnityEngine;

namespace Gruel.Camera {
	public class CameraTracker : CameraTrait {
		
#region Init
		private void LateUpdate() {
			CameraTrackerLateUpdate();
		}
#endregion Init
		
#region CameraTracker
		[Header("CameraTracker")]
		[SerializeField] private Transform _trackingTransform;
		[SerializeField] private Transform _offsetTransform;
		
		private bool _trackTransform = false;
		private Transform _trackedTransform = null;
		private Vector3 _trackTransformOffset = Vector3.zero;

		private const float _trackSpeed = 5.0f;

		public void Track(Transform transformToTrack, Vector3 offset) {
			Debug.Log($"CameraTracker.Track: transform: {transformToTrack.name} | offset: {offset}");
		
			_trackTransform = true;
			_trackedTransform = transformToTrack;
			_trackTransformOffset = offset;

			_offsetTransform.localPosition = _trackTransformOffset;
			_trackingTransform.position = _trackedTransform.position;
		}

		public void StopTracking() {
			Debug.Log("CameraTracker.StopTracking");
		
			_trackTransform = false;
			_trackedTransform = null;
			_trackTransformOffset = Vector3.zero;
		}

		private void CameraTrackerLateUpdate() {
			if (_trackTransform) {
				_trackingTransform.position = _trackedTransform.position;

				// var targetPosition = _trackedTransform.position + _trackTransformOffset;
				// transform.position = Vector3.Lerp(transform.position, targetPosition, Time.unscaledDeltaTime * _trackSpeed);
			}
		}
#endregion CameraTracker
		
	}
}