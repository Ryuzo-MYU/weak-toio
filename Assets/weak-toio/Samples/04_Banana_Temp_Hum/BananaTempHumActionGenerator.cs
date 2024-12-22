#region PCのアクション生成
using Evaluation;
namespace Robot
{
	public class BananaTempHumActionGenerator : ActionGenerator
	{
		BoundaryRange suitableRange = new BoundaryRange(0);
		BoundaryRange cautionRange = new BoundaryRange(-5, 5);

		protected override Action GenerateAction(Result result)
		{
			float score = result.Score;

			if (suitableRange.isWithInRange(score))
			{
				return ToioActionLibrary.PC_Optimal();
			}
			else if (cautionRange.isWithInRange(score))
			{
				return ToioActionLibrary.PC_Normal();
			}
			else
			{
				Action action = ToioActionLibrary.PC_Uncomfortable();
				return action;
			}
		}
	}
}
#endregion