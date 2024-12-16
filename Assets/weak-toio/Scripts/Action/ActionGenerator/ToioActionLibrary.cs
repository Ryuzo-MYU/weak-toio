using toio;

namespace Robot
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
			float duration,
			float speed,
			IMovementCommand movement = null,
			(int r, int g, int b) led = default,
			int? soundId = null,
			float soundDuration = 0
			)
		{
			Action action = new Action();

			// 移動コマンドがある場合
			if (movement != null)
			{
				action.AddMovement(movement);
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
				ISoundCommand soundCommand = PresetSound(soundId.Value, 255, soundDuration);
				action.AddSound(soundCommand);
			}

			return action;
		}

		#region 猫のアクション（気温）
		public static Action Cat_Cold()
		{
			Action action = new Action();
			action.AddMovement(DegRotate(10, 100));
			action.AddLight(TurnOnLED(0, 0, 255, 3000));
			action.AddSound(PresetSound(0, 1, 2f));
			return action;
		}

		public static Action Cat_Hot()
		{
			Action action = new Action();
			action.AddMovement(Translate(50, 20));
			action.AddLight(TurnOnLED(255, 0, 0, 3000));
			action.AddSound(PresetSound(2, 50, 2));
			return action;
		}

		public static Action Cat_Comfortable()
		{
			   Action action = new Action();
    action.AddMovement(Translate(100, 40));  // ゆったりとした動き
    action.AddLight(TurnOnLED(255, 200, 0, 3000));  // 温かみのある黄色
    action.AddSound(PresetSound(0, 255, 1f));  // 満足げな鳴き声
    return action;
		}
		#endregion

		public static Action Grass_Refreshed()
		{
    Action action = new Action();
    action.AddMovement(Translate(100, 80));  // 勢いよく伸びる
    action.AddLight(TurnOnLED(0, 255, 0, 3000));  // 鮮やかな緑
    return action;
		}

		#region  草(湿度)
		public static Action Grass_Normal()
		{
			   Action action = new Action();
    action.AddMovement(DegRotate(20, 40));  // ゆったりと揺れる
    action.AddLight(TurnOnLED(0, 200, 0, 3000));  // 落ち着いた緑
    return action;
		}
		public static Action Grass_Wilting()
		{
		   Action action = new Action();
    action.AddMovement(DegRotate(15, 20));  // 弱々しく揺れる
    action.AddLight(TurnOnLED(255, 255, 0, 3000));  // 枯れかけの黄色
    return action;
		}


		#endregion

		#region 服のアクション（湿度）
		public static Action Clothes_HighHumidity()
		{
	    Action action = new Action();
    action.AddMovement(Translate(30, 30));  // もたついた動き
    action.AddLight(TurnOnLED(100, 100, 100, 3000));  // くすんだ色
    action.AddSound(PresetSound(3, 255, 2f));  // 不快な音
    return action;
		}

		public static Action Clothes_Optimal()
		{
		    Action action = new Action();
    action.AddMovement(Translate(100, 60));  // なめらかな動き
    action.AddLight(TurnOnLED(100, 200, 255, 3000));  // 爽やかな青
    action.AddSound(PresetSound(0, 255, 2f));  // 清々しい音
    return action;
		}
		#endregion

		#region 人のアクション（二酸化炭素）

		public static Action Human_Normal()
		{
			return CreateInterleavedAction(
				duration: 3.0f,
				speed: 50,
				movement: Translate(60, 50),
				led: (0, 120, 0) // 目立ちすぎない緑
			);
		}
		public static Action Human_HighCO2()
		{
			return CreateInterleavedAction(
				duration: 3.0f,
				speed: 30,
				movement: Translate(30, 30), // 不安定な動き
				led: (150, 0, 150), // くすんだ紫
				soundId: 4, // 苦しげな音
				soundDuration: 2f
			);
		}

		public static Action Human_FreshAir()
		{
			return CreateInterleavedAction(
				duration: 3.0f,
				speed: 80,
				movement: Translate(100, 80), // 活発な動き
				led: (100, 255, 255), // 爽やかな水色
				soundId: 0, // 元気な音
				soundDuration: 2f
			);
		}
		#endregion

		#region PCのアクション（気温・湿度）

		public static Action PC_Normal()
		{
			return CreateInterleavedAction(
				duration: 3.0f,
				speed: 50,
				movement: Translate(60, 100), // 速くも遅くもない動き
				led: (0, 150, 0) // 正常な緑
			);
		}

		public static Action PC_Optimal()
		{
			return CreateInterleavedAction(
				duration: 3.0f,
				speed: 100,
				movement: Translate(100, 100), // 効率的な動き
				led: (0, 150, 255), // クリーンな青色
				soundId: 0, // 快適な動作音
				soundDuration: 2f
			);
		}

		public static Action PC_Uncomfortable()
		{
			return CreateInterleavedAction(
				duration: 3.0f,
				speed: 100,
				movement: DegRotate(45, 100), // 異常な動き
				led: (255, 0, 0), // 警告の赤色
				soundId: 5, // 異常音
				soundDuration: 2f
			);
		}
		#endregion
	}
}