using Robot;
using toio;


/// <summary>
/// 任意のサウンドを組んでtoioに鳴らさせるクラス
/// </summary>
public class SoundCommand : IToioCommand
{
	int repeatCount;
	Cube.SoundOperation[] sounds;
	public SoundCommand(int _repeatCount, Cube.SoundOperation[] _sounds)
	{
		repeatCount = _repeatCount;
		sounds = _sounds;
	}

	public void Execute(Toio toio)
	{
		Cube cube = toio.Cube;
		cube.PlaySound(repeatCount, sounds);
	}
}

/// <summary>
/// toio側に登録されているサウンドプリセットを選んで鳴らす
/// </summary>
public class PresetSoundCommand : IToioCommand
{
	int soundId;
	int volume;
	public PresetSoundCommand(int _soundId, int _volume)
	{
		soundId = _soundId;
		volume = _volume;
	}
	public void Execute(Toio toio)
	{
		Cube cube = toio.Cube;
		cube.PlayPresetSound(soundId, volume);
	}
}