using System.Collections.Generic;
using System.Linq;

namespace Robot
{
	public class Action
	{
		private Queue<MovementMotion> movements;
		private Queue<LightMotion> lights;
		private Queue<SoundMotion> sounds;

		public Action()
		{
			movements = new Queue<MovementMotion>();
			lights = new Queue<LightMotion>();
			sounds = new Queue<SoundMotion>();
		}

		public Action(Queue<MovementMotion> motions)
		{
			this.movements = motions;
		}

		public void AddMovement(IMovementCommand command)
		{
			movements.Enqueue(new MovementMotion(command));
		}

		public void AddLight(ILightCommand command)
		{
			lights.Enqueue(new LightMotion(command));
		}

		public void AddSound(ISoundCommand sound){
			sounds.Enqueue(new SoundMotion(sound));
		}

		public int Count()
		{
			List<int> counts = new List<int>{
				movements.Count,
				lights.Count,
				sounds.Count
			};
			return counts.Max();
		}

		/// <summary>
		/// motionsのQueueから次(最も古い)モーションを取って返す
		/// </summary>
		/// <returns></returns>
		public MovementMotion GetNextMovement()
		{
			//motionが残っていないならnull返して早期リターン
			if (movements.Count == 0) return null;
			return movements.Dequeue();
		}

		public LightMotion GetNextLight()
		{
			if (lights.Count == 0) return null;
			return lights.Dequeue();
		}

		public SoundMotion GetNextSound()
		{
			if (sounds.Count == 0) return null;
			return sounds.Dequeue();
		}

		/// <summary>
		/// motionsの個数を返す
		/// </summary>
		/// <returns></returns>
		public int MovementCount()
		{
			return movements.Count;
		}

		public int LightCount()
		{
			return lights.Count;
		}

		public int SoundCount()
		{
			return sounds.Count;
		}

		/// <summary>
		/// motionsの中身をcount回複製する
		/// </summary>
		public void Repeat(int count)
		{
			for (int i = 0; i < count; i++)
			{
				Queue<MovementMotion> repeat = new Queue<MovementMotion>(movements);
				while (repeat.Count > 0)
				{
					MovementMotion motion = repeat.Dequeue();
					movements.Enqueue(motion);
				}
			}
		}
	}
}