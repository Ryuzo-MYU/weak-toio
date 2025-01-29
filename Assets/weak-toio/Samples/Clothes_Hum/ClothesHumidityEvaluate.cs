using Environment;
using UnityEngine;

namespace Evaluation
{
	[RequireComponent(typeof(IHumiditySensor))]
	public class ClothesHumidityEvaluate : EvaluateBase
	{
		[SerializeField] private BoundaryRange suitableRange = new BoundaryRange(30, 50); // 衣類の保管適正湿度
		private IHumiditySensor humiditySensor;

		protected override void GenerateEvaluationResult()
		{
			_currentParam = humiditySensor.GetHumidity();

			string message;
			if (suitableRange.isWithInRange(_currentParam))
			{
				_score = 0;
				message = "湿度は適正範囲内です";
			}
			else if (_currentParam < suitableRange.LowerLimit)
			{
				_score = _currentParam - suitableRange.LowerLimit;
				message = "湿度が低すぎます";
			}
			else
			{
				_score = _currentParam - suitableRange.UpperLimit;
				message = "湿度が高すぎます";
			}

			OnResultGenerated(new Result(_score, Unit, message));
		}

		protected override void OnSensorDecided()
		{
			base.OnSensorDecided();
			humiditySensor = (IHumiditySensor)sensorManager.GetSensor();
		}

		protected override void OnDeserializeCompleted()
		{
			GenerateEvaluationResult();
		}
	}
}