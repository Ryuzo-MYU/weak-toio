using System.Collections;
using UnityEngine;

namespace Environment
{
	[RequireComponent(typeof(SerialHandler))]
	public class DummyM5Stickc : SensorBase, IM5Sensor, DummySensor
	{
		[SerializeField] protected string _deviceName;
		[SerializeField] protected Vector3 _accel;
		[SerializeField] protected Vector3 _gyro;
		[SerializeField] protected float _vbat;
		[SerializeField] protected float updateInterval = 5;
		public string GetDeviceName() { return _deviceName; }
		public Vector3 GetAcceleration() { return _accel; }
		public Vector3 GetGyro() { return _gyro; }
		public float GetVbat() { return _vbat; }

		/// <summary>
		/// Start is called on the frame when a script is enabled just before
		/// any of the Update methods is called the first time.
		/// </summary>
		void Start()
		{
			StartCoroutine(UpdateCoroutine());
		}
		IEnumerator UpdateCoroutine()
		{
			while (true)
			{
				UpdateSensor();
				_OnDeserializeCompleted();
				yield return new WaitForSeconds(updateInterval);
			}
		}
		protected void StartSensor() { }
		protected void UpdateSensor()
		{
			// 加速度とジャイロのダミーデータ（前回の値に基づいて変動）
			float accelX = _accel.x + Random.Range(-2f, 2f);
			float accelY = _accel.y + Random.Range(-2f, 2f);
			float accelZ = _accel.z + Random.Range(-2f, 2f);
			_accel = new Vector3(accelX, accelY, accelZ);

			float gyroX = _gyro.x + Random.Range(-50f, 50f);
			float gyroY = _gyro.y + Random.Range(-50f, 50f);
			float gyroZ = _gyro.z + Random.Range(-50f, 50f);
			_gyro = new Vector3(gyroX, gyroY, gyroZ);

			_vbat += Random.Range(-0.01f, 0.01f);
		}
	}
}