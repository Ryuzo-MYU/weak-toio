namespace Robot
{
	public class LightMotion : BaseMotion
	{
		private int red, green, blue;
		private int durationMs;

		public LightMotion(int _red, int _green, int _blue, int _durationMs, float _interval) : base(_interval)
		{
			red = _red;
			green = _green;
			blue = _blue;
			durationMs = _durationMs;
		}
	}
}