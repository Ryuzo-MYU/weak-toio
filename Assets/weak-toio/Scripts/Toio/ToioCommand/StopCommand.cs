using toio;

namespace ActionGenerate
{
	public class StopCommand : IMovementCommand
	{
		float interval;
		public StopCommand(float _interval)
		{
			interval = _interval;
		}

		public float GetInterval()
		{
			return interval;
		}

		public void Exec(Toio toio)
		{
			CubeHandle handle = toio.Handle;
			handle.Move(0, 0);
		}
	}
}