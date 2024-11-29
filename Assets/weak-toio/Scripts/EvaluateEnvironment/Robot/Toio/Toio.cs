using System.Collections;
using System.Collections.Generic;
using Robot;
using toio;
using UnityEngine;

namespace Robot
{
	public class Toio
	{
		public int ID { get; private set; }
		public Cube Cube { get; private set; }
		public CubeHandle Handle { get; private set; }
		private ToioActionGenerator actionGenerator;
		private ToioActionExecutor actionExecutor;

		public Toio(int _id, CubeManager _cubeManager)
		{
			ID = _id;
			Cube = _cubeManager.cubes[ID];
			Handle = _cubeManager.handles[ID];

			actionGenerator = new ToioActionGenerator(Handle);
			actionExecutor = new ToioActionExecutor(Handle);
		}

		// Generator機能の委譲
		public Movement Translate(float dist, double speed)
		{
			return actionGenerator.Translate(dist, speed);
		}

		public Movement Rotate(float deg, double speed)
		{
			return actionGenerator.Rotate(deg, speed);
		}

		// Executor機能の委譲
		public IEnumerator Move()
		{
			return actionExecutor.Move();
		}

		public bool AddNewAction(Action action)
		{
			return actionExecutor.AddNewAction(action);
		}

		public void Stop()
		{
			actionExecutor.Stop();
		}
	}
}
public class ToioActionGenerator : IToioActionGenerator
{
	private CubeHandle Handle { get; }

	public ToioActionGenerator(CubeHandle handle)
	{
		Handle = handle;
	}

	public Movement Translate(float dist, double speed)
	{
		return Handle.TranslateByDist(dist, speed);
	}

	public Movement Rotate(float deg, double speed)
	{
		return Handle.RotateByDeg(deg, speed);
	}
}

public class ToioActionExecutor : IToioActionExecutor
{
	private const int ACTION_MAX_COUNT = 10;
	private Queue<Action> actions;
	private Action currentAction;
	private CubeHandle Handle { get; }
	private bool isMoving = false;

	public ToioActionExecutor(CubeHandle handle)
	{
		Handle = handle;
		actions = new Queue<Action>();
		currentAction = new Action();
	}

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
				Handle.Update();
				Handle.Move(motion.Movement);
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