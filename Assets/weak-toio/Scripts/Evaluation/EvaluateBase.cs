using System;
using UnityEngine;

namespace Evaluation
{
	public class EvaluateBase : MonoBehaviour
	{
		public event Action OnResultGenerated;
		[SerializeField] protected float _currentParam;
		[SerializeField] protected Unit _unit;
		[SerializeField] protected EnvType _envType;
		[SerializeField] protected float _score;
		public EnvType GetEnvType()
		{
			return _envType;
		}
		protected void Start()
		{
			SensorBase sensor = this.gameObject.GetComponent<SensorBase>();
			sensor.OnDeserializeCompleted += OnDeserializeCompleted;
		}

		protected virtual void OnDeserializeCompleted() { }
	}
}