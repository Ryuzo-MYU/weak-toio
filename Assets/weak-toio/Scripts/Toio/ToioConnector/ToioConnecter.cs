using toio;
using UnityEngine;

namespace ActionGenerate
{
  public class ToioConnecter : MonoBehaviour
  {
    [Tooltip("UnityEditor上ならSimmurator、現実ならReal、お任せならAuto")]
    public ConnectType connectType = ConnectType.Real;
  }
}