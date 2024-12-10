using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using Environment;
using Evaluation;
using Robot;
using toio;
using UnityEngine;

public class Main_CO2 : MonoBehaviour
{
	[SerializeField] string exceptToioName;
	[SerializeField] string toioName;
	public ToioConnector connector;
	public string PORTNAME = "COM9";
	public int BAUDRATE = 115200;
	SerialHandler serial;
	ICO2Sensor sensor;
	public bool UseDummy;

	// 基準値参照
	// https://www.mhlw.go.jp/content/11130500/000771220.pdf
	[SerializeField] CO2Bounds co2Bounds;
	public EnvType envType = EnvType.NotAppointed;
	CO2Evaluate co2Eval;
	ActionSender co2Action;
	private bool connected = false;
	Toio toio;
	[SerializeField] float currentPPM;

	private void Awake()
	{
		connector.OnConnectSuccessed += OnConnectSuccessed;
		// センサー系の初期化
		serial = new SerialHandler(PORTNAME, BAUDRATE);
		// シリアルポートの初期化が失敗したらダミーを使う
		UseDummy = !serial.Awake();
		sensor = new M5CO2Sensor(serial);
		if (UseDummy) sensor = new DummyCO2Sensor();
		envType = sensor.GetEnvType();

		// 評価システムの初期化
		co2Eval = new CO2Evaluate(co2Bounds.DangerPPM);
		co2Action = new CO2ActionGenerator(co2Bounds.SuitablePPM, co2Bounds.CautionPPM, co2Bounds.DangerPPM);
	}
	private void Start()
	{
		sensor.Start();
	}
	void Update()
	{
		serial.Update();
		sensor.Update();
	}
	private void OnConnectSuccessed(List<Toio> toios)
	{
		Debug.Log("接続開始");
		toio = toios.Find(t => t.Name == exceptToioName);
		toioName = toio.Name;
		toio.EnvType = sensor.GetEnvType(); // 役割を割り当て
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
			currentPPM = sensor.GetPPM();
			Result result = co2Eval.GetEvaluationResult(sensor);
			Robot.Action action = co2Action.GenerateAction(result);
			if (!toio.AddNewAction(action))
			{
				Debug.LogWarning("アクション溜まってんね");
			}
			yield return StartCoroutine(toio.Move());
		}
	}
}

[Serializable]
struct CO2Bounds
{
	public float DangerPPM;
	public float CautionPPM;
	public float SuitablePPM;
}
