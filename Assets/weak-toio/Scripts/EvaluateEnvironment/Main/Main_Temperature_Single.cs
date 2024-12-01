using System;
using System.Collections;
using Environment;
using Evaluation;
using PlasticGui.WebApi.Requests;
using Robot;
using toio;
using UnityEngine;

public class Main_Temperature_Single : MonoBehaviour
{
	public static string PORTNAME = "COM9";
	public static int BAUDRATE = 115200;
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
		serial = new SerialHandler(PORTNAME, BAUDRATE);
		bool portConnectSuccessed = serial.Awake();

		// ダミーセンサーを使う場合
		if (!portConnectSuccessed || UseDummy) sensor = new DummySensor();
		// M5と接続する場合
		else sensor = new M5DataReceiver(serial);

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
