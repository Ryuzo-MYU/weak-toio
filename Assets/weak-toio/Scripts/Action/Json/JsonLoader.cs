using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonLoader
{
	public static string JSON_FILE_PATH = "Assets/weak-toio/Resources/DataType.json";

	public string LoadDataType(string dataName)
	{
		TextAsset jsonFile = Resources.Load<TextAsset>(Path.GetFileNameWithoutExtension(JSON_FILE_PATH));

		if (jsonFile == null)
		{
			Debug.LogError($"JSON file not found at {JSON_FILE_PATH}");
			return string.Empty;
		}

		// 全体のJSONを直接辞書形式でパース
		var allData = JsonUtility.FromJson<Dictionary<string, DataTypeConfig>>(jsonFile.text);

		if (allData.ContainsKey(dataName))
		{
			var dataConfig = allData[dataName];
			Debug.Log($"DataType: {dataConfig.DataType}, Unit: {dataConfig.Unit}");
			return dataConfig.DataType;
		}

		Debug.LogError($"DataType for {dataName} not found in JSON.");
		return string.Empty;
	}

	[System.Serializable]
	public class DataTypeConfig
	{
		public string DataType;  // データ型
		public string Unit;      // 単位
	}
}

