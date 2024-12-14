namespace Evaluation
{
	public interface IEvaluationResultSender<T>
	{
		public void GenerateEvaluationResult(T sensor);
		public EnvType GetEnvType();
	}
}