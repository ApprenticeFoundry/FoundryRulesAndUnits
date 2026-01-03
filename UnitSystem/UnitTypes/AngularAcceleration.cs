using FoundryRulesAndUnits.Extensions;
using System;

namespace FoundryRulesAndUnits.Units;

[System.Serializable]
[UnitType(UnitFamilyName.AngularAcceleration, Description = "Angular acceleration measurement")]
public class AngularAcceleration : MeasuredValue
{
	// UnitFamily comes from UnitTypeAttribute - no need for redundant property override

	/// <summary>
	/// Constructor with UnitGroup injection - use UnitSystem to create instances
	/// </summary>
	public AngularAcceleration(UnitGroup unitGroup) : base(unitGroup)
	{
		if (unitGroup.Family != UnitFamilyName.AngularAcceleration)
			throw new ArgumentException($"UnitGroup must be for AngularAcceleration family, got {unitGroup.Family}");
	}

	public AngularAcceleration Assign(double value, string? units)
	{
		Init(value, units);
		return this;
	}

	public AngularAcceleration Assign(AngularAcceleration source)
	{
		Init(source.Value(), source.U);
		return this;
	}

	public AngularAcceleration Copy()
	{
		var copy = new AngularAcceleration(_unitGroup);
		copy.Init(Value(), Internal());
		return copy;
	}

	// Comparison operators
	public static bool operator <(AngularAcceleration left, AngularAcceleration right) => left.Value() < right.Value();
	public static bool operator >(AngularAcceleration left, AngularAcceleration right) => left.Value() > right.Value();
	public static bool operator ==(AngularAcceleration left, AngularAcceleration right) => Math.Abs(left.Value() - right.Value()) < 1e-10;
	public static bool operator !=(AngularAcceleration left, AngularAcceleration right) => !(left == right);
	
	public override bool Equals(object? obj) => obj is AngularAcceleration other && this == other;
	public override int GetHashCode() => Value().GetHashCode();

	// Arithmetic operators
	public static AngularAcceleration operator +(AngularAcceleration left, AngularAcceleration right)
	{
		var result = new AngularAcceleration(left._unitGroup);
		result.Init(left.Value() + right.Value(), left.Internal());
		return result;
	}

	public static AngularAcceleration operator -(AngularAcceleration left, AngularAcceleration right)
	{
		var result = new AngularAcceleration(left._unitGroup);
		result.Init(left.Value() - right.Value(), left.Internal());
		return result;
	}

	public static AngularAcceleration operator *(double scalar, AngularAcceleration right)
	{
		var result = new AngularAcceleration(right._unitGroup);
		result.Init(scalar * right.Value(), right.Internal());
		return result;
	}

	public static AngularAcceleration operator *(AngularAcceleration left, double scalar)
	{
		var result = new AngularAcceleration(left._unitGroup);
		result.Init(left.Value() * scalar, left.Internal());
		return result;
	}

	public static AngularAcceleration operator /(AngularAcceleration left, double scalar)
	{
		var result = new AngularAcceleration(left._unitGroup);
		result.Init(left.Value() / scalar, left.Internal());
		return result;
	}

	public static double operator /(AngularAcceleration left, AngularAcceleration right) => left.Value() / right.Value();
}
