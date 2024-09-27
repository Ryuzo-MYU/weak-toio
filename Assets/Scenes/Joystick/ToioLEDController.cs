using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using toio;
using Unity.VisualScripting.FullSerializer;

public class ToioLEDController : MonoBehaviour
{
	public JoystickMove joystickMove;
	CubeManager cm;

	public bool[] rgb = new bool[3];

	void Start()
	{
		cm = joystickMove.cm;
	}


	void Update()
	{
		if (Input.GetButton("LED Button R"))
		{
			Debug.Log("LED Button R Pushed!");
			rgb = new bool[3] { rgb[0], false, false };
			rgb[0] = !rgb[0];
		}
		if (Input.GetButton("LED Button G"))
		{
			Debug.Log("LED Button G Pushed!");
			rgb = new bool[3] { false, rgb[0], false };
			rgb[1] = !rgb[1];
		}
		if (Input.GetButton("LED Button B"))
		{
			Debug.Log("LED Button B Pushed!");
			rgb = new bool[3] { false, false, rgb[0] };
			rgb[2] = !rgb[2];
		}

		// CubeManagerからモジュールを間接利用した場合:
		foreach (var cube in cm.syncCubes)
		{
			if (rgb[0]) { cube.TurnLedOn(255, 0, 0, 200); }
			else if (rgb[1]) { cube.TurnLedOn(0, 255, 0, 200); }
			else if (rgb[2]) { cube.TurnLedOn(0, 0, 255, 200); }
			else { cube.TurnLedOff(); }
		}

	}
}