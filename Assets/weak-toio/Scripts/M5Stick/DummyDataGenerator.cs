using UnityEngine;

public class DummyDataGenerator : M5DataReceiver
{
	public string DeviceName { get; private set; }
	// 前回の値を保持するフィールド
	public Vector3 PreviousAccel { get; private set; }
	public Vector3 PreviousGyro { get; private set; }
	public float PreviousTemperature{get; private set;}
	public float PreviousHumidity{get; private set;}
	public float PreviousPressure{get; private set;}
	public float PreviousBatteryVoltage{get; private set;}

	public DummyDataGenerator()
	{
		DeviceName = "This-is-Dummy";
		PreviousAccel = Vector3.zero;
		PreviousGyro = Vector3.zero;
		PreviousTemperature = 25f;
		PreviousHumidity = 50f;
		PreviousPressure = 101325f; // https://w.wiki/3DDH
		PreviousBatteryVoltage = 4.0f;
	}

	public void Update()
	{
		// 加速度とジャイロのダミーデータ（前回の値に基づいて変動）
		float accelX = PreviousAccel.x + Random.Range(-0.5f, 0.5f);
		float accelY = PreviousAccel.y + Random.Range(-0.5f, 0.5f);
		float accelZ = PreviousAccel.z + Random.Range(-0.5f, 0.5f);
		float gyroX = PreviousGyro.x + Random.Range(-10f, 10f);
		float gyroY = PreviousGyro.y + Random.Range(-10f, 10f);
		float gyroZ = PreviousGyro.z + Random.Range(-10f, 10f);

		// 気温、湿度、気圧、バッテリー電圧のダミーデータ（前回の値に基づいて変動）
		float temperature = PreviousTemperature + Random.Range(-0.1f, 0.1f);
		float humidity = PreviousHumidity + Random.Range(-1f, 1f);
		float pressure = PreviousPressure + Random.Range(-1f, 1f);
		float batteryVoltage = PreviousBatteryVoltage + Random.Range(-0.01f, 0.01f);

		// 新しい値をフィールドに保存
		PreviousAccel = new Vector3(accelX, accelY, accelZ);
		PreviousGyro = new Vector3(gyroX, gyroY, gyroZ);
		PreviousTemperature = temperature;
		PreviousHumidity = humidity;
		PreviousPressure = pressure;
		PreviousBatteryVoltage = batteryVoltage;

		sensorInfo = new SensorInfo(
			DeviceName,
			new Vector3(accelX, accelY, accelZ),
			new Vector3(gyroX, gyroY, gyroZ),
			temperature,
			humidity,
			pressure,
			batteryVoltage
		);
	}
}
