using Environment;
using UnityEngine;

namespace Evaluation
{
	[RequireComponent(typeof(ITemperatureSensor))]
	public class CatTemperatureEvaluate : EvaluateBase, IEvaluationResultSender<ITemperatureSensor>
	{
		[SerializeField] private BoundaryRange suitableRange = new BoundaryRange(18, 24); // 猫の快適温度
		private ITemperatureSensor tempSensor;

		public void GenerateEvaluationResult(ITemperatureSensor tempSensor)
		{
			_currentParam = tempSensor.GetTemperature();

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
			tempSensor = (ITemperatureSensor)sensorManager.GetSensor();
		}

		protected override void OnDeserializeCompleted()
		{
			GenerateEvaluationResult(tempSensor);
		}
	}
}