using UnityEngine;

public class DummyDataGenerator : M5DataReceiver
{
	private string deviceName;
	// 前回の値を保持するフィールド
	private Vector3 previousAccel;
	private Vector3 previousGyro;
	private float previousTemperature;
	private float previousHumidity;
	private float previousPressure;
	private float previousBatteryVoltage;

	public DummyDataGenerator()
	{
		deviceName = "This-is-Dummy";
		previousAccel = Vector3.zero;
		previousGyro = Vector3.zero;
		previousTemperature = 25f;
		previousHumidity = 50f;
		previousPressure = 101325f; // https://w.wiki/3DDH
		previousBatteryVoltage = 4.0f;
	}

	public void SendDummyData()
	{
		// デバイス名を定義
		string deviceName = this.deviceName;

		// 加速度とジャイロのダミーデータ（前回の値に基づいて変動）
		float accelX = previousAccel.x + Random.Range(-0.5f, 0.5f);
		float accelY = previousAccel.y + Random.Range(-0.5f, 0.5f);
		float accelZ = previousAccel.z + Random.Range(-0.5f, 0.5f);
		float gyroX = previousGyro.x + Random.Range(-10f, 10f);
		float gyroY = previousGyro.y + Random.Range(-10f, 10f);
		float gyroZ = previousGyro.z + Random.Range(-10f, 10f);

		// 気温、湿度、気圧、バッテリー電圧のダミーデータ（前回の値に基づいて変動）
		float temperature = previousTemperature + Random.Range(-0.1f, 0.1f);
		float humidity = previousHumidity + Random.Range(-1f, 1f);
		float pressure = previousPressure + Random.Range(-1f, 1f);
		float batteryVoltage = previousBatteryVoltage + Random.Range(-0.01f, 0.01f);

		// 新しい値をフィールドに保存
		previousAccel = new Vector3(accelX, accelY, accelZ);
		previousGyro = new Vector3(gyroX, gyroY, gyroZ);
		previousTemperature = temperature;
		previousHumidity = humidity;
		previousPressure = pressure;
		previousBatteryVoltage = batteryVoltage;

		sensorInfo = new SensorInfo(
			deviceName,
			new Vector3(accelX, accelY, accelZ),
			new Vector3(gyroX, gyroY, gyroZ),
			temperature,
			humidity,
			pressure,
			batteryVoltage
		);
	}
}
