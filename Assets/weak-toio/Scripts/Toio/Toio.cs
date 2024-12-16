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

		[SerializeField] private int movementCount;
		[SerializeField] private int lightCount;
		[SerializeField] private int soundCount;
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
			StartCoroutine(actionGenerator.StartMove(this));
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
				StartCoroutine(Move());
				StartCoroutine(ControllLED());
				StartCoroutine(PlaySound());
			}
			yield return null;
		}

		private IEnumerator Move()
		{
			if (currentAction.MovementCount() == 0) yield return null;
			IMovementCommand move = currentAction.GetNextMovement();
			movementCount = currentAction.MovementCount();
			move.Exec(this);
			yield return new WaitForSeconds(move.GetInterval());
		}

		private IEnumerator ControllLED()
		{
			if (currentAction.LightCount() == 0) yield return null;
			ILightCommand light = currentAction.GetNextLight();
			lightCount = currentAction.LightCount();
			light.Exec(this);
			yield return new WaitForSeconds(light.GetInterval());
		}

		private IEnumerator PlaySound()
		{
			if (currentAction.SoundCount() == 0) yield return null;
			ISoundCommand sound = currentAction.GetNextSound();
			soundCount = currentAction.SoundCount();
			sound.Exec(this);
			yield return new WaitForSeconds(sound.GetInterval());
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
	}

}