using ActionGenerate;

namespace ActionLibrary
{
  public static class Test_CO2
  {
    public static Action Test_CO2_Suitable(this ToioActionLibrary library)
    {
      Action action = new Action();
      float deg = 90;
      double speed = 45;
      action.AddMovement(library.DegRotate(deg, speed));
      action.AddMovement(library.DegRotate(-deg, speed));

      int soundID = 1;
      int volume = 1;
      action.AddSound(library.PresetSound(soundID, volume, 2f));

      return action;
    }

    public static Action Test_CO2_Caution(this ToioActionLibrary library)
    {
      Action action = new Action();
      float deg = 50f;
      double speed = 50;
      action.AddMovement(library.DegRotate(deg, speed));
      action.AddMovement(library.DegRotate(-deg, speed));

      return action;
    }

    public static Action Test_CO2_Danger(this ToioActionLibrary library)
    {
      Action action = new Action();
      float deg = 90f;
      double speed = 200;
      action.AddMovement(library.DegRotate(deg, speed));
      action.AddMovement(library.DegRotate(-deg, speed));

      return action;
    }
  }
}