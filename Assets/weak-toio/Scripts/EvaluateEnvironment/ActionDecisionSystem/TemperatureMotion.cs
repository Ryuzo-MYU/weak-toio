using EvaluateEnvironment;
using toio;
using UnityEngine;

namespace ActionDecisionSystem
{
	public class TemperatureActionGenerator : ActionGenerator
	{

	}
	public class TemperatureAction : ActionDecisionSystem.Action
	{
		protected CubeManager cm;
		protected float elapsedTime;
		protected bool isMoving;
		protected MovementState currentState;
		protected MovementState targetState;  // 遷移先の状態
		protected bool isTransitioning;       // 状態遷移中かどうか
		protected float transitionTimer;      // 遷移用タイマー
		protected const float PAUSE_DURATION = 1.0f;  // 一時停止の時間（秒）

		// 現在の動作パラメータを保持
		protected MovementParams currentParams;

		// 各状態での動作パラメータ
		protected struct MovementParams
		{
			public float minDeg;
			public float maxDeg;
			public int minSpeed;
			public int maxSpeed;
			public float interval;  // 動作の更新間隔（秒）
		}

		protected MovementParams coldParams = new MovementParams
		{
			minDeg = -20f,
			maxDeg = 20f,
			minSpeed = 30,
			maxSpeed = 50,
			interval = 0.3f
		};

		protected MovementParams suitableParams = new MovementParams
		{
			minDeg = -180f,
			maxDeg = 180f,
			minSpeed = 40,
			maxSpeed = 60,
			interval = 2.0f
		};

		protected MovementParams hotParams = new MovementParams
		{
			minDeg = -360f,
			maxDeg = 360f,
			minSpeed = 150,
			maxSpeed = 230,
			interval = 0.5f
		};

		public async void Start(ConnectType connect, int cubeCount)
		{
			cm = new CubeManager(connect);
			await cm.MultiConnect(cubeCount);
			elapsedTime = 0f;
			isMoving = false;
			isTransitioning = false;
			transitionTimer = 0f;
			currentState = MovementState.Suitable;
			targetState = MovementState.Suitable;
			currentParams = suitableParams;
		}

		public void Update(int score)
		{
			if (cm.handles == null) return;

			MovementState newState = DetermineState(score);

			// 状態変更の検出と遷移開始
			if (newState != currentState && !isTransitioning)
			{
				StartTransition(newState);
				return;
			}

			// 遷移中の処理
			if (isTransitioning)
			{
				HandleTransition();
				return;
			}

			// 通常の動作更新
			elapsedTime += Time.deltaTime;
			if (elapsedTime >= currentParams.interval)
			{
				ExecuteMovement();
				elapsedTime = 0f;
			}
		}

		protected void StartTransition(MovementState newState)
		{
			isTransitioning = true;
			targetState = newState;
			transitionTimer = 0f;

			// 現在の動きを停止
			StopAllMovement();

			Debug.Log($"状態遷移開始: {currentState} → {targetState}");
		}

		protected void HandleTransition()
		{
			transitionTimer += Time.deltaTime;

			if (transitionTimer >= PAUSE_DURATION)
			{
				// 遷移完了
				currentState = targetState;
				currentParams = GetParamsForState(currentState);
				isTransitioning = false;
				elapsedTime = currentParams.interval; // 即座に新しい動きを開始

				Debug.Log($"状態遷移完了: {currentState}");
			}
		}

		protected void StopAllMovement()
		{
			if (cm.handles == null) return;

			foreach (var handle in cm.handles)
			{
				// モーターを停止
				handle.Stop();
			}
		}

		protected MovementState DetermineState(int score)
		{
			if (score < 0) return MovementState.Cold;
			if (score > 0) return MovementState.Hot;
			return MovementState.Suitable;
		}

		protected MovementParams GetParamsForState(MovementState state)
		{
			switch (state)
			{
				case MovementState.Cold:
					return coldParams;
				case MovementState.Hot:
					return hotParams;
				default:
					return suitableParams;
			}
		}

		protected void ExecuteMovement()
		{
			if (isTransitioning) return;

			switch (currentState)
			{
				case MovementState.Cold:
					ShiverMovement();
					break;
				case MovementState.Suitable:
					SmoothRotation();
					break;
				case MovementState.Hot:
					IntenseRotation();
					break;
			}
		}

		protected void ShiverMovement()
		{
			foreach (var handle in cm.handles)
			{
				float randomDeg = Random.Range(currentParams.minDeg, currentParams.maxDeg);
				int randomSpeed = Random.Range(currentParams.minSpeed, currentParams.maxSpeed);

				Movement movement = handle.RotateByDeg(randomDeg, randomSpeed);
				handle.Move(movement, false);
			}
		}

		protected void SmoothRotation()
		{
			foreach (var handle in cm.handles)
			{
				float deg = Random.value > 0.5f ? currentParams.maxDeg : -currentParams.maxDeg;
				int speed = Random.Range(currentParams.minSpeed, currentParams.maxSpeed);

				Movement movement = handle.RotateByDeg(deg, speed);
				handle.Move(movement, false);
			}
		}

		protected void IntenseRotation()
		{
			foreach (var handle in cm.handles)
			{
				float randomDeg = Random.Range(currentParams.minDeg, currentParams.maxDeg);
				int speed = currentParams.maxSpeed;

				Movement movement = handle.RotateByDeg(randomDeg, speed);
				handle.Move(movement, false);
			}
		}

		protected enum MovementState
		{
			Cold,
			Suitable,
			Hot
		}
	}
}