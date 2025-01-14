using ActionLibrary;
using Evaluation;

namespace ActionGenerate
{
  public class LightTestActionGenerator : ActionGenerator
  {
    protected override Action GenerateAction(Result result)
    {
      return actionLibrary.LightTest();
    }
  }
}