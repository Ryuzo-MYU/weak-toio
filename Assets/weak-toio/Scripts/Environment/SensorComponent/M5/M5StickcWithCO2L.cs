using UnityEngine;

namespace Environment
{
	public class M5StickcWithCO2L : M5Stickc, ICO2LSensor
	{
		[SerializeField] private float _temp;
		[SerializeField] private float _hum;
		[SerializeField] private float _ppm;

		// ==============================
		// ICO2LSensor実装
		// ==============================
		public float GetTemperature() { return _temp; }
		public float GetHumidity() { return _hum; }
		public float GetPPM() { return _ppm; }
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
				float.TryParse(splittedMessage[10], out _ppm);    // ENV2の気圧
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