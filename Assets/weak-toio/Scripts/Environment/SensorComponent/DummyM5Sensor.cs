using Environment;
using UnityEngine;

public class DummyM5Sensor : MonoBehaviour, IM5Sensor
{
	[SerializeField] protected string deviceName;
	[SerializeField] protected Vector3 accel;
	[SerializeField] protected Vector3 gyro;
	[SerializeField] protected float vbat;
	public string GetDeviceName() { return deviceName; }
	public Vector3 GetAcceleration() { return accel; }
	public Vector3 GetGyro() { return gyro; }
	public float GetVbat() { return vbat; }
	void Start() { }
	void Update() {
		UpdateSensor();
	 }
	public void StartSensor() { }
	public void UpdateSensor()
	{
		// 加速度とジャイロのダミーデータ（前回の値に基づいて変動）
		float accelX = accel.x + Random.Range(-2f, 2f);
		float accelY = accel.y + Random.Range(-2f, 2f);
		float accelZ = accel.z + Random.Range(-2f, 2f);
		accel = new Vector3(accelX, accelY, accelZ);

		float gyroX = gyro.x + Random.Range(-50f, 50f);
		float gyroY = gyro.y + Random.Range(-50f, 50f);
		float gyroZ = gyro.z + Random.Range(-50f, 50f);
		gyro = new Vector3(gyroX, gyroY, gyroZ);

		vbat += Random.Range(-0.01f, 0.01f);
	}
}
