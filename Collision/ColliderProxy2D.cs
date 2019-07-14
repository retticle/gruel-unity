using System;
using UnityEngine;

namespace Gruel {
	public class ColliderProxy2D : MonoBehaviour {
	
#region Properties
		/// <summary>
		/// Sent when an incoming collider makes contact with this object's collider (2D physics only).
		/// </summary>
		public Action<Collision2D> _onCollisionEnter2D;
		
		/// <summary>
		/// Sent when a collider on another object stops touching this object's collider (2D physics only).
		/// </summary>
		public Action<Collision2D> _onCollisionExit2D;
		
		/// <summary>
		/// Sent each frame where a collider on another object is touching this object's collider (2D physics only).
		/// </summary>
		public Action<Collision2D> _onCollisionStay2D;
		
		/// <summary>
		/// Sent when another object enters a trigger collider attached to this object (2D physics only).
		/// </summary>
		public Action<Collider2D> _onTriggerEnter2D;
		
		/// <summary>
		/// Sent each frame where another object is within a trigger collider attached to this object (2D physics only).
		/// </summary>
		public Action<Collider2D> _onTriggerStay2D;
		
		/// <summary>
		/// Sent when another object leaves a trigger collider attached to this object (2D physics only).
		/// </summary>
		public Action<Collider2D> _onTriggerExit2D;
		
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
		private void OnCollisionEnter2D(Collision2D other) {
			_onCollisionEnter2D?.Invoke(other);
		}

		private void OnCollisionExit2D(Collision2D other) {
			_onCollisionExit2D?.Invoke(other);
		}


		private void OnCollisionStay2D(Collision2D other) {
			_onCollisionStay2D?.Invoke(other);
		}
		

		private void OnTriggerEnter2D(Collider2D other) {
			_onTriggerEnter2D?.Invoke(other);
		}
		
		private void OnTriggerStay2D(Collider2D other) {
			_onTriggerStay2D?.Invoke(other);
		}


		private void OnTriggerExit2D(Collider2D other) {
			_onTriggerExit2D?.Invoke(other);
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