#region 草（湿度）

using Environment;
using Evaluation;
using UnityEngine;

[RequireComponent(typeof(IHumiditySensor))]
public class GrassCO2Evaluate : EvaluateBase, IEvaluationResultSender<IHumiditySensor>
{
	[SerializeField] private BoundaryRange suitableRange = new BoundaryRange(40, 60); // 草の適正湿度
	private IHumiditySensor humiditySensor;

	public void GenerateEvaluationResult(IHumiditySensor humiditySensor)
	{
		_currentParam = humiditySensor.GetHumidity();

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
		humiditySensor = (IHumiditySensor)sensorManager.GetSensor();
	}

	protected override void OnDeserializeCompleted()
	{
		GenerateEvaluationResult(humiditySensor);
	}
}
#endregion