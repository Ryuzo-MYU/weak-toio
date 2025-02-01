using System.Collections;
using UnityEngine;

namespace Environment
{
	public class DummyM5StickcWithENV2 : DummyM5Stickc, IENV2Sensor
	{
		[SerializeField] protected float _temp;
		[SerializeField] protected float _hum;
		[SerializeField] protected float _pressure;
		public float GetTemperature() { return _temp; }
		public float GetHumidity() { return _hum; }
		public float GetPressure() { return _pressure; }

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
			base.UpdateSensor();
			UpdateENV2Sensor();
			_OnDeserializeCompleted();
			yield return new WaitForSeconds(updateInterval);
		}
		private void UpdateENV2Sensor()
		{
			_temp += Random.Range(-1f, 1f);
			_hum += Random.Range(-1f, 1f);
			_pressure += Random.Range(-1f, 1f);
		}
	}
}