using NUnit.Framework;
using UnityEngine;

public class TemperatureEvaluationTest
{
	private TemperatureEvaluation temperatureEvaluation;

	[SetUp]
	public void Setup()
	{
		temperatureEvaluation = new TemperatureEvaluation();
	}

	[Test]
	public void TestHotCondition()
	{
		// テスト用のM5DataReceiverオブジェクトを作成し、気温を設定
		var mockM5DataReceiver = new M5DataReceiver();
		mockM5DataReceiver.sensorInfo.temp = 30.0f; // 暑すぎる基準以上

		// Evaluateを呼び出し、結果を確認
		var result = temperatureEvaluation.Evaluate(mockM5DataReceiver) as TemperatureResult;

		Assert.AreEqual("HOT", result.Condition);
		Assert.AreEqual(30.0f, result.CurrentTemperature);
		Assert.AreEqual("現在の気温は30 ℃です。HOTです。平均気温から3℃離れています。", result.Message);
	}

	[Test]
	public void TestColdCondition()
	{
		// テスト用のM5DataReceiverオブジェクトを作成し、気温を設定
		var mockM5DataReceiver = new M5DataReceiver();
		mockM5DataReceiver.sensorInfo.temp = 20.0f; // 寒すぎる基準以下

		// Evaluateを呼び出し、結果を確認
		var result = temperatureEvaluation.Evaluate(mockM5DataReceiver) as TemperatureResult;

		Assert.AreEqual("COLD", result.Condition);
		Assert.AreEqual(20.0f, result.CurrentTemperature);
		Assert.AreEqual("現在の気温は20 ℃です。COLDです。平均気温から7℃離れています。", result.Message);
	}

	[Test]
	public void TestSuitableCondition()
	{
		// テスト用のM5DataReceiverオブジェクトを作成し、気温を設定
		var mockM5DataReceiver = new M5DataReceiver();
		mockM5DataReceiver.sensorInfo.temp = 26.0f; // 適温範囲内

		// Evaluateを呼び出し、結果を確認
		var result = temperatureEvaluation.Evaluate(mockM5DataReceiver) as TemperatureResult;

		Assert.AreEqual("SUITABLE", result.Condition);
		Assert.AreEqual(26.0f, result.CurrentTemperature);
		Assert.AreEqual("現在の気温は26 ℃です。SUITABLEです。平均気温から1℃離れています。", result.Message);
	}
}
