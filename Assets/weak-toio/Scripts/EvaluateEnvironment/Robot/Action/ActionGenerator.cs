using System.Collections;
using Evaluation;
using toio;

namespace Robot
{
	public abstract class ActionGenerator
	{
		protected readonly IToioMovement _toio;
		protected Result result;
		public abstract MovementOperation GenerateAction(Result result);

		/// <summary>
		/// 前後移動のMovementを返す
		/// </summary>
		/// <param name="dist">距離</param>
		/// <param name="speed">速度</param>
		/// <returns></returns>
		protected MovementOperation Translate(float dist, double speed)
		{
			Movement translate = _toio.Translate(dist, speed);
			float intarval = (float)dist / (float)speed;
			MovementOperation operation = new MovementOperation(translate, intarval);
			return operation;
		}

		protected MovementOperation Rotate(float deg, double speed)
		{
			Movement rotate = _toio.Rotate(deg, speed);
			float intarval = (float)deg / (float)speed;
			MovementOperation operation = new MovementOperation(rotate, intarval);
			return operation;
		}
	}
	public struct MovementOperation
	{
		public Movement Movement;
		public float interval;
		public MovementOperation(Movement _movement, float _intervel)
		{
			Movement = _movement;
			interval = _intervel;
		}
	}
}