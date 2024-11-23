using System.Collections.Generic;
using toio;
using UnityEngine;

namespace Robot
{
	public class ToioManager : MonoBehaviour
	{
		public int CubeCount { get; private set; }
		CubeManager cubeManager;
		List<Toio> Toios;

		public ToioManager(int cubeCount)
		{
			CubeCount = cubeCount;
		}
		public async void Start()
		{
			cubeManager = new CubeManager();
			await cubeManager.MultiConnect(CubeCount);

			int id = 0;
			for (int count = 0; count < cubeManager.cubes.Count; count++)
			{
				var cube = cubeManager.cubes[count];
				var handle = cubeManager.handles[count];
				Toio toio = new Toio(id, cube, handle);
				Toios.Add(toio);
				id++;
			}
		}
		public void UpdateAction(Action nextAc)
		{
			foreach (Toio toio in Toios)
			{
				StartCoroutine(toio.UpdateAction(nextAc));
			}
		}
	}
}