using System;
using System.Collections;
using Gruel.CoroutineSystem;
using Gruel.ObjectPool;
using UnityEngine;
using UnityEngine.UI;

namespace Gruel.Flipbook {
	public class Flipbook : MonoBehaviour, IPoolable {
		
#region Properties
		public bool Playing { get; private set; }
		public Action OnFinishedPlaying;

		public bool PlayOnStart {
			get => _playOnStart;
			set => _playOnStart = value;
		}

		public bool StartAtRandomPlaybackPosition {
			get => _startAtRandomPlaybackPosition;
			set => _startAtRandomPlaybackPosition = value;
		}

		public float Delay {
			get => _delay;
			set => _delay = value;
		}

		public bool ClearLastFrame {
			get => _clearLastFrame;
			set => _clearLastFrame = value;
		}
		
		public Color Tint {
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
#endregion Properties

#region Fields
		[Header("Flipbook")]
		[SerializeField] private FlipbookData _flipbookData;
		[SerializeField] private bool _playOnStart = true;
		[SerializeField] private bool _startAtRandomPlaybackPosition;
		[SerializeField] private float _delay;
		[SerializeField] private bool _clearLastFrame;

		[Header("Renderer")]
		[SerializeField] private SpriteRenderer _spriteRenderer;
		[SerializeField] private Image _image;

		[Header("Poolable")]
		[SerializeField] private bool _poolWhenFinished;

		private ManagedCoroutine _flipbookCor;
		
		private bool _loop = true;
		private int _numberOfFrames;
		private float _frameDelay;
		private float _duration;
		
		private int _hash;
#endregion Fields

#region Public Methods
		public void Pool() {
			if (Playing) {
				Play(false);
			}

			_flipbookData = null;
			_delay = 0.0f;
			_spriteRenderer.color = Color.white;

			OnFinishedPlaying = null;
		
			// Disable our GameObject.
			gameObject.SetActive(false);
			
			// Reset transform.
			transform.rotation = Quaternion.identity;
			transform.localScale = Vector3.one;
		}
	
		public void Unpool() {
			// Listen for when flipbook finishes playing.
			OnFinishedPlaying += PoolableOnFinishedPlaying;
			
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
		
		public void Play(FlipbookData flipbookData) {
			_flipbookData = flipbookData;
			Play(true);
		}
	
		public void Play(bool play) {
			if (_flipbookCor != null) {
				// Stop routine.
				_flipbookCor?.Stop();
			
				Playing = false;

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
				_frameDelay = 1.0f / _flipbookData.FramesPerSecond;
				_numberOfFrames = _flipbookData.Length;
				_duration = _frameDelay * _numberOfFrames;
				_loop = _flipbookData.Loop;
			
				if (_spriteRenderer != null) {
					_spriteRenderer.material = _flipbookData.Material;
				}

				if (_image != null) {
					_image.material = _flipbookData.Material;
				}

				Playing = true;
				_flipbookCor = CoroutineRunner.StartManagedCoroutine(FlipbookCor());
			}
		}
#endregion Public Methods

#region Private Methods
		private void Start() {
			// If _playOnStart is enabled start playing.
			if (_playOnStart) {
				Play(true);
			}
		}

		private void OnDestroy() {
			// Stop playing.
			Play(false);
		}
		
		private void PoolableOnFinishedPlaying() {
			if (_poolWhenFinished) {
				ObjectPool.ObjectPool.Repool(this);
			}
		}
		
		private IEnumerator FlipbookCor() {
			if (_delay > 0.0f) {
				yield return new WaitForSeconds(_delay);
			}
		
			var playTime = _startAtRandomPlaybackPosition ? Time.time + -UnityEngine.Random.Range(0.0f, _duration) : Time.time;
			while (Playing) {
				var timeSinceStart = Time.time - playTime;
				var playbackTime = timeSinceStart % _duration;
				var playbackTimeNormal = Mathf.InverseLerp(0.0f, _duration, playbackTime);
				
				// Check if we should continue playing.
				if (_loop == false
					&& timeSinceStart >= _duration) {
					Playing = false;

					if (_clearLastFrame) {
						if (_spriteRenderer != null) {
							_spriteRenderer.sprite = null;
						}

						if (_image != null) {
							_image.sprite = null;
						}
					}
				
					OnFinishedPlaying?.Invoke();
					break;
				}
				
				var frame = (int)Mathf.Lerp(0.0f, _numberOfFrames, playbackTimeNormal);
			
				if (_spriteRenderer != null) {
					_spriteRenderer.sprite = _flipbookData[frame];
				}

				if (_image != null) {
					_image.sprite = _flipbookData[frame];
				}
				
				yield return null;
			}
		}
#endregion Private Methods

	}
}