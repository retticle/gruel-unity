using System;
using UnityEngine;

namespace Gruel.VariableObjects {
	[Serializable]
	public class StringReference {
		[SerializeField] private string _value = string.Empty;
		[SerializeField] private StringObject _stringObject;

		public void AddListener(Action<string> onValueChanged) {
			_stringObject._onValueChanged += onValueChanged;
		}

		public void RemoveListener(Action<string> onValueChanged) {
			_stringObject._onValueChanged -= onValueChanged;
		}

		public string Value {
			get { return _stringObject == null ? _value : _stringObject.Value; }
			set {
				if (_stringObject == null) {
					_value = value;
				} else {
					_stringObject.Value = value;
				}
			}
		}

		public static implicit operator string(StringReference reference) {
			return reference.Value;
		}
	}

	[CreateAssetMenu(menuName = "Variables/String")]
	public class StringObject : ScriptableObject {

		public Action<string> _onValueChanged;

		[SerializeField] private string _value = string.Empty;
		public string Value {
			get { return _value; }
			set {
				if (_value != value) {
					_value = value;
					_onValueChanged?.Invoke(_value);
				}
			}
		}

		public static implicit operator string(StringObject stringObject) {
			return stringObject.Value;
		}

	}
}