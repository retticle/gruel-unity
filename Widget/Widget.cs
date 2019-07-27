using System.Collections;
using Gruel.CoroutineUtils;
using UnityEngine;

namespace Gruel.Widget {
	public abstract class Widget : MonoBehaviour {
	
#region Properties
		/// <summary>
		/// Whether the widget is active or not.
		/// </summary>
		public bool Active {
			get => _active;
			protected set => _active = value;
		}
		
		/// <summary>
		/// Priority used to determine which Widget is rendered ontop of others.
		/// 0 is the highest priority.
		/// </summary>
		public virtual int OrderPriority {
			get => _orderPriority;
			protected set => _orderPriority = value;
		}
#endregion Properties

#region Fields
		[Header("Widget Base")]
		[SerializeField] protected int _orderPriority = 1;
		
		protected bool _active;
#endregion Fields

#region Public Methods
		/// <summary>
		/// Async initialize the widget.
		/// </summary>
		/// <returns></returns>
		public Coroutine Init() {
			return CoroutineRunner.StartCoroutine(InitCor());
		}
		
		/// <summary>
		/// Set the widget to be active or inactive.
		/// </summary>
		/// <param name="active"></param>
		/// <returns></returns>
		public Coroutine SetActive(bool active) {
			return CoroutineRunner.StartCoroutine(SetActiveCor(active));
		}
		
		/// <summary>
		/// Destroys the widget.
		/// </summary>
		public abstract void Remove();
#endregion Public Methods

#region Private Methods
		protected abstract IEnumerator InitCor();
		protected abstract IEnumerator SetActiveCor(bool active);
#endregion Private Methods

	}
}