using FoundryRulesAndUnits.Extensions;
using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units;

[System.Serializable]
[UnitType(UnitFamilyName.Energy, Description = "Energy measurement")]
public class Energy : MeasuredValue
{
	// UnitFamily comes from UnitTypeAttribute - no need for redundant property override

	/// <summary>
	/// Constructor with UnitGroup injection - use UnitSystem to create instances
	/// </summary>
	public Energy(UnitGroup unitGroup) : base(unitGroup)
	{
		if (unitGroup.Family != UnitFamilyName.Energy)
			throw new ArgumentException($"UnitGroup must be for Energy family, got {unitGroup.Family}");
	}

	public Energy Assign(double value, string? units)
	{
		Init(value, units); // Base class handles everything!
		return this;
	}

	public Energy Assign(Energy source)
	{
		Init(source.Value(), source.U); // Base class handles everything!
		return this;
	}

	public Energy Copy()
	{
		var copy = new Energy(_unitGroup);
		copy.Init(Value(), Internal());
		return copy;
	}

	// Comparison operators
	public static bool operator <(Energy left, Energy right) => left.Value() < right.Value();
	public static bool operator >(Energy left, Energy right) => left.Value() > right.Value();
	public static bool operator ==(Energy left, Energy right) => Math.Abs(left.Value() - right.Value()) < 1e-10;
	public static bool operator !=(Energy left, Energy right) => !(left == right);
	
	// Override Equals and GetHashCode to be consistent with == operator
	public override bool Equals(object? obj) => obj is Energy other && this == other;
	public override int GetHashCode() => Value().GetHashCode();

	// Arithmetic operators
	public static Energy operator +(Energy left, Energy right)
	{
		var result = new Energy(left._unitGroup);
		result.Init(left.Value() + right.Value(), left.Internal());
		return result;
	}

	public static Energy operator -(Energy left, Energy right)
	{
		var result = new Energy(left._unitGroup);
		result.Init(left.Value() - right.Value(), left.Internal());
		return result;
	}

	public static Energy operator *(double scalar, Energy right)
	{
		var result = new Energy(right._unitGroup);
		result.Init(scalar * right.Value(), right.Internal());
		return result;
	}

	public static Energy operator *(Energy left, double scalar)
	{
		var result = new Energy(left._unitGroup);
		result.Init(left.Value() * scalar, left.Internal());
		return result;
	}

	public static Energy operator /(Energy left, double scalar)
	{
		var result = new Energy(left._unitGroup);
		result.Init(left.Value() / scalar, left.Internal());
		return result;
	}

	public static double operator /(Energy left, Energy right) => left.Value() / right.Value();
}