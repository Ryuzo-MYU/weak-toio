using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using toio;
using System;

public class ToioSoundPlayer : MonoBehaviour
{
	public JoystickMove joystickMove;
	CubeManager cm;

	public int[] soundID = new int[4];
	void Start()
	{
		cm = joystickMove.cm;
	}

	// Update is called once per frame
	void Update()
	{

		// CubeManagerからモジュールを間接利用した場合:
		foreach (var cube in cm.syncCubes)
		{
			if (Input.GetButton("Sound Button 0"))
			{
				Debug.Log("Sound Buttom 1 Pushed!");
				cube.PlayPresetSound(soundID[0], 255, Cube.ORDER_TYPE.Strong);
				Debug.Log("Sound Play Success!");
			}

			if (Input.GetButton("Sound Button 1"))
			{
				Debug.Log("Sound Button 1 Pushed!");
				cube.PlayPresetSound(soundID[1], 255, Cube.ORDER_TYPE.Strong);
				Debug.Log("Sound Play Success!");
			}
			if (Input.GetButton("Sound Button 2"))
			{
				Debug.Log("Sound Button 2 Pushed!");
				cube.PlayPresetSound(soundID[2], 255, Cube.ORDER_TYPE.Strong);
				Debug.Log("Sound Play Success!");
			}
			if (Input.GetButton("Sound Button 3"))
			{
				Debug.Log("Sound Button 3 Pushed!");
				cube.PlayPresetSound(soundID[3], 255, Cube.ORDER_TYPE.Strong);
				Debug.Log("Sound Play Success!");
			}
		}
	}
}
