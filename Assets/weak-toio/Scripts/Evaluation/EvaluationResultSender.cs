using System.Collections.Generic;

namespace Evaluation
{
	public interface EvaluationResultSender<T>
	{
		public Result GetEvaluationResult(T sensor);
		public List<EnvType> GetEnvTypes();
	}

}