using Evaluation;
using toio;

namespace Robot
{
	public abstract class ActionGenerator
	{
		protected Result _result;

		/// <summary>
		/// 前後移動のMovementを返す
		/// </summary>
		/// <param name="_dist">距離</param>
		/// <param name="_speed">速度</param>
		/// <returns>Movement</returns>
		protected Motion Translate(float _dist, double _speed)
		{
			IToioCommand translate = new TranslateCommand(_dist, _speed);
			float interval = (float)_dist / (float)_speed;
			return new Motion(translate, interval);
		}

		/// <summary>
		/// 回転移動のMovementを返す
		/// </summary>
		/// <param name="_deg">角度</param>
		/// <param name="_speed">速度</param>
		/// <returns>Movement</returns>
		protected Motion DegRotate(float _deg, double _speed)
		{
			IToioCommand degRotate = new DegRotateCommand(_deg, _speed);
			float interval = (float)_deg / (float)_speed;
			return new Motion(degRotate, interval);
		}

		protected Motion RadRotate(float _rad, double _speed)
		{
			IToioCommand radRotate = new RadRotateCommand(_rad, _speed);
			float interval = (float)_rad / (float)_speed;
			return new Motion(radRotate, interval);
		}

		protected Motion Sound(int _repeatCount, Cube.SoundOperation[] _sounds)
		{
			SoundCommand soundCommand = new SoundCommand(_repeatCount, _sounds);
			float interval = 0;
			foreach (var sound in _sounds)
			{
				interval += sound.durationMs;
			}
			return new Motion(soundCommand, interval);
		}

		protected Motion PresetSound(int _soundId, int _volume)
		{
			PresetSoundCommand presetSound = new PresetSoundCommand(_soundId, _volume);
			float interval = 0.5f; // サウンドごとの間隔を取得できないため、決め打ち。
			return new Motion(presetSound, interval);
		}

		protected Motion TurnOnLED(int _red, int _green, int _blue, int _durationMills)
		{
			TurnOnLEDCommand lEDCommand = new TurnOnLEDCommand(_red, _green, _blue, _durationMills);
			float interval = _durationMills;
			return new Motion(lEDCommand, interval);
		}

		protected Motion LEDBlink(int _repeatCount, Cube.LightOperation[] _lightOperations)
		{
			LEDBlinkCommand lEDBlink = new LEDBlinkCommand(_repeatCount, _lightOperations);
			float interval = 0;
			foreach (var operation in _lightOperations)
			{
				interval += operation.durationMs;
			}
			return new Motion(lEDBlink, interval);
		}
	}
}