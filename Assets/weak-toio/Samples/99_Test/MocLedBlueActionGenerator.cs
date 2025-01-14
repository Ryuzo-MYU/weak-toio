using ActionLibrary;
using Evaluation;

namespace ActionGenerate
{
  public class MocLedBlueActionGenerator : ActionGenerator
  {
    protected override Action GenerateAction(Result result)
    {
      return actionLibrary.MocLedB();
    }
  }
}