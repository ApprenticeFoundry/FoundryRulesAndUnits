using FoundryRulesAndUnits.Extensions;
using System;

namespace FoundryRulesAndUnits.Units;

[System.Serializable]
[UnitType(UnitFamilyName.Inductance, Description = "Inductance measurement")]
public class Inductance : MeasuredValue
{
	// UnitFamily comes from UnitTypeAttribute - no need for redundant property override

	/// <summary>
	/// Constructor with UnitGroup injection - use UnitSystem to create instances
	/// </summary>
	public Inductance(UnitGroup unitGroup) : base(unitGroup)
	{
		if (unitGroup.Family != UnitFamilyName.Inductance)
			throw new ArgumentException($"UnitGroup must be for Inductance family, got {unitGroup.Family}");
	}

	public Inductance Assign(double value, string? units)
	{
		Init(value, units);
		return this;
	}

	public Inductance Assign(Inductance source)
	{
		Init(source.Value(), source.U);
		return this;
	}

	public Inductance Copy()
	{
		var copy = new Inductance(_unitGroup);
		copy.Init(Value(), Internal());
		return copy;
	}

	// Comparison operators
	public static bool operator <(Inductance left, Inductance right) => left.Value() < right.Value();
	public static bool operator >(Inductance left, Inductance right) => left.Value() > right.Value();
	public static bool operator ==(Inductance left, Inductance right) => Math.Abs(left.Value() - right.Value()) < 1e-10;
	public static bool operator !=(Inductance left, Inductance right) => !(left == right);
	
	public override bool Equals(object? obj) => obj is Inductance other && this == other;
	public override int GetHashCode() => Value().GetHashCode();

	// Arithmetic operators
	public static Inductance operator +(Inductance left, Inductance right)
	{
		var result = new Inductance(left._unitGroup);
		result.Init(left.Value() + right.Value(), left.Internal());
		return result;
	}

	public static Inductance operator -(Inductance left, Inductance right)
	{
		var result = new Inductance(left._unitGroup);
		result.Init(left.Value() - right.Value(), left.Internal());
		return result;
	}

	public static Inductance operator *(double scalar, Inductance right)
	{
		var result = new Inductance(right._unitGroup);
		result.Init(scalar * right.Value(), right.Internal());
		return result;
	}

	public static Inductance operator *(Inductance left, double scalar)
	{
		var result = new Inductance(left._unitGroup);
		result.Init(left.Value() * scalar, left.Internal());
		return result;
	}

	public static Inductance operator /(Inductance left, double scalar)
	{
		var result = new Inductance(left._unitGroup);
		result.Init(left.Value() / scalar, left.Internal());
		return result;
	}

	public static double operator /(Inductance left, Inductance right) => left.Value() / right.Value();
}
