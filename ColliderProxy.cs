using System;
using UnityEngine;

namespace Gruel {
	public class ColliderProxy : MonoBehaviour {
	
#region Collision
		public Action<Collision> _onCollisionEnter;
		public Action<Collision> _onCollisionExit;
		public Action<Collision> _onCollisionStay;

		/// <summary>
		/// OnCollisionEnter is called when this collider/rigidbody has begun touching another rigidbody/collider.
		/// </summary>
		/// <param name="other"></param>
		private void OnCollisionEnter(Collision other) {
			_onCollisionEnter?.Invoke(other);
		}

		/// <summary>
		/// OnCollisionExit is called when this collider/rigidbody has stopped touching another rigidbody/collider.
		/// </summary>
		/// <param name="other"></param>
		private void OnCollisionExit(Collision other) {
			_onCollisionExit?.Invoke(other);
		}

		/// <summary>
		/// OnCollisionStay is called once per frame for every collider/rigidbody that is touching rigidbody/collider.
		/// </summary>
		/// <param name="other"></param>
		private void OnCollisionStay(Collision other) {
			_onCollisionStay?.Invoke(other);
		}
#endregion Collision
	
#region Trigger
		public Action<Collider> _onTriggerEnter;
		public Action<Collider> _onTriggerStay;
		public Action<Collider> _onTriggerExit;

		/// <summary>
		/// OnTriggerEnter is called when the Collider other enters the trigger.
		/// </summary>
		/// <param name="other"></param>
		private void OnTriggerEnter(Collider other) {
			_onTriggerEnter?.Invoke(other);
		}

		/// <summary>
		/// OnTriggerExit is called when the Collider other has stopped touching the trigger.
		/// </summary>
		/// <param name="other"></param>
		private void OnTriggerExit(Collider other) {
			_onTriggerExit?.Invoke(other);
		}
	
		/// <summary>
		/// OnTriggerStay is called almost all the frames for every Collider other that is touching the trigger. The function is on the physics timer so it won't necessarily run every frame.
		/// </summary>
		/// <param name="other"></param>
		private void OnTriggerStay(Collider other) {
			_onTriggerStay?.Invoke(other);
		}
#endregion Trigger
	
#region Mouse
		public Action _onMouseDown;
		public Action _onMouseUp;
		public Action _onMouseDrag;

		/// <summary>
		/// OnMouseDown is called when the user has pressed the mouse button while over the GUIElement or Collider.
		/// </summary>
		private void OnMouseDown() {
			_onMouseDown?.Invoke();
		}

		/// <summary>
		/// OnMouseUp is called when the user has released the mouse button.
		/// </summary>
		private void OnMouseUp() {
			_onMouseUp?.Invoke();
		}

		/// <summary>
		/// OnMouseDrag is called when the user has clicked on a GUIElement or Collider and is still holding down the mouse.
		/// </summary>
		private void OnMouseDrag() {
			_onMouseDrag?.Invoke();
		}
#endregion Mouse

	}
}