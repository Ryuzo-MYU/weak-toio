using System.Collections;
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
			handle = cubeManager.handles[0];

			StartCoroutine(StartMoveQueue());
			StartCoroutine(StartLEDQueue());
			// StartCoroutine(StartSoundQueue());
		}

		private IEnumerator StartMoveQueue()
		{
			while (true)
			{
				Debug.Log("Moveするよ");
				handle.Update();
				handle.TranslateByDist(50, 50).Exec(false);
				yield return new WaitForSeconds(50 / 50);
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