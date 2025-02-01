using Environment;
using UnityEngine;

namespace Evaluation
{
	[RequireComponent(typeof(ITemperatureSensor))]
	[RequireComponent(typeof(IHumiditySensor))]
	public class BananaTempHumEvaluate : EvaluateBase
	{
		///	低い分には最低でも外の湿度と同じ程度で，その程度であればむしろよい
		/// 低温(<14℃)は低温障害，高温(20℃<)は本来の生育環境に近いが，保存に不適
		/// 高低どちらも評価
		/// 
		/// 湿度は過多による特殊な影響は少ない．多湿の場合カビ，病気等の悪影響
		/// 高い方だけ評価

		[SerializeField] private BoundaryRange suitableTempRange = new BoundaryRange(14, 20); // https://www.dole.co.jp/lp/jp/magazine/banana/preservation/#a03
		[SerializeField] private BoundaryRange suitableHumidRange = new BoundaryRange(45, 85); // 「なお45～85%の湿度範囲を常湿という」https://kikakurui.com/z8/Z8703-1983-01.html p.2
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
					tempScore = suitableTempRange.LowerLimit - temp;
				}
				else
				{
					tempScore = temp - suitableTempRange.UpperLimit;
				}
			}

			// 湿度評価
			float humidity = humiditySensor.GetHumidity();
			if (!suitableHumidRange.isWithInRange(humidity))
			{
				if (humidity > suitableHumidRange.UpperLimit)
				{
					humidScore = humidity - suitableHumidRange.UpperLimit;
				}
			}

			// 高温多湿の場合、相乗効果で悪化
			string message;
			if (temp > suitableTempRange.UpperLimit && humidity > suitableHumidRange.UpperLimit)
			{
				_score = (tempScore + humidScore) * 1.2f; // 20%増しでペナルティ
				message = "高温多湿です";
			}
			else
			{
				_score = Mathf.Max(tempScore, humidScore); // より悪い方を採用
				message = "温度または湿度が適正範囲外です";
			}

			// 低温障害の評価
			if (temp < suitableTempRange.LowerLimit)
			{
				_score += (suitableTempRange.LowerLimit - temp) * 0.5f; // 低温障害のペナルティ
				message = "低温障害の可能性があります";
			}

			OnResultGenerated(new Result(_score, Unit, message));
		}

		protected override void OnSensorDecided()
		{
			base.OnSensorDecided();
			tempSensor = (ITemperatureSensor)sensorManager.GetSensor();
			humiditySensor = (IHumiditySensor)sensorManager.GetSensor();
			Debug.Log(tempSensor.GetType());
			Debug.Log(humiditySensor.GetType());
		}

		protected override void OnDeserializeCompleted()
		{
			GenerateEvaluationResult();
		}
	}
}