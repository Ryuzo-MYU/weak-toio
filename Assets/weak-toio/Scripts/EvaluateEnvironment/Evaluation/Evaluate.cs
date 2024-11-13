using Environment;

namespace Evaluation
{
	/// <summary>
	/// 環境データを評価するクラス
	/// </summary>	
	public abstract class Evaluate
	{
		protected SensorUnit sensor;
		protected EnvType envType;
		public abstract Result EvaluateEnv(SensorUnit sensor);
		
		/// <summary>
		/// 対象とする環境タイプの取得。ない場合はエラーを返す 
		/// </summary>
		/// <param name="expectedType"></param>
		/// <returns></returns>
		public abstract EnvType GetEnvType();
	}
}