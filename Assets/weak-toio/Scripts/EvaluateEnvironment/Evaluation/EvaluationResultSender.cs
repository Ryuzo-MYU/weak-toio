using System.Numerics;
using Environment;

namespace Evaluation
{
	public interface EvaluationResultSender
	{
		public Result GetEvaluationResult(SensorUnit sensor);
	}
	public struct SensorInfo
	{
		public string deviceName;
		public Vector3 acceleration;
		public Vector3 gyro;
		public float temp; // M5StickCの内部温度
		public float humidity; // ENV2の湿度
		public float pressure; // ENV2の気圧
		public float vbat; // バッテリー残量

		// コンストラクタ
		public SensorInfo(string name, Vector3 accelaraiton, Vector3 gyro, float temp, float humidity, float pressure, float vbat)
		{
			this.deviceName = name;
			this.acceleration = accelaraiton;
			this.gyro = gyro;
			this.temp = temp;
			this.humidity = humidity;
			this.pressure = pressure;
			this.vbat = vbat;
		}
	}
}