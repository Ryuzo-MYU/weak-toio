namespace Environment
{
	public interface IHumiditySensor : ISensorUnit
	{
		public float GetHumidity();
	}
}