using System;
using System.Collections;
using Environment;
using Evaluation;
using Robot;
using toio;
using UnityEngine;

public class Main_Temperature_Single : MonoBehaviour
{
	public string PORTNAME = "COM9";
	public int BAUDRATE = 115200;
	[Tooltip("UnityEditor上ならSimmurator、現実ならReal、お任せならAuto")]
	public ConnectType connectType = ConnectType.Auto;

	[Tooltip("接続したいtoioの数")] public int cubeCount = 0;
	SerialHandler serial;
	SensorUnit sensor;
	public bool UseDummy;
	[SerializeField] TempBoundary tempBoundary;
	EvaluationResultSender tempEval;
	ActionSender tempAction;
	Toio toio;
	CubeManager cubeManager;
	private bool connected = false;

	private void Awake()
	{
		// センサー系の初期化
		serial = new SerialHandler(PORTNAME, BAUDRATE);

		// シリアルポートの初期化が失敗したらダミーを使う
		UseDummy = !serial.Awake();
		sensor = new M5DataReceiver(serial);
		if (UseDummy) sensor = new DummySensor();

		// 評価システムの初期化
		tempEval = new TemperatureEvaluate(tempBoundary.UpperBound, tempBoundary.LowerBound);
		tempAction = new TemperatureActionGenerator();
	}
	private async void Start()
	{
		sensor.Start();
		try
		{
			// toioに接続
			cubeManager = new CubeManager(connectType);
			await cubeManager.MultiConnect(cubeCount);
			Debug.Log("接続完了");

			for (int id = 0; id < cubeManager.connectedCubes.Count; id++)
			{
				if (!cubeManager.connectedCubes[id].isConnected)
				{
					toio = new Toio(id, cubeManager);
					connected = true;
					break;
				}
			}
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
		serial.Update();
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
