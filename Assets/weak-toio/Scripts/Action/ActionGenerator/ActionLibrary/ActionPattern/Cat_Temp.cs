using System;
using Action = ActionGenerate.Action;

namespace ActionLibrary
{
  public static class Cat_TempImplementation
  {
    public static Action Cat_Temp_Cold(this ToioActionLibrary library)
    {
      Action action = new Action();
      action.AddMovement(library.RadRotate(Math.PI / 6, 50));
      action.AddMovement(library.RadRotate(-Math.PI / 6, 50));
      action *= 6;

      action.AddLight(library.TurnOnLED(200, 200, 255, 1000));
      return action;
    }

    public static Action Cat_Temp_Hot(this ToioActionLibrary library)
    {
      Action action = new Action();
      action.AddMovement(library.Translate(50, 100));
      action.AddMovement(library.DegRotate(90, 50));
      action.AddMovement(library.Translate(50, 100));
      action.AddMovement(library.DegRotate(-90, 50));
      action.AddMovement(library.Translate(50, 100));
      action.AddMovement(library.DegRotate(90, 50));
      action.AddMovement(library.Translate(50, 100));
      action.AddMovement(library.DegRotate(-90, 50));

      action.AddLight(library.TurnOnLED(255, 100, 100, 3000));
      return action;
    }

    public static Action Cat_Temp_Comfortable(this ToioActionLibrary library)
    {
      Action action = new Action();

      action.AddLight(library.TurnOnLED(0, 255, 0, 5000));

      return action;
    }

    public static Action Cat_Temp_Normal(this ToioActionLibrary library)
    {
      Action action = new Action();
      action.AddMovement(library.Translate(30, 80));
      action.AddMovement(library.DegRotate(45, 40));
      action.AddMovement(library.Translate(30, 80));
      action.AddMovement(library.DegRotate(-45, 40));

      action.AddLight(library.TurnOnLED(150, 150, 150, 3000));
      return action;
    }
  }
}