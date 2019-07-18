using System;
using UnityEngine;

namespace Gruel.VariableObjects {
	[Serializable]
	public class IntReference {

#region Properties
		public int Value {
			get => _intObject == null ? _value : _intObject.Value;
			set {
				if (_intObject == null) {
					_value = value;
				} else {
					_intObject.Value = value;
				}
			}
		}
#endregion Properties

#region Fields
		[SerializeField] private int _value = 0;
		[SerializeField] private IntObject _intObject;
#endregion Fields

#region Public Methods
		public void AddListener(Action<int, int> onValueChanged) {
			_intObject.OnValueChanged += onValueChanged;
		}

		public void RemoveListener(Action<int, int> onValueChanged) {
			_intObject.OnValueChanged -= onValueChanged;
		}
		
		public static implicit operator int(IntReference reference) {
			return reference.Value;
		}
#endregion Public Methods

	}

	[CreateAssetMenu(menuName = "Gruel/Variables/Int")]
	public class IntObject : ScriptableObject {

#region Properties
		public Action<int, int> OnValueChanged;
		
		public int Value {
			get => _value;
			set {
				if (_value != value) {
					var delta = -(_value - value);
					_value = value;
					OnValueChanged?.Invoke(_value, delta);
				}
			}
		}
#endregion Properties

#region Fields
		[SerializeField] private int _value;
#endregion Fields

#region Public Methods
		public static implicit operator int(IntObject intObject) {
			return intObject.Value;
		}
#endregion Public Methods

	}
}