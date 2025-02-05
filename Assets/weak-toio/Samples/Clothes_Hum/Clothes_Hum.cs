using ActionGenerate;

namespace ActionLibrary
{
	public static class Clothes_Hum
	{
		public static Action Clothes_Hum_Good(this ToioActionLibrary lib)
		{
			Action action = new Action();
			IMovementCommand movement = lib.Translate(100, 60);
			IMovementCommand rotate = lib.DegRotate(360 * 2, 50);
			action.AddMovement(movement);
			action.AddMovement(rotate);
			return action;
		}

		public static Action Clothes_Hum_Normal(this ToioActionLibrary lib)
		{
			Action action = new Action();
			IMovementCommand movement = lib.Translate(60, 45);
			IMovementCommand rotate = lib.DegRotate(180, 50);
			action.AddMovement(movement);
			action.AddMovement(rotate);
			return action;
		}

		public static Action Clothes_Hum_Bad(this ToioActionLibrary lib)
		{
			// 不規則な動作と音でユーザーの注意を引く
			// この時点でロボットはクローゼット内にいるので激しくアピールする

			Action action = new Action();
			IMovementCommand erraticMovement1 = lib.Translate(30, 60);
			IMovementCommand erraticRotate1 = lib.DegRotate(90, 60);
			IMovementCommand erraticMovement2 = lib.Translate(-30, 40);
			IMovementCommand erraticRotate2 = lib.DegRotate(-90, 40);
			IMovementCommand erraticMovement3 = lib.Translate(15, 40);
			IMovementCommand erraticRotate3 = lib.DegRotate(45, 60);

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