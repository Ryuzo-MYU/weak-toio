using Environment;
using UnityEngine;

namespace Evaluation
{
	[RequireComponent(typeof(ITemperatureSensor))]
	[RequireComponent(typeof(IHumiditySensor))]
	public class BananaTempHumEvaluate : EvaluateBase
	{
		[SerializeField] private BoundaryRange suitableTempRange = new BoundaryRange(10, 35); // PCの動作適正温度
		[SerializeField] private BoundaryRange suitableHumidRange = new BoundaryRange(20, 80); // PCの動作適正湿度
		private ITemperatureSensor tempSensor;
		private IHumiditySensor humiditySensor;

		protected override void GenerateEvaluationResult()
		{
			float tempScore = 0;
			float humidScore = 0;

			// 温度評価
			float temp = tempSensor.GetTemperature();
			if (!suitableTempRange.isWithInRange(temp))
			{
				if (temp < suitableTempRange.LowerLimit)
				{
					tempScore = (temp - suitableTempRange.LowerLimit); // 低温は比較的許容
				}
				else
				{
					tempScore = (temp - suitableTempRange.UpperLimit) * 1.5f; // 高温はより深刻
				}
			}

			// 湿度評価
			float humidity = humiditySensor.GetHumidity();
			if (!suitableHumidRange.isWithInRange(humidity))
			{
				if (humidity < suitableHumidRange.LowerLimit)
				{
					humidScore = (humidity - suitableHumidRange.LowerLimit); // 低湿度は比較的許容
				}
				else
				{
					humidScore = (humidity - suitableHumidRange.UpperLimit) * 1.5f; // 高湿度はより深刻
				}
			}

			// 高温多湿の場合、相乗効果で悪化
			if (temp > suitableTempRange.UpperLimit && humidity > suitableHumidRange.UpperLimit)
			{
				_score = (tempScore + humidScore) * 1.2f; // 20%増しでペナルティ
			}
			else
			{
				_score = Mathf.Max(tempScore, humidScore); // より悪い方を採用
			}

			_OnResultGenerated(new Result(_score, _unit));
		}

		protected override void OnSensorDecided()
		{
			base.OnSensorDecided();
			tempSensor = sensorManager.GetComponent<ITemperatureSensor>();
			humiditySensor = sensorManager.GetComponent<IHumiditySensor>();
		}

		protected override void OnDeserializeCompleted()
		{
			GenerateEvaluationResult();
		}
	}
}