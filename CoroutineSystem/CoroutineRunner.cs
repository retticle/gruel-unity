using System.Collections;
using UnityEngine;

namespace Gruel.CoroutineSystem {
	public class CoroutineRunner : MonoBehaviour {
	
#region RoutineRunner
		public new static Coroutine StartCoroutine(IEnumerator coroutine) {
			// Make sure the CoroutineRunner object is still alive.
			if (_instance == null
		    || _instance.gameObject == null) {
				return null;
			}
			
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
		
#region Properties
		
#endregion Properties

#region Fields
		private static CoroutineRunner _instance;
#endregion Fields

#region Public Methods
		public void Init() {
			// Setup instance.
			if (_instance != null) {
				Debug.LogError("RoutineRunner: There is already an instance of RoutineRunner!");
				Destroy(gameObject);
			} else {
				_instance = this;
			}
		}
#endregion Public Methods

#region Private Methods
#endregion Private Methods
	
	}
}