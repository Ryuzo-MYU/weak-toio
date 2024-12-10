using UnityEngine;
using TMPro;
using Robot;
using UnityEngine.UI;
using System.Collections.Generic;

public class ToioNameListUI : MonoBehaviour
{
	[SerializeField]
	private TMP_Text[] text;
	[SerializeField] ToioConnector toioConnector;
	private void Start() {
		toioConnector.OnConnectSucceeded += OnConnectSucessed;	
	}
	private void OnConnectSucessed(List<Toio> toios)
	{

	}
}
