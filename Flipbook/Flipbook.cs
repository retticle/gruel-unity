using System;
using System.Collections;
using Gruel.CoroutineSystem;
using Gruel.ObjectPool;
using UnityEngine;
using UnityEngine.UI;

namespace Gruel.Flipbook {
	// [ExecuteInEditMode]
	public class Flipbook : MonoBehaviour, IPoolable {

#region Init
		private void Start() {
			FlipbookStart();
		}

		private void OnDestroy() {
			FlipbookOnDestroy();
		}
#endregion Init
	
#region Poolable
		[Header("Poolable")]
		[SerializeField] private bool _poolWhenFinished = false;
		
		private int _hash = 0;
	
		public void Pool() {
			// Reset flipbook.
			FlipbookPool();
		
			// Disable our GameObject.
			gameObject.SetActive(false);
			
			// Reset transform.
			transform.rotation = Quaternion.identity;
			transform.localScale = Vector3.one;
		}
	
		public void Unpool() {
			// Listen for when flipbook finishes playing.
			_onFinishedPlaying += PoolableOnFinishedPlaying;
			
			// Enable our GameObject.
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
				GameObject.Destroy(gameObject);
			}
		}
		
		private void PoolableOnFinishedPlaying() {
			if (_poolWhenFinished) {
				ObjectPool.ObjectPool.Repool(this);
			}
		}
#endregion Poolable

#region Flipbook
		[Header("Flipbook")]
		[SerializeField] private FlipbookData _flipbookData;
		[SerializeField] private bool _playOnStart = true;
		public bool _startAtRandomPlaybackPosition = false;
		public float _delay = 0.0f;

		[Header("Renderer")]
		[SerializeField] private SpriteRenderer _spriteRenderer;
		[SerializeField] private Image _image;

		private ManagedCoroutine _flipbookCor = null;
		public bool _playing { get; private set; }
	
		private bool _loop = true;
		private int _numberOfFrames = 0;
		private float _frameDelay = 0.0f;
		private float _duration = 0.0f;

		public Action _onFinishedPlaying;
	
		public Color _tint {
			get {
				if (_spriteRenderer != null) {
					return _spriteRenderer.color;
				}

				if (_image != null) {
					return _image.color;
				}

				throw new System.InvalidOperationException("Flipbook doesn't have a renderer set!");
			}

			set {
				if (_spriteRenderer != null) {
					_spriteRenderer.color = value;
					return;
				}

				if (_image != null) {
					_image.color = value;
					return;
				}
			}
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
				_flipbookCor?.Stop();
			
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
				_flipbookCor = CoroutineRunner.StartManagedCoroutine(FlipbookCor());
			}
		}

		private IEnumerator FlipbookCor() {
			if (_delay > 0.0f) {
				yield return new WaitForSeconds(_delay);
			}
		
			var playTime = _startAtRandomPlaybackPosition ? Time.time + -UnityEngine.Random.Range(0.0f, _duration) : Time.time;
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

					if (_spriteRenderer != null) {
						_spriteRenderer.sprite = null;
					}

					if (_image != null) {
						_image.sprite = null;
					}
				
					_onFinishedPlaying?.Invoke();
				}
			
				yield return null;
			}
		}
#endregion Flipbook

	}
}