using UnityEngine;

namespace Gruel.Actor.ActorMotor2D {
	public class WallJump : MonoBehaviour, IActorMotor2DTrait {
		
#region IActorMotor2DTrait
		private Actor _actor;
		private ActorMotor2D _actorMotor2D;
		
		public void InitializeTrait(Actor actor, ActorMotor2D actorMotor2D) {
			this._actor = actor;
			this._actorMotor2D = actorMotor2D;
		}

		public void RemoveTrait() {}

		public void Tick(TickFrame tickFrame) {
			// Don't continue if jump wasn't pressed.
			if (tickFrame._inputJumpButtonDown == false) {
				return;
			}

			// Don't continue if the actor is on the ground.
			if (tickFrame._isGrounded) {
				return;
			}

			var leftDistance = _actorMotor2D._distancesLeft[0];
			var rightDistance = _actorMotor2D._distancesRight[0];
			
			// Check if left trace hit something, and that it's within the closeEnoughDistance.
			if (leftDistance > 0.0f
		    && leftDistance < _closeEnoughDistance) {
				// Wall is on the left side, so jump to the right.
				tickFrame._velocityCarried += new Vector3(_horizontalForce * 1.0f, _verticalForce, 0.0f);
			}

			// Check if right trace hit something, and that it's within the closeEnoughDistance.
			if (rightDistance > 0.0f
		    && rightDistance < _closeEnoughDistance) {
				// Wall is on the right side, so jump to the left.
				tickFrame._velocityCarried += new Vector3(_horizontalForce * -1.0f, _verticalForce, 0.0f);
			}
		}
#endregion IActorMotor2DTrait
		
#region WallJump
		[Header("WallJump Settings")]
		private float _closeEnoughDistance = 0.1f;
		private float _horizontalForce = 8.0f;
		private float _verticalForce = 15.0f;
#endregion WallJump

	}
}