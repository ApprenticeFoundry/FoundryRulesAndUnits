using FoundryRulesAndUnits.Extensions;
using System;

namespace FoundryRulesAndUnits.Units;

[System.Serializable]
[UnitType(UnitFamilyName.Damping, Description = "Damping coefficient measurement (Ns/m)")]
public class Damping : MeasuredValue
{
	// UnitFamily comes from UnitTypeAttribute - no need for redundant property override
	// Without this class, CreateMeasuredValue(UnitFamilyName.Damping, …) fell back to base
	// MeasuredValue — no cross-family operators, so DampingCoefficient × RelativeVelocity
	// decayed to a bare scalar (bugs 034/035).

	/// <summary>
	/// Constructor with UnitGroup injection - use UnitSystem to create instances
	/// </summary>
	public Damping(UnitGroup unitGroup) : base(unitGroup)
	{
		if (unitGroup.Family != UnitFamilyName.Damping)
			throw new ArgumentException($"UnitGroup must be for Damping family, got {unitGroup.Family}");
	}

	public Damping Assign(double value, string? units)
	{
		Init(value, units);
		return this;
	}

	public Damping Assign(Damping source)
	{
		Init(source.Value(), source.U);
		return this;
	}

	public Damping Copy()
	{
		var copy = new Damping(_unitGroup);
		copy.Init(Value(), Internal());
		return copy;
	}

	// Comparison operators
	public static bool operator <(Damping left, Damping right) => left.Value() < right.Value();
	public static bool operator >(Damping left, Damping right) => left.Value() > right.Value();
	public static bool operator ==(Damping left, Damping right) => Math.Abs(left.Value() - right.Value()) < 1e-10;
	public static bool operator !=(Damping left, Damping right) => !(left == right);

	public override bool Equals(object? obj) => obj is Damping other && this == other;
	public override int GetHashCode() => Value().GetHashCode();

	// Arithmetic operators
	public static Damping operator +(Damping left, Damping right)
	{
		var result = new Damping(left._unitGroup);
		result.Init(left.Value() + right.Value(), left.Internal());
		return result;
	}

	public static Damping operator -(Damping left, Damping right)
	{
		var result = new Damping(left._unitGroup);
		result.Init(left.Value() - right.Value(), left.Internal());
		return result;
	}

	public static Damping operator *(double scalar, Damping right)
	{
		var result = new Damping(right._unitGroup);
		result.Init(scalar * right.Value(), right.Internal());
		return result;
	}

	public static Damping operator *(Damping left, double scalar)
	{
		var result = new Damping(left._unitGroup);
		result.Init(left.Value() * scalar, left.Internal());
		return result;
	}

	public static Damping operator /(Damping left, double scalar)
	{
		var result = new Damping(left._unitGroup);
		result.Init(left.Value() / scalar, left.Internal());
		return result;
	}

	public static double operator /(Damping left, Damping right) => left.Value() / right.Value();

	// Cross-family: Damping × Speed → Force (viscous damping, F = c·v)
	public static Force operator *(Damping left, Speed right)
	{
		var forceValue = left.BaseValue() * right.BaseValue(); // (Ns/m) × (m/s) = N
		var unitSystem = new UnitSystem(left._unitGroup.SystemType);
		return (Force)unitSystem.CreateTypedMeasuredValue(UnitFamilyName.Force, forceValue, "N");
	}

	public static Force operator *(Speed left, Damping right)
	{
		var forceValue = left.BaseValue() * right.BaseValue(); // (m/s) × (Ns/m) = N
		var unitSystem = new UnitSystem(right._unitGroup.SystemType);
		return (Force)unitSystem.CreateTypedMeasuredValue(UnitFamilyName.Force, forceValue, "N");
	}
}
