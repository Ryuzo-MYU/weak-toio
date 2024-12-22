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

			_cube.ConfigCollisionThreshold(collisionThreshold);

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
						AddEmergencyAction(avoidanceAction);
					}
					isCollisionDetected = _cube.isCollisionDetected;
				}

				yield return null;
			}
		}

		// 実行関連のメソッド
		public IEnumerator Act()
		{
			if (currentAction == null || currentAction.Count() == 0)
			{
				if (actions.Count > 0)
				{
					currentAction = actions.Dequeue();
					Debug.Log("アクション無いんで入れ替えますね");
				}
				else
				{
					yield return null;
				}
			}

			while (currentAction.Count() > 0)
			{
				yield return StartCoroutine(Move());
				yield return StartCoroutine(ControllLED());
				yield return StartCoroutine(PlaySound());
			}
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
				actions.Enqueue(action);
				if (!isMoving)
				{
					isMoving = true;
					StartCoroutine(Act());
				}
			}
		}

		public void AddEmergencyAction(Action emergencyAction)
		{
			if (emergencyAction == null) return;

			// 現在のアクションキューを保存
			Queue<Action> tempActions = new Queue<Action>(actions);

			// キューをクリアして緊急アクションを追加
			actions.Clear();
			actions.Enqueue(emergencyAction);

			// 保存していたアクションを後ろに追加
			foreach (var action in tempActions)
			{
				actions.Enqueue(action);
			}

			// 現在実行中のアクションがあれば中断
			if (currentAction != null)
			{
				actions.Enqueue(currentAction);
				currentAction = null;
			}

			if (!isMoving)
			{
				isMoving = true;
			}
		}
	}
}