using UnityEngine;

public class M5DataReceiver : SensorUnit
{
	public SerialHandler serialHandler;

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
	public void Awake()
	{
		serialHandler = new SerialHandler();
		serialHandler.Awake();
	}
	public override void Start()
	{
		serialHandler.OnDataReceived += this.OnDataReceived;
	}
	public override void Update()
	{
		serialHandler.Update();
	}
}