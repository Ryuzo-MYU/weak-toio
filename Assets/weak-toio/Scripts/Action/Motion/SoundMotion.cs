namespace Robot
{
	public class SoundMotion : BaseMotion
	{
		public ISoundCommand SoundCommand { get; private set; }

		public SoundMotion(ISoundCommand _sound) : base()
		{
			SoundCommand = _sound;
		}

		public override void Exec(Toio toio)
		{
			SoundCommand.Execute(toio);
		}
	}
}