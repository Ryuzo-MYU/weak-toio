using UnityEngine;

public abstract class SensorUnit
{
	public SensorInfo sensorInfo;
	// M5が取得したセンサー情報
	public abstract void Start();
	public abstract void Update();
	public struct SensorInfo
	{
		public string DeviceName { get; private set; }
		public Vector3 Accelaration { get; private set; }
		public Vector3 Gyro { get; private set; }
		public float Temp { get; private set; } // M5StickCの内部温度
		public float Humidity { get; private set; } // ENV2の湿度
		public float Pressure { get; private set; } // ENV2の気圧
		public float Vbat { get; private set; } // バッテリー残量

		// コンストラクタ
		public SensorInfo(string name, Vector3 Accelaration, Vector3 Gyro, float Temp, float Humidity, float Pressure, float Vbat)
		{
			this.DeviceName = name;
			this.Accelaration = Accelaration;
			this.Gyro = Gyro;
			this.Temp = Temp;
			this.Humidity = Humidity;
			this.Pressure = Pressure;
			this.Vbat = Vbat;
		}
	}
}