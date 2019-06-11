using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gruel.CommandQueue {
	public class CommandQueueController : MonoBehaviour {
		
#region Public
		public void Init() {
			_commandQueue = new Queue<Action>();
		}
		
		public static void AddCommand(Action command) {
			_commandQueue.Enqueue(command);
		}
#endregion Public
		
#region Private
		private static Queue<Action> _commandQueue;
		
		private void Update() {
			if (_commandQueue.Count > 0) {
				_commandQueue.Dequeue()();
			}
		}
#endregion Private

	}
}