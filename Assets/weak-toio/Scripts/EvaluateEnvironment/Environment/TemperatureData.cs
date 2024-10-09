using System.IO;
using EvaluateEnvironment;
using UnityEngine;

public class Temperature : EnvData<float>
{
	public Temperature(float data) : base(data, "Temperature")
	{
		this.data = data;
		JsonLoader jsonLoader = new JsonLoader();
		this.dataType = jsonLoader.LoadDataType("Temperature");
	}
}
