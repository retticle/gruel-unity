using System;
using UnityEngine;

namespace Gruel.VariableObjects {
	[Serializable]
	public class StringReference {

#region Properties
		public string Value {
			get => _stringObject == null ? _value : _stringObject.Value;
			set {
				if (_stringObject == null) {
					_value = value;
				} else {
					_stringObject.Value = value;
				}
			}
		}
#endregion Properties

#region Fields
		[SerializeField] private string _value = string.Empty;
		[SerializeField] private StringObject _stringObject;
#endregion Fields

#region Public Methods
		public void AddListener(Action<string> onValueChanged) {
			_stringObject.OnValueChanged += onValueChanged;
		}

		public void RemoveListener(Action<string> onValueChanged) {
			_stringObject.OnValueChanged -= onValueChanged;
		}
		
		public static implicit operator string(StringReference reference) {
			return reference.Value;
		}
#endregion Public Methods

#region Private Methods
#endregion Private Methods
		
	}

	[CreateAssetMenu(menuName = "Gruel/Variables/String")]
	public class StringObject : ScriptableObject {
		
#region Properties
		public Action<string> OnValueChanged;
		
		public string Value {
			get => _value;
			set {
				if (_value != value) {
					_value = value;
					OnValueChanged?.Invoke(_value);
				}
			}
		}
#endregion Properties

#region Fields
		[SerializeField] private string _value = string.Empty;
#endregion Fields

#region Public Methods
		public static implicit operator string(StringObject stringObject) {
			return stringObject.Value;
		}
#endregion Public Methods

	}
}