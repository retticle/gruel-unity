using System;
using Gruel.Actor;
using UnityEngine;

public class ActorMotor2D : MonoBehaviour, IActorTrait {
	
#region ActorTrait
	private Actor _actor;
	
	public void Init(Actor actor) {
		this._actor = actor;
		
		PhysicsSettingsInit();
		SimulationInit();
	}

	public void Remove() {}
#endregion ActorTrait
	
#region Physics Settings
	[Header("Settings")]
	public bool _calculateGravityAndJumpForce = true;
	public float _velocityMax = 100.0f;
	
	public float _walkSpeed = 1.25f;
	
	public float _jumpHeightMax = 0.64f;
	public float _jumpApexTime = 0.35f;

	public float _airControlScalar = 1.0f;

	public float _gravity = -9.8f;
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
	[SerializeField] private CharacterController _cc;
	
	public PlayerMotor_SimulationResults _simulationResults { get; private set; }

	private float _smoothX = 0.0f;
	private float _smoothY = 0.0f;
	private const float _smoothTime = 0.25f;

	public Action _onLanded;
	
	private void SimulationInit() {
		_simulationResults = new PlayerMotor_SimulationResults();
	}

	public void SetVelocity(Vector3 velocity) {
		_simulationResults._velocityCarried = velocity;
	}

	public void AddForce(Vector3 force) {
		Debug.Log("ActorMotor.AddForce: " + force);
		_simulationResults._velocityCarried += force;
	}

	public void Simulate(float horizontalInput) {
		// Cache previous results.
		var wasGrounded = _simulationResults._isGrounded;
		
		// Reset results.
		_simulationResults.Reset();
		
		// Grounded.
		_simulationResults._isGrounded = _cc.isGrounded;

		// OnLanded callback.
		if (wasGrounded == false
	    && _simulationResults._isGrounded) {
			_onLanded?.Invoke();
		}
		
		// Set isWalking.
		if (horizontalInput != 0.0f) {
			_simulationResults._isWalking = true;
		}

		// Calculate movement velocity contribution.
		var walkSpeedActual = _simulationResults._isGrounded ? _walkSpeed : _walkSpeed * _airControlScalar;
		_simulationResults._velocityMovement.x = horizontalInput * walkSpeedActual;
		
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
#endregion Simulation

}