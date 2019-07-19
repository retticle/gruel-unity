namespace Gruel.ObjectPool {
	public interface IPoolable {

#region Properties
		int Hash { get; set; }
#endregion Properties

#region Methods
		void Pool();
		void Unpool();
		void Destroy();
#endregion Methods
	
	}
}