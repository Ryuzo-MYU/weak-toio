using ActionLibrary;
using Evaluation;
using UnityEngine;

namespace ActionGenerate
{
	/// <summary>
	/// toioのアクションを生成する抽象クラス
	/// 各環境の具象クラス用に基本のMotion生成メソッドを提供する
	/// </summary>
	public abstract class ActionGenerator : MonoBehaviour
	{
		public event System.Action<Action> OnActionGenerated;
		private Result currentResult;
		protected ToioActionLibrary actionLibrary;
		private void Start()
		{
			EvaluateBase evaluate = gameObject.GetComponent<EvaluateBase>();
			evaluate._OnResultGenerated += OnResultGenerated;
			currentResult = new Result();

		}

		protected void OnResultGenerated(Result result)
		{
			currentResult = result;
		}
		public Action GetNewAction()
		{
			return GenerateAction(currentResult);
		}
		public void InitializeActionLibrary()
		{
			actionLibrary = new ToioActionLibrary();
		}
		protected abstract Action GenerateAction(Result result);
	}
}