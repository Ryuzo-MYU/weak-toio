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
			return CreateInterleavedAction(
				duration: 3.0f,
				speed: 100,
				movement: new DegRotateCommand(10, 100), // 震える動作
				led: (0, 0, 255), // 青色LED
				soundId: 1 // 寒そうな鳴き声
			);
		}

		public static Action Cat_Hot()
		{
			return CreateInterleavedAction(
				duration: 3.0f,
				speed: 20,
				movement: Translate(50, 20), // だるそうな動き
				led: (255, 0, 0), // 赤色LED
				soundId: 2 // 不快な鳴き声
			);
		}

		public static Action Cat_Comfortable()
		{
			return CreateInterleavedAction(
				duration: 3.0f,
				speed: 40,
				movement: Translate(100, 40), // ゆったりとした動き
				led: (255, 200, 0), // 温かみのある黄色
				soundId: 0, // 満足げな鳴き声
				soundDuration: 1f
			);
		}
		#endregion


		/// <summary>
		/// 複数のアクションを結合するユーティリティメソッド
		/// </summary>
		public static Action CombineActions(params Action[] actions)
		{
			return CreateInterleavedAction(
				duration: 3.0f,
				speed: 80,
				movement: Translate(100, 80), // 勢いよく伸びる
				led: (0, 255, 0) // 鮮やかな緑
			);
		}

		#region  草(湿度)
		public static Action Grass_Normal()
		{
			return CreateInterleavedAction(
				duration: 3.0f,
				speed: 40,
				movement: DegRotate(20, 40), // ゆったりと揺れる
				led: (0, 200, 0) // 落ち着いた緑
			);
		}
		public static Action Grass_Wilting()
		{
			return CreateInterleavedAction(
				duration: 3.0f,
				speed: 20,
				movement: DegRotate(15, 20), // 弱々しく揺れる
				led: (255, 255, 0) // 枯れかけの黄色
			);
		}


		#endregion

		#region 服のアクション（湿度）
		public static Action Clothes_HighHumidity()
		{
			return CreateInterleavedAction(
				duration: 3.0f,
				speed: 30,
				movement: Translate(30, 30), // もたついた動き
				led: (100, 100, 100), // くすんだ色
				soundId: 3, // 不快な音
				soundDuration: 2f
			);
		}

		public static Action Clothes_Optimal()
		{
			return CreateInterleavedAction(
				duration: 3.0f,
				speed: 60,
				movement: Translate(100, 60), // なめらかな動き
				led: (100, 200, 255), // 爽やかな青
				soundId: 0, // 清々しい音
				soundDuration: 2f
			);
		}
		#endregion

		#region 人のアクション（二酸化炭素）
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