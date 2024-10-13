using System;
using System.IO;
using EvaluateEnvironment;
using UnityEngine;

/// <summary>
/// 気温データを取得し、評価をするクラス
/// </summary>
public class TemperatureEvaluation : Evaluation
{
	private const float BASE_TEMPERATURE = 27.0f;
	private const float HOT_THRESHOLD = 28.0f;  // 暑すぎる基準
	private const float COLD_THRESHOLD = 24.0f; // 寒すぎる基準
	private const string UNIT = "℃";
	public float CurrentTemperature { get; private set; }
	public float BaseTemperature => BASE_TEMPERATURE;
	public string Condition { get; private set; }

	/// <summary>
	/// M5DataReceiverから気温のデータを取得し、基準となる気温と比較した結果を返す
	/// </summary>
	/// <param name="m5">M5DataReceiverインスタンス</param>
	/// <returns>評価結果を集約したResult型データ</returns>
	public override Result Evaluate(M5DataReceiver m5)
	{
		CurrentTemperature = m5.sensorInfo.temp; // M5DataReceiverから気温を取得

		// 基準値と比較して評価
		if (CurrentTemperature >= HOT_THRESHOLD)
		{
			Condition = "HOT";
		}
		else if (CurrentTemperature <= COLD_THRESHOLD)
		{
			Condition = "COLD";
		}
		else
		{
			Condition = "SUITABLE";
		}

		TemperatureResult temperatureResult = new TemperatureResult(Condition, CurrentTemperature, BaseTemperature, UNIT);
		return temperatureResult;
	}
}

/// <summary>
/// TemperatureEvaluationクラス用のResultフォーマットクラス
/// </summary>
public class TemperatureResult : Result
{
	public string Condition { get; }
	public float CurrentTemperature { get; }
	public float BaseTemperature { get; }
	public string Unit { get; }
	public string Message
	{
		get
		{
			return $"現在の気温は{CurrentTemperature} {Unit}です。{Condition}です。平均気温から{Math.Abs(BaseTemperature - CurrentTemperature)}{Unit}離れています。";
		}
	}
	public TemperatureResult(string condition, float currentTemp, float baseTemp, string unit)
	{
		Condition = condition;
		CurrentTemperature = currentTemp;
		BaseTemperature = baseTemp;
		Unit = unit;
	}
}
