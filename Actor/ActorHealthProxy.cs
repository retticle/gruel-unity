using UnityEngine;

namespace Gruel.Actor {
	public class ActorHealthProxy : MonoBehaviour, IDamageable, IHealable {

#region ActorHealthProxy
		[Header("Health")]
		[SerializeField] private ActorHealth _actorHealth;
#endregion ActorHealthProxy
		
#region Interfaces
		public int GetTeamId() {
			return _actorHealth.GetTeamId();
		}
		
		public bool IsInvulnerable() {
			return _actorHealth.IsInvulnerable();
		}
		
		public void Damage(int damage) {
			_actorHealth.Damage(damage);
		}
		
		public void Kill() {
			_actorHealth.Kill();
		}
		
		public void Heal(int healDelta) {
			_actorHealth.Heal(healDelta);
		}
#endregion Interfaces

	}
}