using System;
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

	public float GetInterval()
	{
		return Math.Abs(deg / (float)speed);
	}
	public void Exec(Toio toio)
	{
		CubeHandle handle = toio.Handle;
		Movement rotate = handle.RotateByDeg(deg, speed);
		handle.Update();
		handle.Move(rotate, false);
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
	public float GetInterval()
	{
		return Math.Abs(rad / (float)speed);
	}

	public void Exec(Toio toio)
	{
		CubeHandle handle = toio.Handle;
		Movement radRotate = handle.RotateByRad(rad, speed);
		handle.Update();
		handle.Move(radRotate, false);
	}
}
