namespace Robot
{
	public abstract class BaseMotion
	{
		public float Interval { get; protected set; }
		public bool IsCompleted { get; protected set; }

		protected BaseMotion()
		{
			this.IsCompleted = false;
		}

		public abstract void Exec(Toio toio);
	}
}