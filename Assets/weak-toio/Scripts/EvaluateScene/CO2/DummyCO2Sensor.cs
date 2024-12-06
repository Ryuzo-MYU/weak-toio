using System.Collections;
using Evaluation;

namespace Environment
{
	public class DummyCO2Sensor : ISensorUnit, IM5Sensor, ICO2Sensor
	{
		private string _deviceName;
		private System.Numerics.Vector3 _accel;
		private System.Numerics.Vector3 _gyro;
		private float _vbat;
		private float _ppm;

		public string GetDeviceName() { return _deviceName; }
		public System.Numerics.Vector3 GetAcceleration() { return _accel; }
		public System.Numerics.Vector3 GetGyro() { return _gyro; }
		public float GetVbat() { return _vbat; }
		public float GetPPM() { return _ppm; }

		public DummyCO2Sensor()
		{
			_deviceName = "This-is-Dummy";
			_accel = System.Numerics.Vector3.Zero;
			_gyro = System.Numerics.Vector3.Zero;
			_ppm = 700f; // 初期値参照: https://www.mhlw.go.jp/content/11130500/000771220.pdf
			_vbat = 4.0f;
		}
		public EnvType GetEnvType() { return EnvType.CO2; }
		public void Start() { } // 何もしない

		/// <summary>
		/// 前回の値に基づいて、センサの値を変動させる
		/// </summary>
		public void Update()
		{
			float accelX = _accel.X + UnityEngine.Random.Range(-2f, 2f);
			float accelY = _accel.Y + UnityEngine.Random.Range(-2f, 2f);
			float accelZ = _accel.Z + UnityEngine.Random.Range(-2f, 2f);
			_accel = new System.Numerics.Vector3(accelX, accelY, accelZ);

			float gyroX = _gyro.X + UnityEngine.Random.Range(-50f, 50f);
			float gyroY = _gyro.Y + UnityEngine.Random.Range(-50f, 50f);
			float gyroZ = _gyro.Z + UnityEngine.Random.Range(-50f, 50f);
			_gyro = new System.Numerics.Vector3(gyroX, gyroY, gyroZ);

			_ppm += UnityEngine.Random.Range(-1f, 1f);
			_vbat += UnityEngine.Random.Range(-0.01f, 0.01f);
		}
		public void OnDataReceived(string message) { } // 何もしない
	}
}