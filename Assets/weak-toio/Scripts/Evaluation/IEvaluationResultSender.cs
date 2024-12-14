namespace Evaluation
{
	public interface IEvaluationResultSender<T>
	{
		public void GetEvaluationResult(T sensor);
		public EnvType GetEnvType();
	}
}