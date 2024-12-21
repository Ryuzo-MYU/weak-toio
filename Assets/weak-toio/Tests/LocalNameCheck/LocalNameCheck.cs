using UnityEngine;
using toio;
using System.Collections.Generic;

public class ToioCubeManagerScanner : MonoBehaviour
{
	[SerializeField] ConnectType connectType;
	[SerializeField] int cubeCount = 0;
	private CubeManager cubeManager;
	private bool isScanning = false;

	async void Start()
	{
		cubeManager = new CubeManager(connectType);
		Debug.Log("CubeManagerを使用したスキャンを開始しました。");
		await cubeManager.MultiConnect(cubeCount);


		Debug.Log("接続数 :" + cubeManager.cubes.Count);
		if (cubeManager.connectedCubes.Count != 0)
		{
			Debug.Log($"接続成功:");
			foreach (Cube cube in cubeManager.connectedCubes)
			{
				Debug.Log($"- 名前: {cube.localName}");
				Debug.Log($"- ID: {cube.id}");
				Debug.Log($"- アドレス: {cube.addr}");
				Debug.Log($"- 接続状態: {cube.isConnected}");

				// 接続確認のためLEDを点灯
				cube.TurnLedOn(255, 0, 0, 0);
			}
		}
		else
		{
			Debug.Log("接続できませんでした。");
		}
	}

	void OnDestroy()
	{
		if (cubeManager != null)
		{
			cubeManager.DisconnectAll();
		}
	}
}