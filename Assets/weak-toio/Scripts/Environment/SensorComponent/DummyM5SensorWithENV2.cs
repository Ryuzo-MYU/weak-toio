using Environment;
using UnityEngine;

public class DummyM5SensorWithENV2 : DummyM5Sensor, IENV2Sensor
{
	[SerializeField] protected float temp;
	[SerializeField] protected float hum;
	[SerializeField] protected float pressure;
	public float GetTemperature() { return temp; }
	public float GetHumidity() { return hum; }
	public float GetPressure() { return pressure; }
	void Start()
	{

	}

	void Update()
	{
		base.UpdateSensor();
		UpdateENV2Sensor();
	}
	private void UpdateENV2Sensor()
	{
		temp += Random.Range(-1f, 1f);
		hum += Random.Range(-1f, 1f);
		pressure += Random.Range(-1f, 1f);
	}
}
