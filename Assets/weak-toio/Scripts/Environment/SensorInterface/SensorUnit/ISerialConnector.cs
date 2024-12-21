namespace Environment
{
	public interface ISerialConnector
	{
		public void OnDataReceived(string message);
	}
}