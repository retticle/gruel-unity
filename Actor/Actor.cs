using System;
using System.Collections.Generic;
using System.Linq;
using Gruel.TimeDilation;
using UnityEngine;

namespace Gruel.Actor {
	public abstract class Actor : MonoBehaviour, ITimeDilatable {
		
#region MonoBehaviour
		protected virtual void Awake() {
			TraitsInit();
			TimeDilationInit();
		}

		protected virtual void Start() {
			
		}
#endregion MonoBehaviour
		
#region Info
		[Header("Info")]
		[SerializeField] private int _playerId = -1;
		[SerializeField] private int _teamId = -1;

		public int PlayerId {
			get { return _playerId; }
			set { _playerId = value; }
		}
		
		public int TeamId {
			get { return _teamId; }
			set { _teamId = value; }
		}
#endregion Info
		
#region Traits
		[Header("Traits")]
		[SerializeField]
		private Component[] _traitComponents = new Component[0];

		private List<IActorTrait> _traits = null;

		private void TraitsInit() {
			_traits = new List<IActorTrait>();

			for (int i = 0, n = _traitComponents.Length; i < n; i++) {
				var trait = (IActorTrait)_traitComponents[i];
				_traits.Add(trait);
				trait.InitializeTrait(this);
			}
		}

		public void AddTrait(IActorTrait trait) {
			_traits.Add(trait);
			trait.InitializeTrait(this);
		}

		public void RemoveTrait(IActorTrait trait) {
			_traits.Remove(trait);
			trait.RemoveTrait();
		}

		public T GetTrait<T>() where T : class, IActorTrait {
			return _traits.OfType<T>().Select(trait => trait).FirstOrDefault();
		}

		public bool TryGetTrait<T>(out T outTrait) where T : class, IActorTrait {
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
#endregion Traits
		
#region TimeDilation
		[Header("TimeDilation")]
		[SerializeField]
		protected ETimeDilationSorting _timeDilationSorting = ETimeDilationSorting.Latest;

		protected Dictionary<UnityEngine.Object, float> _timeDilationAffectors = new Dictionary<UnityEngine.Object, float>();
		protected Action<float> _onTimeDilationChanged;

		public float _customTimeDilation { get; private set; }

		private void TimeDilationInit() {
			_customTimeDilation = 1.0f;
		}

		public void SetCustomTimeDilation(float timeDilation) {
			_customTimeDilation = timeDilation;
			_onTimeDilationChanged?.Invoke(_customTimeDilation);
		}

		public void AddTimeDilationAffector(UnityEngine.Object obj, float timeDilation) {
			if (_timeDilationAffectors.ContainsKey(obj)) {
				Debug.LogError($"Actor.AddTimeDilationAffector: This affector ({obj.name}) has already been added!");
				return;
			}

			_timeDilationAffectors.Add(obj, timeDilation);
			EvaluateTimeDilation();
		}

		public void RemoveTimeDilationAffector(UnityEngine.Object obj) {
			if (_timeDilationAffectors.ContainsKey(obj) == false) {
				Debug.LogError($"Actor.RemoveTimeDilationAffector: This affector ({obj.name})  doesn't exist in the system!");
				return;
			}

			_timeDilationAffectors.Remove(obj);
			EvaluateTimeDilation();
		}

		public bool ContainsTimeDilationAffector(UnityEngine.Object obj) {
			return _timeDilationAffectors.ContainsKey(obj);
		}

		protected void EvaluateTimeDilation() {
			// If there are no affectors in the system reset custom time dilation and exit.
			if (_timeDilationAffectors.Count < 1) {
				// Debug.Log("TimeDilation.Evaluate: No more affectors in system, setting _customTimeDilation to 1.0f");
				SetCustomTimeDilation(1.0f);
				return;
			}

			// Convert the affectors dictionary to a list so we can sort it.
			var list = _timeDilationAffectors.Values.ToList();

			switch (_timeDilationSorting) {
				case ETimeDilationSorting.Latest:
					// The latest affector will already be the last index in the list.
					SetCustomTimeDilation(list[list.Count - 1]);
					return;
				case ETimeDilationSorting.Slowest:
					// Sort the list in ascending order.
					// Slowest will be first.
					list.Sort((a, b) => a.CompareTo(b));
					SetCustomTimeDilation(list[0]);
					return;
				case ETimeDilationSorting.Fastest:
					// Sort the list in descending order.
					// Fastest will be first.
					list.Sort((a, b) => -1 * a.CompareTo(b));
					SetCustomTimeDilation(list[0]);
					return;
				default:
					SetCustomTimeDilation(1.0f);
					throw new ArgumentOutOfRangeException();
			}
		}
#endregion TimeDilation
		
#region Physics
		public virtual void SetKinematic(bool kinematic) {
			
		}
		
		public virtual void SetVelocity(Vector3 velocity) {
		
		}
		
		public virtual void ApplyForce(Vector3 force) {
			
		}
#endregion Physics

	}
}