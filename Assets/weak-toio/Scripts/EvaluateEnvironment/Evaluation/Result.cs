namespace Evaluation
{
	/// <summary>
	/// 環境データの評価結果を格納するクラス
	/// </summary>
	public struct Result
	{
		public float Score;
		public Unit Unit;
		public Result(float _score, Unit _unit)
		{
			Score = _score;
			Unit = _unit;
		}
	}
}