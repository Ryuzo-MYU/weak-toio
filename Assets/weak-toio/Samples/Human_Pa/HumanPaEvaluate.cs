using Environment;
using UnityEngine;

namespace Evaluation
{
	[RequireComponent(typeof(IPressureSensor))]
	public class HumanPaEvaluate : EvaluateBase, IEvaluationResultSender<IPressureSensor>
	{
		[SerializeField] private float baselinePressure = 1013.0f; // 基準気圧(hPa)
		[SerializeField] private float warningThreshold = 10.0f;   // 警戒しきい値(±hPa)
		private IPressureSensor pressureSensor;

		public void GenerateEvaluationResult(IPressureSensor pressureSensor)
		{
			_currentParam = pressureSensor.GetPressure();
			float pressureDiff = _currentParam - baselinePressure;

			if (Mathf.Abs(pressureDiff) <= warningThreshold)
			{
				_score = 0; // 快適範囲
			}
			else
			{
				_score = pressureDiff; // 気圧差をそのままスコアとして使用
			}

			_OnResultGenerated(new Result(_score, _unit));
		}

		protected override void OnSensorDecided()
		{
			base.OnSensorDecided();
			pressureSensor = (IPressureSensor)sensorManager.GetSensor();
		}

		protected override void OnDeserializeCompleted()
		{
			GenerateEvaluationResult(pressureSensor);
		}
	}
}