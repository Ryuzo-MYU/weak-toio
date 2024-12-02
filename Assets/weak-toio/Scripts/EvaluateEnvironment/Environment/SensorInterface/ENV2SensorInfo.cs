namespace Environment
{
	public interface IENV2Sensor
	{
		public float GetTemperature(); // M5StickCの内部温度
		public float GetHumidity(); // ENV2の湿度
		public float GetPressure(); // ENV2の気圧
	}
}