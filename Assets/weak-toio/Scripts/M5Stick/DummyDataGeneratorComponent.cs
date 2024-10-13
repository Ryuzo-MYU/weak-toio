using UnityEngine;
public class DummyDataGeneratorComponent : MonoBehaviour
{
	private float timer = 0f;
	// データ送信の間隔（秒）
	public float sendInterval = 1.0f;
	public DummyDataGenerator dummyDataGenerator;
	void Start()
	{
		dummyDataGenerator = new DummyDataGenerator();
	}

	void Update()
	{
		UpdateDummyData();
		timer += Time.deltaTime;

		// 一定間隔でダミーデータを生成
		if (timer >= sendInterval)
		{
			dummyDataGenerator.SendDummyData();
			timer = 0f;  // タイマーリセット
		}
	}
	void UpdateDummyData()
	{
		// 文字列の連結のためのStringBuilderを使用（効率的な文字列操作のため）
		System.Text.StringBuilder debugMessage = new System.Text.StringBuilder();
		debugMessage.AppendLine("Generated Dummy Data");

		// SensorInfoの各フィールドを列挙し、名前と値を文字列に追加
		foreach (var data in dummyDataGenerator.sensorInfo)
		{
			debugMessage.AppendLine(data.ToString());
		}

		// 1回のDebug.Logで全ての情報を出力
		Debug.Log(debugMessage.ToString());
	}
}