using UnityEngine;
using UnityEngine.UI;

namespace Gruel.Console.Obelisk {
	public class ObeliskStackTrace : MonoBehaviour {
		
#region Fields
		[Header("Main")]
		[SerializeField] private GameObject _container;
		[SerializeField] private Image _containerBackgroundImage;
		[SerializeField] private Outline _outline;
		[SerializeField] private Image _resizeHandleBackgroundImage;
		[SerializeField] private Image _resizeHandleImage;

		[Header("Titlebar")]
		[SerializeField] private Image _titlebarBackgroundImage;
		[SerializeField] private Image _titlebarIconImage;
		[SerializeField] private Text _titlebarTitleText;
		[SerializeField] private Button _closeButton;
		[SerializeField] private Image _closeButtonBackgroundImage;
		[SerializeField] private Image _closeButtonIconImage;

		[Header("Scrollbar")]
		[SerializeField] private ScrollRect _scrollRect;
		[SerializeField] private Scrollbar _scrollbar;
		[SerializeField] private Image _scrollbarBackgroundImage;

		[Header("Content")]
		[SerializeField] private Text _stackTraceText;
#endregion Fields

#region Public Methods
		public void Open(ConsoleLog consoleLog) {
			_stackTraceText.text = consoleLog.LogString + "\n\n" + consoleLog.StackTrace;
			SetEnabled(true);
			_scrollRect.normalizedPosition = new Vector2(0, 1);
		}	
#endregion Public Methods

#region Private Methods
		private void Awake() {
			ApplyColorSet();
			_closeButton.onClick.AddListener(OnCloseButtonClicked);
		}
		
		private void SetEnabled(bool enable) {
			_container.SetActive(enable);
		}

		private void OnCloseButtonClicked() {
			SetEnabled(false);
		}
		
		private void ApplyColorSet() {
			// Main.
			_containerBackgroundImage.color = ObeliskConsole.ColorSet.BackgroundColor;
			_outline.effectColor = ObeliskConsole.ColorSet.OutlineColor;
			_resizeHandleBackgroundImage.color = ObeliskConsole.ColorSet.InputContainerBackgroundColor;
			_resizeHandleImage.color = ObeliskConsole.ColorSet.ButtonColor;

			// Titlebar.
			_titlebarBackgroundImage.color = ObeliskConsole.ColorSet.TitlebarBackgroundColor;
			_titlebarIconImage.color = ObeliskConsole.ColorSet.IconColor;
			_titlebarTitleText.color = ObeliskConsole.ColorSet.TitlebarTextColor;
			_closeButtonBackgroundImage.color = ObeliskConsole.ColorSet.ButtonColor;
			_closeButtonIconImage.color = ObeliskConsole.ColorSet.IconColor;

			// Scrollbar.
			_scrollbarBackgroundImage.color = ObeliskConsole.ColorSet.ScrollbarBackgroundColor;

			var scrollbarColorBlock = new ColorBlock();
			scrollbarColorBlock.normalColor = ObeliskConsole.ColorSet.ScrollbarSliderColor;
			scrollbarColorBlock.highlightedColor = ObeliskConsole.ColorSet.ScrollbarSliderHighlightedColor;
			scrollbarColorBlock.pressedColor = ObeliskConsole.ColorSet.ScrollbarSliderPressedColor;
			scrollbarColorBlock.colorMultiplier = 1.0f;
			_scrollbar.colors = scrollbarColorBlock;

			// Content.
			_stackTraceText.color = ObeliskConsole.ColorSet.StackTraceTextColor;
		}
#endregion Private Methods
		
	}
}