using System.Collections;
using UnityEngine;

namespace Environment
{
	public class DummyM5StickcWithCO2L : DummyM5Stickc, ICO2LSensor
	{
		[SerializeField] protected float _temp;
		[SerializeField] protected float _hum;
		[SerializeField] protected float _ppm;
		public float GetTemperature() { return _temp; }
		public float GetHumidity() { return _hum; }
		public float GetCO2() { return _ppm; }
		/// <summary>
		/// Start is called on the frame when a script is enabled just before
		/// any of the Update methods is called the first time.
		/// </summary>
		void Start()
		{
			StartCoroutine(UpdateCoroutine());
		}
		IEnumerator UpdateCoroutine()
		{
			while (true)
			{
				base.UpdateSensor();
				UpdateCO2L2Sensor();
				_OnDeserializeCompleted();
				yield return new WaitForSeconds(updateInterval);
			}
		}
		private void UpdateCO2L2Sensor()
		{
			_temp += Random.Range(-1f, 1f);
			_hum += Random.Range(-1f, 1f);
			_ppm += Random.Range(-1f, 1f);
		}
	}
}