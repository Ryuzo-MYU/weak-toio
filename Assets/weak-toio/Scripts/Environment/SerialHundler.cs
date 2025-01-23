/// シリアル通信を行うスクリプト
/// https://qiita.com/mori_166/items/9487aeddd3b9fa9a8f1d 

using UnityEngine;
using System.IO.Ports;
using System.Threading;
using System.Collections.Generic;
using System;

public class SerialHandler : MonoBehaviour
{
	public delegate void SerialDataReceivedEventHandler(string message);
	public delegate void SerialErrorEventHandler(string errorMessage);
	public event SerialDataReceivedEventHandler OnDataReceived;
	public event SerialErrorEventHandler OnError;
	public event Action OnConnectSucceeded;
	public event Action OnConnectFailed;

	public string portName = null;
	public int baudRate = 115200;

	private SerialPort serialPort_;
	private Thread thread_;
	private bool isRunning_ = false;

	private string message_;
	private bool isNewMessageReceived_ = false;

	[SerializeField] const int Timeout = 20000;

	// 使用中のポートを追跡する静的コレクション
	private static HashSet<string> usedPorts = new HashSet<string>();
	private static readonly object portLock = new object();

	public SerialHandler(string _portName, int _baudRate)
	{
		portName = _portName;
		baudRate = _baudRate;
	}
	public void Awake()
	{
		try
		{
			// 利用可能なポートをチェック
			string[] availablePorts = GetAvailableSerialPorts();
			if (availablePorts.Length == 0)
			{
				throw new System.InvalidOperationException("利用可能なシリアルポートがありません");
			}

			// ポート名が指定されていない場合は最初の利用可能なポートを使用
			if (string.IsNullOrEmpty(portName))
			{
				portName = availablePorts[0];
				Debug.Log($"ポート名が指定されていないため、{portName}を使用します");
			}
			// 指定されたポートが利用可能かチェック
			else if (!System.Array.Exists(availablePorts, port => port == portName))
			{
				string availablePortsStr = string.Join(", ", availablePorts);
				throw new System.InvalidOperationException(
					$"指定されたポート {portName} は利用できません\n" +
					$"利用可能なポート: {availablePortsStr}");
			}

			OnConnectSucceeded?.Invoke();
			Open();
		}
		catch (System.Exception e)
		{
			OnConnectFailed?.Invoke();
			Debug.LogWarning($"シリアルポートの初期化に失敗しました: {e.Message}");
			OnError?.Invoke($"シリアルポート初期化エラー: {e.Message}");
			Debug.LogWarning("接続できるポートが無いのでダミーセンサーを使用します");
		}
	}

	// ポートが使用可能かチェック
	private bool IsPortAvailable(string port)
	{
		if (string.IsNullOrEmpty(port)) return false;

		lock (portLock)
		{
			if (usedPorts.Contains(port))
			{
				return false;
			}

			// システムで利用可能なポートかチェック
			string[] availablePorts = SerialPort.GetPortNames();
			if (!System.Array.Exists(availablePorts, p => p == port))
			{
				return false;
			}

			return true;
		}
	}

	private void RegisterPort(string port)
	{
		lock (portLock)
		{
			usedPorts.Add(port);
		}
	}

	private void UnregisterPort(string port)
	{
		lock (portLock)
		{
			usedPorts.Remove(port);
		}
	}

	public void Update()
	{
		if (isNewMessageReceived_ && OnDataReceived != null)
		{
			OnDataReceived(message_);
			isNewMessageReceived_ = false;
		}
	}

	void OnDestroy()
	{
		Close();
	}

	private void Open()
	{
		if (string.IsNullOrEmpty(portName))
		{
			throw new System.ArgumentException("ポート名が指定されていません");
		}

		try
		{
			RegisterPort(portName);  // ポートを使用中としてマーク

			serialPort_ = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One);
			serialPort_.ReadTimeout = Timeout;
			serialPort_.WriteTimeout = 1000;
			serialPort_.Open();

			isRunning_ = true;

			thread_ = new Thread(Read);
			thread_.Start();

			Debug.Log($"ポート {portName} に接続しました");
		}
		catch (System.Exception e)
		{
			UnregisterPort(portName);  // エラー時はポートの使用をキャンセル
			throw new System.InvalidOperationException($"シリアルポートのオープンに失敗しました: {e.Message}", e);
		}
	}

	private void Read()
	{
		while (isRunning_ && serialPort_ != null && serialPort_.IsOpen)
		{
			try
			{
				message_ = serialPort_.ReadLine();
				isNewMessageReceived_ = true;
				// デバッグ用にメッセージ内容を確認
				Debug.Log($"受信データ: {message_}");
			}
			catch (System.TimeoutException)
			{
				Debug.Log("タイムアウト");
				continue;
			}
			catch (System.Exception e)
			{
				Debug.LogError($"シリアル通信でエラーが発生: {e.Message}");
				OnError?.Invoke($"読み取りエラー: {e.Message}");
				isRunning_ = false;
				break;
			}
		}
	}

	private void Close()
	{
		isNewMessageReceived_ = false;
		isRunning_ = false;

		if (thread_ != null && thread_.IsAlive)
		{
			thread_.Join();
		}

		if (serialPort_ != null)
		{
			if (serialPort_.IsOpen)
			{
				serialPort_.Close();
				serialPort_.Dispose();
			}
			UnregisterPort(portName);  // ポートの使用を解除
			Debug.Log($"ポート {portName} を解放しました");
		}
	}

	// 利用可能なポートの一覧を取得
	public static string[] GetAvailableSerialPorts()
	{
		var availablePorts = new List<string>();
		string[] systemPorts = SerialPort.GetPortNames();

		lock (portLock)
		{
			foreach (string port in systemPorts)
			{
				if (!usedPorts.Contains(port))
				{
					availablePorts.Add(port);
				}
			}
		}

		return availablePorts.ToArray();
	}
}
