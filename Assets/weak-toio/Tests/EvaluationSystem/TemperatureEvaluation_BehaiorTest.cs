using NUnit.Framework;
using System;
using UnityEngine;

public class TemperatureEvaluationBehaviorTest
{
	private class MockM5DataReceiver : M5DataReceiver
	{
		public float MockTemperature { get; set; }

		public MockM5DataReceiver(float temp)
		{
			sensorInfo = new SensorInfo { temp = temp };
		}
	}

	[Test]
	public void TestTemperatureEvaluation_BehaviorTest()
	{
		var temperatureEvaluation = new TemperatureEvaluation();

		for (int i = 0; i < 10; i++)
		{
			float min = 0f;
			float max = 40f;
			float randomTemp = GetRandomFloat(min, max);
			var mockM5DataReceiver = new MockM5DataReceiver(randomTemp);
			var result = temperatureEvaluation.Evaluate(mockM5DataReceiver) as TemperatureResult;

			Debug.Log(result.Message);
		}
	}

	/// <summary>
	/// min, maxの範囲でランダムなfloatを返す
	/// </summary>
	/// <param name="min">ほしい値の下限</param>
	/// <param name="max">ほしい値の上限</param>
	/// <returns>ランダムなfloat</returns>
	private float GetRandomFloat(float min, float max)
	{
		System.Random random = new System.Random();
		// 任意の範囲のfloatを生成する
		float randomFloat = (float)(random.NextDouble() * (max - min) + min);

		return randomFloat;
	}
}
