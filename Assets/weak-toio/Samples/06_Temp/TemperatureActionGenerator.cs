using System;
using System.Collections.Generic;
using Evaluation;

namespace Robot
{
	public class TemperatureActionGenerator : ActionGenerator
	{
		BoundaryRange SuitableRange = new BoundaryRange(0);
		BoundaryRange CautionRange = new BoundaryRange(-5, 5);
		BoundaryRange DangerRange = new BoundaryRange(-10, 10);

		protected override Action GenerateAction(Result result)
		{
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
			Queue<MovementMotion> suitableAction = new Queue<MovementMotion>();
			float deg = 90;
			double speed = 45;
			suitableAction.Enqueue(ToioActionLibrary.DegRotate(deg, speed));
			suitableAction.Enqueue(ToioActionLibrary.DegRotate(-deg, speed));

			int soundID = 1;
			int volume = 1;
			suitableAction.Enqueue(ToioActionLibrary.PresetSound(soundID, volume));

			Action action = new Action(suitableAction);
			return action;
		}

		private Action ColdCautionAction()
		{
			Queue<MovementMotion> cautionShiver = new Queue<MovementMotion>();
			float rad = (float)(10 * Math.PI / 180);
			double speed = 50;
			cautionShiver.Enqueue(ToioActionLibrary.RadRotate(rad, speed));
			cautionShiver.Enqueue(ToioActionLibrary.RadRotate(-rad, speed));

			Action action = new Action(cautionShiver);
			return action;
		}

		private Action ColdDangerAction()
		{
			Queue<MovementMotion> dangerShiver = new Queue<MovementMotion>();
			float deg = 10f;
			double speed = 100;
			dangerShiver.Enqueue(ToioActionLibrary.DegRotate(deg, speed));
			dangerShiver.Enqueue(ToioActionLibrary.DegRotate(-deg, speed));

			Action action = new Action(dangerShiver);
			return action;
		}

		private Action HotCautionAction()
		{
			Queue<MovementMotion> cautionTwist = new Queue<MovementMotion>();
			float deg = 50f;
			double speed = 50;
			cautionTwist.Enqueue(ToioActionLibrary.DegRotate(deg, speed));
			cautionTwist.Enqueue(ToioActionLibrary.DegRotate(-deg, speed));

			Action action = new Action(cautionTwist);
			return action;
		}

		private Action HotDangerAction()
		{
			Queue<MovementMotion> dangerTwist = new Queue<MovementMotion>();
			float deg = 90f;
			double speed = 200;
			dangerTwist.Enqueue(ToioActionLibrary.DegRotate(deg, speed));
			dangerTwist.Enqueue(ToioActionLibrary.DegRotate(-deg, speed));

			Action action = new Action(dangerTwist);
			return action;
		}
	}
}