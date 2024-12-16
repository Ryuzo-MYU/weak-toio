namespace Robot
{
	public abstract class BaseMotion
	{
		public float Interval { get; protected set; }
		public bool IsCompleted { get; protected set; }

		protected BaseMotion(float interval)
		{
			this.Interval = interval;
			this.IsCompleted = false;
		}

		public abstract void Exec(Toio toio);
	}
}