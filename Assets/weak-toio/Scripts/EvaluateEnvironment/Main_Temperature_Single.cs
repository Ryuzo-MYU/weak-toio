using System;
using System.Collections.Generic;
using Environment;
using Evaluation;
using Robot;
using toio;
using UnityEngine;

public class Main_Temperature_Single : MonoBehaviour
{
	[Tooltip("UnityEditor上ならSimmurator、現実ならReal、お任せならAuto")] public ConnectType connectType = ConnectType.Auto;

	[Tooltip("接続したいtoioの数")] public int cubeCount = 0;
	[Tooltip("Mainをぶち込め")][SerializeField] SensorUnit sensor;
	public bool UseDummy;
	[SerializeField] TempBoundary tempBoundary;
	EvaluationResultSender tempEval;
	ActionSender tempAction;
	IToioMovement toio;
	CubeManager cubeManager;
	private bool connected = false;

	private async void Start()
	{
		// ダミーセンサーを使う場合
		if (UseDummy) sensor = new DummySensor();

		// 評価システムの初期化
		tempEval = new TemperatureEvaluate(tempBoundary.UpperBound, tempBoundary.LowerBound);

		try
		{
			// Connect処理の完了を確実に待機
			await cubeManager.SingleConnect();
			connected = true;
			// 接続が成功したか確認
			if (!connected)
			{
				Debug.LogError("toioの接続に失敗しました");
				return;
			}

			int id = cubeManager.cubes.Count;
			Cube cube = cubeManager.cubes[0];
			CubeHandle handle = cubeManager.handles[0];
			toio = new Toio(id, cube, handle);
			tempAction = new TemperatureActionGenerator(toio);
		}
		catch (Exception e)
		{
			Debug.LogError($"エラーが発生しました: {e.Message}");
		}

		StartCoroutine(toio.Move());
	}
	private void Update()
	{
		if (connected)
		{
			sensor.Update();
			Result result = tempEval.GetEvaluationResult(sensor);
			Robot.Action action = tempAction.GenerateAction(result);
			toio.AddNewAction(action);
		}
	}

	[Serializable]
	struct TempBoundary
	{
		public float UpperBound;
		public float LowerBound;
	}
}
