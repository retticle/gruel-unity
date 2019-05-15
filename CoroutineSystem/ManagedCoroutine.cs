using System.Collections;
using UnityEngine;

namespace Gruel.CoroutineSystem {
	public class ManagedCoroutine {
	
		public bool IsRunning { get; private set; }
		public Coroutine RootRoutine { get; private set; }
		public Coroutine ChildRoutine { get; private set; }

		public ManagedCoroutine(IEnumerator routine) {
			RootRoutine = CoroutineRunner.StartCoroutine(RootRoutineCor(routine));
		}

		public void Stop() {
			if (IsRunning) {
				if (RootRoutine != null) {
					CoroutineRunner.StopCoroutine(RootRoutine);
					RootRoutine = null;
				}

				if (ChildRoutine != null) {
					CoroutineRunner.StopCoroutine(ChildRoutine);
					ChildRoutine = null;
				}

				IsRunning = false;
			}
		}

		private IEnumerator RootRoutineCor(IEnumerator routine) {
			IsRunning = true;
			yield return ChildRoutine = CoroutineRunner.StartCoroutine(routine);
			IsRunning = false;
		}
	
	}
}