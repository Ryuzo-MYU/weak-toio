namespace Evaluation
{
	public struct BoundaryRange
	{
		public float UpperLimit;
		public float LowerLimit;
		public BoundaryRange(float upper, float lower)
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