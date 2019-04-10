using UnityEngine;

public class IHealthProxy : MonoBehaviour, IHealth {

	[Header("Health")]
	[SerializeField] private ActorHealth _actorHealth;

	public int GetTeamId() {
		return _actorHealth.GetTeamId();
	}

	public void SetHealth(int health) {
		_actorHealth.SetHealth(health);
	}

	public void AddHealth(int delta) {
		_actorHealth.AddHealth(delta);
	}

	public void RemoveHealth(int delta) {
		_actorHealth.RemoveHealth(delta);
	}

	public void Kill() {
		_actorHealth.Kill();
	}

	public bool IsInvulnerable() {
		return _actorHealth.IsInvulnerable();
	}

}