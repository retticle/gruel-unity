using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gruel.Actor.ActorMotor2D {
	public class ActorMotor2D : MonoBehaviour, IActorTrait {
	
#region ActorTrait
		private Actor _actor;
	
		public void InitializeTrait(Actor actor) {
			this._actor = actor;
		
			TraitsInit();
			PhysicsSettingsInit();
			DetectionInit();
			SimulationInit();
		}

		public void RemoveTrait() {}
#endregion ActorTrait
		
#region Traits
		[Header("Traits")]
		[SerializeField] private Component[] _traitComponents = new Component[0];

		private List<IActorMotor2DTrait> _traits = null;

		private void TraitsInit() {
			_traits = new List<IActorMotor2DTrait>();

			for (int i = 0, n = _traitComponents.Length; i < n; i++) {
				var trait = (IActorMotor2DTrait)_traitComponents[i];
				_traits.Add(trait);
				trait.InitializeTrait(_actor, this);
			}
		}

		public void AddTrait(IActorMotor2DTrait trait) {
			_traits.Add(trait);
			trait.InitializeTrait(_actor, this);
		}

		public void RemoveTrait(IActorMotor2DTrait trait) {
			_traits.Remove(trait);
			trait.RemoveTrait();
		}

		public T GetTrait<T>() where T : class, IActorMotor2DTrait {
			return _traits.OfType<T>().Select(trait => trait).FirstOrDefault();
		}

		public bool TryGetTrait<T>(out T outTrait) where T : class, IActorMotor2DTrait {
			var type = typeof(T);
			outTrait = null;

			foreach (var trait in _traits) {
				if (trait.GetType() == type) {
					outTrait = (T)trait;
					return true;
				}
			}

			return false;
		}
#endregion Traits
	
#region Character Controller
		[Header("Character Controller")]
		public CharacterController _cc;
#endregion Character Controller
	
#region Physics Settings
		[Header("Settings")]
		public bool _calculateGravityAndJumpForce = true;
		public float _velocityMax = 100.0f;
		public float _airControlScalar = 1.0f;
		public float _gravity = -9.8f;

		public float _dragGrounded = 0.25f;
		public float _dragAirborne = 0.25f;
	
		[Header("Walk")]
		public float _walkSpeed = 1.25f;
	
		[Header("Jump")]
		public float _jumpHeightMax = 0.64f;
		public float _jumpApexTime = 0.35f;
		public float _jumpForce = 1.0f;

		private void PhysicsSettingsInit() {
			if (_calculateGravityAndJumpForce) {
				_gravity = -(2.0f * _jumpHeightMax) / Mathf.Pow(_jumpApexTime, 2.0f);
				_jumpForce = Mathf.Sqrt(2.0f * Mathf.Abs(_gravity) * _jumpHeightMax);
			}
		
			Debug.Log($"ActorMotor.PhysicsSettingsInit: gravity: {_gravity} | jumpForce: {_jumpForce}");
		}
#endregion Physics Settings
	
#region Simulation
		[Header("Simulation")]
		[SerializeField] private bool _isKinematic = false;
		[SerializeField] private float _gravityScalar = 1.0f;
	
		public TickFrame _tickFrame { get; private set; }
	
		public bool IsKinematic {
			get { return _isKinematic; }
			set {
				_isKinematic = value;
			
				if (_isKinematic) {
					_tickFrame._velocityMovement = Vector3.zero;
					_tickFrame._velocityCarried = Vector3.zero;
					_tickFrame._velocity = Vector3.zero;
				}
			}
		}
		
		public float GravityScalar {
			get { return _gravityScalar; }
			set { _gravityScalar = value; }
		}

		/// <summary>
		/// Invoked when the actor starts falling.
		/// </summary>
		public Action _onStartedFalling;
	
		/// <summary>
		/// Invoked when the actor lands.
		/// </summary>
		public Action _onLanded;
	
		private void SimulationInit() {
			_tickFrame = new TickFrame();
		}

		public void SetVelocity(Vector3 velocity) {
			_tickFrame._velocityCarried = velocity;
		}

		public void AddForce(Vector3 force) {
			_tickFrame._velocityCarried += force;
		}

		public void Tick(float horizontal, bool jumpButtonDown, bool crouch) {
			var customDeltaTime = Time.fixedDeltaTime * _actor._customTimeDilation;
			
			// Add input to tick frame.
			_tickFrame._inputHorizontal = horizontal;
			_tickFrame._inputJumpButtonDown = jumpButtonDown;
			_tickFrame._inputCrouch = crouch;
			
			// Cache previous results.
			var wasGrounded = _tickFrame._isGrounded;
		
			// Reset results.
			_tickFrame.Reset();
			
			// Detection.
			UpdateRaycastOrigins();
			HorizontalDetection();
			VerticalDetection();
		
			// Grounded.
			_tickFrame._isGrounded = _cc.isGrounded;
			
			// Traits.
			for (int i = 0, n = _traits.Count; i < n; i++) {
				_traits[i].Tick(_tickFrame);
			}

			// OnLanded event.
			if (wasGrounded == false
			    && _tickFrame._isGrounded) {
				_onLanded?.Invoke();
			}
		
			// OnStartedFalling event.
			if (wasGrounded
			    && _tickFrame._isGrounded == false) {
				_onStartedFalling?.Invoke();
			}
		
			// Set isWalking.
			if (Mathf.Abs(_tickFrame._inputHorizontal) > 0.0f) {
				_tickFrame._isWalking = true;
			}

			if (_isKinematic == false) {
				// Apply drag to the carried velocity.
				_tickFrame._velocityCarried = Vector3.Lerp(
					_tickFrame._velocityCarried,
					Vector3.zero,
					(_tickFrame._isGrounded ? _dragGrounded : _dragAirborne) * customDeltaTime
				);
				
				// Gravity.
				if (_tickFrame._isGrounded
				&& _tickFrame._velocityCarried.y < 0.0f) {
					_tickFrame._velocityCarried.y = (_gravity * _gravityScalar) * Time.fixedDeltaTime;
				} else {
					_tickFrame._velocityCarried.y += (_gravity * _gravityScalar) * customDeltaTime;
				}
				
				// Jump.
				if (_tickFrame._inputJumpButtonDown
			    && _tickFrame._isGrounded) {
					_tickFrame._isJumping = true;
					_tickFrame._velocityCarried.y = _jumpForce;
				}
			
				// Calculate movement velocity contribution.
				var walkSpeed = _tickFrame._inputHorizontal * _walkSpeed;
				_tickFrame._velocityMovement.x = _tickFrame._isGrounded ? walkSpeed : walkSpeed * _airControlScalar;
		
				// Calculate final velocity.
				_tickFrame._velocity = _tickFrame._velocityCarried + _tickFrame._velocityMovement;
		
				// Clamp velocity.
				_tickFrame._velocity = Vector3.ClampMagnitude(_tickFrame._velocity, _velocityMax);
		
				// Apply velocity.
				_cc.Move(_tickFrame._velocity * customDeltaTime);	
			}
		}
#endregion Simulation
		
#region Detection
		[Header("Detection")]
		[SerializeField] private LayerMask _collisionMask;
		
		private const int _horizontalRayCount = 4;
		private const int _verticalRayCount = 4;
		private const float _rayLength = 5.0f;
		
		private Vector3 _originTopLeft;
		private Vector3 _originTopRight;
		private Vector3 _originBottomLeft;
		private Vector3 _originBottomRight;
		
		private float _raySpacingHorizontal;
		private float _raySpacingVertical;

		[NonSerialized] public float[] _distancesLeft = null;
		[NonSerialized] public float[] _distancesRight = null;
		[NonSerialized] public float[] _distancesUp = null;
		[NonSerialized] public float[] _distancesDown = null;
		
		private void DetectionInit() {
			_distancesLeft = new float[_horizontalRayCount];
			_distancesRight = new float[_horizontalRayCount];
			_distancesUp = new float[_verticalRayCount];
			_distancesDown = new float[_verticalRayCount];
			
			UpdateRaySpacing();
		}
		
		public void UpdateRaycastOrigins() {
			var bounds = _cc.bounds;
			_originBottomLeft = new Vector3(bounds.min.x, bounds.min.y, 0.0f);
			_originBottomRight = new Vector3(bounds.max.x, bounds.min.y, 0.0f	);
			_originTopLeft = new Vector3(bounds.min.x, bounds.max.y, 0.0f);
			_originTopRight = new Vector3(bounds.max.x, bounds.max.y, 0.0f	);
		}

		private void UpdateRaySpacing() {
			var bounds = _cc.bounds;
			_raySpacingHorizontal = bounds.size.y / (_horizontalRayCount - 1);
			_raySpacingVertical = bounds.size.x / (_verticalRayCount - 1);
		}

		private void HorizontalDetection() {
			var rayLength = _rayLength;

			// Left.
			for (int i = 0; i < _horizontalRayCount; i++) {
				var rayOrigin = _originBottomLeft;
				rayOrigin += Vector3.up * (_raySpacingHorizontal * i);

				RaycastHit hitInfo;
				bool hit = Physics.Raycast(rayOrigin, Vector3.left, out hitInfo, rayLength, _collisionMask);
				Debug.DrawRay(rayOrigin, Vector3.left * rayLength, Color.magenta);

				_distancesLeft[i] = hit ? hitInfo.distance : -1.0f;
			}
			
			// Right.
			for (int i = 0; i < _horizontalRayCount; i++) {
				var rayOrigin = _originBottomRight;
				rayOrigin += Vector3.up * (_raySpacingHorizontal * i);

				RaycastHit hitInfo;
				bool hit = Physics.Raycast(rayOrigin, Vector3.right, out hitInfo, rayLength, _collisionMask);
				Debug.DrawRay(rayOrigin, Vector3.right * rayLength, Color.magenta);

				_distancesRight[i] = hit ? hitInfo.distance : -1.0f;
			}
		}

		private void VerticalDetection() {
			var rayLength = _rayLength;
			
			// Up.
			for (int i = 0; i < _verticalRayCount; i++) {
				var rayOrigin = _originTopLeft;
				rayOrigin += Vector3.right * (_raySpacingVertical * i);

				RaycastHit hitInfo;
				var hit = Physics.Raycast(rayOrigin, Vector3.up, out hitInfo, rayLength, _collisionMask);
				Debug.DrawRay(rayOrigin, Vector3.up * rayLength, Color.magenta);

				_distancesUp[i] = hit ? hitInfo.distance : -1.0f;
			}
			
			// Down.
			for (int i = 0; i < _verticalRayCount; i++) {
				var rayOrigin = _originBottomLeft;
				rayOrigin += Vector3.right * (_raySpacingVertical * i);

				RaycastHit hitInfo;
				var hit = Physics.Raycast(rayOrigin, Vector3.down, out hitInfo, rayLength, _collisionMask);
				Debug.DrawRay(rayOrigin, Vector3.down * rayLength, Color.magenta);

				_distancesDown[i] = hit ? hitInfo.distance : -1.0f;
			}
		}
#endregion Detection

	}
}