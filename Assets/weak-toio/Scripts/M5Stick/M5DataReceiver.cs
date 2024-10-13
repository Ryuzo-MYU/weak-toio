using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M5DataReceiver
{
	public SerialHandler serialHandler;
	public SensorInfo sensorInfo;

	//受信した信号(message)に対する処理
	public void OnDataReceived(string message)
	{
		Debug.Log(message);
		var data = message.Split(
				new string[] { "\t" }, System.StringSplitOptions.None);
		if (data.Length < 1) return;

		try
		{
			Debug.Log(data.Length);
			foreach (var item in data) { Debug.Log(item); }

			string name = data[0];

			float[] imuInfo = new float[6];
			for (int i = 1; i <= 6; i++) { float.TryParse(data[i], out imuInfo[i - 1]); }

			float temp, humidity, pressure, vbat;
			float.TryParse(data[7], out temp); // M5StickCの温度
			float.TryParse(data[8], out humidity); // ENV2の湿度
			float.TryParse(data[9], out pressure); // ENV2の気圧
			float.TryParse(data[10], out vbat); // バッテリー残量

			sensorInfo = new SensorInfo(
				name,
				new Vector3(imuInfo[0], imuInfo[1], imuInfo[2]),
				new Vector3(imuInfo[3], imuInfo[4], imuInfo[5]),
				temp,
				humidity,
				pressure,
				vbat
			);

		}
		catch (System.Exception e)
		{
			Debug.LogWarning(e.Message);
			Debug.LogError(e.StackTrace);
		}
	}

	// M5が取得したセンサー情報
	public struct SensorInfo : IEnumerable<object>
	{
		public string deviceName;

		public Vector3 accelaration;
		public Vector3 gyro;

		public float temp; // M5StickCの内部温度
		public float humidity; // ENV2の湿度
		public float pressure; // ENV2の気圧
		public float vbat; // バッテリー残量

		// コンストラクタ
		public SensorInfo(string name, Vector3 accelaration, Vector3 gyro, float temp, float humidity, float pressure, float vbat)
		{
			this.deviceName = name;
			this.accelaration = accelaration;
			this.gyro = gyro;
			this.temp = temp;
			this.humidity = humidity;
			this.pressure = pressure;
			this.vbat = vbat;
		}
		// IEnumerable<object>のGetEnumerator()の実装
		public IEnumerator<object> GetEnumerator()
		{
			yield return deviceName;
			yield return accelaration;
			yield return gyro;
			yield return temp;
			yield return humidity;
			yield return pressure;
			yield return vbat;
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}