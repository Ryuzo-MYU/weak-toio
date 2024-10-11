using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace EvaluateEnvironment
{
	public abstract class EnvData
	{
		protected float data;
		protected EnvData(float data)
		{
			this.data = data;
		}
		public float Data => data;
	}
}