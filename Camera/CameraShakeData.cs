using UnityEngine;

namespace Gruel.Camera {
	[CreateAssetMenu(menuName = "Gruel/CameraShakeData")]
	public class CameraShakeData : ScriptableObject {

#region Properties
		public float Strength {
			get => _strength;
		}

		public float DecreaseFactor {
			get => _decreaseFactor;
		}

		public int Points {
			get => _points;
		}

		public float Duration {
			get => _duration;
		}
#endregion Properties

#region Fields
		[SerializeField] private float _strength;
		[SerializeField] private float _decreaseFactor;
		[SerializeField] private int _points;
		[SerializeField] private float _duration;
#endregion Fields

	}
}