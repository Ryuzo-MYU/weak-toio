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

			if (suitableRange.isWithInRange(_currentParam))
			{
				_score = 0;
			}
			else if (_currentParam < suitableRange.LowerLimit)
			{
				_score = _currentParam - suitableRange.LowerLimit;
			}
			else
			{
				_score = _currentParam - suitableRange.UpperLimit;
			}

			_OnResultGenerated(new Result(_score, _unit));
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