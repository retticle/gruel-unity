using UnityEngine;

namespace Gruel.Actor {
	public class ActorHealthProxy : MonoBehaviour, IDamageable, IHealable {

#region ActorHealthProxy
		[Header("Health")]
		[SerializeField] private ActorHealth _actorHealth;
#endregion ActorHealthProxy
		
#region Interfaces
		public int TeamId {
			get { return _actorHealth.TeamId; }
			set { _actorHealth.TeamId = value; }
		}
		
		public bool IsInvulnerable {
			get { return _actorHealth.IsInvulnerable; }
			set { _actorHealth.IsInvulnerable = value; }
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