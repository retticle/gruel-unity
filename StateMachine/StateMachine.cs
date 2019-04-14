using System.Collections.Generic;
using UnityEngine;

namespace Gruel.StateMachine {
	public class StateMachine {

#region Init
		public StateMachine(string stateMachineName) {
			this._stateMachineName = stateMachineName;
		}
#endregion Init

#region State Machine
		private string _stateMachineName;
		private Dictionary<int, IState> _states = new Dictionary<int, IState>();
	
		private int _stateCurrent = -1;
		public int State {
			get { return _stateCurrent; }
			set { SetState(value); }
		}

		public void AddState(int stateId, IState state) {
			_states.Add(stateId, state);
		}

		private void SetState(int stateNext) {
			// Log the intended state change.
			Debug.LogFormat(
				"{0}.StateMachine.SetState: switching from {1} to {2}",
				_stateMachineName,
				_stateCurrent == -1 ? "Uninitialized" : _states[_stateCurrent].StateName(),
				stateNext == -1 ? "Uninitialized" : _states[stateNext].StateName()
			);

			// Exit the current state.
			if (_stateCurrent != -1) {
				_states[_stateCurrent].StateExit();
			}

			// Switch state.
			_stateCurrent = stateNext;

			// Enter the new state.
			if (_stateCurrent != -1) {
				_states[_stateCurrent].StateEnter();
			}
		}
#endregion State Machine

	}
}