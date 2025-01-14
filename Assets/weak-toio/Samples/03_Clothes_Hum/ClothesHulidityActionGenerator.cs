using ActionLibrary;
using Evaluation;
namespace ActionGenerate
{
  public class ClothHumActionGenerator : ActionGenerator
  {
    BoundaryRange suitableRange = new BoundaryRange(0);
    BoundaryRange cautionRange = new BoundaryRange(-10, 10);

    protected override Action GenerateAction(Result result)
    {
      float score = result.Score;

      if (suitableRange.isWithInRange(score))
      {
        return actionLibrary.Clothes_Hum_Good();
      }
      else if (cautionRange.isWithInRange(score))
      {
        return actionLibrary.Clothes_Hum_Normal();
      }
      else
      {
        return actionLibrary.Clothes_Hum_Bad();
      }
    }
  }
}