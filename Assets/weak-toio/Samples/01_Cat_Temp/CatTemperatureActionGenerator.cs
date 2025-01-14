using ActionLibrary;
using Evaluation;

namespace ActionGenerate
{
  public class CatTemperatureActionGenerator : ActionGenerator
  {
    BoundaryRange suitableRange = new BoundaryRange(0);
    BoundaryRange cautionRange = new BoundaryRange(-3, 3);
    BoundaryRange dangerRange = new BoundaryRange(-6, 6);

    protected override Action GenerateAction(Result result)
    {
      float score = result.Score;

      if (suitableRange.isWithInRange(score))
      {
        return actionLibrary.Cat_Temp_Comfortable();
      }
      else if (cautionRange.isWithInRange(score))
      {
        return score < 0 ? actionLibrary.Cat_Temp_Cold() : actionLibrary.Cat_Temp_Hot();
      }
      else
      {
        Action action = score < 0 ? actionLibrary.Cat_Temp_Cold() : actionLibrary.Cat_Temp_Hot();
        action *= 2;
        return action;
      }
    }
  }
}