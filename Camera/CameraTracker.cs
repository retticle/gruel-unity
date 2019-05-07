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
		private Vector3 _offset = Vector3.zero;

		private const float _trackSpeed = 5.0f;
		
		public Vector3 Offset {
			get { return _offset; }
			set {
				_offset = value;
				UpdateOffsetTransform();
			}
		}

		public void Track(Transform transformToTrack, Vector3 offset) {
			Debug.Log($"CameraTracker.Track: transform: {transformToTrack.name} | offset: {offset}");
		
			_trackTransform = true;
			_trackedTransform = transformToTrack;
			
			// Save offset amount.
			_offset = offset;

			// Update OffsetTransform to the correct offset.
			UpdateOffsetTransform();
			
			// Set TrackedTransform to the position of the transform we need to track.
			_trackingTransform.position = _trackedTransform.position;
		}

		public void StopTracking() {
			Debug.Log("CameraTracker.StopTracking");
		
			_trackTransform = false;
			_trackedTransform = null;
			_offset = Vector3.zero;
		}

		private void CameraTrackerLateUpdate() {
			if (_trackTransform) {
				_trackingTransform.position = _trackedTransform.position;

				// var targetPosition = _trackedTransform.position + _trackTransformOffset;
				// transform.position = Vector3.Lerp(transform.position, targetPosition, Time.unscaledDeltaTime * _trackSpeed);
			}
		}

		private void UpdateOffsetTransform() {
			_offsetTransform.localPosition = _offset;
		}
#endregion CameraTracker
		
	}
}