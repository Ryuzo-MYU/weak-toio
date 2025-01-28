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
		[SerializeField] Unit unit = new Unit("℃");
		string message;
		private ITemperatureSensor tempSensor;
		protected override void GenerateEvaluationResult()
		{
			float temp = tempSensor.GetTemperature();
			float score = 0;
			if (!suitableRange.isWithInRange(temp))
			{
				if (temp < suitableRange.LowerLimit)
				{
					score = suitableRange.LowerLimit - temp;
					message = "温度が低すぎます。暖房を使用してください。";
				}
				else if (suitableRange.UpperLimit < temp)
				{
					score = temp - suitableRange.UpperLimit;
					message = "温度が高すぎます。冷房を使用してください。";
				}
			}
			else
			{
				message = "温度は適切です。";
			}
			_OnResultGenerated(new Result(score, this.unit, this.message));
		}

		protected override void OnDeserializeCompleted()
		{
			GenerateEvaluationResult();
		}
	}
}