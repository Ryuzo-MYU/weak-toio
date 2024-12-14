using Evaluation;
using UnityEngine;
using System.Text;
using System.Collections.Generic;

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
		public List<EnvType> GetEnvType()
		{
			return new List<EnvType>{
				EnvType.Temperature
			};
		}
		public void StartSensor() { _serial.OnDataReceived += OnDataReceived; }
		public void UpdateSensor() { } // 何もしない

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
	}
}