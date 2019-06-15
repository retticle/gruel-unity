using System.Collections.Generic;
using UnityEngine;

namespace Gruel.StateMachine {
	public class StateMachine {

#region Init
		public StateMachine(string stateMachineName) {
			_stateMachineName = stateMachineName;
		}
#endregion Init

#region State Machine
		private readonly string _stateMachineName;
		private Dictionary<int, IState> _states = new Dictionary<int, IState>();

		private const int UninitializedStateId = -1;
		private const string UninitializedStateName = "Uninitialized";
	
		private int _stateCurrent = UninitializedStateId;
		
		public int PreviousState { get; private set; }
		
		public int State {
			get => _stateCurrent;
			set => SetState(value);
		}

		public void AddState(int stateId, IState state) {
			_states.Add(stateId, state);
		}

		private void SetState(int stateNext) {
			// Log the intended state change.
			Debug.LogFormat(
				"{0}.StateMachine.SetState: switching from {1} to {2}",
				_stateMachineName,
				_stateCurrent == UninitializedStateId ? UninitializedStateName : _states[_stateCurrent].StateName(),
				stateNext == UninitializedStateId ? UninitializedStateName : _states[stateNext].StateName()
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
#endregion State Machine

	}
}