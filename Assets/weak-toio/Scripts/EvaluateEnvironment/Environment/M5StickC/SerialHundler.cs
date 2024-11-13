using UnityEngine;
using System.IO.Ports;
using System.Threading;
using System.Linq;
using System;

namespace Environment
{
	public class SerialHandler
	{
		public delegate void SerialDataReceivedEventHandler(string message);
		public event SerialDataReceivedEventHandler OnDataReceived;

		//ポート名
		//例
		//Linuxでは/dev/ttyUSB0
		//windowsではCOM1
		//Macでは/dev/tty.usbmodem1421など
		public string[] ports;
		public string portname;
		public int baudrate;

		private SerialPort serialPort_;
		private Thread thread_;
		private bool isRunning_ = false;

		private string message_;
		private bool isNewMessageReceived_ = false;

		public SerialHandler(string[] ports, string portname, int baud)
		{
			this.ports = ports;
			this.portname = portname;
			this.baudrate = baud;
		}
		public void Awake()
		{
			// 利用可能なシリアルポートの取得
			string[] availablePorts = SerialPort.GetPortNames();

			// 利用可能なポートと一致するポートを検索
			portname = FindMatchingPort(ports, availablePorts);

			// 利用可能なポートと一致するポートを検索
			if (string.IsNullOrEmpty(portname))
			{
				Debug.LogError("利用可能なシリアルポートが見つかりませんでした。");
				return;
			}

			Open();
		}
		public void Update()
		{
			if (isNewMessageReceived_)
			{
				OnDataReceived(message_);
			}
			isNewMessageReceived_ = false;
		}

		public void OnDestroy()
		{
			Close();
		}

		// 利用可能なポートと一致するポートを検索するメソッド
		public string FindMatchingPort(string[] allowedPorts, string[] availablePorts)
		{
			foreach (string port in availablePorts)
			{
				if (allowedPorts.Contains(port))
				{
					return port; // 一致するポートが見つかった場合、そのポートを返す
				}
			}
			return null; // 一致するポートが見つからなかった場合、nullを返す
		}

		public void Open()
		{
			// serialPort_ = new SerialPort(portname, baudrate, Parity.None, 8, StopBits.One);
			//または
			serialPort_ = new SerialPort(portname, baudrate);
			serialPort_.ReadTimeout = 1000;  // 読み取りタイムアウトを1000msに設定
			serialPort_.WriteTimeout = 1000; // 書き込みタイムアウトを1000msに設定
			serialPort_.Open();

			isRunning_ = true;

			thread_ = new Thread(Read);
			thread_.Start();
		}

		private void Close()
		{
			isNewMessageReceived_ = false;
			isRunning_ = false;

			if (thread_ != null && thread_.IsAlive)
			{
				thread_.Join();
			}

			if (serialPort_ != null && serialPort_.IsOpen)
			{
				serialPort_.Close();
				serialPort_.Dispose();
			}
		}

		private void Read()
		{
			int retryCount = 0;
			int maxRetryCount = 3; // リトライ回数の上限

			while (isRunning_ && serialPort_ != null && serialPort_.IsOpen)
			{
				try
				{
					message_ = serialPort_.ReadLine();
					isNewMessageReceived_ = true;
					retryCount = 0; // 成功したらリセット
				}
				catch (TimeoutException)
				{
					retryCount++;
					if (retryCount >= maxRetryCount)
					{
						Debug.LogWarning("連続タイムアウトが発生しました。");
						Close();
						break;
					}
				}
				catch (System.Exception e)
				{
					Debug.LogWarning(e.Message);
				}
			}
		}


		public void Write(string message)
		{
			try
			{
				serialPort_.Write(message);
			}
			catch (System.Exception e)
			{
				Debug.LogWarning(e.Message);
			}
		}
	}
}