namespace Gruel.Actor {
	public interface IHealable {

		/// <summary>
		/// Returns the healable's teamId.
		/// </summary>
		/// <returns></returns>
		int TeamId { get; set; }

		/// <summary>
		/// Attempts to heal the actor with the amount specified.
		/// </summary>
		/// <param name="healDelta"></param>
		void Heal(int healDelta);

	}
}