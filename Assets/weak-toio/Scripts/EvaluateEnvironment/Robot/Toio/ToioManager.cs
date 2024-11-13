using System.Collections.Generic;
using toio;

namespace Robot
{
	public class ToioManager
	{
		public int CubeCount { get; private set; }
		CubeManager cubeManager;
		List<Toio> Toios;

		public ToioManager(int cubeCount)
		{
			CubeCount = cubeCount;
		}

		public async void Start(EnvType appointedType)
		{
			cubeManager = new CubeManager();
			await cubeManager.MultiConnect(CubeCount);

			for (int count = 0; count < cubeManager.cubes.Count; count++)
			{
				var cube = cubeManager.cubes[count];
				var handle = cubeManager.handles[count];
				Toio toio = new Toio(appointedType, cube, handle);
				Toios.Add(toio);
			}
		}

		public void Update(Action nextAc)
		{
			foreach (Toio toio in Toios)
			{
				toio.Update(nextAc);
			}
		}
	}
}