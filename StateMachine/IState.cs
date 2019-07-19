namespace Gruel.StateMachine {
	public interface IState {
		
#region Properties
		string StateName { get; }
#endregion Properties

#region Methods
		void StateEnter();
		void StateExit();
		void StateTick();
#endregion Methods

	}
}