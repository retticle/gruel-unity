using System;
using System.Collections;
using UnityEngine;

public static class AsyncLoader {
	
#region AsyncLoader
	public static Coroutine LoadResource(string path, Type type = null, Action<UnityEngine.Object> onLoaded = null, Action<float> onProgress = null) {
		// Debug.Log($"AsyncLoader.LoadResource: path: {path}");
		
		return RoutineRunner.StartRoutine(AsyncLoaderCor(path, type, onLoaded, onProgress));
	}

	private static IEnumerator AsyncLoaderCor(string path, Type type = null, Action<UnityEngine.Object> onLoaded = null, Action<float> onProgress = null) {
		yield return null;
		
		var asyncOperation = Resources.LoadAsync(path, type == null ? typeof(UnityEngine.Object) : type);

		while (asyncOperation.isDone == false) {
			onProgress?.Invoke(asyncOperation.progress);
			yield return null;
		}

		onLoaded?.Invoke(asyncOperation.asset);
	}
#endregion AsyncLoader
	
}