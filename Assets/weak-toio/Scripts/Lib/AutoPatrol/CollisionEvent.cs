using UnityEngine;

public class CollisionEvent : MonoBehaviour
{
    public AutoPatrol autoPatrol;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name != "Mat" && autoPatrol.phase == 0)
        {
            Debug.Log(collision.gameObject.name + "�ƏՓ�");
            autoPatrol.VirtualCollision();
        }
    }
}
