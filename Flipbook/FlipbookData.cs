using UnityEngine;

namespace Gruel.Flipbook {
	public abstract class FlipbookData : ScriptableObject {

#region Properties
		public bool Loop => _loop;
		public int FramesPerSecond => _framesPerSecond;
		public float FrameDelay => 1.0f / _framesPerSecond;
		public float Duration => FrameDelay * Length;
		public abstract int Length { get; }
#endregion Properties

#region Fields
		[SerializeField] protected bool _loop = true;
		[SerializeField] protected int _framesPerSecond = 30;
#endregion Fields

	}
}