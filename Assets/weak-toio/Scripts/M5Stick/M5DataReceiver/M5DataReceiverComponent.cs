using UnityEngine;
public class M5DataReceiverComponent : MonoBehaviour
{
	[SerializeField] M5DataReceiver sensor;
	public string deviceName;
	public Vector3 accelaration;
	public Vector3 gyro;
	public float temp;// M5StickCの内部温度
	public float humidity;// ENV2の湿度
	public float pressure;// ENV2の気圧
	public float vbat;// バッテリー残量
	void Start()
	{
		sensor.serialHandler = gameObject.GetComponent<SerialHandler>();
		sensor = new M5DataReceiver();
		sensor.Start();
	}
	void Update()
	{
		deviceName = sensor.sensorInfo.DeviceName;
		accelaration = sensor.sensorInfo.Accelaration;
		gyro = sensor.sensorInfo.Gyro;
		temp = sensor.sensorInfo.Temp;
		humidity = sensor.sensorInfo.Humidity;
		pressure = sensor.sensorInfo.Pressure;
		vbat = sensor.sensorInfo.Vbat;
	}
}