using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/GameEventTransform")]
public class GameEventTransform : ScriptableObject {

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