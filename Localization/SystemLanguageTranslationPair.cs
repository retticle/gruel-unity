using System;
using UnityEngine;

namespace Gruel.Localization {
	[Serializable]
	public class SystemLanguageTranslationPair {

#region Properties
		public SystemLanguage SystemLanguage {
			get => _systemLanguage;
		}

		public TextAsset TranslationFile {
			get => _translationFile;
		}
#endregion Properties

#region Fields
		[SerializeField] private SystemLanguage _systemLanguage;
		[SerializeField] private TextAsset _translationFile;
#endregion Fields

	}
}