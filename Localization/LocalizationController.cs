using System;
using UnityEngine;

namespace Gruel.Localization {
	public class LocalizationController : MonoBehaviour {

#region Properties
		public static Action<SystemLanguage> OnLocaleChanged;
		public static bool Initialized { get; private set; }
		
		public static SystemLanguage Locale {
			get => Instance._locale;
			set {
				Instance._locale = value;
				OnLocaleChanged?.Invoke(Instance._locale);
			}
		}
#endregion Properties
		
#region Fields
		[Header("Config")]
		[SerializeField] private LocalizationConfig _config;
		
		private static LocalizationController Instance { get; set; }
		
		private SystemLanguage _locale = SystemLanguage.English;
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
		
		public static TranslationData GetTranslationData(string key) {
			return Instance._config.GetTranslationData(Tuple.Create(Instance._locale, key));
		}
		
		public static TranslationData GetTranslationData(SystemLanguage locale, string key) {
			return Instance._config.GetTranslationData(Tuple.Create(locale, key));
		}
		
		public static string GetTranslation(string key) {
			return Instance._config.GetTranslationData(Tuple.Create(Instance._locale, key))._translation;
		}
		
		public static string GetTranslation(SystemLanguage locale, string key) {
			return Instance._config.GetTranslationData(Tuple.Create(locale, key))._translation;
		}
#endregion Public Methods
		
#region Private Methods
#endregion Private Methods

	}
}