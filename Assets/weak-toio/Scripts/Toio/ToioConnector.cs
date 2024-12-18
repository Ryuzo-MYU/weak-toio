using System;
using System.Collections.Generic;
using toio;
using UnityEngine;
using UnityEngine.Events;

namespace Robot
{
	/// <summary>
	/// 複数のキューブへの接続を行うクラス
	/// キューブ1つ1つへの接続が難しそうなので、
	/// 一旦このクラスですべてのキューブに接続したあと、
	/// 各Toioに自分のキューブを持っていってもらう
	/// </summary>
	public class ToioConnector : MonoBehaviour
	{
		public event System.Action OnConnectSucceeded;
		[Tooltip("UnityEditor上ならSimmurator、現実ならReal、お任せならAuto")]
		public ConnectType connectType = ConnectType.Auto;
		public List<string> toioNames;
		[SerializeField] private int cubeCount = 0;
		[SerializeField] private string toioTag = "Toio";
		private CubeManager cubeManager;
		private async void Awake()
		{
			toioNames = new List<string>();
			OnConnectSucceeded += ConnectSucceeded;

			cubeCount = GameObject.FindGameObjectsWithTag(toioTag).Length;
			Debug.Log(cubeCount);
			// toioに接続
			cubeManager = new CubeManager(connectType);
			await cubeManager.MultiConnect(cubeCount);

			Debug.Log("Toio接続完了");
			OnConnectSucceeded.Invoke();
		}
		private void ConnectSucceeded()
		{
			foreach (var toio in cubeManager.cubes)
			{
				toioNames.Add(toio.localName);
			}
		}
		private void OnDestroy()
		{
			cubeManager.DisconnectAll();
		}

		public void RegisterToio(Toio toio)
		{
			int id = cubeManager.cubes.FindIndex(c => c.localName == toio.Name);
			toio.Register(id, cubeManager);
		}
	}
}