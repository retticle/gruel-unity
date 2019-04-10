using System;
using UnityEngine;

[Serializable]
public class IntReference {
	[SerializeField] private int _value = 0;
	[SerializeField] private IntObject _intObject;

	public void AddListener(Action<int, int> onValueChanged) {
		_intObject._onValueChanged += onValueChanged;
	}

	public void RemoveListener(Action<int, int> onValueChanged) {
		_intObject._onValueChanged -= onValueChanged;
	}

	public int Value {
		get { return _intObject == null ? _value : _intObject.Value; }
		set {
			if (_intObject == null) {
				_value = value;
			} else {
				_intObject.Value = value;
			}
		}
	}

	public static implicit operator int(IntReference reference) {
		return reference.Value;
	}
}

[CreateAssetMenu(menuName = "Variables/Int")]
public class IntObject : ScriptableObject {

	public Action<int, int> _onValueChanged;

	[SerializeField] private int _value = 0;
	public int Value {
		get { return _value; }
		set {
			if (_value != value) {
				var delta = -(_value - value);
				_value = value;
				_onValueChanged?.Invoke(_value, delta);
			}
		}
	}

	public static implicit operator int(IntObject intObject) {
		return intObject.Value;
	}

}