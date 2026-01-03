using FoundryRulesAndUnits.Extensions;
using System;

namespace FoundryRulesAndUnits.Units;

[System.Serializable]
[UnitType(UnitFamilyName.AngularVelocity, Description = "Angular velocity measurement")]
public class AngularVelocity : MeasuredValue
{
	// UnitFamily comes from UnitTypeAttribute - no need for redundant property override

	/// <summary>
	/// Constructor with UnitGroup injection - use UnitSystem to create instances
	/// </summary>
	public AngularVelocity(UnitGroup unitGroup) : base(unitGroup)
	{
		if (unitGroup.Family != UnitFamilyName.AngularVelocity)
			throw new ArgumentException($"UnitGroup must be for AngularVelocity family, got {unitGroup.Family}");
	}

	public AngularVelocity Assign(double value, string? units)
	{
		Init(value, units);
		return this;
	}

	public AngularVelocity Assign(AngularVelocity source)
	{
		Init(source.Value(), source.U);
		return this;
	}

	public AngularVelocity Copy()
	{
		var copy = new AngularVelocity(_unitGroup);
		copy.Init(Value(), Internal());
		return copy;
	}

	// Comparison operators
	public static bool operator <(AngularVelocity left, AngularVelocity right) => left.Value() < right.Value();
	public static bool operator >(AngularVelocity left, AngularVelocity right) => left.Value() > right.Value();
	public static bool operator ==(AngularVelocity left, AngularVelocity right) => Math.Abs(left.Value() - right.Value()) < 1e-10;
	public static bool operator !=(AngularVelocity left, AngularVelocity right) => !(left == right);
	
	public override bool Equals(object? obj) => obj is AngularVelocity other && this == other;
	public override int GetHashCode() => Value().GetHashCode();

	// Arithmetic operators
	public static AngularVelocity operator +(AngularVelocity left, AngularVelocity right)
	{
		var result = new AngularVelocity(left._unitGroup);
		result.Init(left.Value() + right.Value(), left.Internal());
		return result;
	}

	public static AngularVelocity operator -(AngularVelocity left, AngularVelocity right)
	{
		var result = new AngularVelocity(left._unitGroup);
		result.Init(left.Value() - right.Value(), left.Internal());
		return result;
	}

	public static AngularVelocity operator *(double scalar, AngularVelocity right)
	{
		var result = new AngularVelocity(right._unitGroup);
		result.Init(scalar * right.Value(), right.Internal());
		return result;
	}

	public static AngularVelocity operator *(AngularVelocity left, double scalar)
	{
		var result = new AngularVelocity(left._unitGroup);
		result.Init(left.Value() * scalar, left.Internal());
		return result;
	}

	public static AngularVelocity operator /(AngularVelocity left, double scalar)
	{
		var result = new AngularVelocity(left._unitGroup);
		result.Init(left.Value() / scalar, left.Internal());
		return result;
	}

	public static double operator /(AngularVelocity left, AngularVelocity right) => left.Value() / right.Value();
}
