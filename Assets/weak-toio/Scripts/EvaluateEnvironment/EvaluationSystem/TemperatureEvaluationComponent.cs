using System;
using System.Collections;
using UnityEngine;
using EvaluateEnvironment;

public class TemperatureEvaluationComponent : MonoBehaviour
{
	TemperatureEvaluation temp;
	DummyDataGenerator m5;
	Result result;
	[SerializeField] float interval;
	Action m5Update;
	private void Start()
	{
		// 初期化
		temp = new TemperatureEvaluation();
		m5 = new DummyDataGenerator();
		m5.Update();
		result = temp.Evaluate(m5);

		// コルーチン設定
		m5Update += M5UpdateAndOutputMessage;
		StartCoroutine(GenerateDummyData(interval, m5Update));
	}
	private IEnumerator GenerateDummyData(float interval, Action action)
	{
		while (true)
		{
			action?.Invoke();
			yield return new WaitForSeconds(interval);
		}
	}
	private void M5UpdateAndOutputMessage()
	{
		m5.Update();
		result = temp.Evaluate(m5);
		Debug.Log(result.Message);
	}
}