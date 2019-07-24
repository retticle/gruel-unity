using UnityEngine;
using UnityEngine.Audio;

namespace Gruel.Audio {
	[CreateAssetMenu(menuName = "Gruel/SfxData")]
	public class SfxData : ScriptableObject {

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

		public int MaxSources {
			get => _sourcesMax;
		}

		public AudioMixerGroup AudioMixerGroup {
			get => _mixerGroup;
		}

		public bool VolumeLerpEnabled {
			get => _volumeLerpEnabled;
		}

		public float VolumeEnd {
			get => _volumeEnd;
		}

		public float VolumeLerpTime {
			get => _volumeLerpTime;
		}

		public bool VolumeRndEnabled {
			get => _volumeRndEnabled;
		}

		public float VolumeRndMin {
			get => _volumeRndMin;
		}

		public float VolumeRndMax {
			get => _volumeRndMax;
		}

		public bool PitchLerpEnabled {
			get => _pitchLerpEnabled;
		}

		public float PitchEnd {
			get => _pitchEnd;
		}

		public float PitchLerpTime {
			get => _pitchLerpTime;
		}

		public bool PitchRndEnabled {
			get => _pitchRndEnabled;
		}

		public float PitchRndMin {
			get => _pitchRndMin;
		}

		public float PitchRndMax {
			get => _pitchRndMax;
		}
#endregion Properties

#region Fields
		[Header("SFX")]
		[SerializeField] private AudioClip _audioClip;
		[SerializeField] private float _volume = 1.0f;
		[SerializeField] private float _pitch = 1.0f;
		[SerializeField] private bool _loop;
		[SerializeField] private int _sourcesMax = 8;
		[SerializeField] private AudioMixerGroup _mixerGroup;

		[Header("Volume Lerp")]
		[SerializeField] private bool _volumeLerpEnabled;
		[SerializeField] private float _volumeEnd;
		[SerializeField] private float _volumeLerpTime;

		[Header("Volume Rnd")]
		[SerializeField] private bool _volumeRndEnabled;
		[SerializeField] private float _volumeRndMin;
		[SerializeField] private float _volumeRndMax;

		[Header("Pitch Lerp")]
		[SerializeField] private bool _pitchLerpEnabled;
		[SerializeField] private float _pitchEnd;
		[SerializeField] private float _pitchLerpTime;

		[Header("Pitch Rnd")]
		[SerializeField] private bool _pitchRndEnabled;
		[SerializeField] private float _pitchRndMin;
		[SerializeField] private float _pitchRndMax;
#endregion Fields
		
#region Public Methods
		public static bool IsValid(SfxData sfxData) {
			return (sfxData != null) && (sfxData._audioClip != null);
		}
#endregion Public Methods

	}
}