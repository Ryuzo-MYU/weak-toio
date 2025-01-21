using System.Runtime.CompilerServices;
using Environment;
using UnityEngine;

namespace Evaluation
{
	/// <summ
	/// 気温を取得し、評価するクラス
	/// </summary>
	[RequireComponent(typeof(ITemperatureSensor))]
	public class TemperatureEvaluate : EvaluateBase
	{
		[SerializeField] private BoundaryRange suitableRange;
		private ITemperatureSensor tempSensor;

		/// <summary>
		/// SensorUnitから気温のデータを取得し、労働環境の適温範囲と比較した結果を返す
		/// </summary>
		/// <returns>評価結果を集約したResult型データ</returns>
		protected override void GenerateEvaluationResult()
		{
			_currentParam = tempSensor.GetTemperature(); // SensorUnitから気温を取得

			// 気温に基づく評価
			// 適温範囲内なら
			string message;
			if (suitableRange.isWithInRange(_currentParam))
			{
				_score = 0;
				message = "適温です";
			}
			// 適温より寒ければ
			else if (_currentParam < suitableRange.LowerLimit)
			{
				_score = _currentParam - suitableRange.LowerLimit;
				message = "寒いです";
			}
			else
			{
				_score = _currentParam - suitableRange.UpperLimit;
				message = "暑いです";
			}

			Result temperatureResult = new Result(_score, _unit, message);

			_OnResultGenerated(temperatureResult);
		}
		protected override void OnSensorDecided()
		{
			base.OnSensorDecided();
			tempSensor = (ITemperatureSensor)sensorManager.GetSensor();
		}
		protected override void OnDeserializeCompleted()
		{
			GenerateEvaluationResult();
		}
	}
}