using Environment;
using Evaluation;
using Robot;
using toio;
using UnityEngine;

public class TemperatureEvaluation : MonoBehaviour
{
	[Tooltip("UnityEditor上ならSimmurator、現実ならReal、お任せならAuto")] public ConnectType connectType = ConnectType.Auto;

	[Tooltip("接続したいtoioの数")] public int cubeCount = 0;
	[Tooltip("Mainをぶち込め")][SerializeField] SensorUnit sensor;
	EvaluationResultSender tempEval;
	ActionSender tempAction;
	ToioManager toioManager;

	private void Start()
	{
		// 評価システムの初期化
		tempEval = new TemperatureEvaluate();
		toioManager = new ToioManager(connectType, cubeCount);
		tempAction = new TemperatureActionGenerator(toioManager.GetToio(0));
		toioManager.Setup();
	}
	private void Update()
	{
		Result result = tempEval.GetEvaluationResult(sensor);
		Action action = tempAction.GenerateAction(result);
		toioManager.AddNewAction(action);
	}
}
