using NUnit.Framework;
using Moq;
using EvaluateEnvironment;
using UnityEngine;
using System.Collections.Generic;

[TestFixture]
public class TemperatureTest
{
    // JSONデータを読み込む部分をモックする
    private Mock<EnvData<float>> mockEnvData;

    [SetUp]
    public void SetUp()
    {
        // EnvDataのモックを作成
        mockEnvData = new Mock<EnvData<float>>(25.5f) { CallBase = true };
        
        // LoadDataTypeメソッドをモックして、テスト用のデータタイプを返すように設定
        mockEnvData.Setup(m => m.LoadDataType()).Returns("Temperature");
    }

    [Test]
    public void Temperature_CreatesInstanceCorrectly()
    {
        // モックしたEnvDataをTemperatureクラスに渡す
        var temperature = new Temperature(25.5f);

        // 期待する値であるか検証
        Assert.AreEqual(25.5f, temperature.Data, "Temperature data should be set correctly.");
        Assert.AreEqual("Temperature", temperature.DataType, "DataType should be 'Temperature'.");
    }

    [Test]
    public void Temperature_HandlesInvalidJson()
    {
        // JSONデータが無効な場合の動作を確認
        mockEnvData.Setup(m => m.LoadDataType()).Returns(string.Empty);

        var temperature = new Temperature(25.5f);

        // DataTypeが空であることを確認
        Assert.AreEqual(string.Empty, temperature.DataType, "DataType should be empty when JSON is invalid.");
    }
}

