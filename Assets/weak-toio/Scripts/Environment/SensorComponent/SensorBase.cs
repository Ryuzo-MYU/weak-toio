using System;
using UnityEngine;

public class SensorBase : MonoBehaviour
{
	[SerializeField] protected SerialHandler _serial;
	public event Action OnDeserializeCompleted;
	private void Start()
	{
		_serial = gameObject.GetComponent<SerialHandler>();
		_serial.OnDataReceived += OnDataReceived;
		_serial.OnConnectFailed += OnConnectFailed;
		_serial.OnConnectSucceeded += OnConnectSucceeded;
	}
	protected virtual void OnDataReceived(string message) { }
	protected virtual void OnConnectSucceeded() { }
	protected virtual void OnConnectFailed() { }
	protected void DeserializeCompleted()
	{
		OnDeserializeCompleted?.Invoke();
	}
}
