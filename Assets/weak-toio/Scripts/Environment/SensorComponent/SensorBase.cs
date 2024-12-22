using System;
using System.Collections;
using UnityEngine;

public class SensorBase : MonoBehaviour
{
	protected SerialHandler _serial;

	public event Action OnDeserializeCompleted;

	private void Awake()
	{
		_serial = gameObject.GetComponent<SerialHandler>();
	}
	protected void _OnDeserializeCompleted()
	{
		OnDeserializeCompleted?.Invoke();
	}
}
