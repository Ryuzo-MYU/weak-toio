using System.IO;
using EvaluateEnvironment;
using UnityEngine;

public class Temperature : EnvData<float>
{
	public Temperature(float data) : base(data)
	{
		this.data = data;
		this.dataType = LoadDataType();
	}
}
