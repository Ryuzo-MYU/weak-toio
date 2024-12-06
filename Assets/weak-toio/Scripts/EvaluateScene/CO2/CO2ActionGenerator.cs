using System;
using System.Collections.Generic;
using Evaluation;
using toio;
using UnityEngine;

namespace Robot
{
	public class CO2ActionGenerator : ActionGenerator, ActionSender
	{
		BoundaryRange SuitableRange = new BoundaryRange(0);
		BoundaryRange CautionRange = new BoundaryRange(-5, 5);
		BoundaryRange DangerRange = new BoundaryRange(-10, 10);

		public Action GenerateAction(Result result)
		{
			// 型チェック
			Result co2Result = new Result();
			if (result.GetType() != co2Result.GetType())
			{
				Debug.Assert(false, "co2Result以外のクラスをいれるな");
				Motion doAnything = new Motion(new TranslateCommand(0, 0), 0);
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
			Queue<Motion> suitableAction = new Queue<Motion>();
			float deg = 90;
			double speed = 45;
			suitableAction.Enqueue(DegRotate(deg, speed));
			suitableAction.Enqueue(DegRotate(-deg, speed));

			int soundID = 1;
			int volume = 1;
			suitableAction.Enqueue(PresetSound(soundID, volume));

			Action action = new Action(suitableAction);
			return action;
		}
		private Action ColdCautionAction()
		{
			Queue<Motion> cautionShiver = new Queue<Motion>();
			float rad = (float)(10 * Math.PI / 180);
			double speed = 50;
			cautionShiver.Enqueue(RadRotate(rad, speed));
			cautionShiver.Enqueue(RadRotate(-rad, speed));

			Action action = new Action(cautionShiver);
			return action;
		}
		private Action ColdDangerAction()
		{
			Queue<Motion> dangerShiver = new Queue<Motion>();
			float deg = 10f;
			double speed = 100;
			dangerShiver.Enqueue(DegRotate(deg, speed));
			dangerShiver.Enqueue(DegRotate(-deg, speed));

			Action action = new Action(dangerShiver);
			return action;
		}
		private Action HotCautionAction()
		{
			Queue<Motion> cautionTwist = new Queue<Motion>();
			float deg = 50f;
			double speed = 50;
			cautionTwist.Enqueue(DegRotate(deg, speed));
			cautionTwist.Enqueue(DegRotate(-deg, speed));

			Action action = new Action(cautionTwist);
			return action;
		}
		private Action HotDangerAction()
		{
			Queue<Motion> dangerTwist = new Queue<Motion>();
			float deg = 90f;
			double speed = 200;
			dangerTwist.Enqueue(DegRotate(deg, speed));
			dangerTwist.Enqueue(DegRotate(-deg, speed));

			Action action = new Action(dangerTwist);
			return action;
		}
	}
}