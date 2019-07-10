namespace Gruel.ObjectPool {
	public interface IPoolable {

		void Pool();
		void Unpool();
	
		int GetHash();
		void SetHash(int hash);

		void Destroy();
	
	}
}