using System.Collections;
using toio;
using UnityEngine;
using UnityEngine.UIElements;

public class ToioMovementTest : MonoBehaviour
{
	[SerializeField] float dist;
	[SerializeField] float translateSpeed;
	Movement translate;
	[SerializeField] float deg;
	[SerializeField] float rotateSpeed;
	Movement rotate;
	CubeManager cubeManager;

	async void Start()
	{
		cubeManager = new CubeManager(ConnectType.Auto);
		await cubeManager.MultiConnect(2);
		translate = cubeManager.handles[0].TranslateByDist(dist, translateSpeed);
		rotate = cubeManager.handles[0].RotateByDeg(deg, rotateSpeed);

		foreach (var handle in cubeManager.handles)
		{
			StartCoroutine(GoCube(handle));
		}
	}

	private IEnumerator RotateCube(CubeHandle handle)
	{
		Debug.Log("RotateCubeが呼ばれたよ");
		handle.Update();
		handle.Move(rotate);
		yield return new WaitForSeconds(1.0f);
		StartCoroutine(GoCube(handle));
	}
	private IEnumerator GoCube(CubeHandle handle)
	{
		Debug.Log("GoCubeが呼ばれたよ");
		handle.Update();
		handle.Move(translate);
		yield return new WaitForSeconds(1.0f);
		StartCoroutine(RotateCube(handle));
	}
}