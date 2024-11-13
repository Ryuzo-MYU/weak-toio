using toio;

namespace Robot
{
	/// <summary>
	/// toioに行動命令を送るためのフォーマットクラス
	/// </summary>
	public abstract class Action
	{
		public abstract string ActionName { get; protected set; }
		public abstract EnvType Env { get; set; }
		public abstract ActionType AcType { get; protected set; }
		public abstract double Dist { get; protected set; }
		public abstract double Deg { get; protected set; }
		public abstract double Speed { get; protected set; }
		public abstract Movement[] Movements { get; protected set; }

		public float Interval
		{
			get { return Interval; }

			private set
			{
				if (AcType == ActionType.Rotate)
				{
					Interval = (float)(Deg / Speed);
					return;
				}
				if (AcType == ActionType.Translate)
				{
					Interval = (float)(Dist / Speed);
					return;
				}
				Interval = 0;
				return;
			}
		}
	}
}