using System;
using UnityEngine;
using UnityEngine.UI;

namespace Gruel.Localization {
	public class LocalizedText : MonoBehaviour {
	
#region Init
		private void Awake() {
			TextAwake();
			EventsAwake();
		
			SetText();
		}

		private void Start() {
			TextStart();
		}

		private void OnDestroy() {
			EventsOnDestroy();
		}
#endregion Init
	
#region Events
		private void EventsAwake() {
			LocalizationController.OnLocaleChanged += LocaleChanged;
		}

		private void EventsOnDestroy() {
			LocalizationController.OnLocaleChanged -= LocaleChanged;
		}
	
		private void LocaleChanged(SystemLanguage locale) {
			SetText();
		}
#endregion Events
	
#region Text
		[Header("Text Settings")]
		[SerializeField] private ETextType _textType = ETextType.Text;
		[SerializeField] private bool _autoGetComponents = true;
	
		[Header("Text")]
		[SerializeField] private Text _text;
		private RectTransform _rect;
	
		// [Header("TextMeshPro")]
		// [SerializeField] private TextMeshPro _tmp = null;
		// [SerializeField] private TextMeshProUGUI _tmpUgui = null;

		[Header("Translation Settings")]
		[SerializeField] private bool _replaceTextUsingKey = true;
		[SerializeField] private string _key;
		[SerializeField] private string[] _stringReplace = new string[0];
		[SerializeField] private KeyReplacementPair[] _keyReplacements;

		private void TextAwake() {
			// Get text components.
			if (_autoGetComponents) {
				GetTextComponent();
			}
		}

		private void TextStart() {
			// If there's a rect transform rebuild the layout.
			if (_rect != null) {
				LayoutRebuilder.ForceRebuildLayoutImmediate(_rect);
			}
		}
	
		private void SetText() {
			var text = _replaceTextUsingKey ? LocalizationController.GetTranslation(_key) : _text.text;
			
			// Replace parts of translation.
			for (int i = 0, n = _stringReplace.Length; i < n; i++) {
				text = text.Replace("{ + i + }", _stringReplace[i]);
			}
			
			// Replace target keys with replacement keys.
			for (int i = 0, n = _keyReplacements.Length; i < n; i++) {
				var pair = _keyReplacements[i];
				var replacementTranslation = LocalizationController.GetTranslation(pair._replacementKey);
				text = text.Replace(
					"{" + pair._targetKey + "}", 
					replacementTranslation
				);
			}
			
			// Set text component's text.
			switch (_textType) {
				case ETextType.Text:
					_text.text = text;
					break;
				case ETextType.TextMeshPro:
					// _tmp.text = text;
					break;
				case ETextType.TextMeshProUgui:
					// _tmpUgui.text = text;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		
			// If there's a rect transform rebuild the layout.
			if (_rect != null) {
				LayoutRebuilder.ForceRebuildLayoutImmediate(_rect);
			}
		}

		private void GetTextComponent() {
			switch (_textType) {
				case ETextType.Text:
					_text = GetComponent<Text>();
					_rect = _text.gameObject.GetComponent<RectTransform>();

					break;
				case ETextType.TextMeshPro:
					// _tmp = GetComponent<TextMeshPro>();
				
					break;
				case ETextType.TextMeshProUgui:
					// _tmpUgui = GetComponent<TextMeshProUGUI>();
					// _rect = _tmpUgui.gameObject.GetComponent<RectTransform>();
				
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
#endregion Text

	}
}