using System.Collections.Generic;
using UnityEngine;

namespace Gruel.Camera {
	public class CameraAttachables : CameraTrait {

#region Properties
#endregion Properties

#region Fields
		[Header("CameraAttachables")]
		[SerializeField] private Transform _attachablesContainer;

		private List<Transform> _attachedTransforms = new List<Transform>();
#endregion Fields

#region Public Methods
		public void Attach(Transform attachable, Vector3 offset) {
			if (_attachedTransforms.Contains(attachable)) {
				Debug.LogError("CameraAttachables.Attach: Transform is already attached!");
				return;
			}
		
			_attachedTransforms.Add(attachable);
			attachable.SetParent(_attachablesContainer, true);
			attachable.localPosition = offset;
		}

		public void Detach(Transform attachable) {
			if (_attachedTransforms.Contains(attachable) == false) {
				Debug.LogError("CameraAttachables.Detach: Transform isn't attached!");
				return;
			}

			_attachedTransforms.Remove(attachable);
			attachable.transform.parent = null;
		}
#endregion Public Methods

#region Private Methods
#endregion Private Methods

	}
}