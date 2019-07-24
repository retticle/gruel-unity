using UnityEngine;

namespace Gruel.FlowMachine {
	public interface IFlow {
		
#region Properties
		string FlowName { get; }
		bool ShouldRun { get; }
#endregion Properties
		
#region Methods
		Coroutine RunFlow();
#endregion Methods

	}
}