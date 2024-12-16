using System.Collections.Generic;
using Evaluation;

namespace Robot
{
	public class CO2ActionGenerator : ActionGenerator
	{
		private float suitableScore = 0;
		private float cautionScore = 150;
		private float dangerScore = 300;


		protected override Action GenerateAction(Result result)
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
			Action action = new Action();
			float deg = 90;
			double speed = 45;
			action.AddMovement(ToioActionLibrary.DegRotate(deg, speed));
			action.AddMovement(ToioActionLibrary.DegRotate(-deg, speed));

			int soundID = 1;
			int volume = 1;
			action.AddSound(ToioActionLibrary.PresetSound(soundID, volume, 2f));

			return action;
		}

		private Action CautionAction()
		{
			Action action = new Action();
			float deg = 50f;
			double speed = 50;
			action.AddMovement(ToioActionLibrary.DegRotate(deg, speed));
			action.AddMovement(ToioActionLibrary.DegRotate(-deg, speed));

			return action;
		}

		private Action DangerAction()
		{
			Action action = new Action();
			float deg = 90f;
			double speed = 200;
			action.AddMovement(ToioActionLibrary.DegRotate(deg, speed));
			action.AddMovement(ToioActionLibrary.DegRotate(-deg, speed));

			return action;
		}
	}
}