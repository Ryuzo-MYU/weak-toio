using UnityEngine;
using TMPro;
using Robot;
using System.Collections.Generic;

public class ToioNameListUI : MonoBehaviour
{
	[SerializeField]
	private GameObject textUI;
	[SerializeField] ToioConnector toioConnector;
	[SerializeField] GameObject contentParent;
	private void Start()
	{
		toioConnector.OnConnectSucceeded += OnConnectSucessed;
	}
	private void OnConnectSucessed(List<Toio> toios)
	{
		foreach (Toio toio in toios)
		{
			string name = toio.Name;
			TMP_Text text = textUI.GetComponent<TMP_Text>();
			text.text = name;
			Instantiate(textUI, contentParent.transform);
		}
	}
}
