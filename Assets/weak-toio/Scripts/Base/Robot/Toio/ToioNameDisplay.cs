using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Robot;
using UnityEngine.UI;

public class ToioNameListUI : MonoBehaviour
{
	[SerializeField]
	private GameObject listItemPrefab;

	[SerializeField]
	private RectTransform contentParent;

	[SerializeField]
	private ToioConnector toioConnector;

	private List<GameObject> listItems = new List<GameObject>();

	private void Awake()
	{
		// ContentのLayout設定
		SetupContentLayout();
	}

	private void Start()
	{
		toioConnector.OnConnectSuccessed += OnToioConnected;
	}

	private void SetupContentLayout()
	{
		if (contentParent == null) return;

		// Content の VerticalLayoutGroup の設定
		var verticalLayout = contentParent.GetComponent<VerticalLayoutGroup>();
		if (verticalLayout == null)
		{
			verticalLayout = contentParent.gameObject.AddComponent<VerticalLayoutGroup>();
		}
		verticalLayout.spacing = 5f;
		verticalLayout.padding = new RectOffset(5, 5, 5, 5);
		verticalLayout.childAlignment = TextAnchor.UpperRight;
		verticalLayout.childControlHeight = true;
		verticalLayout.childControlWidth = true;
		verticalLayout.childForceExpandHeight = false;
		verticalLayout.childForceExpandWidth = true;

		// Content の ContentSizeFitter の設定
		var contentSizeFitter = contentParent.GetComponent<ContentSizeFitter>();
		if (contentSizeFitter == null)
		{
			contentSizeFitter = contentParent.gameObject.AddComponent<ContentSizeFitter>();
		}
		contentSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
	}

	private void OnToioConnected(List<Toio> toios)
	{
		ClearList();
		foreach (string toioName in toioConnector.toioNames)
		{
			CreateListItem(toioName);
		}
	}

	private void CreateListItem(string toioName)
	{
		GameObject listItem = Instantiate(listItemPrefab, contentParent);

		// ListItemのレイアウト設定
		var itemRect = listItem.GetComponent<RectTransform>();
		if (itemRect != null)
		{
			itemRect.anchorMin = new Vector2(0, 0);
			itemRect.anchorMax = new Vector2(1, 0);
			itemRect.pivot = new Vector2(1, 0);
		}

		var layoutElement = listItem.GetComponent<LayoutElement>();
		if (layoutElement == null)
		{
			layoutElement = listItem.AddComponent<LayoutElement>();
		}
		layoutElement.minHeight = 30f;
		layoutElement.flexibleWidth = 1;

		TMP_Text nameText = listItem.GetComponentInChildren<TMP_Text>();
		if (nameText != null)
		{
			nameText.text = $"Toio: {toioName}";
			nameText.alignment = TextAlignmentOptions.Right;
		}

		listItems.Add(listItem);
	}

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
		if (toioConnector != null)
		{
			toioConnector.OnConnectSuccessed -= OnToioConnected;
		}
	}
}
