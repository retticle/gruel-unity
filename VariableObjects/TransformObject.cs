using System;
using UnityEngine;

namespace Gruel.VariableObjects {
	[CreateAssetMenu(menuName = "Variables/Transform")]
	public class TransformObject : ScriptableObject {

		public Action<Transform> _onValueChanged;

		[SerializeField] private Transform _value = null;

		public Transform Value {
			get { return _value; }
			set {
				if (_value != value) {
					_value = value;
					_onValueChanged?.Invoke(_value);
				}
			}
		}

		public static implicit operator Transform(TransformObject transformObject) {
			return transformObject.Value;
		}

	}
}
