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

		public EnvType GetEnvType()
		{
			return _envType;
		}

		private void Awake()
		{
			sensorManager = gameObject.GetComponent<SensorManager>();
			sensorManager.OnSensorDecided += OnSensorDecided;
		}

		protected virtual void OnSensorDecided() { }
		protected virtual void OnDeserializeCompleted()
		{
			SensorBase sensor = this.gameObject.GetComponent<SensorBase>();
			sensor.OnDeserializeCompleted += OnDeserializeCompleted;
		}
		protected void OnResultGenerated(Result result)
		{
			_onResultGenerated.Invoke(result);
		}
	}
}