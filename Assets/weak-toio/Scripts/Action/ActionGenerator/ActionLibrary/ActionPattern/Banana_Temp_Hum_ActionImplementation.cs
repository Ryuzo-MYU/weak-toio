using ActionGenerate;

namespace ActionLibrary
{
  public static class Banana_Temp_Hum_ActionImplementation
  {
    public static Action Banana_Temp_Hum_Normal(this ToioActionLibrary library)
    {
      Action action = new Action();
      action.AddMovement(library.Translate(50, 40));
      action.AddLight(library.TurnOnLED(255, 255, 0, 50 / 40));
      return action;
    }

    public static Action Banana_Temp_Hum_Warning(this ToioActionLibrary library)
    {
      Action action = new Action();
      IMovementCommand translate = library.Translate(80, 60);
      ILightCommand light = library.TurnOnLED(255, 200, 0, (int)translate.GetInterval());
      ISoundCommand sound = library.PresetSound(1, 50, 0.5f);

      action.AddMovement(translate);
      action.AddLight(light);
      action.AddSound(sound);

      return action;
    }

    public static Action Banana_Temp_Hum_Rotting(this ToioActionLibrary library)
    {
      Action action = new Action();
      IMovementCommand move = library.DegRotate(180, 100);
      action.AddLight(library.TurnOnLED(139, 69, 19, (int)move.GetInterval()));
      action.AddSound(library.PresetSound(5, 50, 0.5f));

      return action;
    }
  }
}