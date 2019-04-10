using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/GameEventBool")]
public class GameEventBool : ScriptableObject {

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