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

		public Toio(int _id, Cube _cube, CubeHandle _handle)
		{
			ID = _id;
			Cube = _cube;
			Handle = _handle;
			actions = new Queue<Action>();
			currentAction = new Robot.Action();
		}
		public IEnumerator Move()
		{
			while (actions.Count != 0)
			{
				Motion motion = currentAction.GetNextMotion();
				Handle.Move(motion.Movement);

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