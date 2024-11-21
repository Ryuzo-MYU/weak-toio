using System.Collections;
using System.Threading.Tasks;
using Evaluation;
using toio;
using UnityEngine;

namespace Robot
{
	public class Toio : IToioMovement
	{
		int id;
		Cube cube;
		CubeHandle handle;
		EnvType appointedType;
		Action[] _action;
		
	}
}