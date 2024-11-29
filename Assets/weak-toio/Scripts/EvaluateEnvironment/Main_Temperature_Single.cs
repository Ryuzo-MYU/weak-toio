using System;
using System.Collections;
using Environment;
using Evaluation;
using Robot;
using toio;
using UnityEngine;

public class Main_Temperature_Single : MonoBehaviour
{
	[Tooltip("UnityEditor上ならSimmurator、現実ならReal、お任せならAuto")]
	public ConnectType connectType = ConnectType.Auto;

	[Tooltip("接続したいtoioの数")] public int cubeCount = 0;
	[Tooltip("Mainをぶち込め")][SerializeField] M5DataReceiver m5;
	SensorUnit sensor;
	public bool UseDummy;
	[SerializeField] TempBoundary tempBoundary;
	EvaluationResultSender tempEval;
	ActionSender tempAction;
	Toio toio;
	CubeManager cubeManager;
	private bool connected = false;

	private async void Start()
	{
		if (m5 == null)
		{
			Debug.LogWarning("M5入ってねえぞ！");
			return;
		}
		if (UseDummy) sensor = new DummySensor(); // ダミーセンサーを使う場合
		else sensor = m5;

		// 評価システムの初期化
		tempEval = new TemperatureEvaluate(tempBoundary.UpperBound, tempBoundary.LowerBound);
		tempAction = new TemperatureActionGenerator();

		try
		{
			// toioに接続
			cubeManager = new CubeManager(connectType);
			await cubeManager.SingleConnect();
			connected = true;
			Debug.Log("接続完了");

			toio = new Toio(0, cubeManager);
		}
		catch (Exception e)
		{
			Debug.LogError($"エラー: {e.Message}\n" +
				  $"場所: {e.StackTrace}\n" +
				  $"メソッド名: {System.Reflection.MethodBase.GetCurrentMethod().Name}\n" +
				  $"行番号: {new System.Diagnostics.StackTrace(e, true).GetFrame(0).GetFileLineNumber()}");
		}

		StartCoroutine(UpdateEvaluate());
	}
	private void Update()
	{
		sensor.Update();
	}

	IEnumerator UpdateEvaluate()
	{
		while (true)
		{
			if (connected && sensor != null)
			{
				Result result = tempEval.GetEvaluationResult(sensor);
				Robot.Action action = tempAction.GenerateAction(result);
				if (!toio.AddNewAction(action))
				{
					Debug.LogWarning("アクション溜まってんね");
				}
				yield return StartCoroutine(toio.Move());
			}
		}
	}

	[Serializable]
	struct TempBoundary
	{
		public float UpperBound;
		public float LowerBound;
	}
}
