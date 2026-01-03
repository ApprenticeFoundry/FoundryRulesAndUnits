using FoundryRulesAndUnits.Extensions;
using System;

namespace FoundryRulesAndUnits.Units;

[System.Serializable]
[UnitType(UnitFamilyName.Inertia, Description = "Moment of inertia measurement")]
public class Inertia : MeasuredValue
{
	// UnitFamily comes from UnitTypeAttribute - no need for redundant property override

	/// <summary>
	/// Constructor with UnitGroup injection - use UnitSystem to create instances
	/// </summary>
	public Inertia(UnitGroup unitGroup) : base(unitGroup)
	{
		if (unitGroup.Family != UnitFamilyName.Inertia)
			throw new ArgumentException($"UnitGroup must be for Inertia family, got {unitGroup.Family}");
	}

	public Inertia Assign(double value, string? units)
	{
		Init(value, units);
		return this;
	}

	public Inertia Assign(Inertia source)
	{
		Init(source.Value(), source.U);
		return this;
	}

	public Inertia Copy()
	{
		var copy = new Inertia(_unitGroup);
		copy.Init(Value(), Internal());
		return copy;
	}

	// Comparison operators
	public static bool operator <(Inertia left, Inertia right) => left.Value() < right.Value();
	public static bool operator >(Inertia left, Inertia right) => left.Value() > right.Value();
	public static bool operator ==(Inertia left, Inertia right) => Math.Abs(left.Value() - right.Value()) < 1e-10;
	public static bool operator !=(Inertia left, Inertia right) => !(left == right);
	
	public override bool Equals(object? obj) => obj is Inertia other && this == other;
	public override int GetHashCode() => Value().GetHashCode();

	// Arithmetic operators
	public static Inertia operator +(Inertia left, Inertia right)
	{
		var result = new Inertia(left._unitGroup);
		result.Init(left.Value() + right.Value(), left.Internal());
		return result;
	}

	public static Inertia operator -(Inertia left, Inertia right)
	{
		var result = new Inertia(left._unitGroup);
		result.Init(left.Value() - right.Value(), left.Internal());
		return result;
	}

	public static Inertia operator *(double scalar, Inertia right)
	{
		var result = new Inertia(right._unitGroup);
		result.Init(scalar * right.Value(), right.Internal());
		return result;
	}

	public static Inertia operator *(Inertia left, double scalar)
	{
		var result = new Inertia(left._unitGroup);
		result.Init(left.Value() * scalar, left.Internal());
		return result;
	}

	public static Inertia operator /(Inertia left, double scalar)
	{
		var result = new Inertia(left._unitGroup);
		result.Init(left.Value() / scalar, left.Internal());
		return result;
	}

	public static double operator /(Inertia left, Inertia right) => left.Value() / right.Value();
}
