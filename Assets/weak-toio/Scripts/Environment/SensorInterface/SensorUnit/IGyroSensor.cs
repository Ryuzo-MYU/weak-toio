using UnityEngine;

namespace Environment
{
	public interface IGyroSensor : ISensorUnit
	{
		public Vector3 GetGyro();
	}
}