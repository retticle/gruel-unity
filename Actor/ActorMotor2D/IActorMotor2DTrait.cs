namespace Gruel.Actor.ActorMotor2D {
	public interface IActorMotor2DTrait {

		ActorMotor2DTraitResult Result { get; }
		
		void InitializeTrait(Actor actor, ActorMotor2D actorMotor2D);
		void RemoveTrait();

		void Tick(TickFrame tickFrame);

	}
}