using Environment;
using UnityEngine;

public class DummyM5StickcWithCO2L : DummyM5Stickc, ICO2LSensor
{
	[SerializeField] protected float _temp;
	[SerializeField] protected float _hum;
	[SerializeField] protected float _ppm;
	public float GetTemperature() { return _temp; }
	public float GetHumidity() { return _hum; }
	public float GetPPM() { return _ppm; }
	void Update()
	{
		base.UpdateSensor();
		UpdateCO2L2Sensor();
		DeserializeCompleted();
	}
	private void UpdateCO2L2Sensor()
	{
		_temp += Random.Range(-1f, 1f);
		_hum += Random.Range(-1f, 1f);
		_ppm += Random.Range(-1f, 1f);
	}
}
