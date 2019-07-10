using System;
using UnityEngine;

namespace Gruel.Localization {
	public class LocalizationController : MonoBehaviour {

#region Properties
		public static Action<SystemLanguage> OnLanguageChanged;
		public static bool Initialized { get; private set; }
		
		public static SystemLanguage Language {
			get => Instance._language;
			set {
				Instance._language = value;
				OnLanguageChanged?.Invoke(Instance._language);
			}
		}

		public static SystemLanguage FallbackLanguage {
			get => Instance._config.FallbackLanguage;
			set => Instance._config.FallbackLanguage = value;
		}
#endregion Properties
		
#region Fields
		[Header("Config")]
		[SerializeField] private LocalizationConfig _config;
		
		private static LocalizationController Instance { get; set; }
		
		private SystemLanguage _language = SystemLanguage.English;
#endregion Fields
		
#region Public Methods
		public void Init() {
			if (Instance != null) {
				Debug.LogError("LocalizationController: There is already an instance of LocalizationController!");
				Destroy(gameObject);
				return;
			}

			Instance = this;
			_config.ParseTranslations();
			Initialized = true;
		}

		public static bool IsLocaleAvailable(SystemLanguage language) {
			return Instance._config.IsLocaleAvailable(language);
		}
		
		public static TranslationData GetTranslationData(string key) {
			return Instance._config.GetTranslationData(Tuple.Create(Instance._language, key));
		}
		
		public static TranslationData GetTranslationData(SystemLanguage locale, string key) {
			return Instance._config.GetTranslationData(Tuple.Create(locale, key));
		}
		
		public static string GetTranslation(string key) {
			return Instance._config.GetTranslationData(Tuple.Create(Instance._language, key)).Translation;
		}
		
		public static string GetTranslation(SystemLanguage locale, string key) {
			return Instance._config.GetTranslationData(Tuple.Create(locale, key)).Translation;
		}
#endregion Public Methods
		
#region Private Methods
#endregion Private Methods

	}
}