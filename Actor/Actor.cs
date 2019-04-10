using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Actor : MonoBehaviour, ITimeDilatable {
	
#region Init
	public virtual void Init() {
		TimeDilationInit();
	}
#endregion Init
	
#region ITimeDilatable
	[Header("TimeDilation")]
	[SerializeField] protected ETimeDilationSorting _timeDilationSorting = ETimeDilationSorting.Latest;
	
	protected Dictionary<UnityEngine.Object, float> _timeDilationAffectors = new Dictionary<UnityEngine.Object,float>();
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

	protected void EvaluateTimeDilation() {
		// If there are now affectors in the system reset custom time dilation and exit.
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
#endregion ITimeDilatable
	
}