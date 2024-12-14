using System;
using Environment;
using UnityEngine;

public class SensorManager : MonoBehaviour
{
	public event Action OnSensorDecided;
	[SerializeField] private SerialHandler _serial;
	[SerializeField] private Component _real;
	[SerializeField] private Component _dummy;
	public SensorBase _remainedSensor;

	private void Awake()
	{
		_real = (Component)gameObject.GetComponent<RealSensor>();
		_dummy = (Component)gameObject.GetComponent<DummySensor>();
		_serial.OnConnectSucceeded += OnConnectSucceeded;
		_serial.OnConnectFailed += OnConnectFailed;
	}
	private void OnConnectSucceeded()
	{
		Destroy(_dummy);
		OnSensorDecided.Invoke();
	}
	protected void OnConnectFailed()
	{
		Destroy(_real);
		OnSensorDecided.Invoke();
	}
	private void Start()
	{
	}
}