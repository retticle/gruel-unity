using UnityEngine;

namespace Gruel.Flipbook {
	[CreateAssetMenu(menuName = "Gruel/SpriteFlipbookData")]
	public class SpriteFlipbookData : FlipbookData {

#region Properties
		public Material Material {
			get => _material;
		}

		public Sprite this[int index] => _keyFrames[index];
		public override int Length => _keyFrames.Length;
#endregion Properties

#region Fields
		[SerializeField] private Material _material;
		[SerializeField] private Sprite[] _keyFrames = new Sprite[0];
#endregion Fields

	}
}