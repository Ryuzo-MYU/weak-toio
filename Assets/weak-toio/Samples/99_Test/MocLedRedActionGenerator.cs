using ActionLibrary;
using Evaluation;

namespace ActionGenerate
{
  public class MocLedRedActionGenerator : ActionGenerator
  {
    protected override Action GenerateAction(Result result)
    {
      return actionLibrary.MocLedR();
    }
  }
}