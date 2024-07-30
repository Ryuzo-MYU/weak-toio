using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
	public Slider moveVerticalSlider;
	public Slider moveHorizontalSlider;
	public Slider rotateButtonHorizontalSlider;
	public JoystickInput joystick;
	void Update()
	{
		float moveHorizontal = joystick.moveHorizontal;
		moveHorizontalSlider.value = moveHorizontal;

		float moveVertical = joystick.moveVertical;
		moveVerticalSlider.value = moveVertical;

		float rotateButton = joystick.rotateButton;
		rotateButtonHorizontalSlider.value = rotateButton;
	}
}
