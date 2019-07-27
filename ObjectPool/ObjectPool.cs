using System;
using System.Collections;
using System.Collections.Generic;
using Gruel.CoroutineUtils;
using Gruel.ResourceManagement;
using UnityEngine;

namespace Gruel.ObjectPool {
	public class ObjectPool : MonoBehaviour {
	
#region Properties
#endregion Properties

#region Fields
		[SerializeField] private Transform _poolContainer;

		private static Dictionary<int, List<IPoolable>> _poolComplete;
		private static Dictionary<int, Queue<IPoolable>> _poolAvailable;
		private static Dictionary<int, List<IPoolable>> _poolCheckedOut;

		private static Dictionary<int, string> _paths;
		
		private static ObjectPool _instance;
#endregion Fields

#region Public Methods
		public void Init() {
			// Setup instance.
			if (_instance != null) {
				Debug.LogError("ObjectPool: There is already an instance of ObjectPool!");
				Destroy(gameObject);
			} else {
				_instance = this;
			}
			
			_poolComplete = new Dictionary<int, List<IPoolable>>();
			_poolAvailable = new Dictionary<int, Queue<IPoolable>>();
			_poolCheckedOut = new Dictionary<int, List<IPoolable>>();
			_paths = new Dictionary<int, string>();
		}
		
		public static Coroutine Add(string path, int amount) {
			Debug.Log($"ObjectPool.Add: Adding {amount} {path}");
		
			return CoroutineRunner.StartCoroutine(_instance.AddCor(path, amount));
		}
		
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
			var hash = poolable.Hash;
		
			// Tell poolable to pool itself.
			poolable.Pool();
		
			// Remove from the checkedout list.
			_poolCheckedOut[hash].Remove(poolable);
		
			// Add to the available queue.
			_poolAvailable[hash].Enqueue(poolable);
		}
#endregion Public Methods

#region Private Methods
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
				poolable.Hash = hash;
			
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
			poolable.Hash = hash;
		
			// Tell the poolable its being pooled.
			poolable.Pool();
		
			// Add to lists/queues.
			_poolComplete[hash].Add(poolable);
			_poolAvailable[hash].Enqueue(poolable);
		}
#endregion Private Methods

	}
}