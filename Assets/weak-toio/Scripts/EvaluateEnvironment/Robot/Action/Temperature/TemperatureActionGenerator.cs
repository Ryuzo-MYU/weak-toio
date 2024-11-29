using System.Collections.Generic;
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

		public TemperatureActionGenerator(IToioActionGenerator toio)
		{
			_toio = toio;
		}
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
			Queue<Motion> suitableRotate = new Queue<Motion>();
			suitableRotate.Enqueue(Rotate(90, 45));
			suitableRotate.Enqueue(Rotate(-90, 45));

			Action action = new Action(suitableRotate);
			return action;
		}
		private Action ColdCautionAction()
		{
			Queue<Motion> CautionShiver = new Queue<Motion>();
			CautionShiver.Enqueue(Rotate(10, 50));
			CautionShiver.Enqueue(Rotate(-10, 50));

			Action action = new Action(CautionShiver);
			return action;
		}
		private Action ColdDangerAction()
		{
			Queue<Motion> DangerShiver = new Queue<Motion>();
			DangerShiver.Enqueue(Rotate(10, 100));
			DangerShiver.Enqueue(Rotate(-10, 100));

			Action action = new Action(DangerShiver);
			return action;
		}
		private Action HotCautionAction()
		{
			Queue<Motion> CautionTwist = new Queue<Motion>();
			CautionTwist.Enqueue(Rotate(45, 50));
			CautionTwist.Enqueue(Rotate(-45, 50));

			Action action = new Action(CautionTwist);
			return action;
		}
		private Action HotDangerAction()
		{
			Queue<Motion> DangerTwist = new Queue<Motion>();
			DangerTwist.Enqueue(Rotate(90, 200));
			DangerTwist.Enqueue(Rotate(-90, 200));

			Action action = new Action(DangerTwist);
			return action;
		}
	}
}