using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using toio;

namespace Robot
{
	public class Toio : MonoBehaviour
	{
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
		public int motionCount;

		[SerializeField] private int actionMaxCount;
		private Queue<Action> actions;
		private Action currentAction;
		private bool isMoving = false;

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


		private void OnConnectSucceeded()
		{
			toioConnector.RegisterToio(this);
		}
		public void Register(int id, CubeManager cubeManager)
		{
			this._id = id;
			this._cube = cubeManager.cubes[_id];
			this._handle = cubeManager.handles[_id];

			OnRegisterCompleted();
		}
		private void OnRegisterCompleted()
		{
			actionGenerator.ProcessAction(this);
		}

		// 実行関連のメソッド
		public IEnumerator Move()
		{
			while (true)
			{
				if (currentAction == null || currentAction.Count() == 0)
				{
					if (actions.Count > 0)
					{
						currentAction = actions.Dequeue();
						motionCount = currentAction.Count();
						Debug.Log("アクション無いんで入れ替えますね");
					}
					else
					{
						yield return null;
						continue;
					}
				}

				Robot.Motion motion = currentAction.GetNextMotion();
				if (motion != null)
				{
					Debug.Log("ほな動きますね");
					motion.command.Execute(this);
					Debug.Log($"インターバル: {motion.interval}");
					yield return new WaitForSeconds(motion.interval);
				}
				yield return null;
			}
		}

		public void AddNewAction(Action action)
		{
			if (action == null)
			{
				Debug.LogWarning("null のアクション送るな");
				return;
			}
			if (actions.Count > actionMaxCount)
			{
				Debug.Log("アクション溜まりすぎ");
				return;
			}
			actions.Enqueue(action);
			Debug.Log("アクション足しました");

			if (!isMoving)
			{
				isMoving = true;
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
	}

}