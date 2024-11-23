using System.Collections;
using System.Collections.Generic;
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
			Result temperatureResult = new Result();
			if (result.GetType() != temperatureResult.GetType())
			{
				Debug.Assert(false, "TemperatureResult以外のクラスをいれるな");
				Motion doAnything = new Motion(new Movement(), 0);
			}

			// 型チェックして問題なければ処理を進める
			float score = result.Score;
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
			Queue<Motion> motions = new Queue<Motion>();
			motions.Enqueue(Translate(30, 30));

			Action action = new Action(motions);
			return action;
		}
		private Action ColdCautionAction()
		{
			Queue<Motion> motions = new Queue<Motion>();
			Action action = new Action(motions);
			return action;
		}
		private Action HotCautionAction()
		{
			Queue<Motion> motions = new Queue<Motion>();
			Action action = new Action(motions);
			return action;
		}
		private Action ColdDangerAction()
		{
			Queue<Motion> motions = new Queue<Motion>();
			Action action = new Action(motions);
			return action;
		}
		private Action HotDangerAction()
		{
			Queue<Motion> motions = new Queue<Motion>();
			Action action = new Action(motions);
			return action;
		}
	}
}