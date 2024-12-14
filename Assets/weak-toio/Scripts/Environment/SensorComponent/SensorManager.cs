using System;
using Environment;
using UnityEngine;

public class SensorManager : MonoBehaviour
{
	public event Action OnSensorDecided;
	[SerializeField] private SerialHandler _serial;
	[SerializeField] private SensorBase _real;
	[SerializeField] private SensorBase _dummy;
	public SensorBase _remainedSensor;

	private void Awake()
	{
		_real = gameObject.GetComponent<SensorBase>();
		_dummy = gameObject.GetComponent<SensorBase>();

		_serial = gameObject.GetComponent<SerialHandler>();
		_serial.OnConnectSucceeded += OnConnectSucceeded;
		_serial.OnConnectFailed += OnConnectFailed;
	}
	private void OnConnectSucceeded()
	{
		Destroy(_dummy);
		_remainedSensor = _real;
		OnSensorDecided.Invoke();
	}
	protected void OnConnectFailed()
	{
		Destroy(_real);
		_remainedSensor = _dummy;
		OnSensorDecided.Invoke();
	}
	private void Start()
	{
	}
}