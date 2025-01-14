using UnityEngine;

public class ChaseTarget : MonoBehaviour
{
  [SerializeField] Transform target;
  [SerializeField] Vector3 offset;
  void Start()
  {
    if (target == null)
    {
      Debug.LogError("追跡対象を指定してください");
      return;
    }
    offset = transform.position - target.position;
    transform.position = target.position + offset;
    transform.LookAt(target);
  }

  private void FixedUpdate()
  {
    transform.position = target.position + offset;
    transform.LookAt(target);
  }
}
