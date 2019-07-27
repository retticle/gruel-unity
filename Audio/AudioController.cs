using System.Collections;
using System.Collections.Generic;
using Gruel.CoroutineUtils;
using UnityEngine;
using UnityEngine.Audio;

namespace Gruel.Audio {
	public class AudioController : MonoBehaviour {

#region Properties
		public bool SfxEnabled {
			get => _sfxEnabled;
			set {
				_sfxEnabled = value;
				SfxEnabledChanged();
			}
		}

		public bool MusicEnabled {
			get => _musicEnabled;
			set {
				_musicEnabled = value;
				MusicEnabledChanged();
			}
		}
		
		public int MaxSfxSources {
			get => _maxSfxSources;
			set => _maxSfxSources = value;
		}
		
		public AudioMixerGroup SfxAudioMixerGroup {
			get => _sfxAudioMixerGroup;
			set => _sfxAudioMixerGroup = value;
		}
		
		public AudioMixerGroup MusicAudioMixerGroup {
			get => _musicAudioMixerGroup;
			set => _musicAudioMixerGroup = value;
		}

		public float MusicFadeDuration {
			get => _musicFadeDuration;
			set => _musicFadeDuration = value;
		}
#endregion Properties

#region Fields
		[SerializeField] private GameObject _audioSourceContainer;
		
		[Header("SFX")]
		[SerializeField] private AudioMixerGroup _sfxAudioMixerGroup;
		[SerializeField] private int _maxSfxSources = 16;
		
		[Header("Music")]
		[SerializeField] private AudioMixerGroup _musicAudioMixerGroup;
		[SerializeField] private float _musicFadeDuration = 0.3f;
		
		private static AudioController _instance;
		
		private bool _sfxEnabled;
		private static AudioSource[] _sfxAudioSources;
		private static ManagedCoroutine[] _sfxRoutines;
		private static Queue<int> _sfxAudioSourceQueue = new Queue<int>();
		private static List<int> _sfxAudioSourceActive = new List<int>();

		private bool _musicEnabled;
		private static AudioSource _musicAudioSource;
		private static ManagedCoroutine _stopMusicCor;
#endregion Fields

#region Public Methods
		public void Init() {
			if (_instance != null) {
				Debug.LogError("AudioController: There is already an instance of AudioController!");
				Destroy(gameObject);
			} else {
				_instance = this;
			}
			
			// Add music audio source.
			_musicAudioSource = _audioSourceContainer.AddComponent<AudioSource>();
			_musicAudioSource.enabled = false;
			
			// Create sfxAudioSources array.
			_sfxAudioSources = new AudioSource[_maxSfxSources];
			_sfxRoutines = new ManagedCoroutine[_maxSfxSources];

			// Instantiate sfx AudioSource components.
			for (int i = 0, n = _sfxAudioSources.Length; i < n; i++) {
				var audioSource = _audioSourceContainer.AddComponent<AudioSource>();
				audioSource.enabled = false;
			
				_sfxAudioSources[i] = audioSource;
				_sfxAudioSourceQueue.Enqueue(i);
			}
		}
		
		public static int PlaySfx(SfxData sfxData, float delay = 0.0f) {
			if (SfxData.IsValid(sfxData) == false) {
				Debug.LogError("AudioController.PlaySFX: sfxData is not valid!");
				return -1;
			}
		
			// Check if there are any audio sources available.
			if (_sfxAudioSourceQueue.Count < 1) {
				Debug.LogWarning("AudioController.PlaySFX: no available audio sources! freeing up the oldest one");
			
				StopSfx(_sfxAudioSourceActive[0]);
			}
		
			// Get audioSourceId, and dequeue it.
			var audioSourceId = _sfxAudioSourceQueue.Dequeue();
		
			// Add audioSourceId to the active list.
			_sfxAudioSourceActive.Add(audioSourceId);

			// Start PlaySFXCor.
			_sfxRoutines[audioSourceId] = CoroutineRunner.StartManagedCoroutine(PlaySfxCor(audioSourceId, sfxData, delay));
		
			return audioSourceId;
		}

		public static void StopSfx(int audioSourceId) {
			// Stop the routine.
			var routine = _sfxRoutines[audioSourceId];
			routine?.Stop();
		
			// Disable AudioSource.
			_sfxAudioSources[audioSourceId].enabled = false;
		
			// Remove from the active queue.
			_sfxAudioSourceActive.Remove(audioSourceId);
		
			// Put the audioSourceId back into the queue.
			_sfxAudioSourceQueue.Enqueue(audioSourceId);
		}

