namespace Robot
{
	public interface IToioCommand
	{
		public float GetInterval();
		public void Exec(Toio toio);
	}
}