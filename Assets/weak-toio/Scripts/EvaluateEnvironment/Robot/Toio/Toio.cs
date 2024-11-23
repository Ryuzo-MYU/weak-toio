using System.Collections;
using System.Collections.Generic;
using toio;
using UnityEngine;

namespace Robot
{
	public class Toio : MonoBehaviour, IToioMovement
	{
		int id;
		Cube cube;
		CubeHandle handle;
		Queue<Action> actions;

		public Toio(int _id, Cube _cube, CubeHandle _handle)
		{
			id = _id;
			cube = _cube;
			handle = _handle;
		}
		public void StartMovement()
		{
			StartCoroutine(MoveOperation(handle));
		}
		private IEnumerator MoveOperation(CubeHandle handle)
		{
			while (actions.Count != 0)
			{
				Action action = actions.Dequeue();
				handle.Move(action.Movement);
				yield return new WaitForSeconds(action.interval);
			}
		}
		public IEnumerator UpdateAction(Action action)
		{
			while (true)
			{
				actions.Enqueue(action);
				yield return null;
			}
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