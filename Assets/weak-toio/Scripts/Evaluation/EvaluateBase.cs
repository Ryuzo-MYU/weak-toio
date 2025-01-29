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
		[SerializeField] private Unit _unit;
		public Unit Unit { get { return _unit; } }
		protected SensorManager sensorManager;
		protected SensorBase sensor;

		private void Awake()
		{
			sensorManager = gameObject.GetComponent<SensorManager>();
			sensorManager.OnSensorDecided += OnSensorDecided;
		}

		protected virtual void OnSensorDecided()
		{
			sensor = sensorManager.GetSensor();
			sensor.OnDeserializeCompleted += OnDeserializeCompleted;
			Debug.Log(sensor.GetType());
		}
		protected abstract void OnDeserializeCompleted();
		protected abstract void GenerateEvaluationResult();
		protected void _OnResultGenerated(Result result)
		{
			OnResultGenerated?.Invoke(result);
		}
	}
}