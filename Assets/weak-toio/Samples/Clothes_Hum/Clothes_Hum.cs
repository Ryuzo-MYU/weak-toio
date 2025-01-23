using ActionGenerate;

namespace ActionLibrary
{
  public static class Clothes_Hum
  {
    public static Action Clothes_Hum_Good(this ToioActionLibrary lib)
    {
      Action action = new Action();
      IMovementCommand movement = lib.Translate(100, 60);
      IMovementCommand rotate = lib.DegRotate(360*2, 2);

      return action;
    }

    public static Action Clothes_Hum_Normal(this ToioActionLibrary lib)
    {
      Action action = new Action();
	
      return action;
    }

    public static Action Clothes_Hum_Bad(this ToioActionLibrary lib)
    {
      Action action = new Action();
      IMovementCommand movement = lib.Translate(30, 30);

      return action;
    }
  }
}