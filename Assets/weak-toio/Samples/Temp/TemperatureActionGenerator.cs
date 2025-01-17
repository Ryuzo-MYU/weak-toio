using System;
using System.Collections.Generic;
using Evaluation;

namespace ActionGenerate
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
			Action action = new Action();
			float deg = 90;
			double speed = 45;
			action.AddMovement(actionLibrary.DegRotate(deg, speed));
			action.AddMovement(actionLibrary.DegRotate(-deg, speed));

			int soundID = 1;
			int volume = 1;
			action.AddSound(actionLibrary.PresetSound(soundID, volume, 2f));

			return action;
		}

		private Action ColdCautionAction()
		{
			Action action = new Action();
			float rad = (float)(10 * Math.PI / 180);
			double speed = 50;
			action.AddMovement(actionLibrary.RadRotate(rad, speed));
			action.AddMovement(actionLibrary.RadRotate(-rad, speed));

			return action;
		}

		private Action ColdDangerAction()
		{
			Action action = new Action();
			float deg = 10f;
			double speed = 100;
			action.AddMovement(actionLibrary.DegRotate(deg, speed));
			action.AddMovement(actionLibrary.DegRotate(-deg, speed));

			return action;
		}

		private Action HotCautionAction()
		{
			Action action = new Action();
			float deg = 50f;
			double speed = 50;
			action.AddMovement(actionLibrary.DegRotate(deg, speed));
			action.AddMovement(actionLibrary.DegRotate(-deg, speed));

			return action;
		}

		private Action HotDangerAction()
		{
			Action action = new Action();
			float deg = 90f;
			double speed = 200;
			action.AddMovement(actionLibrary.DegRotate(deg, speed));
			action.AddMovement(actionLibrary.DegRotate(-deg, speed));

			return action;
		}
	}
}