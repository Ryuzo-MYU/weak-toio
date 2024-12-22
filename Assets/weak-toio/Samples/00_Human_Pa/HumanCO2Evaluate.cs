#region 人（CO2）

using Environment;
using Evaluation;
using UnityEngine;

[RequireComponent(typeof(ICO2Sensor))]
public class HumanCO2Evaluate : EvaluateBase, IEvaluationResultSender<ICO2Sensor>
{
	[SerializeField] private BoundaryRange suitableRange = new BoundaryRange(400, 1000); // CO2濃度の快適範囲(ppm)
	private ICO2Sensor co2Sensor;

	public void GenerateEvaluationResult(ICO2Sensor co2Sensor)
	{
		_currentParam = co2Sensor.GetCO2();

		if (suitableRange.isWithInRange(_currentParam))
		{
			_score = 0;
		}
		else if (_currentParam < suitableRange.LowerLimit)
		{
			_score = _currentParam - suitableRange.LowerLimit;
		}
		else
		{
			_score = _currentParam - suitableRange.UpperLimit;
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
#endregion