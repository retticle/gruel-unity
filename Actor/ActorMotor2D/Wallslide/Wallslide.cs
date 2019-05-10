using System;
using UnityEngine;

namespace Gruel.Actor.ActorMotor2D {
	public class Wallslide : MonoBehaviour, IActorMotor2DTrait {
		
#region Public
		public ActorMotor2DTraitResult Result => _result;

		public Action _onWallslideStart;
		public Action _onWallslideEnd;
		
		public void InitializeTrait(Actor actor, ActorMotor2D actorMotor2D) {
			this._actor = actor;
			this._actorMotor2D = actorMotor2D;
			
			_result = new WallslideResult();
		}

		public void RemoveTrait() {}

		public void Tick(TickFrame tickFrame) {
			if (Detection(tickFrame)) {
				if (_result._active == false) {
					_result._active = true;
					_onWallslideStart?.Invoke();
				}
				
				tickFrame._velocityCarried = Vector3.Lerp(
					tickFrame._velocityCarried,
					Vector3.zero,
					_wallDrag * _actor._customTimeDilation * Time.fixedDeltaTime
				);
			} else {
				if (_result._active) {
					_result._active = false;
					_onWallslideEnd?.Invoke();
				}
			}
		}
#endregion Public
		
#region Private
		[Header("Wallslide Settings")]
		// ReSharper disable once InvalidXmlDocComment
		/// <summary>
		/// The minimum distance the actor needs to be from a wall to wallslide.
		/// </summary>
		[SerializeField] private float _closeEnoughDistance = 0.1f;
		
		/// <summary>
		/// The amount of velocity that will be removed per second while wallslide is active.
		/// </summary>
		[SerializeField] private float _wallDrag = 5.0f;
		
		private Actor _actor;
		private ActorMotor2D _actorMotor2D;
		private WallslideResult _result;
		
		private bool Detection(TickFrame tickFrame) {
			// Require actor to be airborne.
			if (tickFrame._isGrounded) {
				return false;
			}

			// Require actor to have negative y velocity.
			if (tickFrame._velocityCarried.y > 0.0f) {
				return false;
			}

			var leftDistance = _actorMotor2D._distancesLeft[0];
			var rightDistance = _actorMotor2D._distancesRight[0];

			if (leftDistance > 0.0f
			    && leftDistance < _closeEnoughDistance
			    && tickFrame._inputHorizontal < 0.0f) {
				_result._wallSide = -1;
				return true;
			}

			if (rightDistance > 0.0f
			    && rightDistance < _closeEnoughDistance
			    && tickFrame._inputHorizontal > 0.0f) {
				_result._wallSide = 1;
				return true;
			}

			return false;
		}
#endregion Private
		
	}
}