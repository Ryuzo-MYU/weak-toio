using Robot;
using toio;

public class TurnOnLEDCommand : ILightCommand
{
	int red;
	int green;
	int blue;
	int durationMills;
	public TurnOnLEDCommand(int _red, int _green, int _blue, int _durationMills)
	{
		red = _red;
		green = _green;
		blue = _blue;
		durationMills = _durationMills;
	}

	public float GetInterval()
	{
		return durationMills / 1000; //ミリ秒を秒に変換
	}

	public void Exec(Toio toio)
	{
		Cube cube = toio.Cube;
		cube.TurnLedOn(red, green, blue, durationMills);
	}
}
public class LEDBlinkCommand : ILightCommand
{
	int repeatCount;
	Cube.LightOperation[] lightOperations;
	public LEDBlinkCommand(int _repeatCount, Cube.LightOperation[] _lightOperations)
	{
		repeatCount = _repeatCount;
		lightOperations = _lightOperations;
	}

	public float GetInterval()
	{
		float interval = 0;
		foreach (var operation in lightOperations)
		{
			interval += operation.durationMs / 1000;
		}
		return interval;
	}
	public void Exec(Toio toio)
	{
		Cube cube = toio.Cube;
		cube.TurnOnLightWithScenario(repeatCount, lightOperations);
	}
}