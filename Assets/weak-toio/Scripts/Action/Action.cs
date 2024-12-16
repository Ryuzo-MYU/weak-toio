using System;
using System.Collections.Generic;

namespace Robot
{
	public class Action
	{
		Queue<MovementMotion> motions;

		public Action()
		{
			motions = new Queue<MovementMotion>();
		}

		public Action(Queue<MovementMotion> motions)
		{
			this.motions = motions;
		}

		/// <summary>
		/// motionsのQueueから次(最も古い)モーションを取って返す
		/// </summary>
		/// <returns></returns>
		public MovementMotion GetNextMotion()
		{
			//motionが残っていないならnull返して早期リターン
			if (motions.Count == 0) return null;
			return motions.Dequeue();
		}

		/// <summary>
		/// motionsの個数を返す
		/// </summary>
		/// <returns></returns>
		public int Count()
		{
			return motions.Count;
		}

		/// <summary>
		/// motionsの中身をcount回複製する
		/// </summary>
		public void Repeat(int count)
		{
			for (int i = 0; i < count; i++)
			{
				Queue<MovementMotion> repeat = new Queue<MovementMotion>(motions);
				while (repeat.Count > 0)
				{
					MovementMotion motion = repeat.Dequeue();
					motions.Enqueue(motion);
				}
			}
		}
	}
}