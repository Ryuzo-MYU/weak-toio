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
	public void Execute(Toio toio)
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
	public void Execute(Toio toio)
	{
		Cube cube = toio.Cube;
		cube.TurnOnLightWithScenario(repeatCount, lightOperations);
	}
}