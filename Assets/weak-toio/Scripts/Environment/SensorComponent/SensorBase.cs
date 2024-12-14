using System;
using UnityEngine;

public class SensorBase : MonoBehaviour
{
	[SerializeField] protected SerialHandler _serial;
	private void Awake()
	{
		_serial = gameObject.GetComponent<SerialHandler>();
		_serial.OnDataReceived += OnDataReceived;
		_serial.OnConnectFailed += OnConnectFailed;
		_serial.OnConnectSucceeded += OnConnectSucceeded;
	}
	protected virtual void OnDataReceived(string message) { }
	protected virtual void OnConnectSucceeded() { }
	protected virtual void OnConnectFailed() { }

	public event Action OnDeserializeCompleted;
	protected void _OnDeserializeCompleted()
	{
		OnDeserializeCompleted?.Invoke();
	}
	public event Action OnSensorInitialized;
	protected void _OnSensorInitialized(){
		OnSensorInitialized?.Invoke();
	}
}
