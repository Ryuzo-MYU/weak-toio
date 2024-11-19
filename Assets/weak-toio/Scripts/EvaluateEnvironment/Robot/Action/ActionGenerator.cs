using System.Collections;
using Evaluation;
using toio;

namespace Robot
{
	public abstract class ActionGenerator
	{
		public abstract Action GenerateAction(Result result);
		protected IEnumerator Translate(float dist, double speed){
			// メソッドにアクセスするため、オブジェクトを捏造
			var handle = new CubeHandle(null);
			Movement translateMove = handle.TranslateByDist(dist,speed);
			float intarval = (float)dist / (float)speed;
			
		}
	}
}