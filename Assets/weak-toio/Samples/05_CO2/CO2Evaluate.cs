using Environment;
using UnityEngine;

namespace Evaluation
{
	/// <summ
	/// 二酸化炭素濃度を取得し、評価するクラス
	/// </summary>
	[RequireComponent(typeof(ICO2Sensor))]
	public class CO2Evaluate : EvaluateBase, IEvaluationResultSender<ICO2Sensor>
	{
		[SerializeField] private float CAUTION_LIMIT; // 警告が必要なppm
		private ICO2Sensor co2Sensor;

		/// <summary>
		/// CO2センサからPPMを取得し、指定した上限値と比較した結果を返す
		/// </summary>
		/// <returns>評価結果を集約したResult型データ</returns>
		public void GenerateEvaluationResult(ICO2Sensor co2Sensor)
		{
			_currentParam = co2Sensor.GetCO2(); // CO2センサからCO2濃度を取得

			// PPMに基づく評価
			// 適正PPM以下ならば
			if (_currentParam < CAUTION_LIMIT) _score = 0;
			// 適正より高濃度の場合
			else _score = _currentParam - CAUTION_LIMIT;

			Result co2Result = new Result(_score, _unit);

			_OnResultGenerated(co2Result);
		}

		protected override void OnSensorDecided()
		{
			base.OnSensorDecided();
			co2Sensor = (ICO2Sensor)sensorManager.GetSensor();
		}
		protected override void OnDeserializeCompleted()
		{
			GenerateEvaluationResult(co2Sensor);
		}
	}
}