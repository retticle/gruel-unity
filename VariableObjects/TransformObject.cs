using System;
using UnityEngine;

namespace Gruel.VariableObjects {
	[CreateAssetMenu(menuName = "Gruel/Variables/Transform")]
	public class TransformObject : ScriptableObject {
		
#region Properties
		public Action<Transform> OnValueChanged;
		
		public Transform Value {
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
		[SerializeField] private Transform _value;
#endregion Fields

#region Public Methods
		public static implicit operator Transform(TransformObject transformObject) {
			return transformObject.Value;
		}
#endregion Public Methods

	}
}