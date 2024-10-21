using UnityEngine;
public class DummyDataGeneratorComponent : MonoBehaviour
{
	private float timer = 0f;
	// データ送信の間隔（秒）
	public float sendInterval = 1.0f;
	public DummyDataGenerator dummy;
	void Start()
	{
		dummy = new DummyDataGenerator();
	}

	void Update()
	{
		timer += Time.deltaTime;

		// 一定間隔でダミーデータを生成
		if (timer >= sendInterval)
		{
			UpdateDummyData();
			timer = 0f;  // タイマーリセット
		}
	}
	void UpdateDummyData()
	{
		dummy.Update();
		ExportLog();
	}
	void ExportLog()
	{
		Debug.Log($"Device: {dummy.sensorInfo.deviceName}");
		Debug.Log($"Acceleration: {dummy.sensorInfo.accelaration}");
		Debug.Log($"Gyro: {dummy.sensorInfo.gyro}");
		Debug.Log($"Temperature: {dummy.sensorInfo.temp}°C");
		Debug.Log($"Humidity: {dummy.sensorInfo.humidity}%");
		Debug.Log($"Pressure: {dummy.sensorInfo.pressure}Pa");
		Debug.Log($"Battery Voltage: {dummy.sensorInfo.vbat}V\n");
	}
}