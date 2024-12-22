#region 人のアクション生成
using Evaluation;
namespace Robot
{

	public class HumanPaActionGenerator : ActionGenerator
	{
		BoundaryRange suitableRange = new BoundaryRange(0);
		BoundaryRange cautionRange = new BoundaryRange(-200, 200);

		protected override Action GenerateAction(Result result)
		{
			float score = result.Score;

			if (suitableRange.isWithInRange(score))
			{
				return ToioActionLibrary.Human_FreshAir();
			}
			else if (cautionRange.isWithInRange(score))
			{
				return ToioActionLibrary.Human_Normal();
			}
			else
			{
				return ToioActionLibrary.Human_HighCO2();
			}
		}
	}
}
#endregion