using UnityEngine;

namespace Environment
{
	public class M5StickcWithENV2 : M5Stickc, IENV2Sensor
	{
		[SerializeField] private float _temp;
		[SerializeField] private float _hum;
		[SerializeField] private float _pressure;

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
			string[] receivedData = SpritMessage(message);
			base.DeserializeMessages(receivedData);
			DeserializeMessages(receivedData);
			OnDeserializeCompleted.Invoke();
		}
		private void DeserializeMessages(string[] splittedMessage)
		{
			try
			{
				CheckDataLength(splittedMessage, requiredLength);
				float.TryParse(splittedMessage[8], out _temp);         // ENV2の気温
				float.TryParse(splittedMessage[9], out _hum);          // ENV2の湿度
				float.TryParse(splittedMessage[10], out _pressure);    // ENV2の気圧
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
