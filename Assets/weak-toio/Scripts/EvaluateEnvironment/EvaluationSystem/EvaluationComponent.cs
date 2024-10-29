using System;
using System.Collections;
using UnityEngine;
using EvaluateEnvironment;
public class EvaluationComponent : MonoBehaviour
{
	TemperatureEvaluation temp;
	[SerializeField] M5Connector sensor;
	Result result;
	[SerializeField] float interval;
	Action dummyUpdate;
	private void Start()
	{
		// 初期化
		temp = new TemperatureEvaluation();
		result = temp.Evaluate(sensor.m5);

		// コルーチン設定
		dummyUpdate += DummyUpdateAndOutputMessage;
		StartCoroutine(MyLibrary.DoFuncWithInterval(interval, dummyUpdate));
	}


	/// <summary>
	/// ダミーデータの更新とデバッグ出力
	/// </summary> 	
	private void DummyUpdateAndOutputMessage()
	{
		result = temp.Evaluate(sensor.m5);
		Debug.Log(result.Message);
	}
}
