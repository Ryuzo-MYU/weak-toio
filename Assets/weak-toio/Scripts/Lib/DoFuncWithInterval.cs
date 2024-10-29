using System;
using System.Collections;
using UnityEngine;

public static class MyLibrary
{
	/// <summary>
	/// 任意の関数を任意の間隔で実行するコルーチン
	/// </summary>
	/// <param name="interval">任意の間隔。x を渡すと、x秒ごとに実行</param>
	/// <param name="action">実行したい関数。このコルーチンを使う前に、事前にAction型変数に関数を追加する必要がある</param>
	/// <returns></returns>
	public static IEnumerator DoFuncWithInterval(float interval, Action action)
	{
		while (true)
		{
			action?.Invoke();
			yield return new WaitForSeconds(interval);
		}
	}
}