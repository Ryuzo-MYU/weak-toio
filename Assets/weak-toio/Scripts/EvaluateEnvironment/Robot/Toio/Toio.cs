using System.Collections;
using System.Collections.Generic;
using toio;
using UnityEngine;

namespace Robot
{
	public class Toio : IToioMovement
	{
		public int id;
		public Cube cube;
		public CubeHandle handle;
		Queue<Action> actions;
		Action currentAction;

		public Toio(int _id, Cube _cube, CubeHandle _handle)
		{
			id = _id;
			cube = _cube;
			handle = _handle;
		}
		public IEnumerator Move()
		{
			while (actions.Count != 0)
			{
				Motion motion = currentAction.GetNextMotion();
				handle.Move(motion.Movement);

				// 現在のアクションが実行されきったらアクションを更新
				if (currentAction.Count() == 0)
				{
					currentAction = actions.Dequeue();
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
			return handle.TranslateByDist(dist, speed);
		}
		public Movement Rotate(float deg, double speed)
		{
			return handle.RotateByDeg(deg, speed);
		}
	}
}