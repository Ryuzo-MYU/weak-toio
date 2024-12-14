using System;
using Environment;
using UnityEngine;

public class SensorManager : MonoBehaviour
{
	public event Action OnSensorDecided;
	[SerializeField] private Component real;
	[SerializeField] private Component dummy;
	public SensorBase remainedSensor;
	private void OnConnectSucceeded()
	{
		Destroy(dummy);
		OnSensorDecided.Invoke();
	}
	protected void OnConnectFailed()
	{
		Destroy(real);
		OnSensorDecided.Invoke();
	}
	private void Start()
	{
	}
	private void OnSensorInitialized()
	{
		remainedSensor = this.gameObject.GetComponent<SensorBase>();
		OnSensorDecided.Invoke();
	}
}