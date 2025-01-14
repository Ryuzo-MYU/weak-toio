using ActionGenerate;

namespace ActionLibrary
{
  public static class MOC_LED
  {
    public static Action MocLedR(this ToioActionLibrary actionLibrary)
    {
      Action action = new Action();
      ILightCommand onRed = actionLibrary.TurnOnLED(255, 0, 0, 1000);
      action.AddLight(onRed);
      ILightCommand off = actionLibrary.TurnOffLED();
      action.AddLight(off);
      return action;
    }

    public static Action MocLedB(this ToioActionLibrary actionLibrary)
    {
      Action action = new Action();
      ILightCommand onBlue = actionLibrary.TurnOnLED(0, 0, 255, 1000);
      action.AddLight(onBlue);
      ILightCommand off = actionLibrary.TurnOffLED();
      action.AddLight(off);
      return action;
    }

    public static Action MocLedG(this ToioActionLibrary actionLibrary)
    {
      Action action = new Action();
      ILightCommand onGreen = actionLibrary.TurnOnLED(0, 255, 0, 1000);
      action.AddLight(onGreen);
      ILightCommand off = actionLibrary.TurnOffLED();
      action.AddLight(off);
      return action;
    }

    public static Action MocLedY(this ToioActionLibrary actionLibrary)
    {
      Action action = new Action();
      ILightCommand onYellow = actionLibrary.TurnOnLED(255, 255, 0, 1000);
      action.AddLight(onYellow);
      ILightCommand off = actionLibrary.TurnOffLED();
      action.AddLight(off);
      return action;
    }

    public static Action MocLedP(this ToioActionLibrary actionLibrary)
    {
      Action action = new Action();
      ILightCommand onPurple = actionLibrary.TurnOnLED(255, 0, 255, 1000);
      action.AddLight(onPurple);
      ILightCommand off = actionLibrary.TurnOffLED();
      action.AddLight(off);
      return action;
    }
  }
}