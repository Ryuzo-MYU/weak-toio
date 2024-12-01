using System;
using System.Collections;
using System.Collections.Generic;
using Environment;
using Evaluation;
using Robot;
using toio;
using UnityEngine;

public class Main_Temperature_Single : MonoBehaviour
{
	public ToioConnector connector;
	public string PORTNAME = "COM9";
	public int BAUDRATE = 115200;
	SerialHandler serial;
	SensorUnit sensor;
	public bool UseDummy;
	[SerializeField] TempBoundary tempBoundary;
	EvaluationResultSender tempEval;
	ActionSender tempAction;
	private bool connected = false;
	Toio toio;

	private void Awake()
	{
		connector.OnConnectSuccessed += OnConnectSuccessed;
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
	private void Start()
	{
		sensor.Start();
	}
	private void Update()
	{
		serial.Update();
		sensor.Update();
	}

	private void OnConnectSuccessed(Queue<Toio> toios)
	{
		Debug.Log("接続開始");
		toio = toios.Dequeue();
		connected = true;
		StartCoroutine(UpdateEvaluate());
	}
	IEnumerator UpdateEvaluate()
	{
		while (true)
		{
			if (!connected && sensor == null)
			{
				yield return new WaitForSeconds(0.1f);
				continue;
			}
			Result result = tempEval.GetEvaluationResult(sensor);
			Robot.Action action = tempAction.GenerateAction(result);
			if (!toio.AddNewAction(action))
			{
				Debug.LogWarning("アクション溜まってんね");
			}
			yield return StartCoroutine(toio.Move());
		}
	}

	[Serializable]
	struct TempBoundary
	{
		public float UpperBound;
		public float LowerBound;
	}
}
