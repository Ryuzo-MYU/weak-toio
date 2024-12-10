using System;
using System.Collections;
using System.Collections.Generic;
using Environment;
using Evaluation;
using Robot;
using toio;
using UnityEngine;

public class Main_Temperature : EvaluationBase<ITemperatureSensor, TemperatureEvaluate, TemperatureActionGenerator>
{
	protected override void InitializeSensor()
	{
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
		
	}
}
