using System.Collections.Generic;
using UnityEngine;

namespace Gruel.StateMachine {
	public class StateMachine {

#region Properties
		public int State {
			get => _stateCurrent;
			set => SetState(value);
		}
		
		public int PreviousState { get; private set; }
#endregion Properties

#region Fields
		private readonly string _name;
		private Dictionary<int, IState> _states = new Dictionary<int, IState>();

		private const int UninitializedStateId = -1;
		private const string UninitializedStateName = "Uninitialized";
	
		private int _stateCurrent = UninitializedStateId;
#endregion Fields
		
#region Constructor
		public StateMachine(string stateMachineName) {
			_name = stateMachineName;
		}
#endregion Constructor

#region Public Methods
		public void AddState(int stateId, IState state) {
			_states.Add(stateId, state);
		}
#endregion Public Methods

#region Private Methods
		private void SetState(int stateNext) {
			// Log the intended state change.
			Debug.LogFormat(
				"{0}.StateMachine.SetState: switching from {1} to {2}",
				_name,
				_stateCurrent == UninitializedStateId ? UninitializedStateName : _states[_stateCurrent].StateName,
				stateNext == UninitializedStateId ? UninitializedStateName : _states[stateNext].StateName
			);

			// Exit the current state.
			if (_stateCurrent != UninitializedStateId) {
				_states[_stateCurrent].StateExit();
			}

			// Switch state.
			PreviousState = _stateCurrent;
			_stateCurrent = stateNext;

			// Enter the new state.
			if (_stateCurrent != UninitializedStateId) {
				_states[_stateCurrent].StateEnter();
			}
		}
#endregion Private Methods

	}
}