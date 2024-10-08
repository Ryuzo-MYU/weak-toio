using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace EvaluateEnvironment
{
    public abstract class EnvData<T>
    {
        protected T data;
        protected string dataType;
        public static string JSON_FILE_PATH = "Assets/weak-toio/Resources/DataType.json";

        // コンストラクタでデータを受け取り、データタイプを取得
        protected EnvData(T data)
        {
            this.data = data;
            this.dataType = LoadDataType();
        }

        public T Data => data;
        public string DataType => dataType;

        // クラス名に基づいてデータタイプをロード
        public string LoadDataType()
        {
            TextAsset jsonFile = Resources.Load<TextAsset>(Path.GetFileNameWithoutExtension(JSON_FILE_PATH));

            if (jsonFile == null)
            {
                Debug.LogError($"JSON file not found at {JSON_FILE_PATH}");
                return string.Empty;
            }

            // 全体のJSONを辞書形式でパース
            var allData = JsonUtility.FromJson<Dictionary<string, DataTypeConfig>>(jsonFile.text);

            // クラス名（Temperature, Humidityなど）をキーとして使用
            string className = GetType().Name;

            if (allData.ContainsKey(className))
            {
                return allData[className].dataType;
            }

            Debug.LogError($"DataType for {className} not found in JSON.");
            return string.Empty;
        }
    }

    // データタイプ構成の定義
    [System.Serializable]
    public class DataTypeConfig
    {
        public string dataType;
    }
}
