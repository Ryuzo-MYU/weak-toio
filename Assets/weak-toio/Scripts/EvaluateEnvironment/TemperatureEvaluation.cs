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
	Evaluate tempEval;
	ActionGenerator tempAction;
	ActionSender sender;
	ToioManager toioManager;
	EnvType envType;

	private void Start()
	{
		// SensorUnitの取得
		sensor = sensorObject.GetComponent<M5DataReceiver>();

		// 評価システムの初期化
		tempEval = new TemperatureEvaluate();
		tempAction = new TemperatureActionGenerator();
		sender = new ActionSender();

		envType = tempEval.GetEnvType();
		toioManager = new ToioManager(cubeCount);
		toioManager.Start(envType);

	}

	private void Update()
	{
		var result = tempEval.EvaluateEnv(sensor);
		var action = tempAction.GenerateAction(result);
		sender.Update(action);

		var nextAc = sender.NextAction();
		toioManager.Update(nextAc);
	}
}
