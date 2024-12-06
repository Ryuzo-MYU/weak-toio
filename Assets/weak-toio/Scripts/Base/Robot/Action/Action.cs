using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
		public Motion GetNextMotion()
		{
			return motions.Dequeue();
		}
		public int Count()
		{
			return motions.Count;
		}

		public static implicit operator Queue<object>(Action v)
		{
			throw new NotImplementedException();
		}
	}
}