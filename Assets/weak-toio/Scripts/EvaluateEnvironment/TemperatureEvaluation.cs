using System;
using System.Collections.Generic;
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
	public bool UseDummy;
	[SerializeField] TempBoundary tempBoundary;
	[SerializeField] List<GameObject> Cubes;
	EvaluationResultSender tempEval;
	ActionSender tempAction;
	ToioManager toioManager;
	private bool connected = false;

	private async void Start()
	{
		// ダミーセンサーを使う場合
		if (UseDummy)
		{
			// ダミーセンサーの初期化
			sensor = new DummySensor();
		}

		try
		{
			// 評価システムの初期化
			tempEval = new TemperatureEvaluate(tempBoundary.UpperBound, tempBoundary.LowerBound);
			toioManager = new ToioManager(connectType, cubeCount);

			// Connect処理の完了を確実に待機
			connected = await toioManager.Connect();
			// 接続が成功したか確認
			if (!connected)
			{
				Debug.LogError("toioの接続に失敗しました");
				return;
			}

			// 接続成功後の処理
			List<Toio> toios = new List<Toio>();
			foreach (GameObject cube in Cubes)
			{
				toios.Add(cube.GetComponent<Toio>());
			}
			toioManager.Setup(toios);

			// nullチェックを追加
			var toio = toioManager.GetHandle();
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
		if (connected)
		{
			sensor.Update();
			Result result = tempEval.GetEvaluationResult(sensor);
			Robot.Action action = tempAction.GenerateAction(result);
			toioManager.AddNewAction(action);
		}
	}

	[Serializable]
	struct TempBoundary
	{
		public float UpperBound;
		public float LowerBound;
	}
}
