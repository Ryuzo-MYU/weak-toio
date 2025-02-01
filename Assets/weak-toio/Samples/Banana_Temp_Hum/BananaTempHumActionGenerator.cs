using ActionLibrary;
using Evaluation;

namespace ActionGenerate
{
	public class BananaTempHumActionGenerator : ActionGenerator
	{
		/// スコア解釈
		/// 0：最適な環境
		/// 0-10：軽度の環境逸脱
		/// 10-20：中度の環境逸脱
		/// 20以上：深刻な環境逸脱
		BoundaryRange normalRange = new BoundaryRange(0, 10);    // 正常範囲
		BoundaryRange warningRange = new BoundaryRange(10, 20);   // 警戒範囲

		protected override Action GenerateAction(Result result)
		{
			float score = result.Score;

			if (normalRange.isWithInRange(score))
			{
				return actionLibrary.Banana_Temp_Hum_Normal();
			}
			else if (warningRange.isWithInRange(score))
			{
				return actionLibrary.Banana_Temp_Hum_Warning();
			}
			else
			{
				return actionLibrary.Banana_Temp_Hum_Rotting();
			}
		}
	}
}