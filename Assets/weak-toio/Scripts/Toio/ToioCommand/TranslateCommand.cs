using System;
using UnityEngine;
using toio;

namespace ActionGenerate
{
  public class TranslateCommand : IMovementCommand
  {
    float dist;
    double speed;
    private const int MAX_DURATION_MS = 2550;
    private const int MAX_SPEED = 100;
    private CubeHandle handle;

    public TranslateCommand(CubeHandle _handle, float _dist, double _speed)
    {
      handle = _handle;
      dist = _dist;
      speed = Math.Clamp(_speed, -MAX_SPEED, MAX_SPEED);
    }

    public float GetInterval()
    {
      // 基本速度の計算
      double velocity = Math.Abs(speed * CubeHandle.VDotOverU);

      // 基本移動時間の計算
      double baseTime = Math.Abs(dist) / velocity;

      // システムの遅延時間を加算
      double totalTime = baseTime + CubeHandle.lag;

      // 移動距離に応じた加速・減速時間の考慮
      // 短い距離の場合は加速・減速の影響が大きい
      if (Math.Abs(dist) < 50)  // 50は経験的な閾値
      {
        totalTime += CubeHandle.dt * 2;  // 加速・減速のための追加時間
      }

      // 極端に遅い速度や短い距離の場合の最小時間保証
      double minimumTime = CubeHandle.lag + CubeHandle.dt * 3;
      return Math.Max((float)totalTime, (float)minimumTime);
    }

    public void Exec(Toio toio)
    {
      CubeHandle handle = toio.Handle;
      Movement translate = handle.TranslateByDist(dist, speed);
      handle.Update();
      handle.Move(translate, false);
    }
  }
}