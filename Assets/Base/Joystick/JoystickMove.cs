using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using toio;
using UnityEditor;
using toio.VisualScript;

public class JoystickMove : MonoBehaviour
{
	public ConnectType connectType;
	public CubeManager cm;

	[SerializeField] int duration = 200;
	public JoystickInput joystick;
	public WheelSpeedController wheelSpeed;

	public int leftWheel;
	public int rightWheel;
	public int[] soundID = new int[4];
	public bool[] rgb = new bool[3] { false, false, false };
	async void Start()
	{
		// ConnectType.Auto - ビルド対象に応じて内部実装が自動的に変わる
		// ConnectType.Simulator - ビルド対象に関わらずシミュレータのキューブで動作する
		// ConnectType.Real - ビルド対象に関わらずリアル(現実)のキューブで動作する
		cm = new CubeManager(connectType);
		await cm.MultiConnect(2);
	}

	void Update()
	{
		MoveCalcurate();
		RotateCalcurate();

		// CubeManagerからモジュールを間接利用した場合:
		foreach (var cube in cm.syncCubes)
		{
			SoundOperation(cube);
			// LEDOperation(cube);
			cube.Move(leftWheel, rightWheel, duration);
		}
	}

	// toioを前後左右に動かす
	void MoveCalcurate()
	{
		float moveHorizontal = joystick.moveHorizontal;
		float moveVertical = joystick.moveVertical;
		float moveSpeed = wheelSpeed.moveSpeed;

		leftWheel = (int)(moveSpeed * moveVertical);
		rightWheel = (int)(moveSpeed * moveVertical);

		if (moveHorizontal == 1)
		{
			leftWheel = (int)(moveSpeed * moveHorizontal);
			rightWheel = 0;
		}
		else if (moveHorizontal == -1)
		{
			leftWheel = 0;
			rightWheel = (int)(moveSpeed * moveHorizontal * -1);
		}
	}

	// toioをその場で回転させる
	void RotateCalcurate()
	{
		float rotateButton = joystick.rotateButton;
		float moveSpeed = wheelSpeed.moveSpeed;
		if (rotateButton > 0.5)
		{
			leftWheel = (int)(moveSpeed * rotateButton);
			rightWheel = (int)(moveSpeed * -rotateButton);
		}
		else if (-0.5 > rotateButton)
		{
			leftWheel = (int)(moveSpeed * rotateButton);
			rightWheel = (int)(moveSpeed * -rotateButton);
		}
	}
	void SoundOperation(Cube cube)
	{

		int[] soundid = soundID;
		if (Input.GetButton("Sound Button 0"))
		{
			Debug.Log("Sound Buttom 1 Pushed!");
			cube.PlayPresetSound(soundid[0], 255, Cube.ORDER_TYPE.Strong);
			Debug.Log("Sound Play Success!");
		}

		if (Input.GetButton("Sound Button 1"))
		{
			Debug.Log("Sound Button 1 Pushed!");
			cube.PlayPresetSound(soundid[1], 255, Cube.ORDER_TYPE.Strong);
			Debug.Log("Sound Play Success!");
		}
		if (Input.GetButton("Sound Button 2"))
		{
			Debug.Log("Sound Button 2 Pushed!");
			cube.PlayPresetSound(soundid[2], 255, Cube.ORDER_TYPE.Strong);
			Debug.Log("Sound Play Success!");
		}
		if (Input.GetButton("Sound Button 3"))
		{
			Debug.Log("Sound Button 3 Pushed!");
			cube.PlayPresetSound(soundid[3], 255, Cube.ORDER_TYPE.Strong);
			Debug.Log("Sound Play Success!");
		}
	}
	void LEDOperation(Cube cube)
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

		if (rgb[0]) { cube.TurnLedOn(255, 0, 0, 10); }
		else if (rgb[1]) { cube.TurnLedOn(0, 255, 0, 10); }
		else if (rgb[2]) { cube.TurnLedOn(0, 0, 255, 10); }
		else { cube.TurnLedOff(); }
	}
}