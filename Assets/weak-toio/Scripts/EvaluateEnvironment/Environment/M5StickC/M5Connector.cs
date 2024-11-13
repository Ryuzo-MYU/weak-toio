using System;
using Environment;
using UnityEngine;
public class M5Connector : MonoBehaviour
{
	public M5DataReceiver m5;

	[Tooltip("PCの「デバイスマネージャー」or Bluetooth設定の「その他のBluetooth設定」から、M5StickCがつながっているCOMポートを確認して指定。")]
	public string[] PORTS = { "COM9", "COM10" }; // 接続用ポートリスト
	public string PORTNAME = "m5-02"; // ポート名
	public int BAUDRATE = 112500; // ボーレート
	Action updateCoroutine;
	[SerializeField] int interval = 1;
	[SerializeField] SensorInfo sensorInfo;

	private void Awake()
	{
		m5 = new M5DataReceiver(PORTS, PORTNAME, BAUDRATE);
		m5.Awake();
	}

	private void Start()
	{
		updateCoroutine += UpdateM5;
		updateCoroutine += UpdateSensorInfo;
		StartCoroutine(MyLibrary.DoFuncWithInterval(interval, updateCoroutine));

		// 初期化
		m5.Update();
	}

	private void UpdateM5()
	{
		m5.Update();
	}
	private void UpdateSensorInfo()
	{
		sensorInfo.Update(m5);
	}

	[System.Serializable]
	private struct SensorInfo
	{
		public string deviceName;
		public Vector3 accelaraiton;
		public Vector3 gyro;
		public float temp; // M5StickCの内部温度
		public float humidity; // ENV2の湿度
		public float pressure; // ENV2の気圧
		public float vbat; // バッテリー残量

		// コンストラクタ
		public SensorInfo(string name, Vector3 accelaraiton, Vector3 gyro, float temp, float humidity, float pressure, float vbat)
		{
			this.deviceName = name;
			this.accelaraiton = accelaraiton;
			this.gyro = gyro;
			this.temp = temp;
			this.humidity = humidity;
			this.pressure = pressure;
			this.vbat = vbat;
		}
		public void Update(SensorUnit sensor)
		{
			deviceName = sensor.sensorInfo.deviceName;
			accelaraiton = sensor.sensorInfo.acceleration;
			gyro = sensor.sensorInfo.gyro;
			temp = sensor.sensorInfo.temp;
			humidity = sensor.sensorInfo.humidity;
			pressure = sensor.sensorInfo.pressure;
			vbat = sensor.sensorInfo.vbat;
		}
	}
}
