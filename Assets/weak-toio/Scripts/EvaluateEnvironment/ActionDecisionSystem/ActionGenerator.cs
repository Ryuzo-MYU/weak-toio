
using EvaluateEnvironment;
using toio;

namespace ActionDecisionSystem
{
	public abstract class ActionGenerator
	{
		public abstract void GenerateAction(Result result);
	}
	public abstract class Action
	{
		public Action(Movement[] moves)
		{
			movements = moves;
		}
		protected Movement[] movements;
	}
}