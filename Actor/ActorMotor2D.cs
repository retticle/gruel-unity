using System;
using Gruel.Actor;
using UnityEngine;

public class ActorMotor2D : MonoBehaviour, IActorTrait {
	
#region ActorTrait
	private Actor _actor;
	
	public void InitializeTrait(Actor actor) {
		this._actor = actor;
		
		CharacterControllerInit();
		PhysicsSettingsInit();
		SimulationInit();
	}

	public void RemoveTrait() {}
#endregion ActorTrait
	
#region Character Controller
	[Header("Character Controller")]
	[SerializeField] private CharacterController _cc;
	
	private float _ccCenterCache = 0.0f;
	private float _ccHeightCache = 0.0f;

	private void CharacterControllerInit() {
		// Cache capsule settings.
		_ccCenterCache = _cc.center.y;
		_ccHeightCache = _cc.height;
	}
#endregion Character Controller
	
#region Physics Settings
	[Header("Settings")]
	public bool _calculateGravityAndJumpForce = true;
	public float _velocityMax = 100.0f;
	public float _airControlScalar = 1.0f;
	public float _gravity = -9.8f;
	
	[Header("Walk")]
	public float _walkSpeed = 1.25f;
	
	[Header("Jump")]
	public float _jumpHeightMax = 0.64f;
	public float _jumpApexTime = 0.35f;
	public float _jumpForce = 1.0f;

	[Header("Crouch")]
	public float _crouchWalkSpeed = 2.0f;
	public float _crouchCenter = 0.37f;
	public float _crouchHeight = 0.74f;

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
	
	public SimulationResults _simulationResults { get; private set; }
	
	public bool IsKinematic {
		get { return _isKinematic; }
		set {
			_isKinematic = value;
			
			if (_isKinematic) {
				_simulationResults._velocityMovement = Vector3.zero;
				_simulationResults._velocityCarried = Vector3.zero;
				_simulationResults._velocity = Vector3.zero;
			}
		}
	}

	private float _smoothX = 0.0f;
	private float _smoothY = 0.0f;
	private const float _smoothTime = 0.25f;

	/// <summary>
	/// Invoked when the actor starts falling.
	/// </summary>
	public Action _onStartedFalling;
	
	/// <summary>
	/// Invoked when the actor lands.
	/// </summary>
	public Action _onLanded;
	
	private void SimulationInit() {
		_simulationResults = new SimulationResults();
	}

	public void SetVelocity(Vector3 velocity) {
		_simulationResults._velocityCarried = velocity;
	}

	public void AddForce(Vector3 force) {
		Debug.Log("ActorMotor.AddForce: " + force);
		_simulationResults._velocityCarried += force;
	}

	public void Simulate(float horizontal, bool jump, bool crouch) {
		// Cache previous results.
		var wasGrounded = _simulationResults._isGrounded;
		
		// Reset results.
		_simulationResults.Reset();
		
		// Grounded.
		_simulationResults._isGrounded = _cc.isGrounded;

		// Crouch.
		var wasCrouching = _simulationResults._isCrouching;
		if (crouch
		&& _simulationResults._isGrounded) {
			_simulationResults._isCrouching = true;
		} else {
			_simulationResults._isCrouching = false;
		}

		if (wasCrouching != _simulationResults._isCrouching) {
			Crouch(_simulationResults._isCrouching);
		}

		// OnLanded event.
		if (wasGrounded == false
	    && _simulationResults._isGrounded) {
			_onLanded?.Invoke();
		}
		
		// StartedFalling event.
		if (wasGrounded
	    && _simulationResults._isGrounded == false) {
			_onStartedFalling?.Invoke();
		}
		
		// Set isWalking.
		if (Mathf.Abs(horizontal) > 0.0f) {
			_simulationResults._isWalking = true;
		}

		if (_isKinematic == false) {
			// Jump.
			if (jump
		    && _simulationResults._isGrounded) {
				_simulationResults._isJumping = true;
				_simulationResults._velocityCarried.y = _jumpForce;
			}
			
			// Calculate movement velocity contribution.
			_simulationResults._velocityMovement.x = horizontal * GetMovementSpeed();
		
			// Apply drag to the carried velocity.
			_simulationResults._velocityCarried.x = Mathf.SmoothDamp(_simulationResults._velocityCarried.x, 0.0f, ref _smoothX, _smoothTime);
			_simulationResults._velocityCarried.y = Mathf.SmoothDamp(_simulationResults._velocityCarried.y, 0.0f, ref _smoothY, _smoothTime);
		
			// Gravity.
			_simulationResults._velocityCarried.y += _gravity * Time.deltaTime * _actor._customTimeDilation;
		
			// Calculate final velocity.
			_simulationResults._velocity = _simulationResults._velocityCarried + _simulationResults._velocityMovement;
		
			// Clamp velocity.
			_simulationResults._velocity = Vector3.ClampMagnitude(_simulationResults._velocity, _velocityMax);
		
			// Apply velocity.
			_cc.Move(_simulationResults._velocity * Time.deltaTime * _actor._customTimeDilation);	
		}
	}

	private void Crouch(bool crouch) {
		_cc.center = new Vector3(0.0f, crouch ? _crouchCenter : _ccCenterCache, 0.0f);
		_cc.height = crouch ? _crouchHeight : _ccHeightCache;

		if (crouch) {
			_simulationResults._startedCrouching = true;
		}
	}

	private float GetMovementSpeed() {
		if (_simulationResults._isGrounded) {
			return _simulationResults._isCrouching ? _crouchWalkSpeed : _walkSpeed;
		}
		
		return _walkSpeed * _airControlScalar;
	}
	
	public class SimulationResults {

		public Vector3 _velocityCarried;
		public Vector3 _velocityMovement;
		public Vector3 _velocity;
		public bool _isGrounded;
		public bool _isWalking;
		public bool _isJumping;
		public bool _isCrouching;
		public bool _startedCrouching;

		public SimulationResults() {
			_velocityCarried = Vector3.zero;
			_velocityMovement = Vector3.zero;
			_velocity = Vector3.zero;
			_isGrounded = false;
			_isWalking = false;
			_isJumping = false;
		}

		public void Reset() {
			_velocityMovement = Vector3.zero;
			_isWalking = false;
			_isGrounded = false;
			_isJumping = false;
			_startedCrouching = false;
		}
	
	}
#endregion Simulation

}