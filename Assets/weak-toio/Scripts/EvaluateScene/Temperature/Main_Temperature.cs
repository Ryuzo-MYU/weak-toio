using System;
using Environment;
using Evaluation;
using Robot;
using UnityEngine;

public class Main_Temperature : EvaluationBase<ITemperatureSensor, TemperatureEvaluate, TemperatureActionGenerator>
{
	[SerializeField] TempBoundary tempBoundary;
	[SerializeField] float currentTemp;

	protected override void InitializeSensor()
	{
		connector.OnConnectSucceeded += OnConnectSuccessed;
		// センサー系の初期化
		serial = new SerialHandler(PORTNAME, BAUDRATE);
		// シリアルポートの初期化が失敗したらダミーを使う
		UseDummy = !serial.Awake();
		sensor = new M5TemperatureSensor(serial);
		if (UseDummy) sensor = new DummyTemperatureSensor();
		envType = sensor.GetEnvType();
	}
	protected override void InitializeEvaluator()
	{
		// 評価システムの初期化
		evaluator = new TemperatureEvaluate(tempBoundary.UpperBound, tempBoundary.LowerBound);
		actionGenerator = new TemperatureActionGenerator();
	}

	protected override void UpdateEnvParam()
	{
		currentTemp = sensor.GetTemperature();
	}
}

[Serializable]
struct TempBoundary
{
	public float UpperBound;
	public float LowerBound;
}