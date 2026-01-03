using FoundryRulesAndUnits.Extensions;
using System;

namespace FoundryRulesAndUnits.Units;

[System.Serializable]
[UnitType(UnitFamilyName.Torque, Description = "Torque measurement")]
public class Torque : MeasuredValue
{
	// UnitFamily comes from UnitTypeAttribute - no need for redundant property override

	/// <summary>
	/// Constructor with UnitGroup injection - use UnitSystem to create instances
	/// </summary>
	public Torque(UnitGroup unitGroup) : base(unitGroup)
	{
		if (unitGroup.Family != UnitFamilyName.Torque)
			throw new ArgumentException($"UnitGroup must be for Torque family, got {unitGroup.Family}");
	}

	public Torque Assign(double value, string? units)
	{
		Init(value, units);
		return this;
	}

	public Torque Assign(Torque source)
	{
		Init(source.Value(), source.U);
		return this;
	}

	public Torque Copy()
	{
		var copy = new Torque(_unitGroup);
		copy.Init(Value(), Internal());
		return copy;
	}

	// Comparison operators
	public static bool operator <(Torque left, Torque right) => left.Value() < right.Value();
	public static bool operator >(Torque left, Torque right) => left.Value() > right.Value();
	public static bool operator ==(Torque left, Torque right) => Math.Abs(left.Value() - right.Value()) < 1e-10;
	public static bool operator !=(Torque left, Torque right) => !(left == right);
	
	public override bool Equals(object? obj) => obj is Torque other && this == other;
	public override int GetHashCode() => Value().GetHashCode();

	// Arithmetic operators
	public static Torque operator +(Torque left, Torque right)
	{
		var result = new Torque(left._unitGroup);
		result.Init(left.Value() + right.Value(), left.Internal());
		return result;
	}

	public static Torque operator -(Torque left, Torque right)
	{
		var result = new Torque(left._unitGroup);
		result.Init(left.Value() - right.Value(), left.Internal());
		return result;
	}

	public static Torque operator *(double scalar, Torque right)
	{
		var result = new Torque(right._unitGroup);
		result.Init(scalar * right.Value(), right.Internal());
		return result;
	}

	public static Torque operator *(Torque left, double scalar)
	{
		var result = new Torque(left._unitGroup);
		result.Init(left.Value() * scalar, left.Internal());
		return result;
	}

	public static Torque operator /(Torque left, double scalar)
	{
		var result = new Torque(left._unitGroup);
		result.Init(left.Value() / scalar, left.Internal());
		return result;
	}

	public static double operator /(Torque left, Torque right) => left.Value() / right.Value();
}
