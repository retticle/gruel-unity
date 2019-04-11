namespace Gruel.Actor {
	public interface IHealable {

		/// <summary>
		/// Returns the damageable's teamId.
		/// </summary>
		/// <returns></returns>
		int GetTeamId();

		/// <summary>
		/// Attempts to heal the actor with the amount specified.
		/// </summary>
		/// <param name="healDelta"></param>
		void Heal(int healDelta);

	}
}