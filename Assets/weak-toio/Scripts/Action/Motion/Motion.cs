namespace Robot
{
	public class MovementMotion : BaseMotion
	{
		public IToioCommand command;
		public MovementMotion(IToioCommand _command, float _interval) : base(_interval)
		{
			command = _command;
		}
	}
}