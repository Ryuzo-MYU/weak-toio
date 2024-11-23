using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Environment;

namespace Evaluation
{
	public interface EvaluationResultSender
	{
		public Result GetEvaluationResult(SensorUnit sensor);
	}
}