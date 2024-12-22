using Evaluation;
namespace Robot
{
	public class GrassCO2ActionGenerator : ActionGenerator
	{
		BoundaryRange suitableRange = new BoundaryRange(0);
		BoundaryRange cautionRange = new BoundaryRange(-10, 10);

		protected override Action GenerateAction(Result result)
		{
			float score = result.Score;

			if (suitableRange.isWithInRange(score))
			{
				return ToioActionLibrary.Grass_Normal();
			}
			else if (score < 0)
			{
				return ToioActionLibrary.Grass_Wilting();
			}
			else
			{
				return ToioActionLibrary.Grass_Refreshed();
			}
		}
	}
}