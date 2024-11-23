using Evaluation;
using UnityEngine;
using System.Numerics;
using Vector3 = System.Numerics.Vector3;
using System.Text;

namespace Environment
{
	public class M5DataReceiver : MonoBehaviour, SensorUnit
	{
		SerialHandler serial;
		SensorInfo sensorInfo;

		public SensorInfo GetSensorInfo()
		{
			return sensorInfo;
		}
		public void Start()
		{
			serial.OnDataReceived += OnDateReceived;
		}

		//受信した信号(message)に対する処理
		public void OnDateReceived(string message)
		{
			var data = message.Split(
					new string[] { "\t" }, System.StringSplitOptions.None);
			if (data.Length < 1) return;
			try
			{
				string name = data[0];

				float[] imuInfo = new float[6];
				for (int i = 1; i <= 6; i++) { float.TryParse(data[i], out imuInfo[i - 1]); }

				float temp, humidity, pressure, vbat;
				float.TryParse(data[7], out temp); // M5StickCの温度
				float.TryParse(data[8], out humidity); // ENV2の湿度
				float.TryParse(data[9], out pressure); // ENV2の気圧
				float.TryParse(data[10], out vbat); // バッテリー残量

				sensorInfo = new SensorInfo(
					name,
					new Vector3(imuInfo[0], imuInfo[1], imuInfo[2]),
					new Vector3(imuInfo[3], imuInfo[4], imuInfo[5]),
					temp,
					humidity,
					pressure,
					vbat
				);

				Debug.Log("シリアルデータの取得に成功");
			}
			catch (System.Exception e)
			{
				Debug.Log("シリアルデータの取得に失敗");
				Debug.LogWarning(e.Message);
				Debug.LogError(e.StackTrace);
			}
		}

		public static void LogSensorInfo(SensorInfo sensorInfo, string prefix = "")
		{
			StringBuilder sb = new StringBuilder();
			sb.AppendLine($"=== {prefix}SensorInfo Debug Output ===");
			sb.AppendLine($"Name: {sensorInfo.deviceName}");

			// IMUデータ (加速度・角速度) の出力
			sb.AppendLine("IMU Data:");
			sb.AppendLine($"  Acceleration: {FormatVector3(sensorInfo.acceleration)}");
			sb.AppendLine($"  Angular Velocity: {FormatVector3(sensorInfo.gyro)}");

			// 環境データの出力
			sb.AppendLine("Environmental Data:");
			sb.AppendLine($"  Temperature: {sensorInfo.temp:F2}°C");
			sb.AppendLine($"  Humidity: {sensorInfo.humidity:F2}%");
			sb.AppendLine($"  Pressure: {sensorInfo.pressure:F2}hPa");

			// 電圧データの出力
			sb.AppendLine($"Battery Voltage: {sensorInfo.vbat:F2}V");
			sb.AppendLine("===========================");

			Debug.Log(sb.ToString());
		}

		private static string FormatVector3(Vector3 vector)
		{
			return $"(X: {vector.X:F3}, Y: {vector.Y:F3}, Z: {vector.Z:F3})";
		}
	}
}