using System;
using UnityEngine;
using UnityEngine.UI;

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
		// Subscribe to events.
		_config.AddLocaleChangedListener(OnLocaleChanged);
	}

	private void EventsOnDestroy() {
		// Unsubscribe to events.
		_config.RemoveLocaleChangedListener(OnLocaleChanged);
	}
	
	private void OnLocaleChanged() {
		SetText();
	}
#endregion Events
	
#region Text
	[Header("Text Settings")]
	[SerializeField] private ETextType _textType = ETextType.Text;
	[SerializeField] private bool _autoGetComponents = true;
	
	[Header("Text")]
	[SerializeField] private Text _text = null;
	private RectTransform _rect = null;
	
	// [Header("TextMeshPro")]
	// [SerializeField] private TextMeshPro _tmp = null;
	// [SerializeField] private TextMeshProUGUI _tmpUgui = null;

	[Header("Translation Settings")]
	[SerializeField] private LocalizationConfig _config;
	[SerializeField] private string _key;
	[SerializeField] private string[] _stringReplace = new string[0];

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
		// Get translation.
		var translationData = _config.GetTranslation(_key);
		var translation = translationData._translation;

		// Replace parts of translation.
		for (int i = 0, n = _stringReplace.Length; i < n; i++) {
			translation = translation.Replace("{ + i + }", _stringReplace[i]);
		}

		// Set text component's text.
		switch (_textType) {
			case ETextType.Text:
				_text.text = translation;
				
				break;
			case ETextType.TextMeshPro:
				// _tmp.text = translation;
				
				break;
			case ETextType.TextMeshProUgui:
				// _tmpUgui.text = translation;
				
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
