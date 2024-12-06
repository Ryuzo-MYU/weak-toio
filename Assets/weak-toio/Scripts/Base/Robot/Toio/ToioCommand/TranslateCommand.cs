using toio;

namespace Robot
{
	public class TranslateCommand : IToioCommand
	{
		float dist;
		double speed;
		public TranslateCommand(float _dist, double _speed)
		{
			dist = _dist;
			speed = _speed;
		}
		public void Execute(Toio toio)
		{
			CubeHandle handle = toio.Handle;
			Movement translate = handle.TranslateByDist(dist, speed);
			handle.Update();
			handle.Move(translate);
		}
	}
}