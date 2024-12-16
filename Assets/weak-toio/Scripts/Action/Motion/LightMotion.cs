using UnityEngine;

namespace Robot
{
	public class LightMotion : BaseMotion
	{
		public ILightCommand LightCommand { get; private set; }

		public LightMotion(ILightCommand _light, float interval) : base(interval)
		{
			LightCommand = _light;
		}

		public override void Exec(Toio toio)
		{
			LightCommand.Execute(toio);
		}
	}
}