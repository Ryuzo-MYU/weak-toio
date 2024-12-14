using UnityEngine;
using UnityEngine.Events;

namespace Environment
{
	[RequireComponent(typeof(SerialHandler))]
	public class M5Stickc : SensorBase, IM5Sensor, ISerialConnector
	{
		protected int requiredLength;
		protected string[] receivedData;
		[SerializeField] private SerialHandler _serial;
		private string _deviceName;
		private Vector3 _acceleration;
		private Vector3 _gyro;
		private float _vbat;

		public void Start()
		{
			StartSensor();
		}

		// ==============================
		// ISensorUnit実装
		// ==============================
		public void StartSensor()
		{
			_serial = gameObject.GetComponent<SerialHandler>();
			_serial.OnDataReceived += OnDataReceived;
			_serial.OnConnectFailed += OnConnectFailed;
		}

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
			DeserializeMessages(receivedData);
			DeserializeCompleted();
		}

		/// <summary>
		/// シリアルデータを区切り文字で分ける
		/// シリアルデータは\tで区切られたフォーマットである必要がある
		/// </summary>
		/// <param name="message">シリアルデータ</param>
		/// <returns>区切られたシリアルデータ</returns>	
		protected string[] SpritMessage(string message)
		{
			try
			{
				return message.Split(new string[] { "\t" }, System.StringSplitOptions.None);
			}
			catch
			{
				Debug.LogError("シリアルデータのフォーマットが間違ってる？");
				Debug.Log("送信されたデータ: " + message);
				return null;
			}
		}
		protected void DeserializeMessages(string[] splittedMessage)
		{
			try
			{
				CheckDataLength(splittedMessage, requiredLength);
				_deviceName = splittedMessage[0];

				float[] imuInfo = new float[6];
				for (int i = 1; i <= 6; i++) { float.TryParse(splittedMessage[i], out imuInfo[i - 1]); }
				_acceleration = new Vector3(imuInfo[1], imuInfo[2], imuInfo[3]);
				_gyro = new Vector3(imuInfo[4], imuInfo[5], imuInfo[6]);
				float.TryParse(splittedMessage[7], out _vbat);         // バッテリー残量
				Debug.Log("シリアルデータの取得に成功");
			}
			catch (System.Exception e)
			{
				Debug.Log("シリアルデータの取得に失敗");
				Debug.LogWarning(e.Message);
				Debug.LogError(e.StackTrace);
			}
		}
		protected void CheckDataLength(string[] data, int requirLength)
		{
			if (data == null || data.Length < requirLength)
			{
				throw new System.ArgumentException($"受信データが不足しています。必要な長さ: {requirLength}, 実際の長さ: {data?.Length ?? 0}");
			}
		}
		public void OnConnectFailed()
		{
			Destroy(this);
		}
	}
}