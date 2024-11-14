using UnityEngine;

namespace Environment
{
	public class DummySensor : SensorUnit
	{
		public string deviceName;
		// 前回の値を保持するフィールド
		public Vector3 previousAccel;
		public Vector3 previousGyro;
		public float previousTemperature;
		public float previousHumidity;
		public float previousPressure;
		public float previousVbat;

		public DummySensor()
		{
			deviceName = "This-is-Dummy";
			previousAccel = Vector3.zero;
			previousGyro = Vector3.zero;
			previousTemperature = 25f;
			previousHumidity = 50f;
			previousPressure = 101325f; // https://w.wiki/3DDH
			previousVbat = 4.0f;
		}

		public void Update()
		{
			// 加速度とジャイロのダミーデータ（前回の値に基づいて変動）
			float accelX = previousAccel.x + Random.Range(-2f, 2f);
			float accelY = previousAccel.y + Random.Range(-2f, 2f);
			float accelZ = previousAccel.z + Random.Range(-2f, 2f);
			float gyroX = previousGyro.x + Random.Range(-50f, 50f);
			float gyroY = previousGyro.y + Random.Range(-50f, 50f);
			float gyroZ = previousGyro.z + Random.Range(-50f, 50f);

			// 気温、湿度、気圧、バッテリー電圧のダミーデータ（前回の値に基づいて変動）
			float temperature = previousTemperature + Random.Range(-1f, 1f);
			float humidity = previousHumidity + Random.Range(-1f, 1f);
			float pressure = previousPressure + Random.Range(-1f, 1f);
			float batteryVoltage = previousVbat + Random.Range(-0.01f, 0.01f);

			// 新しい値をフィールドに保存
			previousAccel = new Vector3(accelX, accelY, accelZ);
			previousGyro = new Vector3(gyroX, gyroY, gyroZ);
			previousTemperature = temperature;
			previousHumidity = humidity;
			previousPressure = pressure;
			previousVbat = batteryVoltage;

			sensorInfo = new SensorInfo(
				deviceName,
				new Vector3(accelX, accelY, accelZ),
				new Vector3(gyroX, gyroY, gyroZ),
				temperature,
				humidity,
				pressure,
				batteryVoltage
			);
		}
	}
}