namespace Gruel.StateMachine {
	public interface IState {

		string StateName();
	
		void StateEnter();
		void StateExit();
		void StateTick();

	}
}