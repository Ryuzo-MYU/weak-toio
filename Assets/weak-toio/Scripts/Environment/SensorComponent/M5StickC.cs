using UnityEngine;

namespace Environment
{
	public class M5Sensor : MonoBehaviour, IM5Sensor, ISerialConnector
	{
		[SerializeField] private SerialHandler _serial;
		private string _deviceName;
		private Vector3 _acceleration;
		private Vector3 _gyro;
		private float _vbat;
		protected string[] receivedData;

		// ==============================
		// ISensorUnit実装
		// ==============================
		public void StartSensor()
		{
			_serial.OnDataReceived += OnDataReceived;
		}
		public void UpdateSensor() { }

		// ==============================
		// IM5Sensor実装
		// ==============================
		public string GetDeviceName() { return _deviceName; }
		public Vector3 GetAcceleration() { return _acceleration; }
		public Vector3 GetGyro() { return _gyro; }
		public float GetVbat() { return _vbat; }

		// ==============================
		// ISerialConnector実装
		// ==============================
		public virtual void OnDataReceived(string message)
		{
			string[] receivedData = SpritMessage(message);
			if (receivedData.Length < 1) return;
			try
			{
				_deviceName = receivedData[0];

				float[] imuInfo = new float[6];
				for (int i = 1; i <= 6; i++) { float.TryParse(receivedData[i], out imuInfo[i - 1]); }
				_acceleration = new Vector3(imuInfo[1], imuInfo[2], imuInfo[3]);
				_gyro = new Vector3(imuInfo[4], imuInfo[5], imuInfo[6]);
				float.TryParse(receivedData[7], out _vbat);         // バッテリー残量
				Debug.Log("シリアルデータの取得に成功");
			}
			catch (System.Exception e)
			{
				Debug.Log("シリアルデータの取得に失敗");
				Debug.LogWarning(e.Message);
				Debug.LogError(e.StackTrace);
			}
		}

		/// <summary>
		/// シリアルデータを区切り文字で分ける
		/// シリアルデータは\tで区切られたフォーマットである必要がある
		/// </summary>
		/// <param name="message">シリアルデータ</param>
		/// <returns>区切られたシリアルデータ</returns>	
		protected string[] SpritMessage(string message)
		{
			return message.Split(new string[] { "\t" }, System.StringSplitOptions.None);
		}
	}
}