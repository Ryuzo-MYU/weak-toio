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
				return ToioActionLibrary.Human_NormalPressure();
			}
			else if (warningRange.isWithInRange(score))
			{
				return ToioActionLibrary.Human_SensingPressure();
			}
			else
			{
				return ToioActionLibrary.Human_SufferingPressure();
			}
		}
	}
}