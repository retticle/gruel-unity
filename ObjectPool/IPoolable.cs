namespace Gruel.ObjectPool {
	public interface IPoolable {

		void Pool();
		void Unpool();
	
		int Hash { get; set; }

		void Destroy();
	
	}
}