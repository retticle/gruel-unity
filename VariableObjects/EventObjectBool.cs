using System;
using UnityEngine;

namespace Gruel.VariableObjects {
	[CreateAssetMenu(menuName = "ScriptableObjects/EventObjectBool")]
	public class EventObjectBool : ScriptableObject {

		private Action<bool> _action;

		public void AddListener(Action<bool> action) {
			_action += action;
		}

		public void RemoveListener(Action<bool> action) {
			_action -= action;
		}

		public void Invoke(bool value) {
			_action?.Invoke(value);
		}
	
	}
}