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

		_serial = gameObject.GetComponent<SerialHandler>();
		_serial.OnConnectSucceeded += OnConnectSucceeded;
		_serial.OnConnectFailed += OnConnectFailed;
	}
	private void OnConnectSucceeded()
	{
		_dummy.enabled = false;
		_remainedSensor = (SensorBase)_real;
		OnSensorDecided?.Invoke();
	}
	protected void OnConnectFailed()
	{
		Destroy(_real);
		_remainedSensor = (SensorBase)_dummy;
		OnSensorDecided?.Invoke();
	}
	public SensorBase GetSensor()
	{
		return _remainedSensor;
	}
}