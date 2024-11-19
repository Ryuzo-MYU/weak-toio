using System.Collections;
using toio;

namespace Robot
{
	/// <summary>
	/// toioに行動命令を送るためのフォーマットクラス
	/// </summary>
	public abstract class Action
	{
		public string ActionName { get; protected set; }
		public IEnumerator[] MoveOperations { get; protected set; }
		public Action(string name, IEnumerator[] operations)
		{
			ActionName = name;
			MoveOperations = operations;
		}
	}
}