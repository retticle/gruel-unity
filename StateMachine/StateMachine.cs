using System;
using System.Collections;
using System.Collections.Generic;
using Gruel.CoroutineSystem;
using UnityEngine;

namespace Gruel.StateMachine {
	public class StateMachine {
	
#region Init
		public StateMachine(string stateMachineName) {
			_stateMachineName = stateMachineName;
		}
#endregion Init
	
#region State Machine
		private string _stateMachineName;
		private Dictionary<int, IState> _states = new Dictionary<int, IState>();
	
		public int StatePrevious { get; private set; }
		private int _stateCurrent = -1;
		public int State {
			get { return _stateCurrent; }
			set { CoroutineRunner.StartCoroutine(SetState(value)); }
		}

		public void AddState(int stateId, IState state) {
			_states.Add(stateId, state);
		}
	
		private IEnumerator SetState(int stateNext) {
			// Save the previous state.
			StatePrevious = _stateCurrent;

			// Log the intended state change.
			Debug.LogFormat(
				"{0}.StateMachine.SetState: switching from {1} to {2}",
				_stateMachineName,
				_stateCurrent == -1 ? "Uninitialized" : _states[_stateCurrent].StateName(),
				stateNext == -1 ? "Uninitialized" : _states[stateNext].StateName()
			);

			// Exit the current state.
			if (_stateCurrent != -1) {
				bool stateExited = false;
				Action onStateExited = delegate {
					Debug.Log($"{_stateMachineName}.StateMachine.SetState: onStateExcited");
					stateExited = true;
				};
			
				_states[_stateCurrent].StateExit(onStateExited);

				while (stateExited == false) {
					yield return null;
				}
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