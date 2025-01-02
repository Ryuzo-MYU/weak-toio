#region 服のアクション生成
using Evaluation;
namespace ActionGenerate
{
	public class ClothesActionGenerator : ActionGenerator
	{
		BoundaryRange suitableRange = new BoundaryRange(0);
		BoundaryRange cautionRange = new BoundaryRange(-10, 10);

		protected override Action GenerateAction(Result result)
		{
			float score = result.Score;

			if (suitableRange.isWithInRange(score))
			{
				return ToioActionLibrary.Clothes_Optimal();
			}
			else if (cautionRange.isWithInRange(score))
			{
				return ToioActionLibrary.Clothes_Optimal();
			}
			else
			{
				return ToioActionLibrary.Clothes_HighHumidity();
			}
		}
	}
}
#endregion