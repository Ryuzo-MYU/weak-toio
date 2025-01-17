using Environment;
using UnityEngine;

namespace Evaluation
{
	[RequireComponent(typeof(ICO2Sensor))]
	public class GrassCO2Evaluate : EvaluateBase, IEvaluationResultSender<ICO2Sensor>
	{
		[SerializeField] private float OPTIMAL_CO2 = 400.0f;    // 通常大気のCO2濃度(ppm)
		[SerializeField] private float MAX_EFFECT = 1200.0f;    // 光合成が最大になるCO2濃度(ppm)
		private ICO2Sensor co2Sensor;

		public void GenerateEvaluationResult(ICO2Sensor co2Sensor)
		{
			_currentParam = co2Sensor.GetCO2();

			if (_currentParam <= OPTIMAL_CO2)
			{
				_score = (_currentParam - OPTIMAL_CO2) / OPTIMAL_CO2 * 10; // CO2が少ない場合はマイナス評価
			}
			else if (_currentParam <= MAX_EFFECT)
			{
				_score = (_currentParam - OPTIMAL_CO2) / (MAX_EFFECT - OPTIMAL_CO2) * 10; // CO2増加で比例的に評価上昇
			}
			else
			{
				_score = 10; // 最大効果
			}

			_OnResultGenerated(new Result(_score, _unit));
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