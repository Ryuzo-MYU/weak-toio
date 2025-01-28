using Evaluation;

namespace ActionGenerate
{
	public class HumanTempActionGenerator : ActionGenerator
	{
		protected override Action GenerateAction(Result result)
		{
			if (result.Score == 0)
			{
				return actionLibrary.Human_Temp_Suitable();
			}
			else if (result.Score > 0)
			{
				return actionLibrary.Human_Temp_Hot();
			}
			else
			{
				return actionLibrary.Human_Temp_Cold();
			}
		}
	}
}