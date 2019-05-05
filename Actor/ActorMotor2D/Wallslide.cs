using UnityEngine;

namespace Gruel.Actor.ActorMotor2D {
	public class Wallslide : MonoBehaviour, IActorMotor2DTrait {
		
		private Actor _actor;
		private ActorMotor2D _actorMotor2D;

		private float _wallSlideSpeedScalar = 0.25f;

		public ActorMotor2DTraitResult Result { get; }

		public void InitializeTrait(Actor actor, ActorMotor2D actorMotor2D) {
			this._actor = actor;
			this._actorMotor2D = actorMotor2D;
		}

		public void RemoveTrait() {}

		public void Tick(TickFrame tickFrame) {
			
		}
		
	}
}