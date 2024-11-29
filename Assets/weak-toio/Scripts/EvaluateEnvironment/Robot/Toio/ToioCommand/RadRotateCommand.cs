using Robot;
using toio;

public class RadRotateCommand : IToioCommand
{
	float rad;
	double speed;
	public RadRotateCommand(float _rad, double _speed)
	{
		rad = _rad;
		speed = _speed;
	}
	public void Execute(Toio toio)
	{
		CubeHandle handle = toio.Handle;
		Movement radRotate = handle.RotateByRad(rad, speed);
		handle.Move(radRotate);
	}
}
