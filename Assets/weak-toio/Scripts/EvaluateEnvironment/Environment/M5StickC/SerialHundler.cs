using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;

public class SerialHandler : MonoBehaviour
{
	// Deviceのリストを保持するフィールドを追加
	[SerializeField] private List<string> DeviceList = new List<string>();

	private static SerialHandler instance;
	public static SerialHandler Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<SerialHandler>();
				if (instance == null)
				{
					Debug.LogError("MySerialHandler instance not found in the scene. Please ensure MySerialHandler is attached to a GameObject in the scene.");
				}
			}
			return instance;
		}
	}

	[SerializeField] string[] activePorts = null;
	public string portName = null;
	public int baudRate = 112500;

	private SerialPort serialPort;
	private Thread thread;

	private string message;
	public string Message;//ほかのスクリプトからアクセスするためのプロパティ
	private bool isPortOpen;//シリアルポートが既に開かれているかどうかを示すフラグ

	async void Awake()
	{

		Debug.Log("Start");

		// instanceがすでにあったら自分を消去する。
		if (instance && this != instance)
		{
			Debug.Log("Destroyed");
			Destroy(this.gameObject);
			return; // この行を追加して、以下のコードが実行されないようにする

		}

		instance = this;
		// Scene遷移で破棄されなようにする。      
		DontDestroyOnLoad(this);

		Thread.Sleep(2000);

		if (!isPortOpen)//ポート名がnullの場合、接続を試みる
		{
			portName = await SearchPort();
		}

		Debug.Log(portName + " is setting to Device");
		if (portName != null)
		{
			OpenToRead();// Deviceのポートを開く
			Thread.Sleep(100);// 100ms待つ
			Write("SetDevice");// 使用するDeviceに設定する
			await ReadAsync();// 非同期にデータを読み取る

		}
		else
		{
			Debug.Log("Device is not found but try to open COM3");
			manualOpen();// ポート名がnullの場合、COM3を開く
			Thread.Sleep(100); // 100ms待つ
			Write("setDevice");// 使用するDeviceに設定する
			await ReadAsync();// 非同期にデータを読み取る
		}

	}

	private async Task<string> SearchPort()
	{
		Debug.Log("SearchPort Start");
		activePorts = SerialPort.GetPortNames();

		foreach (string port in activePorts)
		{
			Debug.Log(port + " is active");

		}

		for (int i = 0; i < activePorts.Length; i++)
		{
			//Debug.Log(activePorts[i] + " is active");
			portName = activePorts[i]; // activePortの名前をportNameに代入する

			message = null; // メッセージをクリアする
			try
			{
				OpenToSearch();

				Thread.Sleep(2000);

				Write("areyouDevice");//デバイスにシリアル通信で文字列を送信する

				await ReadAreYouDevice();// 非同期にデータを読み取る

				Close();

				if (message != null && message.Contains("iamDevice"))
				{
					Debug.Log(portName + " is Device!");



					DeviceList.Add(portName);// Deviceのリストに追加

					Debug.Log(portName);

					return portName;// Deviceのポート名を返す
				}
				else
				{
					Debug.Log(portName + " is not Device");
					Close();

					Thread.Sleep(1000);
				}
			}
			catch (System.Exception e)
			{
				Debug.Log(e);
				Close();
			}

		}

		Debug.Log("Device is not found in active ports");
		return null;
	}

	private void OpenToSearch()
	{
		Debug.Log(portName + " will be opened to search");
		serialPort = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One)
		{
			ReadTimeout = 500//タイムアウトの設定
		};

		serialPort.Open();//ポートを開く
		isPortOpen = true; // ポートが開かれたことを示す
	}

	private void OpenToRead()
	{
		Debug.Log(portName + " will be opened to read");
		serialPort = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One)
		{
		};
		serialPort.Open();//ポートを開く
		isPortOpen = true; // ポートが開かれたことを示す
	}

	public void Close()
	{
		if (thread != null && thread.IsAlive)//もしスレッドがあったら
		{
			thread.Join();//リードの処理が終わるまで待つ
			thread = null; // スレッドをクリアする
		}
		if (serialPort != null && serialPort.IsOpen)//もしシリアルポートが開いていたら
		{
			serialPort.Close();//ポートを閉じる
			Debug.Log(portName + " is closed");
			serialPort.Dispose();//リソースの解放
			Debug.Log("Resource is cleared");
			serialPort = null; // シリアルポートをクリアする
			isPortOpen = false; // ポートが閉じられたことを示す
		}
	}

	private async Task<string> ReadAreYouDevice()//areyoudeviceのメッセージを読み取る
	{
		message = null;
		await Task.Run(() =>
		{
			if (serialPort != null && serialPort.IsOpen)
			{
				try
				{
					message = serialPort.ReadLine();
				}
				catch (System.Exception e)
				{
					Debug.LogWarning("1:" + e.Message);
					Close();
				}
			}
		});
		return message; // 読み取ったメッセージを返す

	}

	public async Task<string> ReadAsync()//非同期でデータを読み取る
	{
		message = null;
		await Task.Run(() =>
		{
			while (true)
			{
				if (serialPort != null && serialPort.IsOpen)
				{
					try
					{
						message = serialPort.ReadLine();
						//Debug.Log(message + " is received");
					}
					catch (System.Exception e)
					{
						Debug.LogWarning("2:" + e.Message);
						Close();
						break;
					}
				}
				else
				{
					break;
				}
			}
		});
		Message = message;
		return message;
	}

	public void Write(string message)
	{
		try
		{
			serialPort.Write(message);
			serialPort.Write("\n");
			Debug.Log(message + " is sent");

		}
		catch (System.Exception e)
		{

			Debug.LogWarning("3:" + e.Message);
		}
	}

	private void OnApplicationQuit()
	{
		Write("exit");
		Close();
	}

	private void manualOpen()
	{
		if (portName == null)
		{
			portName = "COM3";
			OpenToRead();
		}
	}

	public string SendToAnotherScript()
	{
		return message;
	}

}

