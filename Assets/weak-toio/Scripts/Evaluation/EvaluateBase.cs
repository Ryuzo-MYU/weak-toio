using System;
using UnityEngine;

namespace Evaluation
{
	public abstract class EvaluateBase : MonoBehaviour
	{
		public event Action<Result> OnResultGenerated;
		[SerializeField] protected float _score;
		[SerializeField] protected float _currentParam;
		public float CurrentParam { get { return _currentParam; } }
		[SerializeField] protected Unit _unit;
		protected SensorManager sensorManager;
		private SensorBase sensor;

		private void Awake()
		{
			sensorManager = gameObject.GetComponent<SensorManager>();
			sensorManager.OnSensorDecided += OnSensorDecided;
		}

		protected virtual void OnSensorDecided()
		{
			sensor = sensorManager.GetSensor();
			sensor.OnDeserializeCompleted += OnDeserializeCompleted;
		}
		protected abstract void OnDeserializeCompleted();
		protected abstract void GenerateEvaluationResult();
		protected void _OnResultGenerated(Result result)
		{
			OnResultGenerated?.Invoke(result);
		}
	}
}