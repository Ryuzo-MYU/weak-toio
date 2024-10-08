using System.Collections;
using System.Collections.Generic;
using toio.Simulator;
using UnityEngine;

public class CollisionEvent : MonoBehaviour
{
    public AutoPatrol autoPatrol;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name != "Mat" && autoPatrol.phase == 0)
        {
            Debug.Log(collision.gameObject.name + "Ç∆è’ìÀ");
            autoPatrol.VirtualCollision();
        }
    }
}
