using UnityEngine;

namespace Gruel.Actor.ActorMotor2D {
	public class Crouch : MonoBehaviour, IActorMotor2DTrait {
		
		// Components.
		private Actor _actor;
		private ActorMotor2D _actorMotor2D;
		private CharacterController _cc;
		
		[Header("Crouch Settings")]
		public float _crouchWalkSpeed = 2.0f;
		public float _crouchCenter = 0.37f;
		public float _crouchHeight = 0.74f;
		
		// Cache.
		private float _ccCenterCache = 0.0f;
		private float _ccHeightCache = 0.0f;
		private float _walkSpeedCache = 0.0f;

		// Results.
		public bool _isCrouching;
		public bool _startedCrouching;
		
		public void InitializeTrait(Actor actor, ActorMotor2D actorMotor2D) {
			this._actor = actor;
			this._actorMotor2D = actorMotor2D;
			
			// Cache ActorMotor2D settings.
			this._walkSpeedCache = _actorMotor2D._walkSpeed;
			
			// Cache capsule settings.
			this._cc = _actorMotor2D._cc;
			this._ccCenterCache = _cc.center.y;
			this._ccHeightCache = _cc.height;
		}

		public void RemoveTrait() {}

		public void Tick(TickFrame tickFrame) {
			var wasCrouching = _isCrouching;

			// Reset results.
			_isCrouching = false;
			_startedCrouching = false;
			
			if (tickFrame._inputCrouch
		    && tickFrame._isGrounded) {
				_isCrouching = true;
			} else {
				_isCrouching = false;
			}

			if (wasCrouching != _isCrouching) {
				DoCrouch(tickFrame);
			}
		}
		
		private void DoCrouch(TickFrame tickFrame) {
			var crouch = _isCrouching;
			
			// Set walk speed.
			_actorMotor2D._walkSpeed = crouch ? _crouchWalkSpeed : _walkSpeedCache;
			
			// Set capsule settings.
			_cc.center = new Vector3(0.0f, crouch ? _crouchCenter : _ccCenterCache, 0.0f);
			_cc.height = crouch ? _crouchHeight : _ccHeightCache;
			
			if (crouch) {
				_startedCrouching = true;
			}
		}

	}
}