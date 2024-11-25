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

		public Toio(int _id, CubeManager _cubeManager)
		{
			ID = _id;
			cubeManager = _cubeManager;
			Cube = cubeManager.cubes[ID];
			Handle = cubeManager.handles[ID];
			actions = new Queue<Action>();
			currentAction = new Robot.Action();
		}
		public void StartMove(MonoBehaviour mono)
		{
			Debug.Log("StartMoveが呼ばれたよ");
			mono.StartCoroutine(Move());
		}
		public IEnumerator Move()
		{
			while (actions.Count != 0)
			{
				Debug.Log("ほな動きますね");

				Motion motion = currentAction.GetNextMotion();
				cubeManager.handles[ID].Update();
				cubeManager.handles[ID].Move(motion.Movement);

				// 現在のアクションが実行されきったらアクションを更新
				if (currentAction.Count() == 0)
				{
					currentAction = actions.Dequeue();
					Debug.Log("アクション無いんで入れ替えますね");
				}
				yield return new WaitForSeconds(motion.interval);
			}
		}
		public void AddNewAction(Action action)
		{
			actions.Enqueue(action);
			Debug.Log("アクション足しました");
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