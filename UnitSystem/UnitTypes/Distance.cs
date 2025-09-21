using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units;

[System.Serializable]
[UnitType(UnitFamilyName.Length, Description = "Distance measurement")]
public class Distance : MeasuredValue
{
	// UnitFamily comes from UnitTypeAttribute - no need for redundant property override
	#region Constructors and Factory Methods

	/// <summary>
	/// Constructor with UnitGroup injection - preferred for new code
	/// </summary>
	public Distance(UnitGroup unitGroup) : base(unitGroup)
	{
		if (unitGroup.Family != UnitFamilyName.Length)
			throw new ArgumentException($"Expected UnitGroup for Length, got {unitGroup.Family}");
	}

	#endregion

	#region Operators


	public static Distance operator +(Distance left, Distance right)
	{
		var result = new Distance(left.UnitGroup);
		result.Init(left.Value() + right.Value(), left.Internal());
		return result;
	}

	public static Distance operator -(Distance left, Distance right)
	{
		var result = new Distance(left.UnitGroup);
		result.Init(left.Value() - right.Value(), left.Internal());
		return result;
	}

	public static Distance operator *(Distance left, double scalar)
	{
		var result = new Distance(left.UnitGroup);
		result.Init(left.Value() * scalar, left.Internal());
		return result;
	}

	public static Distance operator *(double scalar, Distance right)
	{
		var result = new Distance(right.UnitGroup);
		result.Init(scalar * right.Value(), right.Internal());
		return result;
	}

	public static Distance operator /(Distance left, double scalar)
	{
		var result = new Distance(left.UnitGroup);
		result.Init(left.Value() / scalar, left.Internal());
		return result;
	}

	public static double operator /(Distance left, Distance right) => left.Value() / right.Value();

	#endregion
}



