using Environment;
using UnityEngine;

public class DummyM5StickcWithENV2 : DummyM5Stickc, IENV2Sensor
{
	[SerializeField] protected float _temp;
	[SerializeField] protected float _hum;
	[SerializeField] protected float _pressure;
	public float GetTemperature() { return _temp; }
	public float GetHumidity() { return _hum; }
	public float GetPressure() { return _pressure; }

	void Update()
	{
		base.UpdateSensor();
		UpdateENV2Sensor();
		OnDeserializeCompleted.Invoke();
	}
	private void UpdateENV2Sensor()
	{
		_temp += Random.Range(-1f, 1f);
		_hum += Random.Range(-1f, 1f);
		_pressure += Random.Range(-1f, 1f);
	}
}
