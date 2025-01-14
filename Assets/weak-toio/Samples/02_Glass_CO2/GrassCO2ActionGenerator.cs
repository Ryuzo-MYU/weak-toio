using ActionLibrary;
using Evaluation;
namespace ActionGenerate
{
  public class GrassCO2ActionGenerator : ActionGenerator
  {
    BoundaryRange normalRange = new BoundaryRange(-2, 2);    // 通常範囲
    BoundaryRange activeRange = new BoundaryRange(2, 10);    // 活発な光合成範囲

    protected override Action GenerateAction(Result result)
    {
      float score = result.Score;

      if (score < normalRange.LowerLimit)
      {
        return actionLibrary.Grass_Temp_Hum_Wilting();        // CO2不足でしおれ気味
      }
      else if (normalRange.isWithInRange(score))
      {
        return actionLibrary.Grass_Temp_Hum_Normal();         // 通常の生育状態
      }
      else
      {
        return actionLibrary.Grass_Temp_Hum_Refreshed();      // 活発な光合成による成長
      }
    }
  }
}