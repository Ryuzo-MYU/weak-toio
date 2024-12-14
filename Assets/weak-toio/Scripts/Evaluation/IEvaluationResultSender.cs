namespace Evaluation
{
	public interface IEvaluationResultSender<T>
	{
		public Result GetEvaluationResult(T sensor);
		public EnvType GetEnvType();
	}
}