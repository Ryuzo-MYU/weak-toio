using System.Collections.Generic;
using toio;

namespace Robot
{
	/// <summary>
	/// 複数のモーションを組み合わせた、意味のあるアクションを提供するライブラリ
	/// ActionGeneratorを継承したクラスから利用することを想定
	/// </summary>
	public class ToioActionLibrary
	{
		// ==============================
		// 基本モーション
		// ==============================

		/// <summary>
		/// 前後移動のMosionを返す
		/// </summary>
		/// <param name="_dist">距離</param>
		/// <param name="_speed">速度</param>
		/// <returns>Motion</returns>
		public static Motion Translate(float _dist, double _speed)
		{
			IToioCommand translate = new TranslateCommand(_dist, _speed);
			float interval = (float)_dist / (float)_speed;
			return new Motion(translate, interval);
		}

		/// <summary>
		/// 回転移動のMotionを返す
		/// </summary>
		/// <param name="_deg">角度(弧度法)</param>
		/// <param name="_speed">速度</param>
		/// <returns>Motion</returns>
		public static Motion DegRotate(float _deg, double _speed)
		{
			IToioCommand degRotate = new DegRotateCommand(_deg, _speed);
			float interval = (float)_deg / (float)_speed;
			return new Motion(degRotate, interval);
		}

		/// <summary>
		/// 回転移動のMotionを返す
		/// </summary>
		/// <param name="_rad">角度(ラジアン)</param>
		/// <param name="_speed">速度</param>
		/// <returns>Motion</returns>
		public static Motion RadRotate(float _rad, double _speed)
		{
			IToioCommand radRotate = new RadRotateCommand(_rad, _speed);
			float interval = (float)_rad / (float)_speed;
			return new Motion(radRotate, interval);
		}

		/// <summary>
		/// toioから任意の音を鳴らすMotionを返す
		/// あらかじめtoio-sdkのCube.SoundOperationインスタンスの配列を作って渡す
		/// </summary>
		/// <param name="_repeatCount">繰り返し回数</param>
		/// <param name="_sounds">Cube.SoundOperationインスタンスの配列</param>
		/// <returns>Motion</returns>
		public static Motion Sound(int _repeatCount, Cube.SoundOperation[] _sounds)
		{
			SoundCommand soundCommand = new SoundCommand(_repeatCount, _sounds);
			float interval = 0;
			foreach (var sound in _sounds)
			{
				interval += sound.durationMs;
			}
			return new Motion(soundCommand, interval);
		}

		/// <summary>
		/// toioに登録されているSEを鳴らすMotionを返す
		/// </summary>
		/// <param name="_soundId">SEのID。0～10まで</param>
		/// <param name="_volume">音量</param>
		/// <returns>Motion</returns>
		public static Motion PresetSound(int _soundId, int _volume)
		{
			PresetSoundCommand presetSound = new PresetSoundCommand(_soundId, _volume);
			float interval = 0.5f; // サウンドごとの間隔を取得できないため、決め打ち。
			return new Motion(presetSound, interval);
		}

		/// <summary>
		/// toioのLEDを一定時間点灯するMotionを返す
		/// </summary>
		/// <param name="_red">R</param>
		/// <param name="_green">G</param>
		/// <param name="_blue">B</param>
		/// <param name="_durationMills">点灯時間</param>
		/// <returns>Motion</returns>
		public static Motion TurnOnLED(int _red, int _green, int _blue, int _durationMills)
		{
			TurnOnLEDCommand lEDCommand = new TurnOnLEDCommand(_red, _green, _blue, _durationMills);
			float interval = _durationMills;
			return new Motion(lEDCommand, interval);
		}

		/// <summary>
		/// LEDを任意の間隔で点灯するMotionを返す
		/// </summary>
		/// <param name="_repeatCount">繰り返し回数</param>
		/// <param name="_lightOperations">Cube.LightOperation。詳しくはtoio-sdkを見て</param>
		/// <returns>Motion</returns>
		public static Motion LEDBlink(int _repeatCount, Cube.LightOperation[] _lightOperations)
		{
			LEDBlinkCommand lEDBlink = new LEDBlinkCommand(_repeatCount, _lightOperations);
			float interval = 0;
			foreach (var operation in _lightOperations)
			{
				interval += operation.durationMs;
			}
			return new Motion(lEDBlink, interval);
		}

		// ==============================
		// 基本モーションを使ったアクションライブラリ
		// ==============================

		/// <summary>
		/// 複数のコマンドを細かく分割して交互実行するためのヘルパーメソッド
		/// </summary>
		private static Action CreateInterleavedAction(
			float duration,
			float speed,
			IToioCommand movement = null,
			(int r, int g, int b) led = default,
			int? soundId = null)
		{
			var motions = new Queue<Motion>();
			int segments = 30; // 分割数
			float segmentTime = duration / segments;
			int segmentMs = (int)(segmentTime * 500);

			for (int i = 0; i < segments; i++)
			{
				// 移動コマンドがある場合
				if (movement != null)
				{
					motions.Enqueue(new Motion(movement, segmentTime));
				}

				// LED点灯コマンドがある場合
				if (led != default)
				{
					motions.Enqueue(new Motion(
						new TurnOnLEDCommand(led.r, led.g, led.b, segmentMs),
						0));
				}

				// サウンドコマンドがある場合
				if (soundId.HasValue && i == 0) // サウンドは最初のセグメントでのみ実行
				{
					motions.Enqueue(new Motion(
						new PresetSoundCommand(soundId.Value, 255),
						0));
				}
			}

			return new Action(motions);
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

		#region 草のアクション（湿度）
		public static Action Grass_Wilting()
		{
			return CreateInterleavedAction(
				duration: 3.0f,
				speed: 20,
				movement: new DegRotateCommand(15, 20), // 弱々しく揺れる
				led: (255, 255, 0) // 枯れかけの黄色
			);
		}

		public static Action Grass_Refreshed()
		{
			return CreateInterleavedAction(
				duration: 3.0f,
				speed: 80,
				movement: new TranslateCommand(100, 80), // 勢いよく伸びる
				led: (0, 255, 0) // 鮮やかな緑
			);
		}

		public static Action Grass_Normal()
		{
			return CreateInterleavedAction(
				duration: 3.0f,
				speed: 40,
				movement: new DegRotateCommand(20, 40), // ゆったりと揺れる
				led: (0, 200, 0) // 落ち着いた緑
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

		public static Action Clothes_Normal()
		{
			return CreateInterleavedAction(
				duration: 3.0f,
				speed: 50,
				movement: new TranslateCommand(50, 50), // 標準的な動き
				led: (255, 255, 255) // 白色
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

		public static Action Human_Normal()
		{
			return CreateInterleavedAction(
				duration: 3.0f,
				speed: 50,
				movement: new TranslateCommand(80, 50), // 普通の動き
				led: (255, 255, 255), // 標準的な白色
				soundId: null
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

		public static Action PC_Normal()
		{
			return CreateInterleavedAction(
				duration: 3.0f,
				speed: 50,
				movement: new TranslateCommand(70, 50), // 通常の動き
				led: (0, 0, 255) // 標準的な青色
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