namespace Evaluation
{
	/// <summary>
	/// 環境データの評価結果を格納するクラス
	/// </summary>
	public struct Result
	{
		public string Message;
		public int Score;
		public Unit Type;
		public Result(string message, int score, Unit type)
		{
			Message = message;
			Score = score;
			Type = type;
		}
	}
}