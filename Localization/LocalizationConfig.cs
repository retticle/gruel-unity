using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/LocalizationConfig")]
public class LocalizationConfig : ScriptableObject {

#region Init
	public void Init() {
		ParseTranslations();
	}
#endregion Init
	
#region Locale
	private ELocale _locale = ELocale.English;
	public ELocale Locale {
		get { return _locale; }
		set {
			_locale = value;
			_onLocaleChanged?.Invoke();
		}
	}
#endregion Locale
	
#region LocaleChanged Callback
	private Action _onLocaleChanged;

	public void AddLocaleChangedListener(Action callback) {
		_onLocaleChanged += callback;
	}

	public void RemoveLocaleChangedListener(Action callback) {
		_onLocaleChanged -= callback;
	}
#endregion LocaleChanged Callback
	
#region Translations
		[Header("Translation files")]
	[SerializeField] private TextAsset[] _translationFiles = new TextAsset[0];

	// private Dictionary<ELocale, Dictionary<string, TranslationData>> _translations = new Dictionary<ELocale, Dictionary<string, TranslationData>>();
	private Dictionary<Tuple<ELocale, string>, TranslationData> _translations = null;

	public TranslationData GetTranslation(string key) {
		// Check if the translation files have already been parsed.
		if (_translations == null) {
			ParseTranslations();
		}

		try {
			return _translations[Tuple.Create(_locale, key)];
		} catch (Exception ex) {
			Debug.LogError($"LocalizationManager.GetTranslation: An error occured when trying to get key \"{key}\" for locale \"{_locale.ToString()}\"");
			Debug.LogException(ex);

			return new TranslationData("NO TRANSLATION");
		}
	}

	private void ParseTranslations() {
		Debug.Log("LocalizationConfig.ParseTranslations");
		
		_translations = new Dictionary<Tuple<ELocale, string>, TranslationData>();
		
		for (int i = 0, n = _translationFiles.Length; i < n; i++) {
			var localeObj = JObject.Parse(_translationFiles[i].text);
			var locale = (ELocale)Enum.Parse(typeof(ELocale), localeObj["locale"].Value<string>());
			var localeTranslations = localeObj["translations"];
			
			foreach (var j in localeTranslations) {
				var key = j["key"].Value<string>();
				var translation = j["translation"].Value<string>();
				_translations.Add(Tuple.Create(locale, key), new TranslationData(translation));
			}
		}
	}
#endregion Translations

}
