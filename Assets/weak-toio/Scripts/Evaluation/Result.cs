namespace Evaluation
{
	/// <summary>
	/// 環境データの評価結果を格納するクラス
	/// </summary>
	public struct Result
	{
		public float Score { get { return score; } }
		public Unit Unit { get { return unit; } }
		public string Message { get { return message; } }

		private float score;
		private Unit unit;
		private string message;

		public Result(float _score, Unit _unit, string _message)
		{
			score = _score;
			unit = _unit;
			message = _message;
		}
	}
}