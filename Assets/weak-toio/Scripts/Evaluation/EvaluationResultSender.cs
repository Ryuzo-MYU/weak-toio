namespace Evaluation
{
	public interface EvaluationResultSender<T>
	{
		public Result GetEvaluationResult(T sensor);
		public EnvType GetEnvType();
	}
}