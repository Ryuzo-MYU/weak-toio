using System;
using System.Collections.Generic;

namespace Robot
{
	public class Action
	{
		Queue<Motion> motions;

		public Action()
		{
			motions = new Queue<Motion>();
		}

		public Action(Queue<Motion> motions)
		{
			this.motions = motions;
		}

		/// <summary>
		/// motionsのQueueから次(最も古い)モーションを取って返す
		/// </summary>
		/// <returns></returns>
		public Motion GetNextMotion()
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
				Queue<Motion> repeat = new Queue<Motion>(motions);
				while (repeat.Count > 0)
				{
					Motion motion = repeat.Dequeue();
					motions.Enqueue(motion);
				}
			}
		}
	}
}