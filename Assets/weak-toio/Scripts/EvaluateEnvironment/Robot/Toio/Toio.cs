using System.Collections;
using System.Threading.Tasks;
using toio;
using UnityEngine;

namespace Robot
{
	public class Toio
	{
		int id;
		EnvType appointedType;
		Cube cube;
		CubeHandle handle;
		Action action;

		public void Update(Action action)
		{
			this.action = action;
		}
		public void ExecuteAction() { }
		public float Interval { get { return action.Interval; } }

		public IEnumerator ExecuteActionWithInterval(Toio toio)
		{
			toio.ExecuteAction();

			float interval = toio.Interval;
			yield return new WaitForSeconds(interval);
		}
		public Toio(EnvType envType, Cube cube, CubeHandle handle)
		{
			appointedType = envType;
			this.cube = cube;
			this.handle = handle;
		}
	}
}