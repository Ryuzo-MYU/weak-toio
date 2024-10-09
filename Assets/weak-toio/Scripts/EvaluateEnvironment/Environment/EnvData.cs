using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace EvaluateEnvironment
{
	public abstract class EnvData<T>
	{
		protected T data;
		protected string dataType;
		// コンストラクタでデータを受け取り、データタイプを取得
		protected EnvData(T data, string dataType)
		{
			this.data = data;
			JsonLoader jsonLoader = new JsonLoader();
			this.dataType = jsonLoader.LoadDataType("");
		}

		public T Data => data;
		public string DataType => dataType;
	}
}