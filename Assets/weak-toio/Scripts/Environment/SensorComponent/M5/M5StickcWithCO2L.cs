using UnityEngine;

namespace Environment
{
	public class M5StickcWithCO2L : M5Stickc, ICO2LSensor
	{
		[SerializeField] private float _temp;
		[SerializeField] private float _hum;
		[SerializeField] private float _co2;

		// ==============================
		// ICO2LSensor実装
		// ==============================
		public float GetTemperature() { return _temp; }
		public float GetHumidity() { return _hum; }
		public float GetCO2() { return _co2; }
		public override void OnDataReceived(string message)
		{
			string[] receivedData = SpritMessage(message);
			base.DeserializeMessages(receivedData);
			DeserializeMessages(receivedData);
			_OnDeserializeCompleted();
		}
		private new void DeserializeMessages(string[] splittedMessage)
		{
			try
			{
				CheckDataLength(splittedMessage, requiredLength);
				float.TryParse(splittedMessage[1], out _temp);         // ENV2の気温
				float.TryParse(splittedMessage[2], out _hum);          // ENV2の湿度
				float.TryParse(splittedMessage[3], out _co2);    // ENV2の気圧
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