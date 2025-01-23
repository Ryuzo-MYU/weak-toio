using ActionGenerate;

namespace ActionLibrary
{
	public static class Clothes_Hum
	{
		public static Action Clothes_Hum_Good(this ToioActionLibrary lib)
		{
			Action action = new Action();
			IMovementCommand movement = lib.Translate(100, 60);
			IMovementCommand rotate = lib.DegRotate(360 * 2, 2);
			action.AddMovement(movement);
			action.AddMovement(rotate);
			return action;
		}

		public static Action Clothes_Hum_Normal(this ToioActionLibrary lib)
		{
			Action action = new Action();
			IMovementCommand movement = lib.Translate(60, 45);
			IMovementCommand rotate = lib.DegRotate(180, 1);
			action.AddMovement(movement);
			action.AddMovement(rotate);
			return action;
		}

		public static Action Clothes_Hum_Bad(this ToioActionLibrary lib)
		{
			Action action = new Action();
			IMovementCommand erraticMovement1 = lib.Translate(30, 20);
			IMovementCommand erraticRotate1 = lib.DegRotate(90, 0.5);
			IMovementCommand erraticMovement2 = lib.Translate(-30, 20);
			IMovementCommand erraticRotate2 = lib.DegRotate(-90, 0.5);
			IMovementCommand erraticMovement3 = lib.Translate(15, 10);
			IMovementCommand erraticRotate3 = lib.DegRotate(45, 0.25);

			action.AddMovement(erraticMovement1);
			action.AddMovement(erraticRotate1);
			action.AddMovement(erraticMovement2);
			action.AddMovement(erraticRotate2);
			action.AddMovement(erraticMovement3);
			action.AddMovement(erraticRotate3);

			return action;
		}
	}
}