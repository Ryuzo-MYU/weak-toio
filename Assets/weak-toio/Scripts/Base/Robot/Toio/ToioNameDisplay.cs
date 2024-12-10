using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Robot;

public class ToioNameListUI : MonoBehaviour
{
    [SerializeField]
    private GameObject listItemPrefab; // UIリストアイテムのプレハブ

    [SerializeField]
    private Transform contentParent; // スクロールビューのContent

    [SerializeField]
    private ToioConnector toioConnector; // ToioConnectorへの参照

    private List<GameObject> listItems = new List<GameObject>();

    private void Start()
    {
        // ToioConnectorの接続完了イベントにリスナーを追加
        toioConnector.OnConnectSuccessed += OnToioConnected;
    }

    // toioの接続が完了したときに呼ばれるメソッド
    private void OnToioConnected(List<Toio> toios)
    {
        // 既存のリストをクリア
        ClearList();

        // 接続されているtoioの名前を表示
        foreach (string toioName in toioConnector.toioNames)
        {
            CreateListItem(toioName);
        }
    }

    // リストアイテムを生成
    private void CreateListItem(string toioName)
    {
        GameObject listItem = Instantiate(listItemPrefab, contentParent);
        TMP_Text nameText = listItem.GetComponentInChildren<TMP_Text>();
        
        if (nameText != null)
        {
            nameText.text = $"Toio: {toioName}";
        }
        
        listItems.Add(listItem);
    }

    // リストをクリア
    private void ClearList()
    {
        foreach (var item in listItems)
        {
            Destroy(item);
        }
        listItems.Clear();
    }

    private void OnDestroy()
    {
        // イベントリスナーの解除
        if (toioConnector != null)
        {
            toioConnector.OnConnectSuccessed -= OnToioConnected;
        }
    }
}
