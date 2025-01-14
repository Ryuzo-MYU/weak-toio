using System;
using ActionGenerate;
using toio;
using UnityEngine;

public class DegRotateCommand : IMovementCommand
{
  double deg;
  double speed;
  private CubeHandle handle;
  private const int MAX_DURATION_MS = 2550;
  private const int MAX_SPEED = 100;

  // 移動命令 制限
  // https://github.com/morikatron/toio-sdk-for-unity/blob/main/docs/tutorials_basic.md#3-%E7%A7%BB%E5%8B%95%E3%81%99%E3%82%8B

  public DegRotateCommand(CubeHandle _handle, double _deg, double _speed)
  {
    handle = _handle;
    deg = _deg;
    speed = Math.Clamp(_speed, -MAX_SPEED, MAX_SPEED);
  }

  public float GetInterval()
  {
    double rad = deg * Math.PI / 180.0;
    double angularVelocity = Math.Abs(speed * CubeHandle.VDotOverU / CubeHandle.TireWidthDot);
    return (float)(Math.Abs(rad) / angularVelocity);
  }
  public void Exec(Toio toio)
  {
    CubeHandle handle = toio.Handle;
    Movement rotate = handle.RotateByDeg(deg, speed);
    handle.Update();
    handle.Move(rotate, false);
  }
}

public class RadRotateCommand : IMovementCommand
{
  double rad;
  double speed;
  private CubeHandle handle;
  private const int MAX_DURATION_MS = 2550;
  private const int MAX_SPEED = 100;

  // 移動命令 制限
  // https://github.com/morikatron/toio-sdk-for-unity/blob/main/docs/tutorials_basic.md#3-%E7%A7%BB%E5%8B%95%E3%81%99%E3%82%8B

  public RadRotateCommand(CubeHandle _handle, double _rad, double _speed)
  {
    handle = _handle;
    rad = _rad;
    speed = Math.Clamp(_speed, -MAX_SPEED, MAX_SPEED);
  }

  public float GetInterval()
  {
    double angularVelocity = Math.Abs(speed * CubeHandle.VDotOverU / CubeHandle.TireWidthDot);
    return (float)(Math.Abs(rad) / angularVelocity);
  }

  public void Exec(Toio toio)
  {
    CubeHandle handle = toio.Handle;
    Movement radRotate = handle.RotateByRad(rad, speed);
    handle.Update();
    handle.Move(radRotate, false);
  }
}
