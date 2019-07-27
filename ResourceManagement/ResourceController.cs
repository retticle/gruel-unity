using System;
using System.Collections;
using System.Collections.Generic;
using Gruel.CoroutineUtils;
using UnityEngine;

namespace Gruel.ResourceManagement {
	public static class ResourceController  {
	
#region Fields
		private static Dictionary<string, UnityEngine.Object> _loadedResources = new Dictionary<string, UnityEngine.Object>();
#endregion Fields

#region Public Methods
		public static Coroutine LoadResource(string resourcePath, Action onLoadedCallback = null) {
			return LoadResource(new[] { resourcePath }, onLoadedCallback);
		}

		public static Coroutine LoadResource(string[] path, Action onLoadedCallback = null) {
			return CoroutineRunner.StartCoroutine(LoadResourceCor(path, onLoadedCallback));
		}

		public static void UnloadResource(string resourcePath) {
			UnloadResource(new[] { resourcePath });
		}

		public static void UnloadResource(string[] resourcePath) {
			for (int i = 0, n = resourcePath.Length; i < n; i++) {
				if (_loadedResources.ContainsKey(resourcePath[i])) {
					_loadedResources.Remove(resourcePath[i]);
				}
			}
		}

		public static UnityEngine.Object GetLoadedResource(string key) {
			try {
				return _loadedResources[key];
			} catch (Exception ex) {
				Debug.LogError("ResourceController.GetLoadedResource: requested resource has not been loaded!");
				Debug.LogException(ex);
			}

			return null;
		}

		public static void UnloadAllResources() {
			_loadedResources = new Dictionary<string, UnityEngine.Object>();
		}
#endregion Public Methods

#region Private Methods
		private static IEnumerator LoadResourceCor(string[] resourcePath, Action onLoadedCallback = null) {
			for (int i = 0, n = resourcePath.Length; i < n; i++) {
				var resourceKey = resourcePath[i];

				if (_loadedResources.ContainsKey(resourceKey)) {
					continue;
				}
			
				Action<UnityEngine.Object> onResourceLoaded = delegate(UnityEngine.Object loadedObject) {
					_loadedResources.Add(resourceKey, loadedObject);
				};
			
				yield return AsyncLoader.LoadResource(resourceKey, onLoaded: onResourceLoaded);
			}
		
			onLoadedCallback?.Invoke();
		}
#endregion Private Methods
	
	}
}