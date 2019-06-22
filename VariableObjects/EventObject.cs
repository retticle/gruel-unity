using System;
using UnityEngine;

namespace Gruel.VariableObjects {
	[CreateAssetMenu(menuName = "ScriptableObjects/EventObject")]
	public class EventObject : ScriptableObject {

		private Action _action;

		public void AddListener(Action action) {
			_action += action;
		}

		public void RemoveListener(Action action) {
			_action -= action;
		}

		public void Invoke() {
			_action?.Invoke();
		}
	
	}
}