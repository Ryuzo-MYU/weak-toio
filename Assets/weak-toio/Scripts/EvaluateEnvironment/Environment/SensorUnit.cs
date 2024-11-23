using Evaluation;

namespace Environment
{
	public interface SensorUnit
	{
		public SensorInfo GetSensorInfo();
		public void Start();
		public void Update();
	}
}