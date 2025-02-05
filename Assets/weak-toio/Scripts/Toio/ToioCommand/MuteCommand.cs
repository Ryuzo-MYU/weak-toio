using toio;

namespace ActionGenerate
{
	public class MuteCommand : ISoundCommand
	{
		/// <summary>
		/// ISoundCommand用の何もしないコマンドクラス
		/// </summary>
		float interval;
		public MuteCommand(float _interval)
		{
			interval = _interval;
		}
		public float GetInterval()
		{
			return interval;
		}
		public void Exec(Toio toio) { }
	}
}