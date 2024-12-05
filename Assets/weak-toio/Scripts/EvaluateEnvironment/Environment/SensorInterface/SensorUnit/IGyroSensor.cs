using System.Numerics;

namespace Environment
{
	public interface IGyroSensor : ISensorUnit
	{
		public Vector3 GetGyro();
	}
}