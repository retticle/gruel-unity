using System;
using UnityEngine;

namespace Gruel.Localization {
	[Serializable]
	public class KeyReplacementPair {
		
#region Properties
		public string TargetKey {
			get => _targetKey;
		}

		public string ReplacementKey {
			get => _replacementKey;
		}
#endregion Properties

#region Fields
		[SerializeField] private string _targetKey;
		[SerializeField] private string _replacementKey;
#endregion Fields

	}
}