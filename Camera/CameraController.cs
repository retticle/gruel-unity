using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gruel.Camera {
	public class CameraController : MonoBehaviour {
		
#region Properties
		public static CameraController Instance { get; private set; }
		
		public UnityEngine.Camera Camera {
			get => _camera;
		}

		public float OrthoSize {
			get => _camera.orthographicSize;
		}
	
		public Vector3 CameraPosition {
			get => _camera.transform.position;
		}
#endregion Properties

#region Fields
		[Header("Camera")]
		[SerializeField] private UnityEngine.Camera _camera;
		
		[Header("Traits")]
		[FormerlySerializedAs("_cameraTraits")] [SerializeField] private CameraTrait[] _traitComponents;

		private List<CameraTrait> _traits;
#endregion Fields

#region Public Methods
		public void SetPosition(Vector3 position) {
			Debug.Log($"CameraController.SetPosition: {position}");
		
			transform.position = position;
		}

		public void AddTrait(CameraTrait trait) {
			_traits.Add(trait);
		}

		public void RemoveTrait(CameraTrait trait) {
			_traits.Remove(trait);
		}
		
		public T GetTrait<T>() where T : CameraTrait {
			return _traits.OfType<T>().Select(trait => trait).FirstOrDefault();
		}

		public bool TryGetTrait<T>(out T outTrait) where T : CameraTrait {
			var type = typeof(T);
			outTrait = null;

			foreach (var trait in _traits) {
				if (trait.GetType() == type) {
					outTrait = (T)trait;
					return true;
				}
			}

			return false;
		}
#endregion Public Methods

#region Private Methods
		private void Awake() {
			// Setup instance.
			if (Instance != null) {
				Debug.LogError("CameraController: There is already an instance of CameraController!");
				Destroy(gameObject);
			} else {
				Instance = this;
			}
			
			// Add initial component traits.
			_traits = new List<CameraTrait>();

			for (int i = 0, n = _traitComponents.Length; i < n; i++) {
				AddTrait(_traitComponents[i]);
			}
		}
#endregion Private Methods

	}
}