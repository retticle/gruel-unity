using UnityEngine;

namespace Gruel.VariableObjects {
	[CreateAssetMenu(menuName = "Variables/AnimationCurve")]
	public class AnimationCurveObject : ScriptableObject {

		[SerializeField] private AnimationCurve _value = null;

		public AnimationCurve Value {
			get { return _value; }
			set { _value = value; }
		}

		public static implicit operator AnimationCurve(AnimationCurveObject animationCurveObject) {
			return animationCurveObject.Value;
		}

	}
}
