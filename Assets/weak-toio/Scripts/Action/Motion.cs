using System;

namespace Robot
{
	public class Motion
	{
		public IToioCommand command;
		public float interval;
		public Motion(IToioCommand _command, float _intervel)
		{
			command = _command;
			interval = Math.Abs(_intervel);
		}
	}
}