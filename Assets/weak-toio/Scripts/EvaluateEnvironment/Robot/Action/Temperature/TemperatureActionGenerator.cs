using System.Collections;
using Codice.Client.Commands;
using Evaluation;
using toio;
using UnityEngine;

namespace Robot
{
	public class TemperatureActionGenerator : ActionGenerator
	{
		BoundaryRange SuitableRange = new BoundaryRange(0);
		BoundaryRange CautionRange = new BoundaryRange(-5, 5);
		BoundaryRange DangerRange = new BoundaryRange(-10, 10);

		public override MovementOperation GenerateAction(Result result)
		{
			// 型チェック
			TemperatureResult temperatureResult = new TemperatureResult();
			if (result.GetType() != temperatureResult.GetType())
			{
				Debug.Assert(false, "TemperatureResult以外のクラスをいれるな");
				MovementOperation doAnything = new MovementOperation(new Movement(), 0);
			}

			// 型チェックして問題なければ処理を進める
			int score = result.Score;
			MovementOperation action;
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

		private MovementOperation SuitableAction()
		{
			MovementOperation operation = new MovementOperation();
			return operation;
		}
		private MovementOperation ColdCautionAction()
		{
			MovementOperation operation = new MovementOperation();
			return operation;
		}
		private MovementOperation HotCautionAction()
		{
			MovementOperation operation = new MovementOperation();
			return operation;
		}
		private MovementOperation ColdDangerAction()
		{
			MovementOperation operation = new MovementOperation();
			return operation;
		}
		private MovementOperation HotDangerAction()
		{
			MovementOperation operation = new MovementOperation();
			return operation;
		}
	}
}