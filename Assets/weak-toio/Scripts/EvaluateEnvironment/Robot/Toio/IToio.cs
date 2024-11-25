using System.Collections;
using toio;

namespace Robot
{
	public interface IToioMovement
	{
		public IEnumerator Move();
		public void AddNewAction(Action action);
		public Movement Translate(float dist, double speed);
		public Movement Rotate(float deg, double speed);
	}
}