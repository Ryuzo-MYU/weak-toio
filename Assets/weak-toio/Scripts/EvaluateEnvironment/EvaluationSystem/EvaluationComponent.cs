using System;
using System.Collections;
using UnityEngine;
using EvaluateEnvironment;
public class EvaluationComponent : MonoBehaviour
{
	TemperatureEvaluation temp;
	M5DataReceiver sensor;
	Result result;
	[SerializeField] float interval;
	Action dummyUpdate;
	private void Awake()
	{
		sensor.serialHandler = gameObject.GetComponent<SerialHandler>();
		sensor = new M5DataReceiver();
		sensor.Awake();
	}
	private void Start()
	{
		// 初期化
		temp = new TemperatureEvaluation();
		sensor.Start();
		result = temp.Evaluate(sensor);


		// コルーチン設定
		dummyUpdate += DummyUpdateAndOutputMessage;
		StartCoroutine(GenerateDummyData(interval, dummyUpdate));
	}

	/// <summary>
	/// 任意の関数を任意の間隔で実行するコルーチン
	/// </summary>
	/// <param name="interval">任意の間隔。x を渡すと、x秒ごとに実行</param>
	/// <param name="action">実行したい関数。このコルーチンを使う前に、事前にAction型変数に関数を追加する必要がある</param>
	/// <returns></returns>
	private IEnumerator GenerateDummyData(float interval, Action action)
	{
		while (true)
		{
			action?.Invoke();
			yield return new WaitForSeconds(interval);
		}
	}
	/// <summary>
	/// ダミーデータの更新とデバッグ出力
	/// </summary> 	
	private void DummyUpdateAndOutputMessage()
	{
		sensor.Update();
		result = temp.Evaluate(sensor);
		Debug.Log(result.Message);
	}
}
