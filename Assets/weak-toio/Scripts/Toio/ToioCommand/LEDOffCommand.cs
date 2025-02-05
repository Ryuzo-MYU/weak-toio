namespace ActionGenerate
{
	public class LEDOffCommand : ILightCommand
	{
		float interval;
		public LEDOffCommand(float _interval)
		{
			interval = _interval;
		}
		public float GetInterval()
		{
			return interval;
		}
		public void Exec(Toio toio)
		{
			toio.Cube.TurnLedOff();
		}
	}
}