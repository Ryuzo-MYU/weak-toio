using ActionGenerate;
using UnityEngine;

namespace ActionLibrary
{
  public static class CollisionedActionImplementation
  {
    public static Action Collisioned(this ToioActionLibrary lib)
    {
      Action action = new Action();

      // 1. 後退
      IMovementCommand movement1 = lib.Translate(-50, 40);
      action.AddMovement(movement1);
      float duration1 = movement1.GetInterval();
      // 2. 回転 (ランダムな角度)
      float rotationAngle = Random.Range(30f, 150f);
      IMovementCommand movement2 = lib.DegRotate(rotationAngle, 30);
      action.AddMovement(movement2);
      float duration2 = movement2.GetInterval();
      // 3. 前進
      IMovementCommand movement3 = lib.Translate(100, 50);
      action.AddMovement(movement3);
      float duration3 = movement3.GetInterval();
      float totalDuration = duration1 + duration2 + duration3;

      // LEDとサウンドのコマンドを追加
      ILightCommand ledCommand = lib.TurnOnLED(255, 255, 255, (int)(totalDuration * 1000));
      action.AddLight(ledCommand);
      ISoundCommand soundCommand = lib.PresetSound(0, 20, totalDuration);
      action.AddSound(soundCommand);

      return action;
    }
  }
}