using System.Collections;
using UnityEngine;

namespace Gruel.CoroutineSystem {
	public class ManagedCoroutine {
	
		public bool _isRunning { get; private set; }
		public Coroutine _rootRoutine { get; private set; }
		public Coroutine _childRoutine { get; private set; }

		public ManagedCoroutine(IEnumerator routine) {
			_rootRoutine = CoroutineRunner.StartCoroutine(RootRoutineCor(routine));
		}

		public void Stop() {
			if (_isRunning) {
				if (_rootRoutine != null) {
					CoroutineRunner.StopCoroutine(_rootRoutine);
					_rootRoutine = null;
				}

				if (_childRoutine != null) {
					CoroutineRunner.StopCoroutine(_childRoutine);
					_childRoutine = null;
				}

				_isRunning = false;
			}
		}

		private IEnumerator RootRoutineCor(IEnumerator routine) {
			_isRunning = true;
			yield return _childRoutine = CoroutineRunner.StartCoroutine(routine);
			_isRunning = false;
		}
	
	}
}