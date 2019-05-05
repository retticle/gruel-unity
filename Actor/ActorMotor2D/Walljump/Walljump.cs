using System;
using UnityEngine;

namespace Gruel.Actor.ActorMotor2D {
	public class Walljump : MonoBehaviour, IActorMotor2DTrait {
		
		[Header("WallJump Settings")]
		// ReSharper disable once InvalidXmlDocComment
		/// <summary>
		/// The minimum distance the actor needs to be from a wall to walljump.
		/// </summary>
		[SerializeField] private float _closeEnoughDistance = 0.1f;
		
		/// <summary>
		/// Amount of horizontal force that is applied when walljumping.
		/// </summary>
		[SerializeField] private float _horizontalForce = 8.0f;
		
		/// <summary>
		/// Amount of vertical force that is applied when walljumping.
		/// </summary>
		[SerializeField] private float _verticalForce = 15.0f;
		
		/// <summary>
		/// Number of frames trait will run over.
		/// </summary>
		[SerializeField] private int _duration = 24;
		
		/// <summary>
		/// Number of  frames until input starts lerping back to normal.
		/// </summary>
		[SerializeField] private int _forceDuration = 6;
		
		private Actor _actor;
		private ActorMotor2D _actorMotor2D;
		private WalljumpTraitResult _result;

		public ActorMotor2DTraitResult Result => _result;

		public Action _onWalljump;

		public void InitializeTrait(Actor actor, ActorMotor2D actorMotor2D) {
			_result = new WalljumpTraitResult();
			
			this._actor = actor;
			this._actorMotor2D = actorMotor2D;
		}

		public void RemoveTrait() {}

		public void Tick(TickFrame tickFrame) {
			if (_result._active) {
				Step(tickFrame);
			} else {
				if (Detection(tickFrame)) {
					_result._active = true;
					_result._frame = 0;
					tickFrame._velocityCarried = new Vector3(_horizontalForce * _result._direction, _verticalForce, 0.0f);
					
					_onWalljump?.Invoke();
					
					Step(tickFrame);
				}
			}
		}

		private bool Detection(TickFrame tickFrame) {
			if (tickFrame._inputJumpButtonDown == false) {
				// Only continue if jump button was pressed on this tick.
				return false;
			}

			if (tickFrame._isGrounded) {
				// Only continue if actor is not on the ground.
				return false;
			}
			
			var leftDistance = _actorMotor2D._distancesLeft[0];
			var rightDistance = _actorMotor2D._distancesRight[0];

			if (leftDistance > 0.0f
			&& leftDistance < _closeEnoughDistance) {
				_result._direction = 1;
				return true;
			}

			if (rightDistance > 0.0f
			&& rightDistance < _closeEnoughDistance) {
				_result._direction = -1;
				return true;
			}

			return false;
		}

		private void Step(TickFrame tickFrame) {
			if (_result._frame >= _duration) {
				// Stop running trait.
				_result._active = false;
				return;
			}

			if (_result._frame >= _forceDuration) {
				var lerpDuration = (float)(_duration - _forceDuration);
				var ratio = ((float)_result._frame - (float)_forceDuration) / lerpDuration;
				
				// Lerp input back to normal.
				tickFrame._inputHorizontal = Mathf.Lerp(_result._direction, tickFrame._inputHorizontal, ratio);
			} else {
				// Override input to direction of walljump.
				tickFrame._inputHorizontal = _result._direction;
			}

			_result._frame++;
		}

	}
}