using Codice.Client.Commands;
using Evaluation;
using toio;
using UnityEngine;

namespace Robot
{
	public class TemperatureActionGenerator : ActionGenerator
	{

		public override Action GenerateAction(Result result)
		{
			// 型チェック
			TemperatureResult temperatureResult = new TemperatureResult();
			if (result.GetType() != temperatureResult.GetType())
			{
				Debug.Assert(false, "TemperatureResult以外のクラスをいれるな");
				return null;
			}

			// 型チェックして問題なければ処理を進める
			int score = result.Score;
			
			if (score == 0)
			{
				TemperatureAction action = GenerateSuitableAction();
				return action;
			}
			else if ()
			{
				TemperatureAction action = GenerateCautionAction();
				return action;
			}
			else
			{
				TemperatureAction action = GenerateDangerAction();
				return action;
			}
		}
	}
}