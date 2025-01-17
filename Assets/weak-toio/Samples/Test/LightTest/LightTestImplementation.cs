using System;
using ActionGenerate;
using Action = ActionGenerate.Action;

namespace ActionLibrary
{
  public static class LightTestImplementation
  {
    public static Action LightTest(this ToioActionLibrary lib)
    {
      Action action = new Action();

      ILightCommand[] lights = {
        lib.TurnOnLED(255, 0, 0, 1000),
        lib.TurnOnLED(0, 255, 0, 1000),
        // lib.TurnOffLED(),
        // lib.TurnOffLED(),
        lib.TurnOnLED(0, 0, 255, 2550)
      };

      foreach (ILightCommand light in lights)
      {
        action.AddLight(light);
      }
      action.AddMovement(lib.RadRotate(Math.PI / 2, 50));
      action.AddMovement(lib.Translate(50, 50));

      return action;
    }
  }
}