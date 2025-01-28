using Environment;
using UnityEngine;

namespace Evaluation
{
	[RequireComponent(typeof(ITemperatureSensor))]
	public class CatTemperatureEvaluate : EvaluateBase
	{
		[SerializeField] private BoundaryRange suitableRange = new BoundaryRange(24, 18); // 猫の快適温度
		public BoundaryRange SuitableRange { get { return suitableRange; } }
		private ITemperatureSensor tempSensor;

		protected override void GenerateEvaluationResult()
		{
			_currentParam = tempSensor.GetTemperature();

			string message;
			if (suitableRange.isWithInRange(_currentParam))
			{
				_score = 0;
				message = "猫にとって適温です";
			}
			else if (_currentParam < suitableRange.LowerLimit)
			{
				_score = _currentParam - suitableRange.LowerLimit;
				message = "猫にとって寒いです";
			}
			else
			{
				_score = _currentParam - suitableRange.UpperLimit;
				message = "猫にとって暑いです";
			}

			_OnResultGenerated(new Result(_score, Unit, message));
		}

		protected override void OnSensorDecided()
		{
			base.OnSensorDecided();
			tempSensor = (ITemperatureSensor)sensorManager.GetSensor();
		}

		protected override void OnDeserializeCompleted()
		{
			GenerateEvaluationResult();
		}
	}
}