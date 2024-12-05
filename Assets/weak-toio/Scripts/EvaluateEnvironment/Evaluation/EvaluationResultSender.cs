using System.Numerics;
using Environment;

namespace Evaluation
{
	public interface EvaluationResultSender<T>
	{
		public Result GetEvaluationResult(T sensor);
	}

}