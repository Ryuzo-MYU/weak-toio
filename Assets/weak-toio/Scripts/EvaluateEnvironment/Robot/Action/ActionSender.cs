using System.Collections.Generic;
using System.Linq;

namespace Robot
{
	public class ActionSender
	{
		private List<Action> Actions;
		public void Update(Action action)
		{
			UpdateAction(action);
		}
		public Action NextAction()
		{
			var oldestAction = Actions.First();
			Actions.Remove(Actions.First());
			return oldestAction;
		}
		private void UpdateAction(Action action)
		{
			if (action != null)
			{
				Actions.Add(action);
			}
		}
	}
}