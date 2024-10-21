namespace EvaluateEnvironment
{
	public abstract class Evaluation
	{
		protected M5DataReceiver m5;
		public abstract Result Evaluate(M5DataReceiver m5);
	}

	/// <summary>
	/// 環境データの評価結果を扱うクラス
	/// </summary>
	public abstract class Result
	{
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