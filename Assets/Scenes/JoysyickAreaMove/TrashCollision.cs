using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class TrashCollision : MonoBehaviour
{
	public string trashTag;
	// Start is called before the first frame update
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{

	}
	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == trashTag)
		{
			Debug.Log("異常と遭遇");
		}
	}
}
