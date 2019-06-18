using System.Collections;
using System.Collections.Generic;
using Gruel.CoroutineSystem;
using UnityEngine;
using UnityEngine.Audio;

namespace Gruel.Audio {
	public class AudioController : MonoBehaviour {

#region Init
		public void Init() {
			CoreInit();
			MusicInit();
			SfxInit();
		}
#endregion Init
	
#region Core
		[Header("Core")]
		[SerializeField] private GameObject _audioSourceContainer;
		
		private static AudioController Instance { get; set; }

		private void CoreInit() {
			if (Instance != null) {
				Debug.LogError("AudioController: There is already an instance of AudioController!");
				Destroy(gameObject);
			} else {
				Instance = this;
			}
		}
#endregion Core
	
#region Music
		[Header("Music")]
		[SerializeField] private AudioMixerGroup _musicAudioMixerGroup;
		[SerializeField] private float _musicFadeDuration = 0.3f;
	
		private static AudioSource _musicAudioSource;
		private static ManagedCoroutine _stopMusicCor;
		
		private void MusicInit() {
			_musicAudioSource = _audioSourceContainer.AddComponent<AudioSource>();
			_musicAudioSource.enabled = false;
		}

		public void SetMusicEnabled(bool musicEnabled) {
			_musicAudioMixerGroup.audioMixer.SetFloat("musicVolume", musicEnabled ? 0.0f : -80.0f);
		}

		public static void PlayMusic(MusicData musicData) {
			if (musicData == null
			|| musicData._audioClip == null) {
				Debug.LogError("AudioController.PlayMusic: musicData is not valid!");
				return;
			}
		
			// Check if there is a StopMusicCor running.
			if (_stopMusicCor != null
		    && _stopMusicCor.IsRunning) {
				_stopMusicCor.Stop();
			}

			_musicAudioSource.clip = musicData._audioClip;
			_musicAudioSource.volume = musicData._volume;
			_musicAudioSource.pitch = musicData._volume;
			_musicAudioSource.loop = musicData._loop;
			_musicAudioSource.outputAudioMixerGroup = musicData._mixerGroup;

			_musicAudioSource.enabled = true;
			_musicAudioSource.Play();
		}

		public static void StopMusic() {
			_stopMusicCor?.Stop();
			_stopMusicCor = CoroutineRunner.StartManagedCoroutine(StopMusicCor());
		}

		private static IEnumerator StopMusicCor() {
			var startVolume = _musicAudioSource.volume;
		
			var time = 0.0f;
			while (time < Instance._musicFadeDuration) {
				_musicAudioSource.volume = Mathf.Lerp(startVolume, 0.0f, time / Instance._musicFadeDuration);

				time += Time.unscaledDeltaTime;
				yield return null;
			}

			// Make sure the lerp ends up at the right value.
			_musicAudioSource.volume = 0.0f;
		
			_musicAudioSource.Stop();
			_musicAudioSource.enabled = false;
		}
#endregion Music
	
#region SFX
		[Header("SFX")]
		[SerializeField] private AudioMixerGroup _sfxAudioMixerGroup;
	
		private static AudioSource[] _sfxAudioSources;
		private static ManagedCoroutine[] _sfxRoutines;
	
		private static Queue<int> _sfxAudioSourceQueue = new Queue<int>();
		private static List<int> _sfxAudioSourceActive = new List<int>();
	
		private const int _sfxSourcesMax = 16;

		private void SfxInit() {
			// Create sfxAudioSources array.
			_sfxAudioSources = new AudioSource[_sfxSourcesMax];
			_sfxRoutines = new ManagedCoroutine[_sfxSourcesMax];

			// Instantiate AudioSource components.
			for (int i = 0, n = _sfxAudioSources.Length; i < n; i++) {
				var audioSource = _audioSourceContainer.AddComponent<AudioSource>();
				audioSource.enabled = false;
			
				_sfxAudioSources[i] = audioSource;
				_sfxAudioSourceQueue.Enqueue(i);
			}
		}

		public void SetSfxEnabled(bool sfxEnabled) {
			_sfxAudioMixerGroup.audioMixer.SetFloat("sfxVolume", sfxEnabled ? 0.0f : -80.0f);
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

		private static IEnumerator PlaySfxCor(int audioSourceId, SfxData sfxData, float delay) {
			// Get a reference to the AudioSource.
			var audioSource = _sfxAudioSources[audioSourceId];
		
			// Determine volume.
			var volume = sfxData._volumeRndEnabled ? UnityEngine.Random.Range(sfxData._volumeRndMin, sfxData._volumeRndMax) : sfxData._volume;

			// Determine pitch.
			var pitch = sfxData._pitchRndEnabled ? UnityEngine.Random.Range(sfxData._pitchRndMin, sfxData._pitchRndMax) : sfxData._pitch;
		
			// Set AudioSource settings.
			audioSource.clip = sfxData._audioClip;
			audioSource.volume = volume * sfxData._volume;
			audioSource.pitch = pitch * sfxData._pitch;
			audioSource.loop = sfxData._loop;
			audioSource.outputAudioMixerGroup = sfxData._mixerGroup;
		
			// Wait for delay.
			yield return new WaitForSeconds(delay);

			// Enable AudioSource and play.
			audioSource.enabled = true;
			audioSource.Play();

			var time = 0.0f;
			while (audioSource.isPlaying) {
				// Volume lerp.
				if (sfxData._volumeLerpEnabled) {
					var alpha = Mathf.Clamp(time / sfxData._volumeLerpTime, 0.0f, 1.0f);
					audioSource.volume = Mathf.Lerp(sfxData._volume, sfxData._volumeEnd, alpha);
				}

				// Pitch lerp.
				if (sfxData._pitchLerpEnabled) {
					var alpha = Mathf.Clamp(time / sfxData._pitchLerpTime, 0.0f, 1.0f);
					audioSource.pitch = Mathf.Lerp(sfxData._pitch, sfxData._pitchEnd, alpha);
				}

				time += Time.deltaTime;
				yield return null;
			}
		
			StopSfx(audioSourceId);
		}
#endregion SFX

	}
}