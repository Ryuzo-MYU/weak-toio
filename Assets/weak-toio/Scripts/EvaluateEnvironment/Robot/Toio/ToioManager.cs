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
		List<Toio> Toios;

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
		public IToioMovement GetHandle()
		{
			if (cubeManager.handles.Count < 1)
			{
				Debug.LogWarning("キューブ無いっす");
				return null;
			}
			return Toios[0];
		}
	}
}