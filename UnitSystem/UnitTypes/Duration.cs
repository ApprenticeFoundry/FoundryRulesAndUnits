using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundryRulesAndUnits.Units;

[System.Serializable]
[UnitType(UnitFamilyName.Duration, Description = "Duration measurement")]
public class Duration : MeasuredValue
{
	// UnitFamily comes from UnitTypeAttribute - no need for redundant property override
	
	#region Constructors and Factory Methods

	/// <summary>
	/// Constructor with UnitGroup injection - preferred for new code
	/// </summary>
	public Duration(UnitGroup unitGroup) : base(unitGroup)
	{
		if (unitGroup.Family != UnitFamilyName.Duration)
			throw new ArgumentException($"Expected UnitGroup for Duration, got {unitGroup.Family}");
	}

	#endregion

	#region Operators

	public static Duration operator +(Duration left, Duration right)
	{
		var result = new Duration(left.UnitGroup);
		result.Init(left.Value() + right.Value(), left.Internal());
		return result;
	}

	public static Duration operator -(Duration left, Duration right)
	{
		var result = new Duration(left.UnitGroup);
		result.Init(left.Value() - right.Value(), left.Internal());
		return result;
	}

	public static Duration operator *(Duration left, double scalar)
	{
		var result = new Duration(left.UnitGroup);
		result.Init(left.Value() * scalar, left.Internal());
		return result;
	}

	public static Duration operator *(double scalar, Duration right)
	{
		var result = new Duration(right.UnitGroup);
		result.Init(scalar * right.Value(), right.Internal());
		return result;
	}

	public static Duration operator /(Duration left, double scalar)
	{
		var result = new Duration(left.UnitGroup);
		result.Init(left.Value() / scalar, left.Internal());
		return result;
	}

	public static double operator /(Duration left, Duration right) => left.Value() / right.Value();

	// Optional comparison operators
	public static bool operator <=(Duration left, Duration right) => left.Value() <= right.Value();
	public static bool operator >=(Duration left, Duration right) => left.Value() >= right.Value();

	#endregion
}



