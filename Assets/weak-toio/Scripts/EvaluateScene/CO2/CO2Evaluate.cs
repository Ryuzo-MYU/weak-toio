using Environment;
using UnityEngine;

namespace Evaluation
{
	/// <summ
	/// 気温データを取得し、評価をするクラス
	/// </summary>
	public class CO2Evaluate : EvaluationResultSender<ICO2Sensor>
	{
		private float UPPER_BOUND;  // 暑すぎる基準
		private float LOWER_BOUND; // 寒すぎる基準
		private BoundaryRange suitableRange;
		private Unit _ppm = new Unit("PPM");
		public float CurrentPPM { get; private set; }
		private float _score;

		public CO2Evaluate(float _upperBound, float _lowerBound)
		{
			UPPER_BOUND = _upperBound;
			LOWER_BOUND = _lowerBound;
			suitableRange = new BoundaryRange(UPPER_BOUND, LOWER_BOUND);
		}
		/// <summary>
		/// SensorUnitから気温のデータを取得し、労働環境の適温範囲と比較した結果を返す
		/// </summary>
		/// <returns>評価結果を集約したResult型データ</returns>
		public Result GetEvaluationResult(ICO2Sensor co2Sensor)
		{
			CurrentPPM = co2Sensor.GetPPM(); // SensorUnitから気温を取得

			// 気温に基づく評価
			// 適温範囲内なら
			if (suitableRange.isWithInRange(CurrentPPM))
			{
				_score = 0;
			}
			// 適温より寒ければ
			else if (CurrentPPM < suitableRange.LowerLimit)
			{
				_score = CurrentPPM - suitableRange.LowerLimit;
			}
			else
			{
				_score = CurrentPPM - suitableRange.UpperLimit;
			}

			Result co2Result = new Result(_score, _ppm);
			Debug.Log($"評価成功。Score: {_score}\n" +
					$"もとの二酸化炭素濃度は{CurrentPPM}{_ppm.unit}です");

			return co2Result;
		}
	}
}