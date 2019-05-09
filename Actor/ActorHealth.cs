using System;
using Gruel.VariableObjects;
using UnityEngine;

namespace Gruel.Actor {
	public class ActorHealth : MonoBehaviour, IActorTrait, IDamageable, IHealable {

#region IActorTrait
		public void InitializeTrait(Actor actor) {
			HealthInit();
		}
		
		public void RemoveTrait() {}
#endregion IActorTrait
		
#region IDamageable / IHealable
		public int TeamId {
			get { return _teamId; }
			set { _teamId = value; }
		}
		
		public bool IsInvulnerable {
			get { return _isInvulnerable; }
			set { _isInvulnerable = value; }
		}

		public void Damage(int damage) {
			if (_isInvulnerable == false) {
				AdjustHealth(-damage);
			}
		}
		
		public void Kill() {
			AdjustHealth(-_health.Value);
		}

		public void Heal(int healDelta) {
			AdjustHealth(healDelta);
		}
#endregion IDamageable / IHealable
		
#region ActorHealth
		[Header("Health Settings")]
		[SerializeField] private IntReference _health;
		[SerializeField] private IntReference _healthStart;
		[SerializeField] private IntReference _healthMin;
		[SerializeField] private IntReference _healthMax;
		
		[SerializeField] private bool _isInvulnerable = false;
		[SerializeField] private int _teamId;

		/// <summary>
		/// The current health.
		/// Setting this directly will not invoke actions.
		/// </summary>
		public int Health {
			get { return _health; }
			set { _health.Value = value; }
		}
		
		/// <summary>
		/// Health will start at this value when the trait is initialized.
		/// </summary>
		public int HealthStart {
			get { return _healthStart; }
			set { _healthStart.Value = value; }
		}
		
		/// <summary>
		/// Health will not be able to go below this value.
		/// </summary>
		public int HealthMin {
			get { return _healthMin; }
			set { _healthMin.Value = value; }
		}
	
		/// <summary>
		/// Health will not be able to go above this value.
		/// </summary>
		public int HealthMax {
			get { return _healthMax; }
			set { _healthMax.Value = value; }
		}

		/// <summary>
		/// Invoked when health is changed.
		/// health, delta
		/// </summary>
		public Action<int, int> _onHealthChanged;

		/// <summary>
		/// Invoked when healed.
		/// health, delta
		/// </summary>
		public Action<int, int> _onHealed;

		/// <summary>
		/// Invoked when damage is taken.
		/// health, delta
		/// </summary>
		public Action<int, int> _onDamaged;

		/// <summary>
		/// Invoked when health reaches the minimum value.
		/// </summary>
		public Action _onHealthEmpty;

		public void HealthInit() {
			_health.Value = _healthStart;
		}

		public void SetSettings(int teamId, int healthStart, int healthMin, int healthMax, bool resetHealth) {
			this._teamId = teamId;
			this._healthStart.Value = healthStart;
			this._healthMin.Value = healthMin;
			this._healthMax.Value = healthMax;

			if (resetHealth) {
				this._health.Value = _healthStart;
			}
		}

		private void AdjustHealth(int delta) {
			// Health won't be adjusted.
			// Return right away so the callbacks aren't invoked.
			if (delta == 0) {
				return;
			}
		
			// Adjust health.
			_health.Value = Mathf.Clamp(_health + delta, _healthMin, _healthMax);

			// Call onHealthChanged action.
			_onHealthChanged?.Invoke(_health, delta);
		
			// Call healed action.
			if (delta > 0) {
				_onHealed?.Invoke(_health, delta);
			}
		
			// Call damaged action.
			if (delta < 0) {
				_onDamaged?.Invoke(_health, delta);
			}

			// Call onHealthEmpty action.
			if (_health <= _healthMin) {
				_onHealthEmpty?.Invoke();
			}
		}
#endregion ActorHealth

	}
}