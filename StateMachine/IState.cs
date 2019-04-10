using System;

namespace Gruel.StateMachine {
	public interface IState {

		string StateName();
	
		void StateEnter();
		void StateExit(Action onStateExited);
		void StateTick();

	}
}