using System.Collections;
using UnityEngine;

namespace Gruel.RoutineRunner {
	public class ManagedRoutine {
	
		public bool _isRunning { get; private set; }
		public Coroutine _rootRoutine { get; private set; }
		public Coroutine _childRoutine { get; private set; }

		public ManagedRoutine(IEnumerator routine) {
			_rootRoutine = RoutineRunner.StartRoutine(RootRoutineCor(routine));
		}

		public void Stop() {
			if (_isRunning) {
				if (_rootRoutine != null) {
					RoutineRunner.StopRoutine(_rootRoutine);
					_rootRoutine = null;
				}

				if (_childRoutine != null) {
					RoutineRunner.StopRoutine(_childRoutine);
					_childRoutine = null;
				}

				_isRunning = false;
			}
		}

		private IEnumerator RootRoutineCor(IEnumerator routine) {
			_isRunning = true;
			yield return _childRoutine = RoutineRunner.StartRoutine(routine);
			_isRunning = false;
		}
	
	}
}