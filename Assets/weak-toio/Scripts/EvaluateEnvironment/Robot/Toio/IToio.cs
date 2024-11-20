using toio;

namespace Robot
{
	public interface IToioMovement
	{
		public Movement Translate(float dist, double speed);
		public Movement Rotate(float deg, double speed);
	}
}