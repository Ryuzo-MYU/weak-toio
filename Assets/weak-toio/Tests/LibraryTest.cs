using NUnit.Framework;
using toio;

public class LibraryImportTest
{
	[Test]
	public void BasicPassTest()
	{
		// 何もしないテストが成功するかを確認
		Assert.Pass();
	}

	[Test]
	public void LibraryImportTest_Passes()
	{
		// ライブラリが正しくインポートされていることを確認するテスト
		// ここに実際のライブラリのオブジェクトなどを初期化して確認できます
		try
		{
			// 例: toio SDKのライブラリを参照してみる
			var cm = new CubeManager(); // この部分をあなたのライブラリのクラスに変更
			Assert.IsNotNull(cm, "ライブラリが正しくインポートされていません。");
		}
		catch (System.Exception ex)
		{
			Assert.Fail($"ライブラリのインポートに問題があります: {ex.Message}");
		}
	}
}
