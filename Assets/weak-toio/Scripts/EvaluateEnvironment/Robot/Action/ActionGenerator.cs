using System;
using System.Collections.Generic;
using Evaluation;
using toio;

namespace Robot
{
	public abstract class ActionGenerator
	{
		protected IToioMovement _toio;
		protected Result _result;

		/// <summary>
		/// 前後移動のMovementを返す
		/// </summary>
		/// <param name="dist">距離</param>
		/// <param name="speed">速度</param>
		/// <returns>Movement</returns>
		protected Motion Translate(float dist, double speed)
		{
			Movement translate = _toio.Translate(dist, speed);
			float intarval = (float)dist / (float)speed;
			Motion operation = new Motion(translate, intarval);
			return operation;
		}

		/// <summary>
		/// 回転移動のMovementを返す
		/// </summary>
		/// <param name="deg">角度</param>
		/// <param name="speed">速度</param>
		/// <returns>Movement</returns>
		protected Motion Rotate(float deg, double speed)
		{
			Movement rotate = _toio.Rotate(deg, speed);
			float intarval = (float)deg / (float)speed;
			Motion operation = new Motion(rotate, intarval);
			return operation;
		}
	}
	public class Action
	{
		Queue<Motion> motions;
		public Action()
		{
			motions = new Queue<Motion>();
		}
		public Action(Queue<Motion> motions)
		{
			this.motions = motions;
		}
		public Motion GetNextMotion()
		{
			return motions.Dequeue();
		}
		public int Count()
		{
			return motions.Count;
		}

		public static implicit operator Queue<object>(Action v)
		{
			throw new NotImplementedException();
		}
	}
	public class Motion
	{
		public Movement Movement;
		public float interval;
		public Motion(Movement _movement, float _intervel)
		{
			Movement = _movement;
			interval = Math.Abs(_intervel);
		}
	}
}