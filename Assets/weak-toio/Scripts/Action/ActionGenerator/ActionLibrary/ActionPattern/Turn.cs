using System;
using ActionGenerate;
using Action = ActionGenerate.Action;

namespace ActionLibrary
{
  public static class TurnImplement
  {
    public static Action Turn(this ToioActionLibrary lib, float turnCount, double speed)
    {
      Action action = new Action();
      action.AddMovement(lib.RadRotate(2 * Math.PI * turnCount, speed));

      return action;
    }
  }
}