using System;
using System.Collections;
using System.Collections.Generic;
using Gruel.CoroutineUtils;
using UnityEngine;

namespace Gruel.FlowMachine {
	public class FlowMachine {

#region Properties
#endregion Properties

#region Fields
		private readonly string _flowMachineName;
	
		private List<IFlow> _flows = new List<IFlow>();
		private ManagedCoroutine _flowCor;
#endregion Fields
	
#region Constructor
		public FlowMachine(string flowMachineName) {
			_flowMachineName = flowMachineName;
		}
#endregion Constructor

#region Public Methods
		public void AddFlow(IFlow flow) {
			_flows.Add(flow);
		}

		public void StartFlow(Action onFlowComplete = null) {
			if (_flowCor != null
			&& _flowCor.IsRunning) {
				Debug.LogError($"{_flowMachineName}.FlowMachine.StartFlow: Flow is already running!");
				return;
			}

			_flowCor = CoroutineRunner.StartManagedCoroutine(FlowCor(onFlowComplete));
		}

		public void StopFlow() {
			_flowCor?.Stop();
			_flowCor = null;
		}
#endregion Public Methods

#region Private Methods
		private IEnumerator FlowCor(Action onFlowCompleteCallback) {
			Debug.Log($"{_flowMachineName}.FlowMachine.FlowCor: Starting flow");

			// Loop through each flow.
			// Check if it should be run or not.
			// Wait for a flow to run before moving on to the next.
			for (int i = 0, n = _flows.Count; i < n; i++) {
				if (_flows[i].ShouldRun) {
					Debug.Log($"{_flowMachineName}.FlowMachine.FlowCor: Starting flow step: {_flows[i].FlowName}");

					yield return _flows[i].RunFlow();
				}
			}
		
			Debug.Log($"{_flowMachineName}.FlowMachine.FlowCor: Finished flow");

			onFlowCompleteCallback?.Invoke();
		}
#endregion Private Methods

	}
}