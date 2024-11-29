using System.Collections;
using toio;

namespace Robot
{
	public interface IToioActionGenerator
	{
		public Movement Translate(float dist, double speed);
		public Movement Rotate(float deg, double speed);
	}
	public interface IToioActionExecutor
	{
		public IEnumerator Move();
		public bool AddNewAction(Action action);

	}
}