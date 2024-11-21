using System.Collections;
using Codice.Client.Commands;
using Evaluation;
using toio;
using UnityEngine;

namespace Robot
{
	public class TemperatureActionGenerator : ActionGenerator, ActionSender
	{
		BoundaryRange SuitableRange = new BoundaryRange(0);
		BoundaryRange CautionRange = new BoundaryRange(-5, 5);
		BoundaryRange DangerRange = new BoundaryRange(-10, 10);

		public Action GenerateAction(Result result)
		{
			// 型チェック
			TemperatureResult temperatureResult = new TemperatureResult();
			if (result.GetType() != temperatureResult.GetType())
			{
				Debug.Assert(false, "TemperatureResult以外のクラスをいれるな");
				Action doAnything = new Action(new Movement(), 0);
			}

			// 型チェックして問題なければ処理を進める
			int score = result.Score;
			Action action;
			if (score == 0)
			{
				action = SuitableAction();
			}
			else if (CautionRange.isWithInRange(score))
			{
				if (score < 0) { action = ColdCautionAction(); }
				else { action = HotCautionAction(); }
			}
			else
			{
				if (score < 0) { action = ColdDangerAction(); }
				else { action = HotDangerAction(); }
			}
			return action;
		}

		private Action SuitableAction()
		{
			Action action = new Action();
			return action;
		}
		private Action ColdCautionAction()
		{
			Action action = new Action();
			return action;
		}
		private Action HotCautionAction()
		{
			Action action = new Action();
			return action;
		}
		private Action ColdDangerAction()
		{
			Action action = new Action();
			return action;
		}
		private Action HotDangerAction()
		{
			Action action = new Action();
			return action;
		}
	}
}