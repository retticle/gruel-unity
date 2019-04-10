using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gruel.ObjectPool {
	public class ObjectPool : MonoBehaviour {
	
#region Init
		public void Init() {
			CoreInit();
			PoolInit();
		}
#endregion Init
	
#region ObjectPool
		private static ObjectPool _instance = null;

		private void CoreInit() {
			// Setup instance.
			if (_instance != null) {
				Debug.LogError("ObjectPool: There is already an instance of ObjectPool!");
				Destroy(gameObject);
			} else {
				_instance = this;
			}
		}
#endregion ObjectPool
	
#region Pool Core
		[Header("Pool Core")]
		[SerializeField] private Transform _poolContainer;

		private static Dictionary<int, List<IPoolable>> _poolComplete = null;
	
		private static Dictionary<int, Queue<IPoolable>> _poolAvailable = null;
		private static Dictionary<int, List<IPoolable>> _poolCheckedOut = null;

		private static Dictionary<int, string> _paths = null;
	
		private void PoolInit() {
			_poolComplete = new Dictionary<int, List<IPoolable>>();
		
			_poolAvailable = new Dictionary<int, Queue<IPoolable>>();
			_poolCheckedOut = new Dictionary<int, List<IPoolable>>();
		
			_paths = new Dictionary<int, string>();
		}
#endregion Pool Core
	
#region Add
		public static Coroutine Add(string path, int amount) {
			Debug.Log($"ObjectPool.Add: Adding {amount} {path}");
		
			return RoutineRunner.RoutineRunner.StartRoutine(_instance.AddCor(path, amount));
		}
	
		private IEnumerator AddCor(string path, int amount) {
			// Get path hash.
			var hash = path.GetHashCode();
		
			// Save path, we may need it again in the future.
			_paths.Add(hash, path);
		
			// Create new queue/list in dictionaries if one doesn't already exist.
			if (_poolComplete.ContainsKey(hash) == false) {
				_poolComplete.Add(hash, new List<IPoolable>());
				_poolAvailable.Add(hash, new Queue<IPoolable>());
				_poolCheckedOut.Add(hash, new List<IPoolable>());
			}
		
			// Load resource. 
			UnityEngine.Object loadedObject = null;
			Action<UnityEngine.Object> onResourceLoaded = delegate(UnityEngine.Object o) {
				loadedObject = o;
			};
			yield return AsyncLoader.LoadResource(path, onLoaded: onResourceLoaded);
		
			// Instantiate objects and add to dictionary.
			for (int i = 0; i < amount; i++) {
				// Instantiate object using loaded resource.
				var instantiatedObject = (GameObject)Instantiate(loadedObject, _poolContainer);
			
				// Get the poolable interface.
				var poolable = instantiatedObject.GetComponent<IPoolable>();
			
				// Save hash in the poolable.
				poolable.SetHash(hash);
			
				// Tell the poolable its being pooled.
				poolable.Pool();
			
				// Add to lists/queues.
				_poolComplete[hash].Add(poolable);
				_poolAvailable[hash].Enqueue(poolable);
			}
		}
	
		private void AddNonAsync(int hash) {
			// Load and instantiate the object.
			var instantiatedObject = (GameObject)Instantiate(Resources.Load(_paths[hash]), _poolContainer);
		
			// Get the poolable interface.
			var poolable = instantiatedObject.GetComponent<IPoolable>();
		
			// Save hash in the poolable.
			poolable.SetHash(hash);
		
			// Tell the poolable its being pooled.
			poolable.Pool();
		
			// Add to lists/queues.
			_poolComplete[hash].Add(poolable);
			_poolAvailable[hash].Enqueue(poolable);
		}
#endregion Add
	
#region Remove
		public static void Remove(int hash) {
			if (_poolAvailable.ContainsKey(hash) == false) {
				Debug.LogError($"ObjectPool.Remove: This hash does not exist in the pool!");
				return;
			}

			// Destroy all poolable objects of this type.
			var list = _poolComplete[hash];
			for (int i = 0, n = list.Count; i < n; i++) {
				list[i].Destroy();
			}

			// Remove this hash from the dictionaries.
			_poolComplete.Remove(hash);
			_poolAvailable.Remove(hash);
			_poolCheckedOut.Remove(hash);
			_paths.Remove(hash);
		}
#endregion Remove
	
#region Get / Repool
		public static IPoolable Get(int hash) {
			if (_poolAvailable.ContainsKey(hash) == false) {
				Debug.LogError($"ObjectPool.Get: This hash does not exist in the pool!");
				return null;
			}
		
			if (_poolAvailable[hash].Count < 1) {
				Debug.LogWarning($"ObjectPool.Get: Not enough \"{_paths[hash]}\" objects in pool! Adding another non-async");
				_instance.AddNonAsync(hash);
			}

			// Remove poolable from available queue.
			var poolable = _poolAvailable[hash].Dequeue();
		
			// Add poolable to the checked out list.
			_poolCheckedOut[hash].Add(poolable);
		
			// Tell the poolable to unpool
			poolable.Unpool();

			// Return poolable to caller.
			return poolable;
		}

		public static void Repool(IPoolable poolable) {
			// Get poolable hash.
			var hash = poolable.GetHash();
		
			// Tell poolable to pool itself.
			poolable.Pool();
		
			// Remove from the checkedout list.
			_poolCheckedOut[hash].Remove(poolable);
		
			// Add to the available queue.
			_poolAvailable[hash].Enqueue(poolable);
		}
#endregion Get / Repool

	}
}