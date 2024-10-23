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
		Debug.Log($"Device: {dummy.sensorInfo.DeviceName}\n" +
				  $"Acceleration: {dummy.sensorInfo.Accelaration}\n" +
				  $"Gyro: {dummy.sensorInfo.Gyro}\n" +
				  $"Temperature: {dummy.sensorInfo.Temp}°C\n" +
				  $"Humidity: {dummy.sensorInfo.Humidity}%\n" +
				  $"Pressure: {dummy.sensorInfo.Pressure}Pa\n" +
				  $"Battery Voltage: {dummy.sensorInfo.Vbat}V\n");

	}
}