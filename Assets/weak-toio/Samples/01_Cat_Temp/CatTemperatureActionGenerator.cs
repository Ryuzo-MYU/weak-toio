#region 猫のアクション生成
using Evaluation;
namespace Robot
{
	public class CatActionGenerator : ActionGenerator
	{
		BoundaryRange suitableRange = new BoundaryRange(0);
		BoundaryRange cautionRange = new BoundaryRange(-3, 3);
		BoundaryRange dangerRange = new BoundaryRange(-6, 6);

		protected override Action GenerateAction(Result result)
		{
			float score = result.Score;

			if (suitableRange.isWithInRange(score))
			{
				return ToioActionLibrary.Cat_Comfortable();
			}
			else if (cautionRange.isWithInRange(score))
			{
				return score < 0 ? ToioActionLibrary.Cat_Cold() : ToioActionLibrary.Cat_Hot();
			}
			else
			{
				// より激しい寒さ・暑さの表現のため、アクションを連続させる
				Action action = score < 0 ? ToioActionLibrary.Cat_Cold() : ToioActionLibrary.Cat_Hot();
				action.Repeat(2); // アクションを2回繰り返す
				return action;
			}
		}
	}
}
#endregion