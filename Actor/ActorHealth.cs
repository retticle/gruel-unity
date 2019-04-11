using System;
using Gruel.VariableObjects;
using UnityEngine;

namespace Gruel.Actor {
	public class ActorHealth : MonoBehaviour, IDamageable, IHealable {

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
		
#region Interfaces
		public int GetTeamId() {
			return _teamId;
		}
		
		public bool IsInvulnerable() {
			return _invulnerable;
		}
		
		public void Damage(int damage) {
			if (_invulnerable == false) {
				AdjustHealth(-damage);
			}
		}
		
		public void Kill() {
			AdjustHealth(-_healthCurrent.Value);
		}
		
		public void Heal(int healDelta) {
			AdjustHealth(healDelta);
		}
#endregion Interfaces
		
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

			_healthCurrent.Value = _healthStart;
		}

		private void HealthAwake() {
			// Set initial health.
			_healthCurrent.Value = _healthStart;
		}

		private void AdjustHealth(int delta) {
			// Health won't be adjusted.
			// Return right away so the callbacks aren't invoked.
			if (delta == 0) {
				return;
			}
		
			// Adjust health.
			_healthCurrent.Value = Mathf.Clamp(_healthCurrent + delta, _healthMin, _healthMax);

			// Call onHealthChanged action.
			_onHealthChanged?.Invoke(_healthCurrent, delta);
		
			// Call onHealthAdded action.
			if (delta > 0) {
				_onHealthAdded?.Invoke(_healthCurrent, delta);
			}
		
			// Call onHealthRemoved action.
			if (delta < 0) {
				_onHealthRemoved?.Invoke(_healthCurrent, delta);
			}

			// Call onHealthEmpty action.
			if (_healthCurrent <= _healthMin) {
				_onHealthEmpty?.Invoke();
			}
		}
#endregion Health
	
	}
}