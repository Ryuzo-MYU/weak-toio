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
		Debug.Log($"Device: {dummy.sensorInfo.deviceName}\n" +
				  $"Acceleration: {dummy.sensorInfo.acceleration}\n" +
				  $"Gyro: {dummy.sensorInfo.gyro}\n" +
				  $"Temperature: {dummy.sensorInfo.temp}°C\n" +
				  $"Humidity: {dummy.sensorInfo.humidity}%\n" +
				  $"Pressure: {dummy.sensorInfo.pressure}Pa\n" +
				  $"Battery Voltage: {dummy.sensorInfo.vbat}V\n");

	}
}