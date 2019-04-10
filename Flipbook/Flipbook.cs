using System;
using System.Collections;
using Gruel.ObjectPool;
using UnityEngine;
using UnityEngine.UI;

// [ExecuteInEditMode]
namespace Gruel.Flipbook {
	public class Flipbook : MonoBehaviour, IPoolable {

#region Init
		public void Init(FlipbookData flipbookData, float delay = 0.0f, Color tint = default(Color)) {
			FlipbookInit(flipbookData, delay, tint);
		}
	
		private void Start() {
			FlipbookStart();
		}

		private void OnDestroy() {
			FlipbookOnDestroy();
		}
#endregion Init
	
#region IPoolable
		private int _hash = 0;
	
		public void Pool() {
			FlipbookPool();
		
			gameObject.SetActive(false);
			transform.rotation = Quaternion.identity;
			transform.localScale = Vector3.one;
		}
	
		public void Unpool() {
			gameObject.SetActive(true);
		}

		public int GetHash() {
			return _hash;
		}
	
		public void SetHash(int hash) {
			_hash = hash;
		}

		public void Destroy() {
			if (gameObject != null) {
				Destroy(gameObject);
			}
		}
#endregion IPoolable

#region Flipbook
		[Header("Flipbook")]
		[SerializeField] private FlipbookData _flipbookData;
		[SerializeField] private bool _playOnStart = true;
		public float _delay = 0.0f;
		[SerializeField] private bool _poolWhenFinished = false;

		[Header("Renderer")]
		[SerializeField] private SpriteRenderer _spriteRenderer;
		[SerializeField] private Image _image;

		private Coroutine _flipbookCor = null;
		public bool _playing { get; private set; }
	
		private bool _loop = true;
	
		private int _numberOfFrames = 0;
		private float _frameDelay = 0.0f;
		private float _duration = 0.0f;
		private static readonly int _emissiveMultiplier = Shader.PropertyToID("_emissiveMultiplier");

		public Action _onFinishedPlaying;
	
		public Color _tint {
			get { return _spriteRenderer.color; }
			set { _spriteRenderer.color = value; }
		}

		private void FlipbookInit(FlipbookData flipbookData, float delay, Color tint) {
			this._flipbookData = flipbookData;
			this._delay = delay;
			this._spriteRenderer.color = tint;
		
			Play(true);
		}

		private void FlipbookStart() {
			// If _playOnStart is enabled start playing.
			if (_playOnStart) {
				Play(true);
			}
		}

		private void FlipbookPool() {
			if (_playing) {
				Play(false);
			}

			_flipbookData = null;
			_delay = 0.0f;
			_spriteRenderer.color = Color.white;

			_onFinishedPlaying = null;
		}

		private void FlipbookOnDestroy() {
			// Stop playing.
			Play(false);
		}

		public void Play(FlipbookData flipbookData) {
			this._flipbookData = flipbookData;
			Play(true);
		}
	
		public void Play(bool play) {
			if (_flipbookCor != null) {
				// Stop routine.
				if (RoutineRunner.RoutineRunner._instance != null) {
					RoutineRunner.RoutineRunner.StopRoutine(_flipbookCor);
				}
			
				_playing = false;

				// Remove last frame.
				if (_spriteRenderer != null) {
					_spriteRenderer.sprite = null;
				}

				if (_image != null) {
					_image.sprite = null;
				}
			}
		
			if (play) {
				// Cache data.
				_frameDelay = 1.0f / _flipbookData._framesPerSecond;
				_numberOfFrames = _flipbookData._keyFrames.Length;
				_duration = _frameDelay * _numberOfFrames;
				_loop = _flipbookData._loop;
			
				if (_spriteRenderer != null) {
					_spriteRenderer.material = _flipbookData._material;
				}

				_playing = true;
				_flipbookCor = RoutineRunner.RoutineRunner.StartRoutine(FlipbookCor());
			}
		}

		private IEnumerator FlipbookCor() {
			if (_delay > 0.0f) {
				yield return new WaitForSeconds(_delay);
			}
		
			var playTime = Time.time;
			while (_playing) {
				var timeSinceStart = Time.time - playTime;
				var playbackTime = timeSinceStart % _duration;
				var playbackTimeNormal = Mathf.InverseLerp(0.0f, _duration, playbackTime);
				var frame = (int)Mathf.Lerp(0.0f, _numberOfFrames, playbackTimeNormal);
			
				if (_spriteRenderer != null) {
					_spriteRenderer.sprite = _flipbookData._keyFrames[frame];
				}

				if (_image != null) {
					_image.sprite = _flipbookData._keyFrames[frame];
				}
			
				// Check if we should loop or not.
				if (_loop == false
				    && timeSinceStart >= _duration) {
					_playing = false;

					_spriteRenderer.sprite = null;
				
					_onFinishedPlaying?.Invoke();

					if (_poolWhenFinished) {
						ObjectPool.ObjectPool.Repool(this);
					}
				}
			
				yield return null;
			}
		}
#endregion Flipbook

	}
}