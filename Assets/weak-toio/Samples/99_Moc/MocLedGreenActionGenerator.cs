using Evaluation;

namespace ActionGenerate
{
	public class MocLedGreenActionGenerator : ActionGenerator
	{
		protected override Action GenerateAction(Result result)
		{
			return ToioActionLibrary.MocLedG();
		}
	}
}