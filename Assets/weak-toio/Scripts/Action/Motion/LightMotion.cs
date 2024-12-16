using UnityEngine;

namespace Robot
{
	public class LightMotion : BaseMotion
	{
		public ILightCommand LightCommand { get; private set; }

		public LightMotion(ILightCommand _light) : base()
		{
			LightCommand = _light;
			Interval = 0.1f;
		}

		public override void Exec(Toio toio)
		{
			LightCommand.Execute(toio);
		}
	}
}