using FoundryRulesAndUnits.Extensions;
using System;

namespace FoundryRulesAndUnits.Units;

[System.Serializable]
[UnitType(UnitFamilyName.Stiffness, Description = "Spring stiffness measurement (N/m)")]
public class Stiffness : MeasuredValue
{
	// UnitFamily comes from UnitTypeAttribute - no need for redundant property override
	// Without this class, CreateMeasuredValue(UnitFamilyName.Stiffness, …) fell back to base
	// MeasuredValue — which has no cross-family operators, so SpringConstant × Extension
	// decayed to a bare scalar and the force lost its units (bugs 034/035).

	/// <summary>
	/// Constructor with UnitGroup injection - use UnitSystem to create instances
	/// </summary>
	public Stiffness(UnitGroup unitGroup) : base(unitGroup)
	{
		if (unitGroup.Family != UnitFamilyName.Stiffness)
			throw new ArgumentException($"UnitGroup must be for Stiffness family, got {unitGroup.Family}");
	}

	public Stiffness Assign(double value, string? units)
	{
		Init(value, units);
		return this;
	}

	public Stiffness Assign(Stiffness source)
	{
		Init(source.Value(), source.U);
		return this;
	}

	public Stiffness Copy()
	{
		var copy = new Stiffness(_unitGroup);
		copy.Init(Value(), Internal());
		return copy;
	}

	// Comparison operators
	public static bool operator <(Stiffness left, Stiffness right) => left.Value() < right.Value();
	public static bool operator >(Stiffness left, Stiffness right) => left.Value() > right.Value();
	public static bool operator ==(Stiffness left, Stiffness right) => Math.Abs(left.Value() - right.Value()) < 1e-10;
	public static bool operator !=(Stiffness left, Stiffness right) => !(left == right);

	public override bool Equals(object? obj) => obj is Stiffness other && this == other;
	public override int GetHashCode() => Value().GetHashCode();

	// Arithmetic operators
	public static Stiffness operator +(Stiffness left, Stiffness right)
	{
		var result = new Stiffness(left._unitGroup);
		result.Init(left.Value() + right.Value(), left.Internal());
		return result;
	}

	public static Stiffness operator -(Stiffness left, Stiffness right)
	{
		var result = new Stiffness(left._unitGroup);
		result.Init(left.Value() - right.Value(), left.Internal());
		return result;
	}

	public static Stiffness operator *(double scalar, Stiffness right)
	{
		var result = new Stiffness(right._unitGroup);
		result.Init(scalar * right.Value(), right.Internal());
		return result;
	}

	public static Stiffness operator *(Stiffness left, double scalar)
	{
		var result = new Stiffness(left._unitGroup);
		result.Init(left.Value() * scalar, left.Internal());
		return result;
	}

	public static Stiffness operator /(Stiffness left, double scalar)
	{
		var result = new Stiffness(left._unitGroup);
		result.Init(left.Value() / scalar, left.Internal());
		return result;
	}

	public static double operator /(Stiffness left, Stiffness right) => left.Value() / right.Value();

	// Cross-family: Stiffness × Length → Force (Hooke's law, F = k·x)
	public static Force operator *(Stiffness left, Length right)
	{
		var forceValue = left.BaseValue() * right.BaseValue(); // (N/m) × m = N
		var unitSystem = new UnitSystem(left._unitGroup.SystemType);
		return (Force)unitSystem.CreateTypedMeasuredValue(UnitFamilyName.Force, forceValue, "N");
	}

	public static Force operator *(Length left, Stiffness right)
	{
		var forceValue = left.BaseValue() * right.BaseValue(); // m × (N/m) = N
		var unitSystem = new UnitSystem(right._unitGroup.SystemType);
		return (Force)unitSystem.CreateTypedMeasuredValue(UnitFamilyName.Force, forceValue, "N");
	}
}
