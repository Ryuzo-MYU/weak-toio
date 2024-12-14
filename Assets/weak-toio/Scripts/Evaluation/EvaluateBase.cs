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
		protected SensorBase sensor;
		public EnvType GetEnvType()
		{
			return _envType;
		}
		private void Start()
		{
			Debug.Log("EvaluateBase Start開始");
			sensor = this.gameObject.GetComponent<SensorBase>();
		}

		protected virtual void OnSensorInitialized() { }
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