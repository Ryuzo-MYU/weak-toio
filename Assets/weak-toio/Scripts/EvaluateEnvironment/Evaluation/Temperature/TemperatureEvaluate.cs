using Environment;
using UnityEngine;

namespace Evaluation
{
	/// <summ
	/// 気温データを取得し、評価をするクラス
	/// </summary>
	public class TemperatureEvaluate : EvaluationResultSender
	{
		private float UPPER_BOUND;  // 暑すぎる基準
		private float LOWER_BOUND; // 寒すぎる基準
		private BoundaryRange suitableRange;
		private Unit _celsius = new Unit("℃");
		public float CurrentTemperature { get; private set; }
		private float _score;

		public TemperatureEvaluate(float _upperBound, float _lowerBound)
		{
			UPPER_BOUND = _upperBound;
			LOWER_BOUND = _lowerBound;
			suitableRange = new BoundaryRange(UPPER_BOUND, LOWER_BOUND);
		}
		/// <summary>
		/// SensorUnitから気温のデータを取得し、労働環境の適温範囲と比較した結果を返す
		/// </summary>
		/// <param name="sensor">SensorUnitインスタンス</param>
		/// <returns>評価結果を集約したResult型データ</returns>
		public Result GetEvaluationResult(SensorUnit sensor)
		{
			CurrentTemperature = sensor.GetSensorInfo().temp; // SensorUnitから気温を取得

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
					$"もとの気温は{CurrentTemperature}{_celsius}です");

			return temperatureResult;
		}
	}
}