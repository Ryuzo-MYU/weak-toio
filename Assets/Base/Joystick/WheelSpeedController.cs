using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WheelSpeedController : MonoBehaviour
{
	public int moveSpeed = 20;
	public int rotateSpeed = 10;
	void Update()
	{
		if (Input.GetButtonDown("Speed Up"))
		{
			moveSpeed += 5;
			rotateSpeed += 5;
		}
		if (Input.GetButtonDown("Speed Down"))
		{
			moveSpeed -= 5;
			rotateSpeed -= 5;
		}
	}
}
