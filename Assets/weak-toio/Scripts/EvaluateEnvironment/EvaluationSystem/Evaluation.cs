namespace EvaluateEnvironment
{
	public abstract class Evaluation
	{
		protected SensorUnit sensor;
		public abstract Result Evaluate(SensorUnit sensor);
	}

	/// <summary>
	/// 環境データの評価結果を扱うクラス
	/// </summary>
	public abstract class Result
	{
		public abstract string Message { get; }
		public abstract int Score { get; }

	}
}