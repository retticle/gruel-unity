using UnityEngine;

namespace Gruel.VariableObjects {
	[CreateAssetMenu(menuName = "Gruel/Variables/AnimationCurve")]
	public class AnimationCurveObject : ScriptableObject {

#region Properties
		public AnimationCurve Value {
			get => _value;
			set => _value = value;
		}
#endregion Properties

#region Fields
		[SerializeField] private AnimationCurve _value;
#endregion Fields

#region Public Methods
		public static implicit operator AnimationCurve(AnimationCurveObject animationCurveObject) {
			return animationCurveObject.Value;
		}
#endregion Public Methods

	}
}