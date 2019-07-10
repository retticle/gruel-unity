using System.Collections;
using UnityEngine;

namespace Gruel.CoroutineSystem {
	public class ManagedCoroutine {
	
#region Properties
		public bool IsRunning { get; private set; }
		public Coroutine RootRoutine { get; private set; }
		public Coroutine ChildRoutine { get; private set; }
#endregion Properties

#region Fields
#endregion Fields
		
#region Constructor
		public ManagedCoroutine(IEnumerator routine) {
			RootRoutine = CoroutineRunner.StartCoroutine(RootRoutineCor(routine));
		}
#endregion Constructor

#region Public Methods
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
#endregion Public Methods

#region Private Methods
		private IEnumerator RootRoutineCor(IEnumerator routine) {
			IsRunning = true;
			yield return ChildRoutine = CoroutineRunner.StartCoroutine(routine);
			IsRunning = false;
		}
#endregion Private Methods
	
	}
}