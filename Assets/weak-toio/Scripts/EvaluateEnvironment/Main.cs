using ActionDecisionSystem;
using EvaluateEnvironment;
using toio;
using UnityEngine;

public class Main : MonoBehaviour
{
	[SerializeField] private string[] COM_PORTS;
	[SerializeField] private string PORTNAME;
	[SerializeField] private int BAUDRATE;

	[SerializeField] private ConnectType connectType;
	[SerializeField] private int cubeCount;

	private SensorUnit sensor;
	private TemperatureEvaluation tempEvaluate;
	private Result result;
	private TemperatureExpression tempExpression;
	[SerializeField] private bool useDummyData = true;

	private void Awake()
	{
		if (useDummyData)
		{
			sensor = new DummyDataGenerator();
		}
		else
		{
			sensor = new M5DataReceiver(COM_PORTS, PORTNAME, BAUDRATE);
		}
	}

	private void Start()
	{
		sensor.Update();
		tempEvaluate = new TemperatureEvaluation();
		result = tempEvaluate.Evaluate(sensor);

		tempExpression = new TemperatureExpression();
		tempExpression.Start(connectType, cubeCount);
	}

	private void Update()
	{
		sensor.Update();
		result = tempEvaluate.Evaluate(sensor);
		tempExpression.Update(result.Score);
	}
}
