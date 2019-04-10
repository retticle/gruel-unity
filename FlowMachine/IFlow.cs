using UnityEngine;

namespace Gruel.FlowMachine {
	public interface IFlow {

		string FlowName();
		bool ShouldRun();
		Coroutine RunFlow();

	}
}