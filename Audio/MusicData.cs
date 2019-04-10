using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "ScriptableObjects/MusicData")]
public class MusicData : ScriptableObject {

	[Header("Music")]
	public AudioClip _audioClip;
	public float _volume = 1.0f;
	public float _pitch = 1.0f;
	public bool _loop = false;
	public AudioMixerGroup _mixerGroup;

}
