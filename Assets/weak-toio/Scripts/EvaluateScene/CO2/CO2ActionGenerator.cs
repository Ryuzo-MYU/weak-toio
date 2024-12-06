using System;
using System.Collections.Generic;
using Evaluation;

namespace Robot
{
	public class CO2ActionGenerator : ActionGenerator, ActionSender
	{
		private float suitableScore;
		private float cautionScore;
		private float dangerScore;

		public Action GenerateAction(Result result)
		{
			float score = result.Score;

			Action action;
			if (score > dangerScore) action = DangerAction();
			else if (score > cautionScore) action = CautionAction();
			else action = SuitableAction();

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
		private Action CautionAction()
		{
			Queue<Motion> cautionTwist = new Queue<Motion>();
			float deg = 50f;
			double speed = 50;
			cautionTwist.Enqueue(DegRotate(deg, speed));
			cautionTwist.Enqueue(DegRotate(-deg, speed));

			Action action = new Action(cautionTwist);
			return action;
		}
		private Action DangerAction()
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