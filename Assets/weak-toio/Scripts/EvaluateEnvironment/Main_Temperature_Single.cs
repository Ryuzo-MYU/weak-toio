using System;
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
			// toioに接続
			cubeManager = new CubeManager(connectType);
			Cube cube =  await cubeManager.SingleConnect();

			// 接続が成功したか確認
			if (!connected)
			{
				Debug.LogError("toioの接続に失敗しました");
				return;
			}

			if (connected)
			{
				int id = cubeManager.cubes.Count;
				toio = new Toio(id, cubeManager);

				tempAction = new TemperatureActionGenerator(toio);

				StartCoroutine(toio.Move());
				Debug.Log("接続完了");
			}
		}
		catch (Exception e)
		{
			Debug.LogError($"エラー: {e.Message}\n" +
				  $"場所: {e.StackTrace}\n" +
				  $"メソッド名: {System.Reflection.MethodBase.GetCurrentMethod().Name}\n" +
				  $"行番号: {new System.Diagnostics.StackTrace(e, true).GetFrame(0).GetFileLineNumber()}");
		}
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
