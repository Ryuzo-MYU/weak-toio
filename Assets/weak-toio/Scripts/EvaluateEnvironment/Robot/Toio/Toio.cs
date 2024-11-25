using System.Collections;
using System.Collections.Generic;
using toio;
using UnityEngine;

namespace Robot
{
	public class Toio : IToioMovement
	{
		public int ID { get; private set; }
		public Cube Cube { get; private set; }
		public CubeHandle Handle { get; private set; }
		Queue<Action> actions;
		Action currentAction;
		CubeManager cubeManager;
		bool isMoving = false;

		public Toio(int _id, CubeManager _cubeManager)
		{
			ID = _id;
			cubeManager = _cubeManager;
			Cube = cubeManager.cubes[ID];
			Handle = cubeManager.handles[ID];
			actions = new Queue<Action>();
			currentAction = new Action();
		}
		public void StartMove(MonoBehaviour mono)
		{
			if (!isMoving)
			{
				Debug.Log("StartMoveが呼ばれたよ");
				isMoving = true;
				mono.StartCoroutine(Move());
			}
		}
		public IEnumerator Move()
		{
			while (isMoving)
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
						yield return new WaitForSeconds(0.1f);
						continue;
					}
				}

				Motion motion = currentAction.GetNextMotion();
				if (motion != null)
				{
					Debug.Log("ほな動きますね");
					Handle.Update();
					Handle.Move(motion.Movement);
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
			if (actions.Count > 10)
			{
				Debug.Log("action溜まってんね");
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

		// アクション作成関数の実装部分。ActionGeneratorで利用
		public Movement Translate(float dist, double speed)
		{
			return Handle.TranslateByDist(dist, speed);
		}
		public Movement Rotate(float deg, double speed)
		{
			return Handle.RotateByDeg(deg, speed);
		}
	}
}