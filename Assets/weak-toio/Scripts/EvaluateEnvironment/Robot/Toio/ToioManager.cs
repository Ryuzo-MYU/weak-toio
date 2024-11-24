using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using toio;
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
		public async Task<bool> Connect()
		{
			try
			{
				cubeManager = new CubeManager(connectType);
				await cubeManager.MultiConnect(cubeCount);

				// 接続されたcubeの数を確認
				if (cubeManager.cubes.Count < cubeCount)
				{
					Debug.LogWarning($"要求された数のtoioに接続できませんでした。要求数: {cubeCount}, 接続数: {cubeManager.cubes.Count}");
					return false;
				}

				Debug.Log($"toioへの接続に成功しました。接続数: {cubeManager.cubes.Count}");
				return true;
			}
			catch (Exception e)
			{
				Debug.LogError($"toioとの接続中にエラーが発生しました: {e.Message}");
				return false;
			}
		}
		public void SetUp()
		{
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
			if (index > Toios.Count)
			{
				Debug.LogWarning("配列外参照してます");
				return null;
			}

			return Toios[index];
		}
	}
}