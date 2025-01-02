using toio;
using UnityEngine;

namespace ActionGenerate
{
	public class ToioActionLibrary
	{
		// ==============================
		// 基本モーション生成メソッド
		// ==============================

		/// <summary>
		/// 前後移動のActionを生成
		/// </summary>
		public static IMovementCommand Translate(float _dist, double _speed)
		{
			IMovementCommand translate = new TranslateCommand(_dist, _speed);
			return translate;
		}

		/// <summary>
		/// 回転移動のActionを生成 (弧度法)
		/// </summary>
		public static IMovementCommand DegRotate(float _deg, double _speed)
		{
			IMovementCommand degRotate = new DegRotateCommand(_deg, _speed);
			return degRotate;
		}

		/// <summary>
		/// 回転移動のActionを生成 (ラジアン)
		/// </summary>
		public static IMovementCommand RadRotate(float _rad, double _speed)
		{
			IMovementCommand radRotate = new RadRotateCommand(_rad, _speed);
			return radRotate;
		}

		/// <summary>
		/// toioから任意の音を鳴らすActionを生成
		/// </summary>
		public static ISoundCommand Sound(int _repeatCount, Cube.SoundOperation[] _sounds)
		{
			ISoundCommand soundCommand = new SoundCommand(_repeatCount, _sounds);
			float interval = 0;
			foreach (var sound in _sounds)
			{
				interval += sound.durationMs;
			}
			return soundCommand;
		}

		/// <summary>
		/// toioに登録されているSEを鳴らすActionを生成
		/// </summary>
		public static ISoundCommand PresetSound(int _soundId, int _volume, float interval)
		{
			if (interval > 3000)
			{
				interval = 3000;
			}
			ISoundCommand presetSound = new PresetSoundCommand(_soundId, _volume, interval);
			return presetSound;
		}

		/// <summary>
		/// toioのLEDを一定時間点灯するActionを生成
		/// </summary>
		public static ILightCommand TurnOnLED(int _red, int _green, int _blue, int _durationMills)
		{
			ILightCommand lEDCommand = new TurnOnLEDCommand(_red, _green, _blue, _durationMills);
			return lEDCommand;
		}

		/// <summary>
		/// toioのLEDを消すActionを生成
		/// </summary>
		/// <returns></returns>
		public static ILightCommand TurnOffLED()
		{
			ILightCommand turnOffCommand = new TurnOffLEDCommand();
			return turnOffCommand;
		}

		/// <summary>
		/// LEDを任意の間隔で点灯するActionを生成
		/// </summary>
		public static ILightCommand LEDBlink(int _repeatCount, Cube.LightOperation[] _lightOperations)
		{
			ILightCommand lEDBlink = new LEDBlinkCommand(_repeatCount, _lightOperations);
			float interval = 0;
			foreach (var operation in _lightOperations)
			{
				interval += operation.durationMs;
			}
			return lEDBlink;
		}

		// ==============================
		// 複合アクション生成
		// ==============================

		private static Action CreateInterleavedAction(
			float speed,
			IMovementCommand movement = null,
			(int r, int g, int b) led = default,
			int? soundId = null
			)
		{
			Action action = new Action();
			float duration = 0;

			// 移動コマンドがある場合
			if (movement != null)
			{
				action.AddMovement(movement);
				duration = movement.GetInterval();
			}

			// LED点灯コマンドがある場合
			if (led != default)
			{
				int durationMs = (int)(duration * 1000);
				ILightCommand ledCommand = TurnOnLED(led.r, led.g, led.b, durationMs);
				action.AddLight(ledCommand);
			}

			// サウンドコマンドがある場合
			if (soundId.HasValue)
			{
				ISoundCommand soundCommand = PresetSound(soundId.Value, 20, duration);
				action.AddSound(soundCommand);
			}

			return action;
		}

		#region 猫のアクション（気温）
		public static Action Cat_Cold()
		{
			IMovementCommand movement = DegRotate(10, 100);
			return CreateInterleavedAction(100, movement, (0, 0, 255), 0);
		}

		public static Action Cat_Hot()
		{
			IMovementCommand movement = Translate(50, 20);
			return CreateInterleavedAction(20, movement, (255, 0, 0), 2);
		}

		public static Action Cat_Comfortable()
		{
			IMovementCommand movement = Translate(100, 40);
			return CreateInterleavedAction(40, movement, (255, 200, 0), 0);
		}
		#endregion

		#region 草(湿度)
		public static Action Grass_Normal()
		{
			IMovementCommand movement = DegRotate(20, 40);
			return CreateInterleavedAction(40, movement, (0, 200, 0));
		}

		public static Action Grass_Wilting()
		{
			IMovementCommand movement = DegRotate(15, 20);
			return CreateInterleavedAction(20, movement, (255, 255, 0));
		}

		public static Action Grass_Refreshed()
		{
			IMovementCommand movement = Translate(100, 80);
			return CreateInterleavedAction(80, movement, (0, 255, 0));
		}
		#endregion

		#region 服のアクション（湿度）
		public static Action Clothes_HighHumidity()
		{
			IMovementCommand movement = Translate(30, 30);
			return CreateInterleavedAction(30, movement, (100, 100, 100), 3);
		}

