using System.Collections;
using UnityEngine;

namespace Environment
{
	[RequireComponent(typeof(SerialHandler))]
	public class M5Stickc : SensorBase, ISerialConnector, RealSensor
	{
		[SerializeField] protected int requiredLength;
		protected string[] receivedData;

		[SerializeField] protected string _deviceName;

		private void Start()
		{
			_serial.OnDataReceived += OnDataReceived;
		}
		// ==============================
		// ISensorUnit実装
		// ==============================
		public void StartSensor() { }

		// ==============================
		// IM5Sensor実装
		// ==============================
		public string GetDeviceName() { return _deviceName; }
		// ==============================
		// ISerialConnector実装
		// ==============================
		public virtual void OnDataReceived(string message)
		{
			string[] receivedData = SpritMessage(message);
			DeserializeMessages(receivedData);
			_OnDeserializeCompleted();
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
	}
}