using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gruel.CommandQueue {
	public class CommandQueue : MonoBehaviour {
		
#region Properties
#endregion Properties

#region Fields
		private static Queue<Action> _commandQueue;
#endregion Fields

#region Public Methods
		public void Init() {
			_commandQueue = new Queue<Action>();
		}
		
		public static void AddCommand(Action command) {
			_commandQueue.Enqueue(command);
		}
#endregion Public Methods

#region Private Methods
		private void Update() {
			if (_commandQueue.Count > 0) {
				_commandQueue.Dequeue()();
			}
		}
#endregion Private Methods

	}
}