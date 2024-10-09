using NUnit.Framework;  // NUnitのテストフレームワークを使用
using UnityEngine.TestTools;  // Unity Test Frameworkを使用
using UnityEngine;
using System.Collections;

public class JsonLoaderTests
{
    private JsonLoader jsonLoader;

    [SetUp]
    public void Setup()
    {
        // JsonLoader のインスタンスを作成
        jsonLoader = new JsonLoader();
    }

    [Test]
    public void LoadDataType_ValidKey_ReturnsCorrectDataType()
    {
        // テスト用のJSONファイルをResourcesフォルダに配置しておき、そこでのデータが返されるかを確認
        string result = jsonLoader.LoadDataType("Temperature");
        
        // "Temperature" のデータタイプが "float" であることを確認
        Assert.AreEqual("float", result);
    }

    [Test]
    public void LoadDataType_InvalidKey_ReturnsEmptyString()
    {
        // 存在しないキーを渡した場合は空文字が返されるかを確認
        string result = jsonLoader.LoadDataType("InvalidKey");
        Assert.AreEqual(string.Empty, result);
    }

    [Test]
    public void LoadDataType_FileNotFound_ReturnsError()
    {
        // 存在しないファイルパスを使うなどの異常系テストも含めることが可能
        string originalPath = JsonLoader.JSON_FILE_PATH;
        JsonLoader.JSON_FILE_PATH = "Assets/InvalidPath/DataType.json";  // 存在しないファイルパス
        string result = jsonLoader.LoadDataType("Temperature");
        Assert.AreEqual(string.Empty, result);
        JsonLoader.JSON_FILE_PATH = originalPath;  // 元に戻す
    }
}
