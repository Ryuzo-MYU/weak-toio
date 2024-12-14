using Environment;
using UnityEngine;

namespace Evaluation
{
	/// <summ
	/// 気温データを取得し、評価をするクラス
	/// </summary>
	public class TemperatureEvaluate : MonoBehaviour, EvaluationResultSender<ITemperatureSensor>
	{
		[SerializeField] private BoundaryRange suitableRange;
		private Unit _celsius;
		[SerializeField] public float CurrentTemperature { get; private set; }
		[SerializeField] private float _score;
		[SerializeField] private EnvType _type;

		/// <summary>
		/// SensorUnitから気温のデータを取得し、労働環境の適温範囲と比較した結果を返す
		/// </summary>
		/// <returns>評価結果を集約したResult型データ</returns>
		public Result GetEvaluationResult(ITemperatureSensor tempSensor)
		{
			CurrentTemperature = tempSensor.GetTemperature(); // SensorUnitから気温を取得

			// 気温に基づく評価
			// 適温範囲内なら
			if (suitableRange.isWithInRange(CurrentTemperature))
			{
				_score = 0;
			}
			// 適温より寒ければ
			else if (CurrentTemperature < suitableRange.LowerLimit)
			{
				_score = CurrentTemperature - suitableRange.LowerLimit;
			}
			else
			{
				_score = CurrentTemperature - suitableRange.UpperLimit;
			}

			Result temperatureResult = new Result(_score, _celsius);
			Debug.Log($"評価成功。Score: {_score}\n" +
					$"もとの気温は{CurrentTemperature}{_celsius.unit}です");

			return temperatureResult;
		}
		public EnvType GetEnvType()
		{
			return _type;
		}
	}
}