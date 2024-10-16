using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EvaluateEnvironment;

namespace ActionDecisionSystem
{
	public class ActionOperator
	{
		Evaluation evaluation;
		Result result;
		Action action;
		public Action GetAction()
		{
			return action;
		}
	}
	public abstract class Action { }
}