using System.Collections;
using Evaluation;

namespace Environment
{
	public class DummyCO2Sensor : ICO2Sensor
	{
		private float _ppm;

		public float GetPPM() { return _ppm; }

		public DummyCO2Sensor()
		{
			_ppm = 700f; // 初期値参照: https://www.mhlw.go.jp/content/11130500/000771220.pdf
		}
		public EnvType GetEnvType() { return EnvType.CO2; }
		public void StartSensor() { } // 何もしない

		/// <summary>
		/// 前回の値に基づいて、センサの値を変動させる
		/// </summary>
		public void UpdateSensor()
		{
			_ppm += UnityEngine.Random.Range(-1f, 1f);
		}
		public void OnDataReceived(string message) { } // 何もしない
	}
}