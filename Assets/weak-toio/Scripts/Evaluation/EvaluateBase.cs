using UnityEngine;
using UnityEngine.Events;

namespace Evaluation
{
	public class EvaluateBase : MonoBehaviour
	{
		public UnityEvent<Result> OnResultGenerated;
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
		}
		protected virtual void OnDeserializeCompleted() { }
	}
}