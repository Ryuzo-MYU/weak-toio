namespace Evaluation
{
	public struct TemperatureRange
	{
		public float UpperLimit;
		public float LowerLimit;
		public TemperatureRange(float upper, float lower)
		{
			UpperLimit = upper;
			LowerLimit = lower;
		}
		public bool isWithInRange(float subject)
		{
			if (subject < LowerLimit || subject > UpperLimit)
			{
				return false;
			}
			else
			{
				return true;
			}
		}
	}
}