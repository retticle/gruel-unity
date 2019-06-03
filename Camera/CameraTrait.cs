using UnityEngine;

namespace Gruel.Camera {
	public abstract class CameraTrait : MonoBehaviour {
		
#region Init
		protected virtual void Awake() { }

		protected virtual void Start() { }

		protected virtual void OnDestroy() { }
#endregion Init
		
	}
}