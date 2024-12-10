using System;
using Environment;
using Evaluation;
using Robot;
using UnityEngine;

public class Main_CO2 : EvaluationBase<ICO2Sensor, CO2Evaluate, CO2ActionGenerator>
{
	// 基準値参照
	// https://www.mhlw.go.jp/content/11130500/000771220.pdf
	[SerializeField] CO2Bounds co2Bounds;
	[SerializeField] float currentPPM;

	protected override void InitializeSensor()
	{
		connector.OnConnectSucceeded += OnConnectSuccessed;
		// センサー系の初期化
		serial = new SerialHandler(PORTNAME, BAUDRATE);
		// シリアルポートの初期化が失敗したらダミーを使う
		UseDummy = !serial.Awake();
		sensor = new M5CO2Sensor(serial);
		if (UseDummy) sensor = new DummyCO2Sensor();
		envType = sensor.GetEnvType();
	}

	protected override void InitializeEvaluator()
	{
		// 評価システムの初期化
		evaluator = new CO2Evaluate(co2Bounds.DangerPPM);
		actionGenerator = new CO2ActionGenerator(co2Bounds.SuitablePPM, co2Bounds.CautionPPM, co2Bounds.DangerPPM);
	}
}

[Serializable]
struct CO2Bounds
{
	public float DangerPPM;
	public float CautionPPM;
	public float SuitablePPM;
}
