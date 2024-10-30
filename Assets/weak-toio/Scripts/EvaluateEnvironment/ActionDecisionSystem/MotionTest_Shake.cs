
using System.Timers;
using toio;
using UnityEngine;

public class MotionTest_Shake : MonoBehaviour
{
	CubeManager cm;
	[SerializeField] float deg;
	[SerializeField] int speed;
	[SerializeField] float repeatRate;
	private float elapsedTime;
	private async void Start()
	{
		elapsedTime = 0;

		cm = new CubeManager(ConnectType.Auto);
		await cm.MultiConnect(1);
	}
	private void Update()
	{
		foreach (var handle in cm.handles)
		{
			handle.Update();
		}
		if (elapsedTime > repeatRate)
		{
			if (cm.handles == null) return;
			foreach (var handle in cm.handles)
			{
				Movement shiver = handle.RotateByDeg(deg, speed);
				handle.Move(shiver);
			}
			deg = -deg;
			elapsedTime = 0;
		}
		elapsedTime += Time.deltaTime;
	}
}