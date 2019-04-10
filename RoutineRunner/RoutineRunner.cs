using System.Collections;
using UnityEngine;

namespace Gruel.RoutineRunner {
	public class RoutineRunner : MonoBehaviour {
	
#region Init
		public void Init() {
			CoreInit();
		}
#endregion Init
	
#region Core
		public static RoutineRunner _instance { get; private set; }

		private void CoreInit() {
			// Setup instance.
			if (_instance != null) {
				Debug.LogError("RoutineRunner: There is already an instance of RoutineRunner!");
				Destroy(gameObject);
			} else {
				_instance = this;
			}
		}
#endregion Core
	
#region RoutineRunner
		public static Coroutine StartRoutine(IEnumerator routine) {
			return _instance.StartCoroutine(routine);
		}

		public static ManagedRoutine StartManagedRoutine(IEnumerator routine) {
			return new ManagedRoutine(routine);
		}

		public static void StopRoutine(Coroutine routine) {
			if (_instance != null) {
				_instance.StopCoroutine(routine);
			}
		}
#endregion RoutineRunner
	
	}
}