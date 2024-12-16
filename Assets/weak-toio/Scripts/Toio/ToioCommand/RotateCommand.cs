using Robot;
using toio;

public class DegRotateCommand : IMovementCommand
{
	float deg;
	double speed;
	public DegRotateCommand(float _deg, double _speed)
	{
		deg = _deg;
		speed = _speed;
	}
	public void Execute(Toio toio)
	{
		CubeHandle handle = toio.Handle;
		Movement rotate = handle.RotateByDeg(deg, speed);
		handle.Update();
		handle.Move(rotate);
	}
}

public class RadRotateCommand : IMovementCommand
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
		handle.Update();
		handle.Move(radRotate);
	}
}
