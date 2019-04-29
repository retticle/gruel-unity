using UnityEngine;

namespace Gruel.Actor.ActorMotor2D {
	public class TickFrame {
		
		// Input.
		public float _inputHorizontal;
		public bool _inputJumpButtonDown;
		public bool _inputCrouch;

		// Results.
		public Vector3 _velocityCarried;
		public Vector3 _velocityMovement;
		public Vector3 _velocity;
		public bool _isGrounded;
		public bool _isWalking;
		public bool _isJumping;

		public TickFrame() {
			_velocityCarried = Vector3.zero;
			_velocityMovement = Vector3.zero;
			_velocity = Vector3.zero;
			_isGrounded = false;
			_isWalking = false;
			_isJumping = false;
		}

		public void Reset() {
			_velocityMovement = Vector3.zero;
			_isWalking = false;
			_isGrounded = false;
			_isJumping = false;
		}
		
	}
}