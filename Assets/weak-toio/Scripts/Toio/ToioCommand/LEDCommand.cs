using System;
using ActionGenerate;
using toio;
using UnityEngine;

public class TurnOnLED : ILightCommand
{
  int red;
  int green;
  int blue;
  int durationMills;
  private const int MIN_DURATION = 10;
  private const int MAX_DURATION = 2550;
  public TurnOnLED(int _red, int _green, int _blue, int _durationMills)
  {
    red = Math.Clamp(_red, 0, 255);
    green = Math.Clamp(_green, 0, 255);
    blue = Math.Clamp(_blue, 0, 255);
    // LED点灯時間のバリデーション
    durationMills = Math.Clamp(_durationMills, MIN_DURATION, MAX_DURATION);

    if (_durationMills != durationMills)
    {
      Debug.LogWarning($"LED点灯時間は{MIN_DURATION}~{MAX_DURATION}の範囲である必要があります。{_durationMills}は{durationMills}に調整されました。");
    }
  }

  public float GetInterval()
  {
    return durationMills / 1000; //ミリ秒を秒に変換
  }

  public void Exec(Toio toio)
  {
    Cube cube = toio.Cube;
    cube.TurnLedOn(red, green, blue, durationMills);
  }
}
public class TurnOffLEDCommand : ILightCommand
{
  public float GetInterval()
  {
    return 0.01f;
  }
  public void Exec(Toio toio)
  {
    Cube cube = toio.Cube;
    cube.TurnLedOff();
  }
}
public class LEDBlinkCommand : ILightCommand
{
  int repeatCount;
  Cube.LightOperation[] lightOperations;
  public LEDBlinkCommand(int _repeatCount, Cube.LightOperation[] _lightOperations)
  {
    repeatCount = _repeatCount;
    lightOperations = _lightOperations;
  }

  public float GetInterval()
  {
    float interval = 0;
    foreach (var operation in lightOperations)
    {
      interval += operation.durationMs / 1000;
    }
    return interval;
  }
  public void Exec(Toio toio)
  {
    Cube cube = toio.Cube;
    cube.TurnOnLightWithScenario(repeatCount, lightOperations);
  }
}