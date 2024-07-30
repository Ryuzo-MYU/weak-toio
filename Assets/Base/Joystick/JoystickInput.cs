using System.Text;
using UnityEngine;

//enumを使うために必要
using System;

public class JoystickInput : MonoBehaviour
{

	// ==============================
	// アナログ入力
	// ==============================
	public float moveHorizontal;
	public float moveVertical;

	// ==============================
	// デジタル入力
	// ==============================
	public float keyHorizontal;
	public float keyVertical;
	public float rotateButton;

	private void Update()
	{
		moveHorizontal = Input.GetAxis("Move Horizontal");
		moveVertical = Input.GetAxis("Move Vertical");
		rotateButton = Input.GetAxis("Rotate Button");
	}
}