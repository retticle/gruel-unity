using System;
using UnityEngine;

namespace Gruel {
	public class ColliderProxy : MonoBehaviour {
	
#region Properties
		/// <summary>
		/// OnCollisionEnter is called when this collider/rigidbody has begun touching another rigidbody/collider.
		/// </summary>
		/// <param name="other"></param>
		public Action<Collision> _onCollisionEnter;
		
		/// <summary>
		/// OnCollisionExit is called when this collider/rigidbody has stopped touching another rigidbody/collider.
		/// </summary>
		/// <param name="other"></param>
		public Action<Collision> _onCollisionExit;
		
		/// <summary>
		/// OnCollisionStay is called once per frame for every collider/rigidbody that is touching rigidbody/collider.
		/// </summary>
		/// <param name="other"></param>
		public Action<Collision> _onCollisionStay;
		
		/// <summary>
		/// OnTriggerEnter is called when the Collider other enters the trigger.
		/// </summary>
		/// <param name="other"></param>
		public Action<Collider> _onTriggerEnter;
		
		/// <summary>
		/// OnTriggerStay is called almost all the frames for every Collider other that is touching the trigger. The function is on the physics timer so it won't necessarily run every frame.
		/// </summary>
		/// <param name="other"></param>
		public Action<Collider> _onTriggerStay;
		
		/// <summary>
		/// OnTriggerExit is called when the Collider other has stopped touching the trigger.
		/// </summary>
		/// <param name="other"></param>
		public Action<Collider> _onTriggerExit;
		
		/// <summary>
		/// OnMouseDown is called when the user has pressed the mouse button while over the GUIElement or Collider.
		/// </summary>
		public Action _onMouseDown;
		
		/// <summary>
		/// OnMouseUp is called when the user has released the mouse button.
		/// </summary>
		public Action _onMouseUp;
		
		/// <summary>
		/// OnMouseDrag is called when the user has clicked on a GUIElement or Collider and is still holding down the mouse.
		/// </summary>
		public Action _onMouseDrag;
#endregion Properties

#region Private Methods
		private void OnCollisionEnter(Collision other) {
			_onCollisionEnter?.Invoke(other);
		}


		private void OnCollisionExit(Collision other) {
			_onCollisionExit?.Invoke(other);
		}


		private void OnCollisionStay(Collision other) {
			_onCollisionStay?.Invoke(other);
		}
		

		private void OnTriggerEnter(Collider other) {
			_onTriggerEnter?.Invoke(other);
		}
		
		private void OnTriggerStay(Collider other) {
			_onTriggerStay?.Invoke(other);
		}


		private void OnTriggerExit(Collider other) {
			_onTriggerExit?.Invoke(other);
		}
	

		private void OnMouseDown() {
			_onMouseDown?.Invoke();
		}


		private void OnMouseUp() {
			_onMouseUp?.Invoke();
		}


		private void OnMouseDrag() {
			_onMouseDrag?.Invoke();
		}
#endregion Private Methods

	}
}