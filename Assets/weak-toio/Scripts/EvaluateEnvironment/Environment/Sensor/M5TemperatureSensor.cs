using Evaluation;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;
using System.Text;

namespace Environment
{
	public class M5TemperatureSensor : ISensorUnit, IM5Sensor, IENV2Sensor, ISerialConnector
	{
		private SerialHandler _serial;
		private string _deviceName;
		private Vector3 _acceleration;
		private Vector3 _gyro;
		private float _vbat;
		private float _temp;
		private float _hum;
		private float _pressure;

		public M5TemperatureSensor(SerialHandler _serial) { this._serial = _serial; }

		// ==============================
		// SensorUnit実装
		// ==============================
		public EnvType GetEnvType() { return EnvType.Temperature; }
		public void Start() { _serial.OnDataReceived += OnDataReceived; }
		public void Update() { } // 何もしない

		// ==============================
		// IM5Sensor実装
		// ==============================
		public string GetDeviceName() { return _deviceName; }
		public Vector3 GetAcceleration() { return _acceleration; }
		public Vector3 GetGyro() { return _gyro; }
		public float GetVbat() { return _vbat; }

		// ==============================
		// IENV2Sensor実装
		// ==============================
		public float GetTemperature() { return _temp; }
		public float GetHumidity() { return _hum; }
		public float GetPressure() { return _pressure; }

		//受信した信号(message)に対する処理
		public void OnDataReceived(string message)
		{
			var data = message.Split(
					new string[] { "\t" }, System.StringSplitOptions.None);
			if (data.Length < 1) return;
			try
			{
				_deviceName = data[0];

				float[] imuInfo = new float[6];
				for (int i = 1; i <= 6; i++) { float.TryParse(data[i], out imuInfo[i - 1]); }
				_acceleration = new Vector3(imuInfo[1], imuInfo[2], imuInfo[3]);
				_gyro = new Vector3(imuInfo[4], imuInfo[5], imuInfo[6]);

				float.TryParse(data[7], out _temp); // M5StickCの温度
				float.TryParse(data[8], out _hum); // ENV2の湿度
				float.TryParse(data[9], out _pressure); // ENV2の気圧
				float.TryParse(data[10], out _vbat); // バッテリー残量

				Debug.Log("シリアルデータの取得に成功");
			}
			catch (System.Exception e)
			{
				Debug.Log("シリアルデータの取得に失敗");
				Debug.LogWarning(e.Message);
				Debug.LogError(e.StackTrace);
			}
		}

		public void LogSensorInfo(string prefix = "")
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine($"=== {prefix}SensorInfo Debug Output ===");
			sb.AppendLine($"Name: {_deviceName}");

			// IMUデータ (加速度・角速度) の出力
			sb.AppendLine("IMU Data:");
			sb.AppendLine($"  Acceleration: {FormatVector3(_acceleration)}");
			sb.AppendLine($"  Angular Velocity: {FormatVector3(_gyro)}");

			// 環境データの出力
			sb.AppendLine("Environmental Data:");
			sb.AppendLine($"  Temperature: {_temp:F2}°C");
			sb.AppendLine($"  Humidity: {_hum:F2}%");
			sb.AppendLine($"  Pressure: {_pressure:F2}hPa");

			// 電圧データの出力
			sb.AppendLine($"Battery Voltage: {_vbat:F2}V");
			sb.AppendLine("===========================");

			Debug.Log(sb.ToString());
		}

		private static string FormatVector3(Vector3 vector)
		{
			return $"(X: {vector.X:F3}, Y: {vector.Y:F3}, Z: {vector.Z:F3})";
		}
	}
}