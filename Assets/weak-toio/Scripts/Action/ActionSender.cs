using Evaluation;

namespace Robot
{
	public interface ActionSender
	{
		public Action GenerateAction(Result result);
	}
}