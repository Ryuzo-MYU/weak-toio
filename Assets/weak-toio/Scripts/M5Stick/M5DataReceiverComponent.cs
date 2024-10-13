using UnityEngine;
public class M5DataReceiverComponent : MonoBehaviour
{
	M5DataReceiver m5DataReceiver;
	void Start()
	{
		m5DataReceiver = new M5DataReceiver();
		//信号を受信したときに、そのメッセージの処理を行う
		m5DataReceiver.serialHandler.OnDataReceived += m5DataReceiver.OnDataReceived;
	}
}