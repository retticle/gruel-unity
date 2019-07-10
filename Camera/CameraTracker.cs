using UnityEngine;

namespace Gruel.Camera {
	public class CameraTracker : CameraTrait {
		
#region Properties
		public Vector3 Offset {
			get => _offset;
			set {
				_offset = value;
				OffsetChanged();
			}
		}
#endregion Properties

#region Fields
		[Header("CameraTracker")]
		[SerializeField] private Transform _trackingTransform;
		[SerializeField] private Transform _offsetTransform;
		
		private bool _trackTransform;
		private Transform _trackedTransform;
		private Vector3 _offset = Vector3.zero;

//		private const float _trackSpeed = 5.0f;
#endregion Fields

#region Public Methods
		public void Track(Transform transformToTrack, Vector3 offset) {
			Debug.Log($"CameraTracker.Track: transform: {transformToTrack.name} | offset: {offset}");
		
			_trackTransform = true;
			_trackedTransform = transformToTrack;
			
			// Set offset.
			Offset = offset;
			
			// Set TrackedTransform to the position of the transform we need to track.
			_trackingTransform.position = _trackedTransform.position;
		}

		public void StopTracking() {
			Debug.Log("CameraTracker.StopTracking");
		
			_trackTransform = false;
			_trackedTransform = null;
			_offset = Vector3.zero;
		}
#endregion Public Methods

#region Private Methods
		private void LateUpdate() {
			if (_trackTransform) {
				_trackingTransform.position = _trackedTransform.position;

				// var targetPosition = _trackedTransform.position + _trackTransformOffset;
				// transform.position = Vector3.Lerp(transform.position, targetPosition, Time.unscaledDeltaTime * _trackSpeed);
			}
		}
		
		private void OffsetChanged() {
			_offsetTransform.localPosition = _offset;
		}
#endregion Private Methods
		
	}
}