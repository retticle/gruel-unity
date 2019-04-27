using System;
using Gruel.VariableObjects;
using UnityEngine;

namespace Gruel.Actor {
	public class ActorEnergy : MonoBehaviour, IActorTrait, IEnergy {

#region IActorTrait
		private Actor _actor;
		
		public void InitializeTrait(Actor actor) {
			this._actor = actor;
			
			EnergyInit();
		}
		
		public void RemoveTrait() {}
#endregion IActorTrait
		
#region IEnergy
		public int TeamId => _actor.TeamId;

		public int Energy {
			get { return _energy; }
			set { _energy.Value = value; }
		}

		public void Charge(int amount) {
			AdjustEnergy(amount);
		}

		public void Drain(int amount) {
			AdjustEnergy(-amount);
		}
#endregion IEnergy
		
#region ActorEnergy
		[Header("Energy Settings")]
		[SerializeField] private IntReference _energy;
		[SerializeField] private IntReference _energyStart;
		[SerializeField] private IntReference _energyMin;
		[SerializeField] private IntReference _energyMax;
		
		/// <summary>
		/// Energy will start at this value when the trait is initialized.
		/// </summary>
		public int EnergyStart {
			get { return _energyStart; }
			set { _energyStart.Value = value; }
		}
		
		/// <summary>
		/// Energy will not be able to go below this value.
		/// </summary>
		public int EnergyMin {
			get { return _energyMin; }
			set { _energyMin.Value = value; }
		}
	
		/// <summary>
		/// Energy will not be able to go above this value.
		/// </summary>
		public int EnergyMax {
			get { return _energyMax; }
			set { _energyMax.Value = value; }
		}

		/// <summary>
		/// Invoked when energy is changed.
		/// energy, delta
		/// </summary>
		public Action<int, int> _onEnergyChanged;

		/// <summary>
		/// Invoked when charged.
		/// energy, delta
		/// </summary>
		public Action<int, int> _onCharged;

		/// <summary>
		/// Invoked when energy is taken.
		/// energy, delta
		/// </summary>
		public Action<int, int> _onDrained;

		/// <summary>
		/// Invoked when energy reaches the minimum value.
		/// </summary>
		public Action _onEnergyEmpty;

		private void EnergyInit() {
			_energy.Value = _energyStart;
		}

		public void SetSettings(int energyStart, int energyMin, int energyMax, bool resetEnergy) {
			this._energyStart.Value = energyStart;
			this._energyMin.Value = energyMin;
			this._energyMax.Value = energyMax;

			if (resetEnergy) {
				this._energy.Value = _energyStart;
			}
		}

		private void AdjustEnergy(int delta) {
			// Energy won't be adjusted.
			// Return right away so the callbacks aren't invoked.
			if (delta == 0) {
				return;
			}
		
			// Adjust energy.
			_energy.Value = Mathf.Clamp(_energy + delta, _energyMin, _energyMax);

			// Call onEnergyChanged event.
			_onEnergyChanged?.Invoke(_energy, delta);
		
			// Call onCharged event.
			if (delta > 0) {
				_onCharged?.Invoke(_energy, delta);
			}
		
			// Call onDrained event.
			if (delta < 0) {
				_onDrained?.Invoke(_energy, delta);
			}

			// Call onEnergyEmpty action.
			if (_energy <= _energyMin) {
				_onEnergyEmpty?.Invoke();
			}
		}
#endregion ActorEnergy

	}
}