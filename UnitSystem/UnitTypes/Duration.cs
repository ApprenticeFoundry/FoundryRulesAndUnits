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

	// ============================================================================
	// PHASE 4A: CROSS-FAMILY OPERATIONS - Duration-specific operators
	// ============================================================================
	
	/// <summary>
	/// Duration × Speed → Length (Distance = Time × Speed)
	/// Example: 5s × 10 m/s = 50m
	/// </summary>
	public static MeasuredValue operator *(Duration left, Speed right)
	{
		var factory = new UnitFactory(left._unitGroup.SystemType);
		var lengthValue = left.BaseValue() * right.BaseValue(); // s × (m/s) = m
		return factory.CreateMeasuredValue(UnitFamilyName.Length, lengthValue, "m");
	}
	
	/// <summary>
	/// Duration × Power → Energy (Energy = Time × Power)
	/// Example: 10s × 100W = 1000J
	/// </summary>
	public static MeasuredValue operator *(Duration left, Power right)
	{
		var factory = new UnitFactory(left._unitGroup.SystemType);
		var energyValue = left.BaseValue() * right.BaseValue(); // s × W = J
		return factory.CreateMeasuredValue(UnitFamilyName.Energy, energyValue, "J");
	}

	// Optional comparison operators
	public static bool operator <=(Duration left, Duration right) => left.Value() <= right.Value();
	public static bool operator >=(Duration left, Duration right) => left.Value() >= right.Value();

	#endregion
}



