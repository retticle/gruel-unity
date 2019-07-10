using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gruel.TimeDilation {
	public static class TimeDilation {
	
#region Fields
		private static Dictionary<UnityEngine.Object, float> _affectors = new Dictionary<Object,float>();
#endregion Fields

#region Public Methods
		public static void AddAffector(UnityEngine.Object obj, float timeDilation) {
			if (_affectors.ContainsKey(obj)) {
				Debug.LogError($"TimeDilation.AddAffector: This affector ({obj.name}) has already been added!");
				return;
			}
		
			_affectors.Add(obj, timeDilation);
			Evaluate();
		}

		public static void RemoveAffector(UnityEngine.Object obj) {
			if (_affectors.ContainsKey(obj) == false) {
				Debug.LogError($"TimeDilation.RemoveAffector: This affector ({obj.name})  doesn't exist in the system!");
				return;
			}
		
			_affectors.Remove(obj);
			Evaluate();
		}
#endregion Public Methods

#region Private Methods
		private static void Evaluate() {
			if (_affectors.Count < 1) {
				Debug.Log("TimeDilation.Evaluate: No more affectors in system, setting time scale to 1.0f");
				Time.timeScale = 1.0f;
				return;
			}
		
			var list = _affectors.Values.ToList();
			list.Sort();
			var timeScale = list[0];
		
			Debug.Log($"TimeDilation.Evaluate: Setting time scale to {timeScale}f");

			Time.timeScale = timeScale;
		}
#endregion Private Methods
	
	}
}