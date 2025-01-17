using ActionLibrary;
using Evaluation;

namespace ActionGenerate
{
  public class BananaTempHumActionGenerator : ActionGenerator
  {
    BoundaryRange normalRange = new BoundaryRange(-3, 3);    // 正常範囲
    BoundaryRange warningRange = new BoundaryRange(-6, 6);   // 警戒範囲

    protected override Action GenerateAction(Result result)
    {
      float score = result.Score;

      if (normalRange.isWithInRange(score))
      {
        return actionLibrary.Banana_Temp_Hum_Normal();
      }
      else if (warningRange.isWithInRange(score))
      {
        return actionLibrary.Banana_Temp_Hum_Warning();
      }
      else
      {
        return actionLibrary.Banana_Temp_Hum_Rotting();
      }
    }
  }
}