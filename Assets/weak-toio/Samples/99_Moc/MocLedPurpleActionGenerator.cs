using Evaluation;

namespace ActionGenerate
{
	public class MocLedPurpleActionGenerator : ActionGenerator
    {
        protected override Action GenerateAction(Result result)
        {
            return ToioActionLibrary.MocLedP();
        }
    }
}