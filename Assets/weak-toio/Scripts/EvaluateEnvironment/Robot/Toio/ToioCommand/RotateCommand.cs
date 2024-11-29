using Robot;
using toio;

public class DegRotateCommand : IToioCommand
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