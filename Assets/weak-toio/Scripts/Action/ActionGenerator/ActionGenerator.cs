using System.Collections;
using Evaluation;
using toio;
using UnityEngine;

namespace Robot
{
	/// <summary>
	/// toioのアクションを生成する抽象クラス
	/// 各環境の具象クラス用に基本のMotion生成メソッドを提供する
	/// </summary>
	public abstract class ActionGenerator : MonoBehaviour
	{
		public event System.Action<Action> OnActionGenerated;
		private Result currentResult;
		private void Start()
		{
			EvaluateBase evaluate = gameObject.GetComponent<EvaluateBase>();
			evaluate.OnResultGenerated += OnResultGenerated;
			currentResult = new Result();
		}

		protected void OnResultGenerated(Result result)
		{
			currentResult = result;
		}

		public IEnumerator AddNewAction(Toio toio)
		{
			while (true)
			{
				Action action = GenerateAction(currentResult);
				toio.AddNewAction(action);
				yield return toio.Act();
			}
		}

		protected abstract Action GenerateAction(Result result);
	}
}