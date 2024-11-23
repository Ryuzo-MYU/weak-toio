namespace Evaluation
{
	/// <summary>
	/// 環境データの評価結果を格納するクラス
	/// </summary>
	public struct Result
	{
		public string Message;
		public int Score;
		public Unit Unit;
		public Result(string _message, int _score, Unit _unit)
		{
			Message = _message;
			Score = _score;
			Unit = _unit;
		}
	}
}