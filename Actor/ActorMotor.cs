using System;
using Gruel.Actor;
using UnityEngine;

public class ActorMotor : MonoBehaviour, IActorTrait {
	
#region ActorTrait
	public void Init() {
		PhysicsSettingsAwake();
		SimulationAwake();
	}

	public void Remove() {
	}
#endregion ActorTrait
	
#region Core
	[Header("Core")]
	[SerializeField] private Actor _actor;
#endregion Core
	
#region Physics Settings
	// private const float WALK_SPEED = 0.8f;
	private const float WALK_SPEED = 1.25f;
	
	private const float JUMP_HEIGHT_MAX = 0.64f;
	private const float JUMP_APEX_TIME = 0.35f;
	
	private const float VELOCITY_MAX = 1.32f;
	
	private float _gravity = 0.0f;
	private float _jumpForce = 0.0f;
	
	private void PhysicsSettingsAwake() {
		_gravity = -(2.0f * JUMP_HEIGHT_MAX) / Mathf.Pow(JUMP_APEX_TIME, 2.0f);
		_jumpForce = Mathf.Sqrt(2.0f * Mathf.Abs(_gravity) * JUMP_HEIGHT_MAX);
		
		Debug.Log($"PlayerMotor.PhysicsSettingsAwake: gravity: {_gravity} | jumpForce: {_jumpForce}");
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
	
	private void SimulationAwake() {
		_simulationResults = new PlayerMotor_SimulationResults();
	}

	public void AddForce(Vector3 force) {
		Debug.Log("PlayerMotor.AddForce: " + force);
		_simulationResults._velocityCarried += force;
	}

	public void Simulate(float horizontal) {
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
		if (horizontal != 0.0f) {
			_simulationResults._isWalking = true;
		}

		// Calculate movement velocity contribution.
		_simulationResults._velocityMovement.x = horizontal * WALK_SPEED;
		
		// Apply drag to the carried velocity.
		_simulationResults._velocityCarried.x = Mathf.SmoothDamp(_simulationResults._velocityCarried.x, 0.0f, ref _smoothX, _smoothTime);
		_simulationResults._velocityCarried.y = Mathf.SmoothDamp(_simulationResults._velocityCarried.y, 0.0f, ref _smoothY, _smoothTime);
		
		// Gravity.
		_simulationResults._velocityCarried.y += _gravity * Time.deltaTime * _actor._customTimeDilation;
		
		// Calculate final velocity.
		_simulationResults._velocity = _simulationResults._velocityCarried + _simulationResults._velocityMovement;
		
		// Clamp velocity.
		// _simulationResults._velocity = Vector3.ClampMagnitude(_simulationResults._velocity, VELOCITY_MAX);
		
		// Apply velocity.
		_cc.Move(_simulationResults._velocity * Time.deltaTime * _actor._customTimeDilation);
	}
#endregion Simulation

}