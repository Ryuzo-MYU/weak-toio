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
		MoveOperation();
		RotateOperation();

		// CubeManagerからモジュールを間接利用した場合:
		foreach (var cube in cm.syncCubes)
		{
			cube.Move(leftWheel, rightWheel, duration);
		}
	}

	// toioを前後左右に動かす
	void MoveOperation()
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
	void RotateOperation()
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
}