using UnityEngine;

public interface IFlow {

	string FlowName();
	bool ShouldRun();
	Coroutine RunFlow();

}