using Environment;
using UnityEngine;

namespace Evaluation
{
	[RequireComponent(typeof(ITemperatureSensor))]
	public class HumanTempEvaluate : EvaluateBase
	{
		/// 空調設備のある部屋の気温基準は 17℃以上28℃以下
		/// https://www.mhlw.go.jp/bunya/kenkou/seikatsu-eisei10/

		[SerializeField] BoundaryRange suitableRange = new BoundaryRange(17, 28);
		string message;
		private ITemperatureSensor tempSensor;
		protected override void GenerateEvaluationResult()
		{
			_currentParam = tempSensor.GetTemperature();
			if (!suitableRange.isWithInRange(_currentParam))
			{
				if (_currentParam < suitableRange.LowerLimit)
				{
					_score = suitableRange.LowerLimit + _currentParam;
					message = "温度が低すぎます。暖房を使用してください。";
				}
				else if (suitableRange.UpperLimit < _currentParam)
				{
					_score = _currentParam - suitableRange.UpperLimit;
					message = "温度が高すぎます。冷房を使用してください。";
				}
			}
			else
			{
				_score = 0;
				message = "温度は適切です。";
			}
			_OnResultGenerated(new Result(_score, Unit, this.message));
		}

		protected override void OnSensorDecided()
		{
			base.OnSensorDecided();
			tempSensor = (ITemperatureSensor)sensorManager.GetSensor();
			Debug.Log(sensor.GetType());
		}

		protected override void OnDeserializeCompleted()
		{
			GenerateEvaluationResult();
		}
	}
}