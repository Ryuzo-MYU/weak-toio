using System;
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

	private async void Start()
	{
		try
		{
			// 評価システムの初期化
			tempEval = new TemperatureEvaluate();
			toioManager = new ToioManager(connectType, cubeCount);

			// Connect処理の完了を確実に待機
			bool connected = await toioManager.Connect();

			// 接続が成功したか確認
			if (!connected)
			{
				Debug.LogError("toioの接続に失敗しました");
				return;
			}

			// 接続成功後の処理
			toioManager.SetUp();

			// nullチェックを追加
			var toio = toioManager.GetToio(0);
			if (toio == null)
			{
				Debug.LogError("toioの取得に失敗しました");
				return;
			}

			tempAction = new TemperatureActionGenerator(toio);
		}
		catch (Exception e)
		{
			Debug.LogError($"エラーが発生しました: {e.Message}");
		}
	}
	private void Update()
	{
		Result result = tempEval.GetEvaluationResult(sensor);
		Robot.Action action = tempAction.GenerateAction(result);
		toioManager.AddNewAction(action);
	}
}
