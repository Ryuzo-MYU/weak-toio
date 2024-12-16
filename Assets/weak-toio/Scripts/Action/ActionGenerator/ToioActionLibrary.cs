using System.Collections.Generic;
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
		public static Action Translate(float _dist, double _speed)
		{
			Action action = new Action();
			IMovementCommand translate = new TranslateCommand(_dist, _speed);
			action.AddMovement(translate);
			return action;
		}

		/// <summary>
		/// 回転移動のActionを生成 (弧度法)
		/// </summary>
		public static Action DegRotate(float _deg, double _speed)
		{
			Action action = new Action();
			IMovementCommand degRotate = new DegRotateCommand(_deg, _speed);
			action.AddMovement(degRotate);
			return action;
		}

		/// <summary>
		/// 回転移動のActionを生成 (ラジアン)
		/// </summary>
		public static Action RadRotate(float _rad, double _speed)
		{
			Action action = new Action();
			IMovementCommand radRotate = new RadRotateCommand(_rad, _speed);
			action.AddMovement(radRotate);
			return action;
		}

		/// <summary>
		/// toioから任意の音を鳴らすActionを生成
		/// </summary>
		public static Action Sound(int _repeatCount, Cube.SoundOperation[] _sounds)
		{
			Action action = new Action();
			ISoundCommand soundCommand = new SoundCommand(_repeatCount, _sounds);
			float interval = 0;
			foreach (var sound in _sounds)
			{
				interval += sound.durationMs;
			}
			action.AddSound(soundCommand);
			return action;
		}

		/// <summary>
		/// toioに登録されているSEを鳴らすActionを生成
		/// </summary>
		public static Action PresetSound(int _soundId, int _volume)
		{
			Action action = new Action();
			ISoundCommand presetSound = new PresetSoundCommand(_soundId, _volume);
			action.AddSound(presetSound);
			return action;
		}

		/// <summary>
		/// toioのLEDを一定時間点灯するActionを生成
		/// </summary>
		public static Action TurnOnLED(int _red, int _green, int _blue, int _durationMills)
		{
			Action action = new Action();
			ILightCommand lEDCommand = new TurnOnLEDCommand(_red, _green, _blue, _durationMills);
			action.AddLight(lEDCommand, _durationMills);
			return action;
		}

		/// <summary>
		/// LEDを任意の間隔で点灯するActionを生成
		/// </summary>
		public static Action LEDBlink(int _repeatCount, Cube.LightOperation[] _lightOperations)
		{
			Action action = new Action();
			ILightCommand lEDBlink = new LEDBlinkCommand(_repeatCount, _lightOperations);
			float interval = 0;
			foreach (var operation in _lightOperations)
			{
				interval += operation.durationMs;
			}
			action.AddLight(lEDBlink, interval);
			return action;
		}

		// ==============================
		// 複合アクション生成
		// ==============================

		private static Action CreateInterleavedAction(
			float duration,
			float speed,
			IMovementCommand movement = null,
			(int r, int g, int b) led = default,
			int? soundId = null)
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
				ILightCommand ledCommand = new TurnOnLEDCommand(led.r, led.g, led.b, durationMs);
				action.AddLight(ledCommand, duration);
			}

			// サウンドコマンドがある場合
			if (soundId.HasValue)
			{
				ISoundCommand soundCommand = new PresetSoundCommand(soundId.Value, 255);
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
				movement: new TranslateCommand(50, 20), // だるそうな動き
				led: (255, 0, 0), // 赤色LED
				soundId: 2 // 不快な鳴き声
			);
		}

		public static Action Cat_Comfortable()
		{
			return CreateInterleavedAction(
				duration: 3.0f,
				speed: 40,
				movement: new TranslateCommand(100, 40), // ゆったりとした動き
				led: (255, 200, 0), // 温かみのある黄色
				soundId: 0 // 満足げな鳴き声
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
				movement: new TranslateCommand(100, 80), // 勢いよく伸びる
				led: (0, 255, 0) // 鮮やかな緑
			);
		}

		#region  草(湿度)
		public static Action Grass_Normal()
		{
			return CreateInterleavedAction(
				duration: 3.0f,
				speed: 40,
				movement: new DegRotateCommand(20, 40), // ゆったりと揺れる
				led: (0, 200, 0) // 落ち着いた緑
			);
		}
		public static Action Grass_Wilting()
		{
			return CreateInterleavedAction(
				duration: 3.0f,
				speed: 20,
				movement: new DegRotateCommand(15, 20), // 弱々しく揺れる
				led: (255, 255, 0) // 枯れかけの黄色
			);
		}

		public static Action Grass_Wilting()
		{
			return CreateInterleavedAction(
				duration: 3.0f,
				speed: 20,
				movement: new DegRotateCommand(15, 20), // 弱々しく揺れる
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
				movement: new TranslateCommand(30, 30), // もたついた動き
				led: (100, 100, 100), // くすんだ色
				soundId: 3 // 不快な音
			);
		}

		public static Action Clothes_Optimal()
		{
			return CreateInterleavedAction(
				duration: 3.0f,
				speed: 60,
				movement: new TranslateCommand(100, 60), // なめらかな動き
				led: (100, 200, 255), // 爽やかな青
				soundId: 0 // 清々しい音
			);
		}
		#endregion

		#region 人のアクション（二酸化炭素）
		public static Action Human_HighCO2()
		{
			return CreateInterleavedAction(
				duration: 3.0f,
				speed: 30,
				movement: new TranslateCommand(30, 30), // 不安定な動き
				led: (150, 0, 150), // くすんだ紫
				soundId: 4 // 苦しげな音
			);
		}

		public static Action Human_FreshAir()
		{
			return CreateInterleavedAction(
				duration: 3.0f,
				speed: 80,
				movement: new TranslateCommand(100, 80), // 活発な動き
				led: (100, 255, 255), // 爽やかな水色
				soundId: 0 // 元気な音
			);
		}
		#endregion

		#region PCのアクション（気温・湿度）
		public static Action PC_Optimal()
		{
			return CreateInterleavedAction(
				duration: 3.0f,
				speed: 100,
				movement: new TranslateCommand(100, 100), // 効率的な動き
				led: (0, 150, 255), // クリーンな青色
				soundId: 0 // 快適な動作音
			);
		}

		public static Action PC_Uncomfortable()
		{
			return CreateInterleavedAction(
				duration: 3.0f,
				speed: 100,
				movement: new DegRotateCommand(45, 100), // 異常な動き
				led: (255, 0, 0), // 警告の赤色
				soundId: 5 // 異常音
			);
		}
		#endregion
	}
}