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

			cube = cubeManager.cubes[0];

			StartCoroutine(StartMoveQueue());
			StartCoroutine(StartLEDQueue());
			StartCoroutine(StartSoundQueue());
		}

		private IEnumerator StartMoveQueue()
		{
			while (true)
			{
				cube.Move(10, 10, 1000);
				yield return new WaitForSeconds(1f);
			}
		}

		private IEnumerator StartLEDQueue()
		{
			while (true)
			{
				cube.TurnLedOn(255, 0, 0, 500);
				cube.TurnLedOff();
				yield return new WaitForSeconds(1);
			}
		}

		private IEnumerator StartSoundQueue()
		{
			while (true)
			{
				cube.PlayPresetSound(1);
				cube.PlayPresetSound(3);
				yield return new WaitForSeconds(1f);
			}
		}
	}
}