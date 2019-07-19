using UnityEngine;

namespace Gruel.FlowMachine {
	public interface IFlow {
		
#region Properties
		string FlowName { get; }
#endregion Properties
		
#region Methods
		bool ShouldRun();
		Coroutine RunFlow();
#endregion Methods

	}
}