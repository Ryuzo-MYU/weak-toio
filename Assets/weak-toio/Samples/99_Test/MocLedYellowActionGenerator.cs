using ActionLibrary;
using Evaluation;
namespace ActionGenerate
{
  public class MocLedYellowActionGenerator : ActionGenerator
  {
    protected override Action GenerateAction(Result result)
    {
      return actionLibrary.MocLedY();
    }
  }
}