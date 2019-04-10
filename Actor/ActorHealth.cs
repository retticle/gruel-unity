using System;
using UnityEngine;

public class ActorHealth : MonoBehaviour, IHealth {

#region Init
	public void Init(int teamId) {
		HealthInit(teamId);
	}
	
	public void Init(int teamId, int healthStart, int healthMin, int healthMax) {
		HealthInit(teamId, healthStart, healthMin, healthMax);
	}
	
	private void Awake() {
		HealthAwake();
	}
#endregion Init
	
#region IHealth
	public int GetTeamId() {
		return _teamId;
	}

	public void SetHealth(int health) {
		// Get the difference from our current health.
		var delta = -(_healthCurrent - health);
		
		AdjustHealth(delta);
	}
	
	public void AddHealth(int delta) {
		AdjustHealth(delta);
	}
	
	public void RemoveHealth(int delta) {
		if (_invulnerable == false) {
			// Make the delta negative so we subtract it from the current health.
			AdjustHealth(-delta);
		}
	}

	public void Kill() {
		SetHealth(_healthMin);
	}

	public bool IsInvulnerable() {
		return _invulnerable;
	}
#endregion IHealth
	
#region Health
	[Header("Health")]
	[SerializeField] private IntReference _healthCurrent;
	[SerializeField] private IntReference _healthStart;
	[SerializeField] private IntReference _healthMax;
	[SerializeField] private IntReference _healthMin;
	
	public int HealthMax { get { return _healthMax; } }
	public int HealthMin { get { return _healthMin; } }
	public int HealthStart { get { return _healthStart; } }

	public bool _invulnerable = false;

	// private int _healthCurrent;
	private int _teamId;

	/// <summary>
	/// healthCurrent, delta
	/// </summary>
	public Action<int, int> _onHealthChanged;

	/// <summary>
	/// healthCurrent, delta
	/// </summary>
	public Action<int, int> _onHealthAdded;

	/// <summary>
	/// healthCurrent, delta
	/// </summary>
	public Action<int, int> _onHealthRemoved;

	public Action _onHealthEmpty;

	private void HealthInit(int teamId) {
		_teamId = teamId;
	}

	private void HealthInit(int teamId, int healthStart, int healthMin, int healthMax) {
		_teamId = teamId;
		_healthStart.Value = healthStart;
		_healthMin.Value = healthMin;
		_healthMax.Value = healthMax;
	}

	private void HealthAwake() {
		// Set initial health.
		_healthCurrent.Value = _healthStart;
	}

	private void AdjustHealth(int delta) {
		// Health won't be adjusted.
		if (delta == 0) {
			return;
		}
		
		// Adjust health.
		_healthCurrent.Value = Mathf.Clamp(_healthCurrent + delta, _healthMin, _healthMax);

		// Call onHealthChanged action.
		_onHealthChanged?.Invoke(_healthCurrent, delta);
		
		// Call onHealthAdded action.
		if (delta > 0) {
			// Health was added.
			_onHealthAdded?.Invoke(_healthCurrent, delta);

			return;
		}
		
		// Call onHealthRemoved action.
		if (delta < 0) {
			// Health was removed.
			_onHealthRemoved?.Invoke(_healthCurrent, delta);

			return;
		}

		// Call onHealthEmpty action.
		if (_healthCurrent <= _healthMin) {
			_onHealthEmpty?.Invoke();
		}
	}
#endregion Health
	
}