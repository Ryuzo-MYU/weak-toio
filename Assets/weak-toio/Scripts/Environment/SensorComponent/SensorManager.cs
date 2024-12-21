using System;
using Environment;
using UnityEngine;

public class SensorManager : MonoBehaviour
{
	public event Action OnSensorDecided;
	[SerializeField] private SerialHandler _serial;
	[SerializeField] private SensorBase _real;
	[SerializeField] private SensorBase _dummy;
	private SensorBase _remainedSensor;


	private void Awake()
	{
		_real = (SensorBase)gameObject.GetComponent<RealSensor>();
		_dummy = (SensorBase)gameObject.GetComponent<DummySensor>();
		_real.enabled = false;
		_dummy.enabled = false;

		_serial = gameObject.GetComponent<SerialHandler>();
		_serial.OnConnectSucceeded += OnConnectSucceeded;
		_serial.OnConnectFailed += OnConnectFailed;
	}
	private void OnConnectSucceeded()
	{
		_real.enabled = true;
		_remainedSensor = (SensorBase)_real;
		OnSensorDecided?.Invoke();
	}
	protected void OnConnectFailed()
	{
		_dummy.enabled = true;
		_remainedSensor = (SensorBase)_dummy;
		OnSensorDecided?.Invoke();
	}
	public SensorBase GetSensor()
	{
		return _remainedSensor;
	}
}