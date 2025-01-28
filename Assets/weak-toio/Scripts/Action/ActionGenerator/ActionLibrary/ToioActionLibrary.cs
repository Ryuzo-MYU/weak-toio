using toio;
using ActionGenerate;

namespace ActionLibrary
{
	/// <summary>
	/// toio用のアクションパターンを提供するクラス
	/// </summary>
	public class ToioActionLibrary
	{

		public ToioActionLibrary()
		{
		}

		// ==============================
		// 基本モーション生成メソッド
		// ==============================

		/// <summary>
		/// 前後移動のActionを生成
		/// </summary>
		public IMovementCommand Translate(float _dist, double _speed)
		{
			IMovementCommand translate = new TranslateCommand(_dist, _speed);
			return translate;
		}

		/// <summary>
		/// 回転移動のActionを生成 (弧度法)
		/// </summary>
		public IMovementCommand DegRotate(float _deg, double _speed)
		{
			IMovementCommand degRotate = new DegRotateCommand(_deg, _speed);
			return degRotate;
		}

		/// <summary>
		/// 回転移動のActionを生成 (ラジアン)
		/// </summary>
		public IMovementCommand RadRotate(double _rad, double _speed)
		{
			IMovementCommand radRotate = new RadRotateCommand(_rad, _speed);
			return radRotate;
		}

		/// <summary>
		/// 指定のinterval秒間停止するActionを生成
		/// </summary>
		/// <param name="interval">停止させたい秒数</param>
		/// <returns>任意時間の停止命令</returns>
		public IMovementCommand Stop(float interval)
		{
			IMovementCommand stop = new StopCommand(interval);
			return stop;
		}

		/// <summary>
		/// toioから任意の音を鳴らすActionを生成
		/// </summary>
		public ISoundCommand Sound(int _repeatCount, Cube.SoundOperation[] _sounds)
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
		public ISoundCommand PresetSound(int _soundId, int _volume, float interval)
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
		/// 第4引数は10~2550まで
		/// </summary>
		public ILightCommand TurnOnLED(int _red, int _green, int _blue, int _durationMills)
		{
			ILightCommand lEDCommand = new TurnOnLED(_red, _green, _blue, _durationMills);
			return lEDCommand;
		}

		/// <summary>
		/// toioのLEDを消すActionを生成
		/// </summary>
		/// <returns></returns>
		public ILightCommand TurnOffLED()
		{
			ILightCommand turnOffCommand = new TurnOffLEDCommand();
			return turnOffCommand;
		}

		/// <summary>
		/// LEDを任意の間隔で点灯するActionを生成
		/// </summary>
		public ILightCommand LEDBlink(int _repeatCount, Cube.LightOperation[] _lightOperations)
		{
			ILightCommand lEDBlink = new LEDBlinkCommand(_repeatCount, _lightOperations);
			float interval = 0;
			foreach (var operation in _lightOperations)
			{
				interval += operation.durationMs;
			}
			return lEDBlink;
		}
	}
}