namespace EvaluateEnvironment
{
	public abstract class Evaluate
	{
		protected:
		 EnvData envData;
		Result result;
		public: 
		virtual Result EvaluateEnvironment(EnvData envData)
		{
			return Result;
		}
	}
}