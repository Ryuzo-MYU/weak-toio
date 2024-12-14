using UnityEngine;

namespace Environment
{
	public class DummyTemperatureSensor : ISensorUnit, IM5Sensor, IENV2Sensor
	{
		private string deviceName;
		private Vector3 accel;
		private Vector3 gyro;
		private float vbat;
		private float temp;
		private float hum;
		private float pressure;

		public string GetDeviceName() { return deviceName; }
		public Vector3 GetAcceleration() { return accel; }
		public Vector3 GetGyro() { return gyro; }
		public float GetVbat() { return vbat; }
		public float GetTemperature() { return temp; }
		public float GetHumidity() { return hum; }
		public float GetPressure() { return pressure; }

		public DummyTemperatureSensor()
		{
			deviceName = "This-is-Dummy";
			accel = Vector3.zero;
			gyro = Vector3.zero;
			temp = 25f;
			hum = 50f;
			pressure = 101325f; // https://w.wiki/3DDH
			vbat = 4.0f;
		}
		// 何もしない
		public void StartSensor() { }
		public void UpdateSensor()
		{
			// 加速度とジャイロのダミーデータ（前回の値に基づいて変動）
			float accelX = accel.x + Random.Range(-2f, 2f);
			float accelY = accel.y + Random.Range(-2f, 2f);
			float accelZ = accel.z + Random.Range(-2f, 2f);
			accel = new Vector3(accelX, accelY, accelZ);

			float gyroX = gyro.x + Random.Range(-50f, 50f);
			float gyroY = gyro.y + Random.Range(-50f, 50f);
			float gyroZ = gyro.z + Random.Range(-50f, 50f);
			gyro = new Vector3(gyroX, gyroY, gyroZ);

			// 気温、湿度、気圧、バッテリー電圧のダミーデータ（前回の値に基づいて変動）
			temp += Random.Range(-1f, 1f);
			hum += Random.Range(-1f, 1f);
			pressure += Random.Range(-1f, 1f);
			vbat += Random.Range(-0.01f, 0.01f);
		}
		public void OnDataReceived(string message) { } // 何もしない
	}
}