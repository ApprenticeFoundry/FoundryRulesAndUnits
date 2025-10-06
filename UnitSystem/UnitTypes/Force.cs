using FoundryRulesAndUnits.Extensions;
using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units;

[System.Serializable]
[UnitType(UnitFamilyName.Force, Description = "Force measurement")]
public class Force : MeasuredValue
{
	// UnitFamily comes from UnitTypeAttribute - no need for redundant property override
	// NO backward compatibility constructors - use UnitFactory.CreateForce() instead

	/// <summary>
	/// Constructor with UnitGroup injection - use UnitFactory to create instances
	/// </summary>
	public Force(UnitGroup unitGroup) : base(unitGroup)
	{
		if (unitGroup.Family != UnitFamilyName.Force)
			throw new ArgumentException($"UnitGroup must be for Force family, got {unitGroup.Family}");
	}

	public Force Assign(double value, string? units)
	{
		Init(value, units); // Base class handles everything!
		return this;
	}

	public Force Assign(Force source)
	{
		Init(source.Value(), source.U); // Base class handles everything!
		return this;
	}

	public Force Copy()
	{
		var copy = new Force(_unitGroup);
		copy.Init(Value(), Internal());
		return copy;
	}

	// Static factory methods removed - use UnitFactory.CreateForce() instead

	// Comparison operators
	public static bool operator <(Force left, Force right) => left.Value() < right.Value();
	public static bool operator >(Force left, Force right) => left.Value() > right.Value();
	public static bool operator ==(Force left, Force right) => Math.Abs(left.Value() - right.Value()) < 1e-10;
	public static bool operator !=(Force left, Force right) => !(left == right);
	
	// Override Equals and GetHashCode to be consistent with == operator
	public override bool Equals(object? obj) => obj is Force other && this == other;
	public override int GetHashCode() => Value().GetHashCode();

	// Arithmetic operators
	public static Force operator +(Force left, Force right)
	{
		var result = new Force(left._unitGroup);
		result.Init(left.Value() + right.Value(), left.Internal());
		return result;
	}

	public static Force operator -(Force left, Force right)
	{
		var result = new Force(left._unitGroup);
		result.Init(left.Value() - right.Value(), left.Internal());
		return result;
	}

	public static Force operator *(double scalar, Force right)
	{
		var result = new Force(right._unitGroup);
		result.Init(scalar * right.Value(), right.Internal());
		return result;
	}

	public static Force operator *(Force left, double scalar)
	{
		var result = new Force(left._unitGroup);
		result.Init(left.Value() * scalar, left.Internal());
		return result;
	}

	public static Force operator /(Force left, double scalar)
	{
		var result = new Force(left._unitGroup);
		result.Init(left.Value() / scalar, left.Internal());
		return result;
	}

	public static double operator /(Force left, Force right) => left.Value() / right.Value();
	
	// ============================================================================
	// PHASE 4A: CROSS-FAMILY OPERATIONS - Force-specific operators
	// ============================================================================
	
	/// <summary>
	/// Force ÷ Area → Pressure (Pressure = Force ÷ Area)
	/// Example: 100N ÷ 10 m² = 10 Pa
	/// </summary>
	public static MeasuredValue operator /(Force left, Area right)
	{
		var factory = new UnitFactory(left._unitGroup.SystemType);
		var pressureValue = left.BaseValue() / right.BaseValue(); // N ÷ m² = Pa
		return factory.CreateMeasuredValue(UnitFamilyName.Pressure, pressureValue, "Pa");
	}
}


