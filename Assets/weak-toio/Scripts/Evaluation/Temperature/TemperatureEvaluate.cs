using Environment;
using UnityEngine;

namespace Evaluation
{
	/// <summ
	/// 気温データを取得し、評価をするクラス
	/// </summary>
	public class TemperatureEvaluate : EvaluateBase, IEvaluationResultSender<ITemperatureSensor>
	{
		[SerializeField] private BoundaryRange suitableRange;
		[SerializeField] private ITemperatureSensor tempSensor;

		/// <summary>
		/// SensorUnitから気温のデータを取得し、労働環境の適温範囲と比較した結果を返す
		/// </summary>
		/// <returns>評価結果を集約したResult型データ</returns>
		public void GetEvaluationResult(ITemperatureSensor tempSensor)
		{
			_currentParam = tempSensor.GetTemperature(); // SensorUnitから気温を取得

			// 気温に基づく評価
			// 適温範囲内なら
			if (suitableRange.isWithInRange(_currentParam))
			{
				_score = 0;
			}
			// 適温より寒ければ
			else if (_currentParam < suitableRange.LowerLimit)
			{
				_score = _currentParam - suitableRange.LowerLimit;
			}
			else
			{
				_score = _currentParam - suitableRange.UpperLimit;
			}

			Result temperatureResult = new Result(_score, _unit);
			Debug.Log($"評価成功。Score: {_score}\n" +
					$"もとの気温は{_currentParam}{_unit.unit}です");

			OnResultGenerated.Invoke(temperatureResult);
		}
	}
}