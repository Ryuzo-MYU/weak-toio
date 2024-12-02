using System.Numerics;

namespace Environment
{
	public interface IM5Sensor
	{
		public string GetDeviceName();
		public Vector3 GetAcceleration();
		public Vector3 GetGyro();
		public float GetVbat(); // バッテリー残量
	}
}