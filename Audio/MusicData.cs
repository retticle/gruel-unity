using UnityEngine;
using UnityEngine.Audio;

namespace Gruel.Audio {
	[CreateAssetMenu(menuName = "Gruel/MusicData")]
	public class MusicData : ScriptableObject {

#region Properties
		public AudioClip AudioClip {
			get => _audioClip;
		}

		public float Volume {
			get => _volume;
		}

		public float Pitch {
			get => _pitch;
		}

		public bool Loop {
			get => _loop;
		}

		public AudioMixerGroup AudioMixerGroup {
			get => _mixerGroup;
		}
#endregion Properties

#region Fields
		[Header("Settings")]
		[SerializeField] private AudioClip _audioClip;
		[SerializeField] private float _volume = 1.0f;
		[SerializeField] private float _pitch = 1.0f;
		[SerializeField] private bool _loop;
		[SerializeField] private AudioMixerGroup _mixerGroup;
#endregion Fields

	}
}