using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Gruel.Localization {
	[CreateAssetMenu(menuName = "ScriptableObjects/LocalizationConfig")]
	public class LocalizationConfig : ScriptableObject {
		
#region Public
		public void ParseTranslations() {
			Debug.Log("LocalizationController.ParseTranslations");
		
			_translations = new Dictionary<Tuple<SystemLanguage, string>, TranslationData>();
		
			for (int i = 0, n = _translationFiles.Length; i < n; i++) {
				var localeObj = JObject.Parse(_translationFiles[i].text);
				var locale = (SystemLanguage)Enum.Parse(typeof(SystemLanguage), localeObj["locale"].Value<string>());
				var localeTranslations = localeObj["translations"];
			
				foreach (var j in localeTranslations) {
					var key = j["key"].Value<string>();
					var translation = j["translation"].Value<string>();
					_translations.Add(Tuple.Create(locale, key), new TranslationData(translation));
				}
			}
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
#endregion Public

#region Private
		[Header("Translation files")]
		[SerializeField] private TextAsset[] _translationFiles = new TextAsset[0];
		
		private Dictionary<Tuple<SystemLanguage, string>, TranslationData> _translations;
#endregion Private

	}
}