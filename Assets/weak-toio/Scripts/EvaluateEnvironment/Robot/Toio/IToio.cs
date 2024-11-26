using System.Collections;
using toio;
using UnityEngine;

namespace Robot
{
	public interface IToioMovement
	{
		public IEnumerator Move();
		public bool AddNewAction(Action action);
		public Movement Translate(float dist, double speed);
		public Movement Rotate(float deg, double speed);
	}
}