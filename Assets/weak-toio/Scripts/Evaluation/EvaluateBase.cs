using System;
using UnityEngine;

namespace Evaluation
{
	public class EvaluateBase : MonoBehaviour
	{
		public event Action<Result> _onResultGenerated;
		[SerializeField] protected float _currentParam;
		[SerializeField] protected Unit _unit;
		[SerializeField] protected EnvType _envType;
		[SerializeField] protected float _score;
		protected SensorManager sensorManager;
		private SensorBase sensor;

		public EnvType GetEnvType()
		{
			return _envType;
		}

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
		protected virtual void OnDeserializeCompleted() { }
		public void OnResultGenerated(Result result)
		{
			_onResultGenerated.Invoke(result);
		}
	}
}