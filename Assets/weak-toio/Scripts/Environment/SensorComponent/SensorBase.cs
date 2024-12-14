using System;
using System.Collections;
using UnityEngine;

public class SensorBase : MonoBehaviour
{
	[SerializeField] protected SerialHandler _serial;

	public event Action OnDeserializeCompleted;
	protected void _OnDeserializeCompleted()
	{
		OnDeserializeCompleted?.Invoke();
	}
}