		public static Action Clothes_Optimal()
		{
			IMovementCommand movement = Translate(100, 60);
			return CreateInterleavedAction(60, movement, (100, 200, 255), 0);
		}
		#endregion

		#region 人のアクション（二酸化炭素）
		public static Action Human_Normal()
		{
			IMovementCommand movement = Translate(60, 50);
			return CreateInterleavedAction(50, movement, (0, 120, 0));
		}

		public static Action Human_HighCO2()
		{
			IMovementCommand movement = Translate(30, 30);
			return CreateInterleavedAction(30, movement, (150, 0, 150), 4);
		}

		public static Action Human_FreshAir()
		{
			IMovementCommand movement = Translate(100, 80);
			return CreateInterleavedAction(80, movement, (100, 255, 255), 0);
		}
		#endregion

		#region PCのアクション（気温・湿度）
		public static Action PC_Normal()
		{
			IMovementCommand movement = Translate(60, 100);
			return CreateInterleavedAction(100, movement, (0, 150, 0));
		}

		public static Action PC_Optimal()
		{
			IMovementCommand movement = Translate(100, 100);
			return CreateInterleavedAction(100, movement, (0, 150, 255), 0);
		}

		public static Action PC_Uncomfortable()
		{
			IMovementCommand movement = DegRotate(45, 100);
			return CreateInterleavedAction(100, movement, (255, 0, 0), 5);
		}
		#endregion

		#region 人のアクション（気圧差）
		public static Action Human_NormalPressure()
		{
			IMovementCommand movement = Translate(60, 50);
			return CreateInterleavedAction(50, movement, (0, 120, 0));
		}

		public static Action Human_SensingPressure()
		{
			IMovementCommand movement = Translate(40, 40);
			return CreateInterleavedAction(40, movement, (255, 165, 0), 1);
		}

		public static Action Human_SufferingPressure()
		{
			IMovementCommand movement = Translate(20, 20);
			return CreateInterleavedAction(20, movement, (255, 0, 0), 5);
		}
		#endregion

		#region バナナのアクション（気温・湿度）
		public static Action Banana_Normal()
		{
			IMovementCommand movement = Translate(50, 40);
			return CreateInterleavedAction(40, movement, (255, 255, 0));
		}

		public static Action Banana_Warning()
		{
			IMovementCommand movement = Translate(80, 60);
			return CreateInterleavedAction(60, movement, (255, 200, 0), 1);
		}

		public static Action Banana_Rotting()
		{
			IMovementCommand movement = DegRotate(180, 100);
			return CreateInterleavedAction(100, movement, (139, 69, 19), 5);
		}
		#endregion

		#region 衝突回避アクション
		public static Action CollisionAvoidance()
		{
			Action action = new Action();

			// 1. 後退
			IMovementCommand movement1 = Translate(-50, 40);
			action.AddMovement(movement1);
			float duration1 = movement1.GetInterval();
			// 2. 回転 (ランダムな角度)
			float rotationAngle = Random.Range(30f, 150f);
			IMovementCommand movement2 = DegRotate(rotationAngle, 30);
			action.AddMovement(movement2);
			float duration2 = movement2.GetInterval();
			// 3. 前進
			IMovementCommand movement3 = Translate(100, 50);
			action.AddMovement(movement3);
			float duration3 = movement3.GetInterval();
			float totalDuration = duration1 + duration2 + duration3;

			// LEDとサウンドのコマンドを追加
			ILightCommand ledCommand = TurnOnLED(255, 255, 255, (int)(totalDuration * 1000));
			action.AddLight(ledCommand);
			ISoundCommand soundCommand = PresetSound(0, 20, totalDuration);
			action.AddSound(soundCommand);

			return action;
		}
		#endregion

		#region MOC用 Lチカアクション
		public static Action MocLedR()
		{
			Action action = new Action();
			ILightCommand onRed = TurnOnLED(255, 0, 0, 1000);
			action.AddLight(onRed);
			ILightCommand off = TurnOffLED();
			action.AddLight(off);
			return action;
		}

		public static Action MocLedB()
		{
			Action action = new Action();
			ILightCommand onBlue = TurnOnLED(0, 0, 255, 1000);
			action.AddLight(onBlue);
			ILightCommand off = TurnOffLED();
			action.AddLight(off);
			return action;
		}

		public static Action MocLedG()
		{
			Action action = new Action();
			ILightCommand onGreen = TurnOnLED(0, 255, 0, 1000);
			action.AddLight(onGreen);
			ILightCommand off = TurnOffLED();
			action.AddLight(off);
			return action;
		}

		public static Action MocLedY()
		{
			Action action = new Action();
			ILightCommand onYellow = TurnOnLED(255, 255, 0, 1000);
			action.AddLight(onYellow);
			ILightCommand off = TurnOffLED();
			action.AddLight(off);
			return action;
		}

		public static Action MocLedP()
		{
			Action action = new Action();
			ILightCommand onPurple = TurnOnLED(255, 0, 255, 1000);
			action.AddLight(onPurple);
			ILightCommand off = TurnOffLED();
			action.AddLight(off);
			return action;
		}
		#endregion
	}
}