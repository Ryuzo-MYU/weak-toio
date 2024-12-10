using System.Collections.Generic;
using toio;
using UnityEngine;

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
		public delegate void CubeConnectSuccessedEventHandler(List<Toio> toios);
		public event CubeConnectSuccessedEventHandler OnConnectSuccessed;
		[Tooltip("UnityEditor上ならSimmurator、現実ならReal、お任せならAuto")]
		public ConnectType connectType = ConnectType.Auto;
		public List<string> toioNames;
		private int cubeCount = 0;
		private string toioTag = "Toio";
		private CubeManager cubeManager;
		private List<Toio> toios;
		private async void Start()
		{
			toioNames = new List<string>();

			cubeCount = GameObject.FindGameObjectsWithTag(toioTag).Length;
			// toioに接続
			cubeManager = new CubeManager(connectType);
			await cubeManager.MultiConnect(cubeCount);

			toios = new List<Toio>();
			for (int id = 0; id < cubeManager.cubes.Count; id++)
			{
				toioNames.Add(cubeManager.connectedCubes[id].localName);
				Toio toio = new Toio(id, cubeManager);
				toios.Add(toio);
			}

			Debug.Log("接続完了");
			OnConnectSuccessed(toios);
		}
	}
}