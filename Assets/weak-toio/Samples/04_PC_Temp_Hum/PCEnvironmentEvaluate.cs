#region PC（気温・湿度）

using Environment;
using Evaluation;
using UnityEngine;

[RequireComponent(typeof(ITemperatureSensor))]
[RequireComponent(typeof(IHumiditySensor))]
public class PCEnvironmentEvaluate : EvaluateBase
{
	[SerializeField] private BoundaryRange suitableTempRange = new BoundaryRange(10, 35); // PCの動作適正温度
	[SerializeField] private BoundaryRange suitableHumidRange = new BoundaryRange(20, 80); // PCの動作適正湿度
	private ITemperatureSensor tempSensor;
	private IHumiditySensor humiditySensor;

	private void EvaluateEnvironment()
	{
		float tempScore = 0;
		float humidScore = 0;

		// 温度評価
		float temp = tempSensor.GetTemperature();
		if (!suitableTempRange.isWithInRange(temp))
		{
			tempScore = temp < suitableTempRange.LowerLimit ?
				temp - suitableTempRange.LowerLimit :
				temp - suitableTempRange.UpperLimit;
		}

		// 湿度評価
		float humidity = humiditySensor.GetHumidity();
		if (!suitableHumidRange.isWithInRange(humidity))
		{
			humidScore = humidity < suitableHumidRange.LowerLimit ?
				humidity - suitableHumidRange.LowerLimit :
				humidity - suitableHumidRange.UpperLimit;
		}

		// 総合評価（より悪い方のスコアを採用）
		_score = Mathf.Abs(tempScore) > Mathf.Abs(humidScore) ? tempScore : humidScore;

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
		EvaluateEnvironment();
	}
}
#endregion