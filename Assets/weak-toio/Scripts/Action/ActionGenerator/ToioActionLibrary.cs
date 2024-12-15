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

		#region 猫のアクション（気温）
		/// <summary>
		/// 寒さで震える猫の様子を表現
		/// 小刻みな左右の揺れと青いLEDで寒さを表現
		/// </summary>
		public static Action Cat_Cold()
		{
			var motions = new Queue<Motion>();

			// 青色LEDで寒さを表現
			motions.Enqueue(TurnOnLED(0, 0, 255, 3000));

			// 小刻みに震える動作を繰り返す
			for (int i = 0; i < 6; i++)
			{
				motions.Enqueue(DegRotate(10, 100));
				motions.Enqueue(DegRotate(-10, 100));
			}

			// 寒そうな鳴き声
			motions.Enqueue(PresetSound(1, 255));

			return new Action(motions);
		}

		/// <summary>
		/// 暑さに不快感を示す猫の様子を表現
		/// ゆっくりとした動きと赤いLEDで暑さを表現
		/// </summary>
		public static Action Cat_Hot()
		{
			var motions = new Queue<Motion>();

			// 赤色LEDで暑さを表現
			motions.Enqueue(TurnOnLED(255, 0, 0, 3000));

			// だるそうにゆっくり動く
			motions.Enqueue(Translate(50, 20));
			motions.Enqueue(DegRotate(45, 20));
			motions.Enqueue(Translate(-50, 20));

			// 不快な鳴き声
			motions.Enqueue(PresetSound(2, 200));

			return new Action(motions);
		}

		/// <summary>
		/// 快適な温度でくつろぐ猫の様子を表現
		/// なめらかな動きと温かみのある色で表現
		/// </summary>
		public static Action Cat_Comfortable()
		{
			var motions = new Queue<Motion>();

			// 温かみのある黄色でLED表現
			motions.Enqueue(TurnOnLED(255, 200, 0, 3000));

			// ゆったりとした動き
			motions.Enqueue(Translate(100, 40));
			motions.Enqueue(DegRotate(180, 30));
			motions.Enqueue(Translate(100, 40));

			// 満足げな鳴き声
			motions.Enqueue(PresetSound(0, 200));

			return new Action(motions);
		}
		#endregion

		#region 草のアクション（湿度）
		/// <summary>
		/// 水不足でしなびている草の様子を表現
		/// 不安定な揺れと黄色いLEDで表現
		/// </summary>
		public static Action Grass_Wilting()
		{
			var motions = new Queue<Motion>();

			// 枯れかけの黄色でLED表現
			motions.Enqueue(TurnOnLED(255, 255, 0, 3000));

			// 弱々しく揺れる動き
			for (int i = 0; i < 4; i++)
			{
				motions.Enqueue(DegRotate(15, 20));
				motions.Enqueue(DegRotate(-15, 20));
			}

			return new Action(motions);
		}

		/// <summary>
		/// 水を得て元気になった草の様子を表現
		/// 勢いのある動きと鮮やかな緑のLEDで表現
		/// </summary>
		public static Action Grass_Refreshed()
		{
			var motions = new Queue<Motion>();

			// 鮮やかな緑でLED表現
			motions.Enqueue(TurnOnLED(0, 255, 0, 3000));

			// 勢いよく伸びる動き
			motions.Enqueue(Translate(100, 80));
			for (int i = 0; i < 3; i++)
			{
				motions.Enqueue(DegRotate(30, 60));
				motions.Enqueue(DegRotate(-30, 60));
			}

			return new Action(motions);
		}

		/// <summary>
		/// 通常状態の草の様子を表現
		/// 穏やかな揺れと緑のLEDで表現
		/// </summary>
		public static Action Grass_Normal()
		{
			var motions = new Queue<Motion>();

			// 落ち着いた緑でLED表現
			motions.Enqueue(TurnOnLED(0, 200, 0, 3000));

			// ゆったりと揺れる動き
			for (int i = 0; i < 3; i++)
			{
				motions.Enqueue(DegRotate(20, 40));
				motions.Enqueue(DegRotate(-20, 40));
			}

			return new Action(motions);
		}
		#endregion

		#region 服のアクション（湿度）
		/// <summary>
		/// 湿度が高くカビそうな服の様子を表現
		/// 不規則な動きと暗い色で表現
		/// </summary>
		public static Action Clothes_HighHumidity()
		{
			var motions = new Queue<Motion>();

			// くすんだ色でLED表現
			motions.Enqueue(TurnOnLED(100, 100, 100, 3000));

			// もたついた動き
			for (int i = 0; i < 4; i++)
			{
				motions.Enqueue(Translate(30, 30));
				motions.Enqueue(DegRotate(45, 40));
				motions.Enqueue(Translate(-30, 30));
			}

			// 不快な音
			motions.Enqueue(PresetSound(3, 200));

			return new Action(motions);
		}

		/// <summary>
		/// 最適な状態の服の様子を表現
		/// スムーズな動きと爽やかな色で表現
		/// </summary>
		public static Action Clothes_Optimal()
		{
			var motions = new Queue<Motion>();

			// 爽やかな青でLED表現
			motions.Enqueue(TurnOnLED(100, 200, 255, 3000));

			// なめらかな動き
			motions.Enqueue(Translate(100, 60));
			motions.Enqueue(DegRotate(360, 50));

			// 清々しい音
			motions.Enqueue(PresetSound(0, 200));

			return new Action(motions);
		}

		/// <summary>
		/// 通常状態の服の様子を表現
		/// 普通の動きと白色LEDで表現
		/// </summary>
		public static Action Clothes_Normal()
		{
			var motions = new Queue<Motion>();

			// 白色でLED表現
			motions.Enqueue(TurnOnLED(255, 255, 255, 3000));

			// 標準的な動き
			motions.Enqueue(Translate(50, 50));
			motions.Enqueue(DegRotate(180, 50));

			return new Action(motions);
		}
		#endregion

		#region 人のアクション（二酸化炭素）
		/// <summary>
		/// 空気が籠もって苦しい様子を表現
		/// 不安定な動きと暗い色で表現
		/// </summary>
		public static Action Human_HighCO2()
		{
			var motions = new Queue<Motion>();

			// くすんだ紫でLED表現
			motions.Enqueue(TurnOnLED(150, 0, 150, 3000));

			// 不安定な動き
			for (int i = 0; i < 3; i++)
			{
				motions.Enqueue(Translate(30, 30));
				motions.Enqueue(DegRotate(60, 30));
				motions.Enqueue(Translate(-30, 30));
			}

			// 苦しげな音
			motions.Enqueue(PresetSound(4, 255));

			return new Action(motions);
		}

		/// <summary>
		/// 通常の空気環境での様子を表現
		/// 標準的な動きで表現
		/// </summary>
		public static Action Human_Normal()
		{
			var motions = new Queue<Motion>();

			// 標準的な白色でLED表現
			motions.Enqueue(TurnOnLED(255, 255, 255, 3000));

			// 普通の動き
			motions.Enqueue(Translate(80, 50));
			motions.Enqueue(DegRotate(90, 50));
			motions.Enqueue(Translate(80, 50));

			return new Action(motions);
		}

		/// <summary>
		/// 空気が清浄で快適な様子を表現
		/// 活発な動きと爽やかな色で表現
		/// </summary>
		public static Action Human_FreshAir()
		{
			var motions = new Queue<Motion>();

			// 爽やかな水色でLED表現
			motions.Enqueue(TurnOnLED(100, 255, 255, 3000));

			// 活発な動き
			for (int i = 0; i < 2; i++)
			{
				motions.Enqueue(Translate(100, 80));
				motions.Enqueue(DegRotate(360, 70));
			}

			// 元気な音
			motions.Enqueue(PresetSound(0, 255));

			return new Action(motions);
		}
		#endregion

		#region PCのアクション（気温・湿度）
		/// <summary>
		/// 最適な環境でのPCの様子を表現
		/// スムーズで効率的な動きで表現
		/// </summary>
		public static Action PC_Optimal()
		{
			var motions = new Queue<Motion>();

			// クリーンな青色でLED表現
			motions.Enqueue(TurnOnLED(0, 150, 255, 3000));

			// 効率的な動き
			motions.Enqueue(Translate(100, 100));
			motions.Enqueue(DegRotate(90, 100));
			motions.Enqueue(Translate(100, 100));
			motions.Enqueue(DegRotate(90, 100));

			// 快適な動作音
			motions.Enqueue(PresetSound(0, 200));

			return new Action(motions);
		}

		/// <summary>
		/// 通常状態のPCの様子を表現
		/// 標準的な動きで表現
		/// </summary>
		public static Action PC_Normal()
		{
			var motions = new Queue<Motion>();

			// 標準的な青色でLED表現
			motions.Enqueue(TurnOnLED(0, 0, 255, 3000));

			// 通常の動き
			motions.Enqueue(Translate(70, 50));
			motions.Enqueue(DegRotate(90, 50));
			motions.Enqueue(Translate(70, 50));

			return new Action(motions);
		}

		/// <summary>
		/// 不快な環境でのPCの様子を表現
		/// 異常な動きと警告色で表現
		/// </summary>
		public static Action PC_Uncomfortable()
		{
			var motions = new Queue<Motion>();

			// 警告の赤色でLED表現
			motions.Enqueue(TurnOnLED(255, 0, 0, 3000));

			// 異常な動き
			for (int i = 0; i < 4; i++)
			{
				motions.Enqueue(DegRotate(45, 100));
				motions.Enqueue(DegRotate(-45, 100));
				// 異常音
				motions.Enqueue(PresetSound(5, 255));
			}

			return new Action(motions);
		}
		#endregion
	}
}