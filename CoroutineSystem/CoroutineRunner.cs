using System.Collections;
using UnityEngine;

namespace Gruel.CoroutineSystem {
	public class CoroutineRunner : MonoBehaviour {
	
#region Init
		public void Init() {
			CoreInit();
		}
#endregion Init
	
#region Core
		public static CoroutineRunner _instance { get; private set; }

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
		public new static Coroutine StartCoroutine(IEnumerator coroutine) {
			return ((MonoBehaviour)_instance).StartCoroutine(coroutine);
		}

		public static ManagedCoroutine StartManagedCoroutine(IEnumerator coroutine) {
			return new ManagedCoroutine(coroutine);
		}

		public new static void StopCoroutine(Coroutine coroutine) {
			if (_instance != null) {
				((MonoBehaviour)_instance).StopCoroutine(coroutine);
			}
		}
#endregion RoutineRunner
	
	}
}