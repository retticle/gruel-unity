using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Gruel.Audio {
	public class AudioController : MonoBehaviour{

#region Init
		public void Init() {
			MusicInit();
			SFXInit();
		}
#endregion Init
	
#region Core
		[Header("Core")]
		[SerializeField] private GameObject _audioSourceContainer;
#endregion Core
	
#region Music
		[Header("Music")]
		[SerializeField] private AudioMixerGroup _musicAudioMixerGroup;
	
		private static AudioSource _musicAudioSource;

		private const float _musicFadeDuration = 0.3f;
		private static Coroutine _stopMusicCor = null;
		
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
			if (_stopMusicCor != null) {
				RoutineRunner.RoutineRunner.StopRoutine(_stopMusicCor);
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
			if (_stopMusicCor != null) {
				RoutineRunner.RoutineRunner.StopRoutine(_stopMusicCor);
			}

			_stopMusicCor = RoutineRunner.RoutineRunner.StartRoutine(StopMusicCor());
		}

		private static IEnumerator StopMusicCor() {
			var startVolume = _musicAudioSource.volume;
		
			var time = 0.0f;
			while (time < _musicFadeDuration) {
				_musicAudioSource.volume = Mathf.Lerp(startVolume, 0.0f, time / _musicFadeDuration);

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
		private static Coroutine[] _sfxRoutines;
	
		private static Queue<int> _sfxAudioSourceQueue = new Queue<int>();
		private static List<int> _sfxAudioSourceActive = new List<int>();
	
		private const int _sfxSourcesMax = 16;

		private void SFXInit() {
			// Create sfxAudioSources array.
			_sfxAudioSources = new AudioSource[_sfxSourcesMax];
			_sfxRoutines = new Coroutine[_sfxSourcesMax];

			// Instantiate AudioSource components.
			for (int i = 0, n = _sfxAudioSources.Length; i < n; i++) {
				var audioSource = _audioSourceContainer.AddComponent<AudioSource>();
				audioSource.enabled = false;
			
				_sfxAudioSources[i] = audioSource;
				_sfxAudioSourceQueue.Enqueue(i);
			}
		}

		public void SetSFXEnabled(bool sfxEnabled) {
			_sfxAudioMixerGroup.audioMixer.SetFloat("sfxVolume", sfxEnabled ? 0.0f : -80.0f);
		}

		public static int PlaySFX(SFXData sfxData, float delay = 0.0f) {
			if (SFXData.IsValid(sfxData) == false) {
				Debug.LogError("AudioController.PlaySFX: sfxData is not valid!");
				return -1;
			}
		
			// Check if there are any audio sources available.
			if (_sfxAudioSourceQueue.Count < 1) {
				Debug.LogWarning("AudioController.PlaySFX: no available audio sources! freeing up the oldest one");
			
				StopSFX(_sfxAudioSourceActive[0]);
			}
		
			// Get audioSourceId, and dequeue it.
			var audioSourceId = _sfxAudioSourceQueue.Dequeue();
		
			// Add audioSourceId to the active list.
			_sfxAudioSourceActive.Add(audioSourceId);

			// Start PlaySFXCor.
			_sfxRoutines[audioSourceId] = RoutineRunner.RoutineRunner.StartRoutine(PlaySFXCor(audioSourceId, sfxData, delay));
		
			return audioSourceId;
		}

		public static void StopSFX(int audioSourceId) {
			// Stop the routine.
			var routine = _sfxRoutines[audioSourceId];
			if (routine != null) {
				RoutineRunner.RoutineRunner.StopRoutine(routine);
			}
		
			// Disable AudioSource.
			_sfxAudioSources[audioSourceId].enabled = false;
		
			// Remove from the active queue.
			_sfxAudioSourceActive.Remove(audioSourceId);
		
			// Put the audioSourceId back into the queue.
			_sfxAudioSourceQueue.Enqueue(audioSourceId);
		}

		private static IEnumerator PlaySFXCor(int audioSourceId, SFXData sfxData, float delay) {
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
		
			StopSFX(audioSourceId);
		}
#endregion SFX

	}
}
