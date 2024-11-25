using System.Collections;
using toio;

namespace Robot
{
	public interface IToioMovement
	{
		public IEnumerator Move();
		public Movement Translate(float dist, double speed);
		public Movement Rotate(float deg, double speed);
	}
}