using ActionLibrary;
using Evaluation;

namespace ActionGenerate
{
  public class CO2ActionGenerator : ActionGenerator
  {
    private float suitableScore = 0;
    private float cautionScore = 150;
    private float dangerScore = 300;

    protected override Action GenerateAction(Result result)
    {
      float score = result.Score;

      Action action;
      if (score > dangerScore) action = actionLibrary.Test_CO2_Danger();
      else if (score > cautionScore) action = actionLibrary.Test_CO2_Caution();
      else action = actionLibrary.Test_CO2_Suitable();

      return action;
    }
  }
}