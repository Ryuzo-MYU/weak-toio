using System;
using UnityEngine;

public class SensorManager : MonoBehaviour
{
	public event Action OnSensorDecided;
	[SerializeField] private SensorBase[] sensors;
	public SensorBase remainedSensor;
	private void Start()
	{
		sensors = gameObject.GetComponents<SensorBase>();
		foreach (var sensor in sensors)
		{
			sensor.OnSensorInitialized += OnSensorInitialized;
		}
	}
	private void OnSensorInitialized()
	{
		remainedSensor = this.gameObject.GetComponent<SensorBase>();
		OnSensorDecided.Invoke();
	}
}
