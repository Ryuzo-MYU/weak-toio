using Environment;
using UnityEngine;

namespace Evaluation
{
	/// <summ
	/// 気温データを取得し、評価をするクラス
	/// </summary>
	public class CO2Evaluate : EvaluateBase, IEvaluationResultSender<ICO2Sensor>
	{
		[SerializeField] private float CAUTION_LIMIT; // 警告が必要なppm

		/// <summary>
		/// CO2センサからPPMを取得し、指定した上限値と比較した結果を返す
		/// </summary>
		/// <returns>評価結果を集約したResult型データ</returns>
		public void GetEvaluationResult(ICO2Sensor co2Sensor)
		{
			_currentParam = co2Sensor.GetPPM(); // CO2センサからCO2濃度を取得

			// PPMに基づく評価
			// 適正PPM以下ならば
			if (_currentParam < CAUTION_LIMIT) _score = 0;
			// 適正より高濃度の場合
			else _score = _currentParam - CAUTION_LIMIT;

			Result co2Result = new Result(_score, _unit);
			Debug.Log($"評価成功。Score: {_score}\n" +
					$"もとの二酸化炭素濃度は{_currentParam}{_unit.unit}です");

			OnResultGenerated.Invoke(co2Result);
		}
	}
}