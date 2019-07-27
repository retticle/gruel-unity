using System;
using System.Collections;
using Gruel.CoroutineUtils;
using UnityEngine;

namespace Gruel.ResourceManagement {
	public static class AsyncLoader {
	
#region Public Methods
		public static Coroutine LoadResource(string path, Type type = null, Action<UnityEngine.Object> onLoaded = null, Action<float> onProgress = null) {
			// Debug.Log($"AsyncLoader.LoadResource: path: {path}");
		
			return CoroutineRunner.StartCoroutine(AsyncLoaderCor(path, type, onLoaded, onProgress));
		}
#endregion Public Methods

#region Private Methods
		private static IEnumerator AsyncLoaderCor(string path, Type type = null, Action<UnityEngine.Object> onLoaded = null, Action<float> onProgress = null) {
			var asyncOperation = Resources.LoadAsync(path, type == null ? typeof(UnityEngine.Object) : type);

			while (asyncOperation.isDone == false) {
				onProgress?.Invoke(asyncOperation.progress);
				yield return null;
			}

			onLoaded?.Invoke(asyncOperation.asset);
		}
#endregion Private Methods
	
	}
}