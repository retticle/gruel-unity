using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Gruel.Localization {
	[CreateAssetMenu(menuName = "Gruel/LocalizationConfig")]
	public class LocalizationConfig : ScriptableObject {
		
#region Properties
		public SystemLanguage FallbackLanguage {
			get => _fallbackLanguage;
			set => _fallbackLanguage = value;
		}
#endregion Properties

#region Fields
		[Header("Settings")]
		[SerializeField] private SystemLanguage _fallbackLanguage = SystemLanguage.English;
		
		[Header("Translation files")]
		[SerializeField] private SystemLanguageTranslationPair[] _translationFiles;

		private List<SystemLanguage> _languages;
		private Dictionary<Tuple<SystemLanguage, string>, TranslationData> _translations;
#endregion Fields

#region Public Methods
		public void ParseTranslations() {
			Debug.Log("LocalizationController.ParseTranslations");
		
			_languages = new List<SystemLanguage>();
			_translations = new Dictionary<Tuple<SystemLanguage, string>, TranslationData>();
		
			for (int i = 0, n = _translationFiles.Length; i < n; i++) {
				var pair = _translationFiles[i];
//				var locale = (SystemLanguage)Enum.Parse(typeof(SystemLanguage), localeObj["locale"].Value<string>());
				var language = pair.SystemLanguage;
				var localeObj = JObject.Parse(pair.TranslationFile.text);
				var localeTranslations = localeObj["translations"];
				
				_languages.Add(language);
			
				foreach (var j in localeTranslations) {
					var key = j["key"].Value<string>();
					var translation = j["translation"].Value<string>();
					_translations.Add(Tuple.Create(language, key), new TranslationData(translation));
				}
			}
		}

		public bool IsLocaleAvailable(SystemLanguage language) {
			if (_translations == null) {
				Debug.LogError("LocalizationConfig.IsLocaleAvailable: translation files have not been parsed!");
				return false;
			}
			
			return _languages.Contains(language);
		}

		public TranslationData GetTranslationData(Tuple<SystemLanguage, string> key) {
			if (_translations == null) {
				Debug.LogError("LocalizationConfig.GetTranslationData: translation files have not been parsed!");
				return new TranslationData("NO TRANSLATION");
			}
			
			try {
				return _translations[key];
			} catch (Exception ex) {
				Debug.LogError($"LocalizationConfig.GetTranslationData: An error occured when trying to get key \"{key}\" for locale \"{key.Item1.ToString()}\"");
				Debug.LogException(ex);
				
				return new TranslationData("NO TRANSLATION");
			}
		}
#endregion Public Methods

#region Private Methods
		
#endregion Private Methods

	}
}