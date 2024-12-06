using System.Numerics;

namespace Environment
{
	public interface IM5Sensor : IAccelerationSensor, IGyroSensor
	{
		public string GetDeviceName();
		public float GetVbat(); // バッテリー残量
	}
}