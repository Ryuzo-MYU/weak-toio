using ActionLibrary;
using Evaluation;

namespace ActionGenerate
{
  public class MocLedGreenActionGenerator : ActionGenerator
  {
    protected override Action GenerateAction(Result result)
    {
      return actionLibrary.MocLedG();
    }
  }
}