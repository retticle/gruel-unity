namespace Gruel.Actor {
	public interface IEnergy {
		
		/// <summary>
		/// Returns teamId.
		/// </summary>
		/// <returns></returns>
		int TeamId { get; }
		
		/// <summary>
		/// Returns the amount of energy.
		/// Setting this directly will not invoke events.
		/// </summary>
		int Energy { get; set; }

		/// <summary>
		/// Attempts to charge the actor's energy with the amount specified.
		/// </summary>
		/// <param name="amount"></param>
		void Charge(int amount);

		/// <summary>
		/// Attempts to drain the actor's energy with the amount specified.
		/// This is the amount that will be drained, so use positive numbers.
		/// </summary>
		/// <param name="amount"></param>
		void Drain(int amount);

	}
}