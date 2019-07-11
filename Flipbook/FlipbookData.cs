using UnityEngine;

namespace Gruel.Flipbook {
	[CreateAssetMenu(menuName = "Gruel/FlipbookData")]
	public class FlipbookData : ScriptableObject {

#region Properties
		public bool Loop {
			get => _loop;
		}

		public Material Material {
			get => _material;
		}

		public int FramesPerSecond {
			get => _framesPerSecond;
		}
		
		public Sprite this[int index] => _keyFrames[index];

		public int Length => _keyFrames.Length;
#endregion Properties

#region Fields
		[SerializeField] private bool _loop = true;
		[SerializeField] private Material _material;
	
		[SerializeField] private int _framesPerSecond = 30;
		[SerializeField] private Sprite[] _keyFrames = new Sprite[0];
#endregion Fields

	}
}