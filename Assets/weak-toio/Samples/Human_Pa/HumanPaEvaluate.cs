using Environment;
using UnityEngine;

namespace Evaluation
{
	[RequireComponent(typeof(IPressureSensor))]
	public class HumanPaEvaluate : EvaluateBase
	{
		[SerializeField] private float baselinePressure = 1013.0f; // 基準気圧(hPa)
		[SerializeField] private BoundaryRange warningThreshold = new BoundaryRange(1003.0f, 1023.0f);   // 警戒しきい値(hPa)
																										 // https://x.gd/wHcBz
		private IPressureSensor pressureSensor;

		protected override void GenerateEvaluationResult()
		{
			_currentParam = pressureSensor.GetPressure();

			_score = warningThreshold.CalcDiff(CurrentParam);
			string _message;
			if (_score == 0)
			{
				_message = "通常の気圧です";
			}
			else
			{
				_message = "あたまがいたい……";
			}
			_OnResultGenerated(new Result(_score, _unit, _message));
		}

		protected override void OnSensorDecided()
		{
			base.OnSensorDecided();
			pressureSensor = (IPressureSensor)sensorManager.GetSensor();
		}

		protected override void OnDeserializeCompleted()
		{
			GenerateEvaluationResult();
		}
	}
}