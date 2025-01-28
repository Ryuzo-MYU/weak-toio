using ActionLibrary;

namespace ActionGenerate
{
	public static class Human_Temp
	{
		public static Action Human_Temp_Suitable(this ToioActionLibrary library)
		{
			// あたりを歩き回る

			Action action = new Action();
			action.AddMovement(library.Translate(100, 50)); // 直線的な移動
			action.AddMovement(library.DegRotate(90, 50)); // 90度回転
			action.AddMovement(library.Translate(100, 50)); // 直線的な移動

			return action;
		}

		public static Action Human_Temp_Hot(this ToioActionLibrary library)
		{
			// 暑さに苛立って激しく動き回る

			Action action = new Action();
			action.AddMovement(library.Translate(50, 100)); // 激しく動き回る
			action.AddMovement(library.DegRotate(180, 100)); // 180度回転
			action.AddMovement(library.Translate(50, 100)); // 激しく動き回る
			action.AddSound(library.PresetSound(2, 100, 1.0f)); // 音を鳴らす

			return action;
		}

		public static Action Human_Temp_Cold(this ToioActionLibrary library)
		{
			// 時折蛇行しながら，寒さで震える

			Action action = new Action();
			action.AddMovement(library.Translate(30, 30)); // 小刻みに震える
			action.AddMovement(library.DegRotate(45, 30)); // 45度回転
			action.AddMovement(library.Translate(30, 30)); // 小刻みに震える
			action.AddLight(library.TurnOnLED(0, 0, 255, 500)); // 青色のLEDを点灯

			return action;
		}
	}
}