namespace Gruel.Actor {
	public interface IDamageable {
		
		/// <summary>
		/// Returns the damageable's teamId.
		/// </summary>
		/// <returns></returns>
		int GetTeamId();
		
		/// <summary>
		/// Returns if the damageable is currently invulnerable.
		/// </summary>
		/// <returns></returns>
		bool IsInvulnerable();

		/// <summary>
		/// Attempts to remove the amount of health specified.
		/// This will apply "damage", so use positive numbers.
		/// Invulnerability may override this.
		/// </summary>
		/// <param name="damage"></param>
		void Damage(int damage);

		/// <summary>
		/// Attempts to kill the actor regardless of it's current health.
		/// Invulnerability may override this.
		/// </summary>
		void Kill();

	}
}