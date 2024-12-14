using UnityEngine;

namespace Environment
{
	public class M5SensorWithEnv2 : M5Sensor, IENV2Sensor
	{
		private float _temp;
		private float _hum;
		private float _pressure;

		// ==============================
		// IENV2Sensor実装
		// ==============================
		public float GetTemperature() { return _temp; }
		public float GetHumidity() { return _hum; }
		public float GetPressure() { return _pressure; }

		// ==============================
		// M5Sensorのデシリアライズ後，ENV2用のデータをデシリアライズ
		// ==============================
		public override void OnDataReceived(string message)
		{
			base.OnDataReceived(message);
			try
			{
				float.TryParse(receivedData[8], out _temp);         // ENV2の気温
				float.TryParse(receivedData[9], out _hum);          // ENV2の湿度
				float.TryParse(receivedData[10], out _pressure);    // ENV2の気圧
			}
			catch
			{
				Debug.LogError("シリアルデータのフォーマットが間違ってる？");
				Debug.Log("送信されたデータ: " + message);
			}
		}
	}
}
