using UnityEngine;

namespace Gruel.Flipbook {
	public class SpriteFlipbook : Flipbook {
		
#region Properties
		public Color Tint {
			get => _spriteRenderer.color;
			set => _spriteRenderer.color = value;
		}
#endregion Properties

#region Fields
		[Header("Sprite Flipbook Player Settings")]
		[SerializeField] private SpriteFlipbookData _flipbookData;
		
		[Header("Renderer")]
		[SerializeField] private SpriteRenderer _spriteRenderer;
		[SerializeField] private bool _clearTintOnPool;
#endregion Fields

#region Public Methods
		public override void Pool() {
			base.Pool();

			if (_clearFlipbookDataOnPool) {
				_flipbookBaseData = null;
				_flipbookData = null;
			}
			
			if (_clearTintOnPool) {
				_spriteRenderer.color = Color.white;
			}
		}

		public void Play(SpriteFlipbookData flipbookData) {
			_flipbookData = flipbookData;
			
			Play(true);
		}

		public override void Play(bool play) {
			if (play) {
				_flipbookBaseData = _flipbookData;
				_spriteRenderer.material = _flipbookData.Material;
			}
			
			base.Play(play);
		}
#endregion Public Methods
		
#region Protected Methods
		protected override void FinishedPlaying() {
			
			base.FinishedPlaying();
			
			if (_clearLastFrame) {
				_spriteRenderer.sprite = null;
			}
		}

		protected override void FrameChanged() {
			_spriteRenderer.sprite = _flipbookData[_frame];
		}

		protected override void ClearFrame() {
			_spriteRenderer.sprite = null;
		}
#endregion Protected Methods

#region Private Methods

#endregion Private Methods

	}
}