using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VelocityUI : MonoBehaviour
{
	public TextMeshProUGUI text;
	public WheelSpeedController wheelSpeed;

	// Update is called once per frame
	void Update()
	{
		int moveSpeed = wheelSpeed.moveSpeed;
		text.text = "Velocity: " + moveSpeed;
	}
}
