using Evaluation;
namespace Robot
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
                return ToioActionLibrary.Banana_Normal();
            }
            else if (warningRange.isWithInRange(score))
            {
                return ToioActionLibrary.Banana_Warning();
            }
            else
            {
                return ToioActionLibrary.Banana_Rotting();
            }
        }
    }
}