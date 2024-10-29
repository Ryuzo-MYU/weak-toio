using EvaluateEnvironment;
using toio;
using UnityEngine;

namespace ActionDecisionSystem
{
	public class TemperatureExpression_HotOnly
	{
		private CubeManager cm;
		private float deg;
		private float repeatRate = 0.5f;  // デフォルト値を設定
		private float elapsedTime;
		private bool isRotating;
		private float rotationStartTime;
		private float rotationDuration = 0.5f;  // 回転にかかる時間を0.5秒に短縮

		// デバッグ用
		private int previousScore = 0;
		private bool isInitialized = false;

		public async void Start(ConnectType connect, int cubeCount)
		{
			cm = new CubeManager(connect);
			await cm.MultiConnect(cubeCount);
			elapsedTime = 0f;
			isRotating = false;
			isInitialized = true;
			Debug.Log("TemperatureExpression initialized");
		}

		public void Update(Result result)
		{
			if (!isInitialized)
			{
				Debug.LogWarning("TemperatureExpression not initialized");
				return;
			}

			if (cm == null || cm.handles == null || cm.handles.Count == 0)
			{
				Debug.LogError("CubeManager not properly initialized");
				return;
			}

			// 現在の回転が完了したかチェック
			if (isRotating && Time.time - rotationStartTime > rotationDuration)
			{
				isRotating = false;
				Debug.Log("Rotation completed");
			}

			// repeatRateの時間間隔を確認
			elapsedTime += Time.deltaTime;
			if (elapsedTime < repeatRate)
			{
				return;
			}

			// 環境データの評価結果(score)に応じて、回転の度合いを変える
			int score = result.Score;

			// スコアが変化したときのみログを出力
			if (score != previousScore)
			{
				Debug.Log($"Score changed from {previousScore} to {score}");
				previousScore = score;
			}
			deg = 60;
			repeatRate = 0.333f;
			Debug.Log($"暑い (deg: {deg}, repeatRate: {repeatRate})");

			// 回転中でなければ新しい回転を開始
			if (!isRotating)
			{
				RobotExpression(deg);
				elapsedTime = 0f;
			}
		}

		private void RobotExpression(float deg)
		{
			if (cm == null || cm.handles == null)
			{
				Debug.LogError("CubeManager not initialized in RobotExpression");
				return;
			}

			foreach (var handle in cm.handles)
			{
				if (handle == null)
				{
					Debug.LogWarning("Null handle found");
					continue;
				}

				handle.Update();

				Movement rotate = handle.Rotate2Deg(deg);
				handle.Move(rotate);
				// 逆回転
				rotate = handle.Rotate2Deg(-deg);
				handle.Move(rotate);

				Debug.Log($"Rotating cube by {deg} degrees");
			}

			isRotating = true;
			rotationStartTime = Time.time;
			Debug.Log("RobotExpression executed");
		}
	}
}