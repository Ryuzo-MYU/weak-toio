using System.Collections.Generic;
using System.Threading.Tasks;
using toio;
using UnityEditor.PackageManager;
using UnityEngine;

namespace Robot
{
	public class ToioManager : MonoBehaviour
	{
		private ConnectType connectType;
		private int cubeCount;
		CubeManager cubeManager;
		List<Toio> Toios = new List<Toio>();
		public bool IsInitialized { get; private set; }

		public ToioManager(ConnectType connectType, int cubeCount)
		{
			this.connectType = connectType;
			this.cubeCount = cubeCount;
		}
		public async Task Setup()
		{
			cubeManager = new CubeManager(connectType);
			await cubeManager.MultiConnect(cubeCount);

			int id = 0;
			for (int count = 0; count < cubeManager.cubes.Count; count++)
			{
				var cube = cubeManager.cubes[count];
				var handle = cubeManager.handles[count];
				Toio toio = new Toio(id, cube, handle);
				Toios.Add(toio);
				id++;
			}

			foreach (IToioMovement toio in Toios)
			{
				toio.StartMovement();
			}

			IsInitialized = true;
		}
		public void AddNewAction(Action nextMotions)
		{
			foreach (Toio toio in Toios)
			{
				StartCoroutine(toio.AddNewAction(nextMotions));
			}
		}
		public Toio GetToio(int index)
		{
			if (Toios[index] == null) Debug.LogWarning("キューブないです");

			return Toios[index];
		}
	}
}