		public static bool IsSfxPlaying(int audioSourceId) {
			return (_sfxRoutines[audioSourceId] != null) && (_sfxRoutines[audioSourceId].IsRunning);
		}
		
		public static void PlayMusic(MusicData musicData) {
			if (musicData == null
			|| musicData.AudioClip == null) {
				Debug.LogError("AudioController.PlayMusic: musicData is not valid!");
				return;
			}
		
			// Check if there is a StopMusicCor running.
			if (_stopMusicCor != null
				&& _stopMusicCor.IsRunning) {
				_stopMusicCor.Stop();
			}

			_musicAudioSource.clip = musicData.AudioClip;
			_musicAudioSource.volume = musicData.Volume;
			_musicAudioSource.pitch = musicData.Pitch;
			_musicAudioSource.loop = musicData.Loop;
			_musicAudioSource.outputAudioMixerGroup = musicData.AudioMixerGroup;

			_musicAudioSource.enabled = true;
			_musicAudioSource.Play();
		}

		public static void StopMusic() {
			_stopMusicCor?.Stop();
			_stopMusicCor = CoroutineRunner.StartManagedCoroutine(StopMusicCor());
		}
#endregion Public Methods

#region Private Methods
		private void SfxEnabledChanged() {
			_sfxAudioMixerGroup.audioMixer.SetFloat("sfxVolume", _sfxEnabled ? 0.0f : -80.0f);
		}

		private void MusicEnabledChanged() {
			_musicAudioMixerGroup.audioMixer.SetFloat("musicVolume", _musicEnabled ? 0.0f : -80.0f);
		}
		
		private static IEnumerator PlaySfxCor(int audioSourceId, SfxData sfxData, float delay) {
			// Get a reference to the AudioSource.
			var audioSource = _sfxAudioSources[audioSourceId];
		
			// Determine volume.
			var volume = sfxData.VolumeRndEnabled ? UnityEngine.Random.Range(sfxData.VolumeRndMin, sfxData.VolumeRndMax) : sfxData.Volume;

			// Determine pitch.
			var pitch = sfxData.PitchRndEnabled ? UnityEngine.Random.Range(sfxData.PitchRndMin, sfxData.PitchRndMax) : sfxData.Pitch;
		
			// Set AudioSource settings.
			audioSource.clip = sfxData.AudioClip;
			audioSource.volume = volume * sfxData.Volume;
			audioSource.pitch = pitch * sfxData.Pitch;
			audioSource.loop = sfxData.Loop;
			audioSource.outputAudioMixerGroup = sfxData.AudioMixerGroup;
		
			// Wait for delay.
			yield return new WaitForSeconds(delay);

			// Enable AudioSource and play.
			audioSource.enabled = true;
			audioSource.Play();

			var time = 0.0f;
			while (audioSource.isPlaying) {
				// Volume lerp.
				if (sfxData.VolumeLerpEnabled) {
					var alpha = Mathf.Clamp(time / sfxData.VolumeLerpTime, 0.0f, 1.0f);
					audioSource.volume = Mathf.Lerp(sfxData.Volume, sfxData.VolumeEnd, alpha);
				}

				// Pitch lerp.
				if (sfxData.PitchLerpEnabled) {
					var alpha = Mathf.Clamp(time / sfxData.PitchLerpTime, 0.0f, 1.0f);
					audioSource.pitch = Mathf.Lerp(sfxData.Pitch, sfxData.PitchEnd, alpha);
				}

				time += Time.deltaTime;
				yield return null;
			}
		
			StopSfx(audioSourceId);
		}
		
		private static IEnumerator StopMusicCor() {
			var startVolume = _musicAudioSource.volume;
		
			var time = 0.0f;
			while (time < _instance._musicFadeDuration) {
				_musicAudioSource.volume = Mathf.Lerp(startVolume, 0.0f, time / _instance._musicFadeDuration);

				time += Time.unscaledDeltaTime;
				yield return null;
			}

			// Make sure the lerp ends up at the right value.
			_musicAudioSource.volume = 0.0f;
		
			_musicAudioSource.Stop();
			_musicAudioSource.enabled = false;
		}
#endregion Private Methods

	}
}