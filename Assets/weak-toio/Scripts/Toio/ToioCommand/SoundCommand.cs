using Robot;
using toio;


/// <summary>
/// 任意のサウンドを組んでtoioに鳴らさせるクラス
/// </summary>
public class SoundCommand : ISoundCommand
{
	int repeatCount;
	Cube.SoundOperation[] sounds;

	public SoundCommand(int _repeatCount, Cube.SoundOperation[] _sounds)
	{
		repeatCount = _repeatCount;
		sounds = _sounds;
	}

	public float GetInterval()
	{
		float interval = 0;
		foreach (var operation in sounds)
		{
			interval += operation.durationMs / 1000;
		}
		return interval;
	}

	public void Exec(Toio toio)
	{
		Cube cube = toio.Cube;
		cube.PlaySound(repeatCount, sounds);
	}
}

/// <summary>
/// toio側に登録されているサウンドプリセットを選んで鳴らす
/// </summary>
public class PresetSoundCommand : ISoundCommand
{
	int soundId;
	int volume;
	float interval;
	public PresetSoundCommand(int _soundId, int _volume, float interval)
	{
		soundId = _soundId;
		volume = _volume;
		this.interval = interval;
	}

	public float GetInterval()
	{
		return interval;
	}

	public void Exec(Toio toio)
	{
		Cube cube = toio.Cube;
		cube.PlayPresetSound(soundId, volume);
	}
}