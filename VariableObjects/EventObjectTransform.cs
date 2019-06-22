using System;
using UnityEngine;

namespace Gruel.VariableObjects {
	[CreateAssetMenu(menuName = "ScriptableObjects/EventObjectTransform")]
	public class EventObjectTransform : ScriptableObject {

		private Action<Transform> _action;

		public void AddListener(Action<Transform> action) {
			_action += action;
		}

		public void RemoveListener(Action<Transform> action) {
			_action -= action;
		}

		public void Invoke(Transform value) {
			_action?.Invoke(value);
		}
	
	}
}