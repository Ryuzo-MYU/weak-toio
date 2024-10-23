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
		/// <summary>
		/// 環境スコアの和 result を返す。
		/// resultの値をもとにActionOperatorのActionを決定する
		/// </summary>
		public int GetEvaluationScore(params int[] scores)
		{
			int result = 0;
			foreach (int score in scores)
			{
				result += score;
			}
			return result;
		}
	}
}