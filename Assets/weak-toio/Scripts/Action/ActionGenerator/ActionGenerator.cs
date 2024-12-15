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

		public void ProcessAction(Toio toio)
		{
			Action action = GenerateAction(currentResult);
			toio.AddNewAction(action);
			StartCoroutine(toio.Move());
		}

		protected abstract Action GenerateAction(Result result);
	}
}