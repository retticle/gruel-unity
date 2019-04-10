using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/GameEvent")]
public class GameEvent : ScriptableObject {

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