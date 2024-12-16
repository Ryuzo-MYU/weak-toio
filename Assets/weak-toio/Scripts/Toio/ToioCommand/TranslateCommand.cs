using System;
using toio;

namespace Robot
{
	public class TranslateCommand : IMovementCommand
	{
		float dist;
		double speed;
		public TranslateCommand(float _dist, double _speed)
		{
			dist = _dist;
			speed = _speed;
		}

		public float GetInterval()
		{
			return Math.Abs(dist / (float)speed);
		}
		public void Exec(Toio toio)
		{
			CubeHandle handle = toio.Handle;
			Movement translate = handle.TranslateByDist(dist, speed);
			handle.Update();
			handle.Move(translate);
		}
	}
}