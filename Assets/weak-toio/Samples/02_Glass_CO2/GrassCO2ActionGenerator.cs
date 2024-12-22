using Evaluation;
namespace Robot
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
				return ToioActionLibrary.Grass_Wilting();        // CO2不足でしおれ気味
			}
			else if (normalRange.isWithInRange(score))
			{
				return ToioActionLibrary.Grass_Normal();         // 通常の生育状態
			}
			else
			{
				return ToioActionLibrary.Grass_Refreshed();      // 活発な光合成による成長
			}
		}
	}
}