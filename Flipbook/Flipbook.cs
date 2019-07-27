using System;
using System.Collections;
using Gruel.CoroutineUtils;
using Gruel.ObjectPool;
using UnityEngine;

namespace Gruel.Flipbook {
	public abstract class Flipbook : MonoBehaviour, IPoolable {

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

		private int Frame {
			get => _frame;
			set {
				_frame = value;
				FrameChanged();
			}
		}
		
		public int Hash { get; set; }
#endregion Properties
		
#region Fields
		[Header("Playback Settings")]
		[SerializeField] protected bool _playOnStart = true;
		[SerializeField] protected bool _startAtRandomPlaybackPosition;
		[SerializeField] protected float _delay;
		[SerializeField] protected bool _clearLastFrame;
		
		[Header("Pool Settings")]
		[SerializeField] protected bool _poolWhenFinished;
		[SerializeField] protected bool _clearFlipbookDataOnPool;
		[SerializeField] protected bool _clearDelayOnPool;
		
		protected FlipbookData _flipbookBaseData;
		protected int _frame;
		private ManagedCoroutine _flipbookCor;
#endregion Fields
		
#region Public Methods
		public virtual void Pool() {
			if (Playing) {
				Play(false);
			}

			if (_clearDelayOnPool) {
				_delay = 0.0f;
			}
			
			OnFinishedPlaying = null;
			gameObject.SetActive(false);
			var transform1 = transform;
			transform1.rotation = Quaternion.identity;
			transform1.localScale = Vector3.one;
		}
	
		public void Unpool() {
			gameObject.SetActive(true);
		}

		public void Destroy() {
			if (gameObject != null) {
				GameObject.Destroy(gameObject);
			}
		}

		public virtual void Play(bool play) {
			Playing = false;
			_flipbookCor?.Stop();

			if (play) {
				Playing = true;
				_flipbookCor = CoroutineRunner.StartManagedCoroutine(FlipbookCor());
			} else {
				ClearFrame();
			}
		}
#endregion Public Methods
		
#region Protected Methods
		protected virtual void Start() {
			if (_playOnStart) {
				Play(true);
			}
		}

		protected virtual void OnDestroy() {
			Play(false);
		}

		protected virtual void FinishedPlaying() {
			
			OnFinishedPlaying?.Invoke();
			
			if (_poolWhenFinished) {
				ObjectPool.ObjectPool.Repool(this);
			}
		}

		protected abstract void FrameChanged();
		protected abstract void ClearFrame();
#endregion Protected Methods
		
#region Private Methods
		private IEnumerator FlipbookCor() {
			if (_delay > 0.0f) {
				yield return new WaitForSeconds(_delay);
			}

			var loop = _flipbookBaseData.Loop;
			var duration = _flipbookBaseData.Duration;
			var numberOfFrames = _flipbookBaseData.Length;
			var playTime = _startAtRandomPlaybackPosition
				? Time.time + -UnityEngine.Random.Range(0.0f, duration)
				: Time.time;

			while (Playing) {
				var timeSinceStart = Time.time - playTime;
				var playbackTime = timeSinceStart % duration;
				var playbackTimeNormal = Mathf.InverseLerp(0.0f, duration, playbackTime);

				if (loop == false
				&& timeSinceStart >= duration) {
					Playing = false;

					if (_clearLastFrame) {
						ClearFrame();
					}
					
					FinishedPlaying();
					break;
				}

				Frame = (int) Mathf.Lerp(0.0f, numberOfFrames, playbackTimeNormal);
				yield return null;
			}
		}
#endregion Private Methods

	}
}