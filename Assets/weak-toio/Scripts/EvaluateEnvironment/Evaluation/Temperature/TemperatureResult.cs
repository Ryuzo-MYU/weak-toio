namespace Evaluation
{
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
		public override string EnvType
		{
			get
			{
				return "Temperature";
			}
		}
		public override string Message
		{
			get
			{
				string tempCondition;
				if (Condition > 0) { tempCondition = "暑い"; }
				else if (Condition < 0) { tempCondition = "寒い"; }
				else { tempCondition = "適温"; }

				return $"現在の気温は{CurrentTemperature}{Unit}です。コンディション: {Condition}。{tempCondition}です。";
			}
		}
		public override int Score
		{
			get
			{
				return Condition;
			}
		}
		public TemperatureResult()
		{
			Condition = 0;
			CurrentTemperature = 0;
			LowerBound = 0;
			UpperBound = 0;
			Unit = null;
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
}