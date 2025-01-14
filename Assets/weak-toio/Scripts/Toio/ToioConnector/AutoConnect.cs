using System.Collections.Generic;
using System.Security.AccessControl;
using toio;
using UnityEngine;

namespace ActionGenerate
{
  /// <summary>
  /// 複数のキューブへの接続を行うクラス
  /// キューブ1つ1つへの接続が難しそうなので、
  /// 一旦このクラスですべてのキューブに接続したあと、
  /// 各Toioに自分のキューブを持っていってもらう
  /// </summary>
  public class AutoConnect : ToioConnecter
  {
    [SerializeField] private int cubeCount = 0;
    [SerializeField] private string toioTag = "Toio";
    private List<Toio> toios;
    private CubeManager cubeManager;
    private async void Awake()
    {
      toios = new List<Toio>();
      cubeCount = GameObject.FindGameObjectsWithTag(toioTag).Length;
      GameObject[] _toios = GameObject.FindGameObjectsWithTag(toioTag);
      foreach (GameObject toio in _toios)
      {
        toios.Add(toio.GetComponent<Toio>());
      }

      // toioに接続
      cubeManager = new CubeManager(connectType);
      await cubeManager.MultiConnect(cubeCount);

      Debug.Log("Toio接続完了");

      RegisterToio(toios);
    }

    private void OnDestroy()
    {
      cubeManager.DisconnectAll();
    }

    private void RegisterToio(List<Toio> toios)
    {
      foreach (Toio toio in toios)
      {
        int id = cubeManager.cubes.FindIndex(t => t.localName == toio.LocalName);
        toio.Register(cubeManager.cubes[id], cubeManager.handles[id]);
      }
    }
  }
}