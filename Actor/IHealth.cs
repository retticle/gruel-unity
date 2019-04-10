public interface IHealth {

	int GetTeamId();

	/// <summary>
	/// Override the current health.
	/// Health may still get clamped.
	/// </summary>
	/// <param name="health"></param>
	void SetHealth(int health);
	
	/// <summary>
	/// Add the amount of health specified.
	/// </summary>
	/// <param name="delta"></param>
	void AddHealth(int delta);
	
	/// <summary>
	/// Remove the amount of health specified.
	/// This will apply "damage", so use positive numbers.
	/// </summary>
	/// <param name="delta"></param>
	void RemoveHealth(int delta);

	/// <summary>
	/// Kills the actor regardless of its health.
	/// </summary>
	void Kill();

	bool IsInvulnerable();

}