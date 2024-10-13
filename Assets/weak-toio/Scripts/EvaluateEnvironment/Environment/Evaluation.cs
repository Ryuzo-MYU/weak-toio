namespace EvaluateEnvironment
{
	public abstract class Evaluation
	{
		protected M5DataReceiver m5;
		public abstract Result Evaluate(M5DataReceiver m5);
	}
	public abstract class Result { }
}