using Evaluation;

namespace ActionGenerate
{
	public class HumanTempActionGenerator : ActionGenerator
	{
		protected override Action GenerateAction(Result result)
		{
			if (result.Score == 0)
			{
				return actionLibrary.Human_Temp_Good();
			}
			else if (result.Score > 0 && result.Score <= 5)
			{
				return actionLibrary.Human_Temp_Normal();
			}
			else
			{
				return actionLibrary.Human_Temp_Bad();
			}
		}
	}
}