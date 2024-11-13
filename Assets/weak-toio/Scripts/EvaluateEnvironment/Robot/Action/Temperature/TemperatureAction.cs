using toio;

namespace Robot
{
	/// <summary>
	/// 気温評価モード用のアクションフォーマットクラス
	/// </summary>
	public class TemperatureAction : Action
	{
		public override string ActionName { get; protected set; }
		public override EnvType Env { get; set; }
		public override ActionType AcType { get; protected set; }
		public override double Dist { get; protected set; }
		public override double Deg { get; protected set; }
		public override double Speed { get; protected set; }
		public override Movement[] Movements { get; protected set; }
	}
}