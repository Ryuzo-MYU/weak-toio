using System.Collections.Generic;
using System.Linq;

namespace ActionGenerate
{
	public class Action
	{
		private Queue<IMovementCommand> movements;
		private Queue<ILightCommand> lights;
		private Queue<ISoundCommand> sounds;

		public Action()
		{
			movements = new Queue<IMovementCommand>();
			lights = new Queue<ILightCommand>();
			sounds = new Queue<ISoundCommand>();
		}

		public static Action operator +(Action action1, Action action2)
		{
			Action newAction = new Action();

			// action1の要素をコピー
			foreach (var move in action1.GetMovements())
			{
				newAction.AddMovement(move);
			}
			foreach (var light in action1.GetLightCommands())
			{
				newAction.AddLight(light);
			}
			foreach (var sound in action1.GetSoundCommands())
			{
				newAction.AddSound(sound);
			}

			// action2の要素をコピー
			foreach (var move in action2.GetMovements())
			{
				newAction.AddMovement(move);
			}
			foreach (var light in action2.GetLightCommands())
			{
				newAction.AddLight(light);
			}
			foreach (var sound in action2.GetSoundCommands())
			{
				newAction.AddSound(sound);
			}

			return newAction;
		}

		public static Action operator *(Action action1, int count)
		{
			Action newAction = new Action();
			for (int i = 0; i < count; i++)
			{
				newAction += action1;
			}
			return newAction;
		}
		public void AddAction(IMovementCommand movement, ILightCommand light, ISoundCommand sound)
		{
			movements.Enqueue(movement);
			lights.Enqueue(light);
			sounds.Enqueue(sound);
		}

		public void AddMovement(IMovementCommand command)
		{
			movements.Enqueue(command);
		}

		public void AddLight(ILightCommand command)
		{
			lights.Enqueue(command);
		}

		public void AddSound(ISoundCommand sound)
		{
			sounds.Enqueue(sound);
		}

		public Queue<IMovementCommand> GetMovements()
		{
			return this.movements;
		}
		public Queue<ILightCommand> GetLightCommands()
		{
			return this.lights;
		}
		public Queue<ISoundCommand> GetSoundCommands()
		{
			return this.sounds;
		}

		/// <summary>
		/// 全体の残モーションのうち，最も多く残っているモーションの個数を返す
		/// </summary>
		/// <returns></returns>
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
		/// 書くmotionsの個数を返す
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

		public float GetInterval()
		{
			float[] intervals = {
	   GetMovementInterval(),
	   GetLightInterval(),
	   GetSoundInterval() };

			return intervals.Max();
		}

		private float GetMovementInterval()
		{
			float movementInterval = 0f;
			if (movements.Count() == 0)
			{
				return 0f;
			}
			foreach (var motion in movements)
			{
				movementInterval += motion.GetInterval();
			}
			return movementInterval;
		}
		private float GetLightInterval()
		{
			float lightInterval = 0f;
			if (lights.Count() == 0)
			{
				return 0f;
			}
			foreach (var motion in lights)
			{
				lightInterval += motion.GetInterval();
			}
			return lightInterval;
		}
		private float GetSoundInterval()
		{
			float soundInterval = 0f;
			if (sounds.Count() == 0)
			{
				return 0f;
			}
			foreach (var motion in sounds)
			{
				soundInterval += motion.GetInterval();
			}
			return soundInterval;
		}

		/// <summary>
		/// Actionの中身をクリアする
		/// </summary>  
		public void Clear()
		{
			movements.Clear();
			lights.Clear();
			sounds.Clear();
		}
	}
}