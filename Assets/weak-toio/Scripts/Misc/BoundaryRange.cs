using System;

[Serializable]
public struct BoundaryRange
{
	public float UpperLimit;
	public float LowerLimit;

	/// <summary>
	/// 値の範囲が1つしかない場合 
	/// </summary>
	/// <param name="targotParam"></param>
	public BoundaryRange(float targotParam)
	{
		UpperLimit = targotParam;
		LowerLimit = targotParam;
	}
	public BoundaryRange(float lower, float upper)
	{
		LowerLimit = lower;
		UpperLimit = upper;
	}
	public bool isWithInRange(float subject)
	{
		// 値域に入る値が一つしかなければ、上限下限どちらかに等しいか判定
		if (UpperLimit == LowerLimit) return subject == UpperLimit;

		// 複数あれば通常通り判定
		return subject > LowerLimit && subject < UpperLimit;
	}
}