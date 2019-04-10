using UnityEngine;

namespace Gruel.Camera {
	[CreateAssetMenu(menuName = "ScriptableObjects/CameraShakeData")]
	public class CameraShakeData : ScriptableObject {

		public float _strength;
		public float _decreaseFactor;
		public int _points;
		public float _duration;

	}
}