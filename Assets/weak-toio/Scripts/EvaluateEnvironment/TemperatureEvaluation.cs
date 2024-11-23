using Environment;
using Evaluation;
using Robot;
using UnityEngine;

public class TemperatureEvaluation : MonoBehaviour
{
	[Tooltip("接続したいtoioの数")]
	public int cubeCount = 0;
	[Tooltip("Mainをぶち込め")]
	public GameObject sensorObject;
	[SerializeField] SensorUnit sensor;
	EvaluationResultSender tempEval;
	ActionSender tempAction;
	IToioMovement toioMovement;
	ToioManager toioManager;

	private void Start()
	{
		// SensorUnitの取得
		sensor = sensorObject.GetComponent<M5DataReceiver>();

		// 評価システムの初期化
		tempEval = new TemperatureEvaluate();
		tempAction = new TemperatureActionGenerator();

		toioManager = new ToioManager(cubeCount);

	}

	private void Update()
	{
		var result = tempEval.GetEvaluationResult(sensor);
		var action = tempAction.GenerateAction(result);

	}
}
