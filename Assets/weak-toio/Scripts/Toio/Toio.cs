using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using toio;

namespace Robot
{
	public class Toio : MonoBehaviour
	{
		[SerializeField] private int collisionThreshold;
		[SerializeField] private int _id;
		[SerializeField] private string _name;
		[SerializeField] private List<EnvType> type;
		private Cube _cube;
		private CubeHandle _handle;

		public int ID { get { return _id; } }
		public string Name { get { return _name; } }
		public List<EnvType> Type { get { return type; } }
		public Cube Cube { get { return _cube; } }
		public CubeHandle Handle { get { return _handle; } }

		[SerializeField] private int movementCount;
		[SerializeField] private int lightCount;
		[SerializeField] private int soundCount;
		[SerializeField] private int actionMaxCount;
		private Queue<Action> actions;
		private Action currentAction;
		private bool isMoving = false;

		private bool isCollisionDetected; // 衝突フラグ
		private bool isEmergencyAction = false; // 緊急アクションフラグ

		private ActionGenerator actionGenerator;
		[SerializeField] private ToioConnector toioConnector;
		private void Start()
		{
			actions = new Queue<Action>();
			currentAction = new Action();

			actionGenerator = gameObject.GetComponent<ActionGenerator>();
			actionGenerator.OnActionGenerated += AddNewAction;

			toioConnector = GameObject.FindWithTag("ToioConnector").GetComponent<ToioConnector>();
			toioConnector.OnConnectSucceeded += OnConnectSucceeded;
			if (toioConnector.connectType != ConnectType.Real)
			{
				this._name = gameObject.name;
			}
		}

		// CubeConnectorでCubeにまとめて接続してから自分のCubeなどを取得させる
		private void OnConnectSucceeded()
		{
			toioConnector.RegisterToio(this);
		}

		// Cubeのデータを持っているのがToioConnectorなので，あっち側で呼び出してもらう
		public void Register(int id, CubeManager cubeManager)
		{
			this._id = id;
			this._cube = cubeManager.cubes[_id];
			this._handle = cubeManager.handles[_id];

			if (toioConnector.connectType == ConnectType.Real)
			{
				_cube.ConfigCollisionThreshold(collisionThreshold);
			}

			OnRegisterCompleted();
		}
		private void OnRegisterCompleted()
		{
			StartCoroutine(actionGenerator.StartMove(this));
			StartCoroutine(HandleCollision());
		}

		private IEnumerator HandleCollision()
		{
			while (true)
			{
				if (_cube != null && _cube.isCollisionDetected)
				{
					// 衝突フラグの変化を1回だけ認識したい
					if (isCollisionDetected != _cube.isCollisionDetected)
					{
						var avoidanceAction = ToioActionLibrary.CollisionAvoidance();
						Debug.Log("衝突！");
						isEmergencyAction = true;
						StartCoroutine(AddEmergencyAction(avoidanceAction));
					}
					isCollisionDetected = _cube.isCollisionDetected;
				}

				yield return null;
			}
		}

		// 実行関連のメソッド
		public IEnumerator Act()
		{
			while (currentAction != null && currentAction.Count() > 0)
			{
				yield return StartCoroutine(Move());
				yield return StartCoroutine(ControllLED());
				yield return StartCoroutine(PlaySound());
			}

			yield return null;
		}

		private IEnumerator Move()
		{
			if (currentAction.MovementCount() == 0) yield return null;
			IMovementCommand move = currentAction.GetNextMovement();
			movementCount = currentAction.MovementCount();
			if (move != null)
			{
				move.Exec(this);
				yield return new WaitForSeconds(move.GetInterval());
			}
			yield return null;
		}

		private IEnumerator ControllLED()
		{
			if (currentAction == null || currentAction.LightCount() == 0)
			{
				yield break;
			}

			ILightCommand light = currentAction.GetNextLight();
			lightCount = currentAction.LightCount();

			if (light != null)
			{
				light.Exec(this);
				yield return new WaitForSeconds(light.GetInterval());
			}
		}

		private IEnumerator PlaySound()
		{
			if (currentAction == null || currentAction.SoundCount() == 0)
			{
				yield break;
			}

			ISoundCommand sound = currentAction.GetNextSound();
			soundCount = currentAction.SoundCount();

			if (sound != null)
			{
				sound.Exec(this);
				yield return new WaitForSeconds(sound.GetInterval());
			}
		}

		public void Stop()
		{
			isMoving = false;
			actions.Clear();
			currentAction = null;
			_handle.Update();
			_handle.Move(new Movement(_handle, 0, 0));
		}

		public void AddNewAction(Action action)
		{
			if (action == null)
			{
				Debug.LogWarning("null のアクション送るな");
				return;
			}

			if (actions.Count < actionMaxCount)
			{
				Debug.Log("アクション溜まりすぎ");
				return;
			}

			actions.Enqueue(action);
		}

		public IEnumerator AddEmergencyAction(Action emergencyAction)
		{
			if (emergencyAction == null) yield return null;

			// 緊急アクションをキューの先頭に追加
			actions.Enqueue(emergencyAction);
			isEmergencyAction = true;

			if (!isMoving)
			{
				isMoving = true;
				yield return Act();
			}
		}
	}
}