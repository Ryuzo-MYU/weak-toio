using Evaluation;
using toio;

namespace Robot
{
	public abstract class ActionGenerator
	{
		protected readonly IToioMovement _toio;
		protected Result _result;

		/// <summary>
		/// 前後移動のMovementを返す
		/// </summary>
		/// <param name="dist">距離</param>
		/// <param name="speed">速度</param>
		/// <returns>Movement</returns>
		protected Action Translate(float dist, double speed)
		{
			Movement translate = _toio.Translate(dist, speed);
			float intarval = (float)dist / (float)speed;
			Action operation = new Action(translate, intarval);
			return operation;
		}

		/// <summary>
		/// 回転移動のMovementを返す
		/// </summary>
		/// <param name="deg">角度</param>
		/// <param name="speed">速度</param>
		/// <returns>Movement</returns>
		protected Action Rotate(float deg, double speed)
		{
			Movement rotate = _toio.Rotate(deg, speed);
			float intarval = (float)deg / (float)speed;
			Action operation = new Action(rotate, intarval);
			return operation;
		}
	}
	public struct Action
	{
		public Movement Movement;
		public float interval;
		public Action(Movement _movement, float _intervel)
		{
			Movement = _movement;
			interval = _intervel;
		}
	}
}