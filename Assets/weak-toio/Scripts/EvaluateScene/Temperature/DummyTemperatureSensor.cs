namespace Environment
{
	public class DummyTemperatureSensor : ISensorUnit, IM5Sensor, IENV2Sensor
	{
		private string deviceName;
		private System.Numerics.Vector3 accel;
		private System.Numerics.Vector3 gyro;
		private float temp;
		private float hum;
		private float pressure;
		private float vbat;

		public string GetDeviceName() { return deviceName; }
		public System.Numerics.Vector3 GetAcceleration() { return accel; }
		public System.Numerics.Vector3 GetGyro() { return gyro; }
		public float GetVbat() { return vbat; }
		public float GetTemperature() { return temp; }
		public float GetHumidity() { return hum; }
		public float GetPressure() { return pressure; }

		public DummyTemperatureSensor()
		{
			deviceName = "This-is-Dummy";
			accel = System.Numerics.Vector3.Zero;
			gyro = System.Numerics.Vector3.Zero;
			temp = 25f;
			hum = 50f;
			pressure = 101325f; // https://w.wiki/3DDH
			vbat = 4.0f;
		}
		public EnvType GetEnvType() { return EnvType.Temperature; }
		// 何もしない
		public void Start() { }
		public void Update()
		{
			// 加速度とジャイロのダミーデータ（前回の値に基づいて変動）
			float accelX = accel.X + UnityEngine.Random.Range(-2f, 2f);
			float accelY = accel.Y + UnityEngine.Random.Range(-2f, 2f);
			float accelZ = accel.Z + UnityEngine.Random.Range(-2f, 2f);
			accel = new System.Numerics.Vector3(accelX, accelY, accelZ);

			float gyroX = gyro.X + UnityEngine.Random.Range(-50f, 50f);
			float gyroY = gyro.Y + UnityEngine.Random.Range(-50f, 50f);
			float gyroZ = gyro.Z + UnityEngine.Random.Range(-50f, 50f);
			gyro = new System.Numerics.Vector3(gyroX, gyroY, gyroZ);

			// 気温、湿度、気圧、バッテリー電圧のダミーデータ（前回の値に基づいて変動）
			temp += UnityEngine.Random.Range(-1f, 1f);
			hum += UnityEngine.Random.Range(-1f, 1f);
			pressure += UnityEngine.Random.Range(-1f, 1f);
			vbat += UnityEngine.Random.Range(-0.01f, 0.01f);
		}
		public void OnDataReceived(string message) { } // 何もしない
	}
}