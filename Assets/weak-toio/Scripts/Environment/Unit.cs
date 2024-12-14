using System;

namespace Evaluation
{
	[Serializable]
	public struct Unit
	{
		public string unit;
		public Unit(string _unit)
		{
			unit = _unit;
		}
	}
}