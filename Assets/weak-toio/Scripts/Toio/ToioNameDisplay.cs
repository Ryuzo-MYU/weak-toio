using UnityEngine;
using TMPro;
using Robot;

public class ToioNameListUI : MonoBehaviour
{
	[SerializeField]
	private GameObject textUI;
	[SerializeField] ToioConnector toioConnector;
	[SerializeField] GameObject contentParent;
	private void Start()
	{
		toioConnector = GameObject.FindWithTag("ToioConnector").GetComponent<ToioConnector>();
		toioConnector.OnConnectSucceeded += OnConnectSucessed;
	}
	private void OnConnectSucessed()
	{
		var toios = GameObject.FindGameObjectsWithTag("Toio");
		foreach (var toio in toios)
		{
			string name = toio.GetComponent<Toio>().Name;
			TMP_Text text = textUI.GetComponent<TMP_Text>();
			text.text = name;
			Instantiate(textUI, contentParent.transform);
		}
	}
}
