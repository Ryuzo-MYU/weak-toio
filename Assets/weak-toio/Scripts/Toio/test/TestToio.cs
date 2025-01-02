using System.Collections;
using toio;
using UnityEngine;

namespace ActionGenerate
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
			StartCoroutine(StartSoundQueue());
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
				Debug.Log("光るよ");
				cube.TurnLedOn(255, 0, 0, 1000);
				cube.TurnLedOff();
				yield return new WaitForSeconds(1);
			}
		}

		private IEnumerator StartSoundQueue()
		{
			while (true)
			{
				Debug.Log("音がなるよ");
				cube.PlayPresetSound(5);
				yield return new WaitForSeconds(2f);
			}
		}
	}
}