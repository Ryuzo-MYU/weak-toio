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

		public Motion GenerateAction(Result result)
		{
			// 型チェック
			Result temperatureResult = new Result();
			if (result.GetType() != temperatureResult.GetType())
			{
				Debug.Assert(false, "TemperatureResult以外のクラスをいれるな");
				Motion doAnything = new Motion(new Movement(), 0);
			}

			// 型チェックして問題なければ処理を進める
			float score = result.Score;
			Motion action;
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
		private Motion SuitableAction()
		{
			Motion action = new Motion();
			return action;
		}
		private Motion ColdCautionAction()
		{
			Motion action = new Motion();
			return action;
		}
		private Motion HotCautionAction()
		{
			Motion action = new Motion();
			return action;
		}
		private Motion ColdDangerAction()
		{
			Motion action = new Motion();
			return action;
		}
		private Motion HotDangerAction()
		{
			Motion action = new Motion();
			return action;
		}
	}
}