using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "ScriptableObjects/SFXData")]
public class SFXData : ScriptableObject {

	[Header("SFX")]
	public AudioClip _audioClip;
	public float _volume = 1.0f;
	public float _pitch = 1.0f;
	public bool _loop = false;
	public int _sourcesMax = 8;
	public AudioMixerGroup _mixerGroup;

	[Header("Volume Lerp")]
	public bool _volumeLerpEnabled = false;
	public float _volumeEnd = 0.0f;
	public float _volumeLerpTime = 0.0f;

	[Header("Volume Rnd")]
	public bool _volumeRndEnabled = false;
	public float _volumeRndMin = 0.0f;
	public float _volumeRndMax = 0.0f;

	[Header("Pitch Lerp")]
	public bool _pitchLerpEnabled = false;
	public float _pitchEnd = 0.0f;
	public float _pitchLerpTime = 0.0f;

	[Header("Pitch Rnd")]
	public bool _pitchRndEnabled = false;
	public float _pitchRndMin = 0.0f;
	public float _pitchRndMax = 0.0f;

	public static bool IsValid(SFXData sfxData) {
		return (sfxData != null) && (sfxData._audioClip != null);
	}

}
