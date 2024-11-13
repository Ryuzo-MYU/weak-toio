using Evaluation;
using toio;

namespace Robot
{
	public abstract class ActionGenerator
	{
		public abstract Action GenerateAction(Result result);
	}
}