using System.Collections;
using Evaluation;

namespace Environment
{
	public class DummyTemperatureSensor : ISensorUnit, IM5Sensor
	{
		public SensorInfo sensorInfo;
		public string deviceName;
		// 前回の値を保持するフィールド
		public System.Numerics.Vector3 previousAccel;
		public System.Numerics.Vector3 previousGyro;
		public float previousTemperature;
		public float previousHumidity;
		public float previousPressure;
		public float previousVbat;

		public DummyTemperatureSensor()
		{
			deviceName = "This-is-Dummy";
			previousAccel = System.Numerics.Vector3.Zero;
			previousGyro = System.Numerics.Vector3.Zero;
			previousTemperature = 25f;
			previousHumidity = 50f;
			previousPressure = 101325f; // https://w.wiki/3DDH
			previousVbat = 4.0f;
		}
		public SensorInfo GetSensorInfo() { return sensorInfo; }
		public EnvType GetEnvType() { return EnvType.Temperature; }
		// 何もしない
		public void Start() { }
		public void Update()
		{
			// 加速度とジャイロのダミーデータ（前回の値に基づいて変動）
			float accelX = previousAccel.X + UnityEngine.Random.Range(-2f, 2f);
			float accelY = previousAccel.Y + UnityEngine.Random.Range(-2f, 2f);
			float accelZ = previousAccel.Z + UnityEngine.Random.Range(-2f, 2f);
			float gyroX = previousGyro.X + UnityEngine.Random.Range(-50f, 50f);
			float gyroY = previousGyro.Y + UnityEngine.Random.Range(-50f, 50f);
			float gyroZ = previousGyro.Z + UnityEngine.Random.Range(-50f, 50f);

			// 気温、湿度、気圧、バッテリー電圧のダミーデータ（前回の値に基づいて変動）
			float temperature = previousTemperature + UnityEngine.Random.Range(-1f, 1f);
			float humidity = previousHumidity + UnityEngine.Random.Range(-1f, 1f);
			float pressure = previousPressure + UnityEngine.Random.Range(-1f, 1f);
			float batteryVoltage = previousVbat + UnityEngine.Random.Range(-0.01f, 0.01f);

			// 新しい値をフィールドに保存
			previousAccel = new System.Numerics.Vector3(accelX, accelY, accelZ);
			previousGyro = new System.Numerics.Vector3(gyroX, gyroY, gyroZ);
			previousTemperature = temperature;
			previousHumidity = humidity;
			previousPressure = pressure;
			previousVbat = batteryVoltage;

			sensorInfo = new SensorInfo(
				deviceName,
				new System.Numerics.Vector3(accelX, accelY, accelZ),
				new System.Numerics.Vector3(gyroX, gyroY, gyroZ),
				temperature,
				humidity,
				pressure,
				batteryVoltage
			);
		}
		public void OnDataReceived(string message) { } // 何もしない
	}
}