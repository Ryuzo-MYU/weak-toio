using UnityEngine;

namespace Evaluation
{
	public class EvaluateBase : MonoBehaviour
	{
		[SerializeField] protected float _currentParam;
		[SerializeField] protected Unit _unit;
		[SerializeField] protected EnvType _envType;
		[SerializeField] protected float _score;
		public EnvType GetEnvType()
		{
			return _envType;
		}
	}
}