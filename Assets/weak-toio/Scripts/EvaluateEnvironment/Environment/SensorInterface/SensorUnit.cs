using Evaluation;

namespace Environment
{
	public interface ISensorUnit
	{
		public EnvType GetEnvType();
		public void Start();
		public void Update();
	}
}