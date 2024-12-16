using System.Collections.Generic;
using System.Linq;

namespace Robot
{
	public class Action
	{
		private Queue<IMovementCommand> motions;
		private Queue<ILightCommand> lights;
		private Queue<ISoundCommand> sounds;

		public Action()
		{
			motions = new Queue<IMovementCommand>();
			lights = new Queue<ILightCommand>();
			sounds = new Queue<ISoundCommand>();
		}

		public void AddMovement(IMovementCommand command)
		{
			motions.Enqueue(command);
		}

		public void AddLight(ILightCommand command)
		{
			lights.Enqueue(command);
		}

		public void AddSound(ISoundCommand sound)
		{
			sounds.Enqueue(sound);
		}

		public int Count()
		{
			List<int> counts = new List<int>{
				motions.Count,
				lights.Count,
				sounds.Count
			};
			return counts.Max();
		}

		/// <summary>
		/// motionsのQueueから次(最も古い)モーションを取って返す
		/// </summary>
		/// <returns></returns>
		public IMovementCommand GetNextMovement()
		{
			//motionが残っていないならnull返して早期リターン
			if (motions.Count == 0) return null;
			return motions.Dequeue();
		}

		public ILightCommand GetNextLight()
		{
			if (lights.Count == 0) return null;
			return lights.Dequeue();
		}

		public ISoundCommand GetNextSound()
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
			return motions.Count;
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
		/// actionsの各アクションキューをcount回複製する
		/// </summary>
		public void Repeat(int count)
		{
			for (int i = 0; i < count; i++)
			{
				Queue<IMovementCommand> moveRepeat = new Queue<IMovementCommand>(motions);
				while (moveRepeat.Count > 0)
				{
					IMovementCommand motion = moveRepeat.Dequeue();
					motions.Enqueue(motion);
				}

				Queue<ILightCommand> lightRepeat = new Queue<ILightCommand>(lights);
				while (lightRepeat.Count > 0)
				{
					ILightCommand light = lightRepeat.Dequeue();
					lights.Enqueue(light);
				}

				Queue<ISoundCommand> soundRepeat = new Queue<ISoundCommand>(sounds);
				while (soundRepeat.Count > 0)
				{
					ISoundCommand sound = soundRepeat.Dequeue();
					sounds.Enqueue(sound);
				}
			}
		}
	}
}