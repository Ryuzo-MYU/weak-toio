using System.Numerics;

namespace Environment
{
	public interface IAccelerationSensor : ISensorUnit
	{
		public Vector3 GetAcceleration();
	}
}