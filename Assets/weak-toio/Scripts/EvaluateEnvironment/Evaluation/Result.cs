namespace Evaluation
{
	/// <summary>
	/// 環境データの評価結果を格納するクラス
	/// </summary>
	public abstract class Result
	{
		public abstract string Message { get; }
		public abstract int Score { get; }
		public abstract string EnvType { get; }
	}
}