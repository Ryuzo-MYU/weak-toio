namespace Robot
{
	public class MovementMotion : BaseMotion
	{
		public IMovementCommand Move { get; private set; }

		public MovementMotion(IMovementCommand _command, float interval) : base(interval)
		{
			Move = _command;
		}

		public override void Exec(Toio toio)
		{
			Move.Execute(toio);
		}
	}
}