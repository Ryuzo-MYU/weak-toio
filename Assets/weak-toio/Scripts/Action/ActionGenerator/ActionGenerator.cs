using Evaluation;
using toio;
using UnityEngine;

namespace Robot
{
	/// <summary>
	/// toioのアクションを生成する抽象クラス
	/// 各環境の具象クラス用に基本のMotion生成メソッドを提供する
	/// </summary>
	public abstract class ActionGenerator : MonoBehaviour
	{
		public event System.Action<Action> OnActionGenerated;
		private Result currentResult;
		private void Start()
		{
			EvaluateBase evaluate = gameObject.GetComponent<EvaluateBase>();
			evaluate.OnResultGenerated += OnResultGenerated;
		}

		protected void _OnActionGenerated(Action action)
		{
			OnActionGenerated?.Invoke(action);
		}
		protected void OnResultGenerated(Result result)
		{
			currentResult = result;
		}

		/// <summary>
		/// 前後移動のMosionを返す
		/// </summary>
		/// <param name="_dist">距離</param>
		/// <param name="_speed">速度</param>
		/// <returns>Motion</returns>
		protected Motion Translate(float _dist, double _speed)
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
		protected Motion DegRotate(float _deg, double _speed)
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
		protected Motion RadRotate(float _rad, double _speed)
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

		/// <summary>
		/// toioに登録されているSEを鳴らすMotionを返す
		/// </summary>
		/// <param name="_soundId">SEのID。0～10まで</param>
		/// <param name="_volume">音量</param>
		/// <returns>Motion</returns>
		protected Motion PresetSound(int _soundId, int _volume)
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
		protected Motion TurnOnLED(int _red, int _green, int _blue, int _durationMills)
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