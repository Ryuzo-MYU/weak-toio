using System.Collections;
using toio;

namespace Robot
{
	public interface IToioMovement
	{
		public IEnumerator MoveOperation(CubeHandle handle);
		public Movement Translate(float dist, double speed);
		public Movement Rotate(float deg, double speed);
	}
}