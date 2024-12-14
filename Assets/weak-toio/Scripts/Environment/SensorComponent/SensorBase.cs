using System;
using UnityEngine;

public class SensorBase : MonoBehaviour
{
	public event Action OnDeserializeCompleted;
	protected void DeserializeCompleted()
	{
		OnDeserializeCompleted?.Invoke();
	}
}
