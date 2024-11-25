using System;
using System.Collections;
using Environment;
using Evaluation;
using Robot;
using toio;
using UnityEngine;

public class Main_Temperature_Single : MonoBehaviour
{
	[Tooltip("UnityEditor上ならSimmurator、現実ならReal、お任せならAuto")]
	 public ConnectType connectType = ConnectType.Auto;

	[Tooltip("接続したいtoioの数")] public int cubeCount = 0;
	[Tooltip("Mainをぶち込め")][SerializeField] SensorUnit sensor;
	public bool UseDummy;
	[SerializeField] TempBoundary tempBoundary;
	EvaluationResultSender tempEval;
	ActionSender tempAction;
	IToioMovement toio;
	CubeManager cubeManager;
	private bool connected = false;

	private async void Start()
	{
		// ダミーセンサーを使う場合
		if (UseDummy) sensor = new DummySensor();

		// 評価システムの初期化
		tempEval = new TemperatureEvaluate(tempBoundary.UpperBound, tempBoundary.LowerBound);

		try
		{
			// toioに接続
			cubeManager = new CubeManager(connectType);
			await cubeManager.SingleConnect();
			connected = true;
			Debug.Log("接続完了");

			int id = 0;
			toio = new Toio(id, cubeManager);

			tempAction = new TemperatureActionGenerator(toio);

			toio.StartMove(this);
		}
		catch (Exception e)
		{
			Debug.LogError($"エラー: {e.Message}\n" +
				  $"場所: {e.StackTrace}\n" +
				  $"メソッド名: {System.Reflection.MethodBase.GetCurrentMethod().Name}\n" +
				  $"行番号: {new System.Diagnostics.StackTrace(e, true).GetFrame(0).GetFileLineNumber()}");
		}

		StartCoroutine(UpdateEvaluate());
	}
	private void Update()
	{
	}

	IEnumerator UpdateEvaluate()
	{
		while (true)
		{
			if (connected && sensor != null)
			{
				sensor.Update();
				Result result = tempEval.GetEvaluationResult(sensor);
				Robot.Action action = tempAction.GenerateAction(result);

				if (!toio.AddNewAction(action))
				{
					Debug.LogWarning("アクション追加失敗");
				}
				yield return new WaitForSeconds(1.0f);
			}
			else
			{
				yield return new WaitForSeconds(1.0f);
			}
		}
	}

	[Serializable]
	struct TempBoundary
	{
		public float UpperBound;
		public float LowerBound;
	}
}
