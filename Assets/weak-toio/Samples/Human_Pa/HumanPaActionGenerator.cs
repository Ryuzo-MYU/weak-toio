using ActionLibrary;
using Evaluation;
namespace ActionGenerate
{
  public class HumanPaActionGenerator : ActionGenerator
  {
    BoundaryRange comfortRange = new BoundaryRange(-10, 10);     // 快適範囲
    BoundaryRange warningRange = new BoundaryRange(-20, 20);     // 警戒範囲

    protected override Action GenerateAction(Result result)
    {
      float score = result.Score;

      if (comfortRange.isWithInRange(score))
      {
        return actionLibrary.Human_Pa_Good();
      }
      else if (warningRange.isWithInRange(score))
      {
        return actionLibrary.Human_Pa_Normal();
      }
      else
      {
        return actionLibrary.Human_Pa_Bad();
      }
    }
  }
}