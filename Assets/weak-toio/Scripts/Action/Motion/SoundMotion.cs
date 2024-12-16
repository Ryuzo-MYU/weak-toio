using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Robot
{
	public class SoundMotion : BaseMotion
	{
		private int soundId;
		private int volume;

		public SoundMotion(int _soundId, int _volume, float _interval) : base(_interval)
		{
			soundId = _soundId;
			volume = _volume;
		}
	}
}