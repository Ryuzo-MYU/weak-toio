using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMove : MonoBehaviour
{
	public float speed;
	public JoystickInput joystick;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	void Move(){
		float moveHorizontal = joystick.moveHorizontal;
		float moveVertical = joystick.moveVertical;
	}
}
