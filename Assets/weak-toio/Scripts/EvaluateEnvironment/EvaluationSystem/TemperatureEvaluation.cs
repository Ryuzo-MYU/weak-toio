using System;
using EvaluateEnvironment;

/// <summary>
/// 気温データを取得し、評価をするクラス
/// </summary>
public class TemperatureEvaluation : Evaluation
{
	private const float LOWER_BOUND = 18.0f; // 寒すぎる基準
	private const float UPPER_BOUND = 28.0f;  // 暑すぎる基準
	private const string UNIT = "℃";
	public float CurrentTemperature { get; private set; }
	public int Condition { get; private set; }

	/// <summary>
	/// M5DataReceiverから気温のデータを取得し、労働環境の適温範囲と比較した結果を返す
	/// </summary>
	/// <param name="m5">M5DataReceiverインスタンス</param>
	/// <returns>評価結果を集約したResult型データ</returns>
	public override Result Evaluate(M5DataReceiver m5)
	{
		CurrentTemperature = m5.sensorInfo.temp; // M5DataReceiverから気温を取得

		// 気温に基づく評価
		if (CurrentTemperature < LOWER_BOUND)
		{
			Condition = (int)(CurrentTemperature - LOWER_BOUND); // マイナスのスコア
		}
		else if (CurrentTemperature > UPPER_BOUND)
		{
			Condition = (int)(CurrentTemperature - UPPER_BOUND); // プラスのスコア
		}
		else
		{
			Condition = 0; // 適温
		}

		TemperatureResult temperatureResult = new TemperatureResult(Condition, CurrentTemperature, LOWER_BOUND, UPPER_BOUND, UNIT);
		return temperatureResult;
	}
}

/// <summary>
/// TemperatureEvaluationクラス用のResultフォーマットクラス
/// </summary>
public class TemperatureResult : Result
{
	public int Condition { get; }
	public float CurrentTemperature { get; }
	public float LowerBound { get; }
	public float UpperBound { get; }
	public string Unit { get; }
	public override string Message
	{
		get
		{
			string tempCondition = Condition == 0 ? "適温"
				: Condition > 0 ? "暑い"
				: "寒い";

			return $"現在の気温は{CurrentTemperature}{Unit}です。コンディション: {Condition}。{tempCondition}です。";
		}
	}
	public TemperatureResult(int condition, float currentTemp, float lowerBound, float upperBound, string unit)
	{
		Condition = condition;
		CurrentTemperature = currentTemp;
		LowerBound = lowerBound;
		UpperBound = upperBound;
		Unit = unit;
	}
}
