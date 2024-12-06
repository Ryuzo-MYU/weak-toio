using Environment;
using UnityEngine;

namespace Evaluation
{
	/// <summ
	/// 気温データを取得し、評価をするクラス
	/// </summary>
	public class CO2Evaluate : EvaluationResultSender<ICO2Sensor>
	{
		private float UPPER_LIMIT;  // 許容できるPPMの上限
		private Unit _ppm = new Unit("PPM");
		public float CurrentPPM { get; private set; }
		private float _score;

		public CO2Evaluate(float _upperLimit)
		{
			UPPER_LIMIT = _upperLimit;
		}
		/// <summary>
		/// CO2センサからPPMを取得し、指定した上限値と比較した結果を返す
		/// </summary>
		/// <returns>評価結果を集約したResult型データ</returns>
		public Result GetEvaluationResult(ICO2Sensor co2Sensor)
		{
			CurrentPPM = co2Sensor.GetPPM(); // CO2センサからCO2濃度を取得

			// PPMに基づく評価
			// 適正PPM以下ならば
			if (CurrentPPM < UPPER_LIMIT)
			{
				_score = 0;
			}
			// 適正より高濃度の場合
			else
			{
				_score = CurrentPPM - UPPER_LIMIT;
			}


			Result co2Result = new Result(_score, _ppm);
			Debug.Log($"評価成功。Score: {_score}\n" +
					$"もとの二酸化炭素濃度は{CurrentPPM}{_ppm.unit}です");

			return co2Result;
		}
	}
}