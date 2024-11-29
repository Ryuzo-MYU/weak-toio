using System.Collections;
using System.Collections.Generic;
using toio;
using UnityEngine;

namespace Robot
{
	public class Toio
	{
		public int ID { get; private set; }
		public Cube Cube { get; private set; }
		public CubeHandle Handle { get; private set; }

		private const int ACTION_MAX_COUNT = 10;
		private Queue<Action> actions;
		private Action currentAction;
		private bool isMoving = false;

		public Toio(int _id, CubeManager _cubeManager)
		{
			ID = _id;
			Cube = _cubeManager.cubes[ID];
			Handle = _cubeManager.handles[ID];
			actions = new Queue<Action>();
			currentAction = new Action();
		}

		// 実行関連のメソッド
		public IEnumerator Move()
		{
			while (actions.Count > 0)
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
			}
		}

		public bool AddNewAction(Action action)
		{
			if (action == null)
			{
				Debug.LogWarning("null のアクション送るな");
				return false;
			}
			if (actions.Count > ACTION_MAX_COUNT)
			{
				return false;
			}
			actions.Enqueue(action);
			Debug.Log("アクション足しました");

			if (!isMoving)
			{
				isMoving = true;
			}

			return true;
		}

		public void Stop()
		{
			isMoving = false;
			actions.Clear();
			currentAction = null;
			Handle.Update();
			Handle.Move(new Movement(Handle, 0, 0));
		}
	}

}