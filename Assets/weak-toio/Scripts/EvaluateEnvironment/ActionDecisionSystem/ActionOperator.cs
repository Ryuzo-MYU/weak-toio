using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
namespace ActionDecisionSystem
{
	public class ActionOperator
	{
		private List<Action> Actions;
		public void Update() { }
		public void ExecuteAction(Action[] actions) { }
		public void UpdateAction(Action action)
		{
			if (action != null)
			{
				Actions.Add(action);
			}
		}
	}
}