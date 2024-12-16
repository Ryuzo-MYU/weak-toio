namespace Robot
{
	public class SoundMotion : BaseMotion
	{
		public ISoundCommand SoundCommand { get; private set; }

		public SoundMotion(ISoundCommand _sound, float _interval) : base()
		{
			SoundCommand = _sound;
			Interval = _interval;
		}

		public override void Exec(Toio toio)
		{
			SoundCommand.Execute(toio);
		}
	}
}