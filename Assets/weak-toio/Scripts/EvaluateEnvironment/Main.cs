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
	[SerializeField] private bool useDummyData = true; // インスペクターでモード切替可能

	private SensorUnit sensor;
	private TemperatureEvaluation tempEvaluate;
	private Result result;
	private TemperatureExpression_HotOnly tempExpression;

	private void Awake()
	{
		Debug.Log("Awake Start");

		// モードに応じてデータプロバイダーを初期化
		if (useDummyData)
		{
			sensor = new DummyDataGenerator();
		}
		else
		{
			sensor = new M5DataReceiver(COM_PORTS, PORTNAME, BAUDRATE);
		}

		Debug.Log("Awake End");
	}

	private void Start()
	{
		Debug.Log("Start Start");

		sensor.Update();
		tempEvaluate = new TemperatureEvaluation();
		result = tempEvaluate.Evaluate(sensor);

		tempExpression = new TemperatureExpression_HotOnly();
		tempExpression.Start(connectType, cubeCount);
		tempExpression.Update(result);

		Debug.Log("Start End");
	}

	private void Update()
	{
		sensor.Update();
		result = tempEvaluate.Evaluate(sensor);
		Debug.Log($"評価スコア: {result.Score}");
		tempExpression.Update(result);
	}
}
