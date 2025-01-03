using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using toio;
using UnityEngine;
using UnityEngine.UI;
using ActionGenerate;

public class ToioConnecter : MonoBehaviour
{
	public ConnectType connectType = ConnectType.Real;
	[SerializeField] private Button scanButton;
	[SerializeField] private RectTransform listContent;
	[SerializeField] private GameObject cubeItemPrefab;

	private CubeScanner scanner;
	private CubeConnecter connecter;
	private Dictionary<string, GameObject> cubeItems = new Dictionary<string, GameObject>();
	private Dictionary<BLEPeripheralInterface, BLEStatus> bleStatus = new Dictionary<BLEPeripheralInterface, BLEStatus>();

	private enum BLEStatus
	{
		Disconnected,
		Connecting,
		Connected
	}

	private void Start()
	{
		scanner = new CubeScanner(connectType);
		connecter = new CubeConnecter(connectType);
		scanButton.onClick.AddListener(StartScan);
	}

	public void StartScan()
	{
		ClearCurrentItems();
		scanButton.interactable = false;
		scanButton.GetComponentInChildren<Text>().text = "スキャン中...";
		scanner.StartScan(OnScanUpdate, OnScanEnd, 20).Forget();
	}

	private void OnScanUpdate(BLEPeripheralInterface[] peripherals)
	{
		foreach (var peripheral in peripherals)
		{
			if (peripheral == null) continue;
			if (!bleStatus.ContainsKey(peripheral))
			{
				bleStatus.Add(peripheral, BLEStatus.Disconnected);
				UpdateToioList(peripheral);
			}
		}
	}

	private void UpdateToioList(BLEPeripheralInterface peripheral)
	{
		var item = Instantiate(cubeItemPrefab, listContent);
		var button = item.GetComponent<Button>();
		var text = item.GetComponentInChildren<Text>();

		text.text = $"{peripheral.device_name}: 未接続";
		button.onClick.AddListener(async () => await ConnectToio(peripheral));

		cubeItems.Add(peripheral.device_address, item);
	}
	private async UniTask ConnectToio(BLEPeripheralInterface peripheral)
	{
		try
		{
			// シーン上の全てのToioコンポーネントを取得
			var toioComponents = FindObjectsOfType<Toio>();
			if (toioComponents == null || toioComponents.Length == 0)
			{
				Debug.LogWarning("Unity上にToioコンポーネントが見つかりません");
				return;
			}

			// LocalNameが一致するToioコンポーネントを探す
			var matchingToio = toioComponents.FirstOrDefault(t => t.LocalName == peripheral.device_name);
			if (matchingToio == null)
			{
				var existingNames = string.Join("\n", toioComponents.Select(t => t.LocalName));
				Debug.LogWarning($"同名のtoioが実機またはUnity上に存在しません\n" +
							   $"Unity側の既存のtoio:\n{existingNames}\n" +
							   $"実機側: {peripheral.device_name}");
				return;
			}

			bleStatus[peripheral] = BLEStatus.Connecting;
			UpdateItemUI(cubeItems[peripheral.device_address], peripheral);

			var cube = await connecter.Connect(peripheral);
			if (cube != null)
			{
				bleStatus[peripheral] = BLEStatus.Connected;
				UpdateItemUI(cubeItems[peripheral.device_address], peripheral);

				// LocalNameが一致するToioコンポーネントに登録
				matchingToio.Register(cube);
				Debug.Log($"接続成功: {peripheral.device_name}をUnity側の{matchingToio.LocalName}に登録しました");
			}
		}
		catch (System.Exception e)
		{
			Debug.LogError($"接続エラー: {e.Message}");
			bleStatus[peripheral] = BLEStatus.Disconnected;
			UpdateItemUI(cubeItems[peripheral.device_address], peripheral);
		}
	}
	private void UpdateItemUI(GameObject item, BLEPeripheralInterface peripheral)
	{
		var status = bleStatus[peripheral];
		var button = item.GetComponent<Button>();
		var text = item.GetComponentInChildren<Text>();

		switch (status)
		{
			case BLEStatus.Disconnected:
				button.interactable = true;
				text.text = $"{peripheral.device_name}: 未接続";
				break;
			case BLEStatus.Connecting:
				button.interactable = false;
				text.text = $"{peripheral.device_name}: 接続中...";
				break;
			case BLEStatus.Connected:
				button.interactable = false;
				text.text = $"{peripheral.device_name}: 接続済み";
				break;
		}
	}

	private void ClearCurrentItems()
	{
		foreach (var item in cubeItems.Values)
		{
			Destroy(item);
		}
		cubeItems.Clear();
		bleStatus.Clear();
	}

	private void OnScanEnd()
	{
		scanButton.interactable = true;
		scanButton.GetComponentInChildren<Text>().text = "スキャン";
	}

	// シーン終了時
	private void OnDestroy()
	{
		var toioComponents = FindObjectsOfType<Toio>();
		foreach (Toio toio in toioComponents)
		{
			connecter.Disconnect(toio.Cube);
		}
	}
}
