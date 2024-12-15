using System.Collections;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using toio;
using UnityEngine;

namespace Robot
{
	public class TestToio : MonoBehaviour
	{
		CubeManager cubeManager;
		Cube cube;
		CubeHandle handle;
		public ConnectType connectType;
		private async void Start()
		{
			cubeManager = new CubeManager(connectType);
			await cubeManager.SingleConnect();

			StartCoroutine(StartMoveQueue());
			StartCoroutine(StartLEDQueue());
			StartCoroutine(StartSoundQueue());
		}

		private IEnumerator StartMoveQueue(){
			while(true){

			}
		}

		private IEnumerator StartLEDQueue(){
			while(true){

			}
		}

		private IEnumerator StartSoundQueue(){
			while(true){

			}
		}
	}
}