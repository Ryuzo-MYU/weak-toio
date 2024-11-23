using toio;

namespace Robot
{
	public interface IToioMovement
	{
		public void StartMovement();
		public Movement Translate(float dist, double speed);
		public Movement Rotate(float deg, double speed);
	}
}