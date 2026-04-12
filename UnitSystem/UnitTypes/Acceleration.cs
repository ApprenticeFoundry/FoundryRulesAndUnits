using FoundryRulesAndUnits.Extensions;
using System;

namespace FoundryRulesAndUnits.Units;

[System.Serializable]
[UnitType(UnitFamilyName.Acceleration, Description = "Linear acceleration measurement")]
public class Acceleration : MeasuredValue
{
	// UnitFamily comes from UnitTypeAttribute - no need for redundant property override

	/// <summary>
	/// Constructor with UnitGroup injection - use UnitSystem to create instances
	/// </summary>
	public Acceleration(UnitGroup unitGroup) : base(unitGroup)
	{
		if (unitGroup.Family != UnitFamilyName.Acceleration)
			throw new ArgumentException($"UnitGroup must be for Acceleration family, got {unitGroup.Family}");
	}

	public Acceleration Assign(double value, string? units)
	{
		Init(value, units);
		return this;
	}

	public Acceleration Assign(Acceleration source)
	{
		Init(source.Value(), source.U);
		return this;
	}

	public Acceleration Copy()
	{
		var copy = new Acceleration(_unitGroup);
		copy.Init(Value(), Internal());
		return copy;
	}

	// Comparison operators
	public static bool operator <(Acceleration left, Acceleration right) => left.Value() < right.Value();
	public static bool operator >(Acceleration left, Acceleration right) => left.Value() > right.Value();
	public static bool operator ==(Acceleration left, Acceleration right) => Math.Abs(left.Value() - right.Value()) < 1e-10;
	public static bool operator !=(Acceleration left, Acceleration right) => !(left == right);

	public override bool Equals(object? obj) => obj is Acceleration other && this == other;
	public override int GetHashCode() => Value().GetHashCode();

	// Arithmetic operators
	public static Acceleration operator +(Acceleration left, Acceleration right)
	{
		var result = new Acceleration(left._unitGroup);
		result.Init(left.Value() + right.Value(), left.Internal());
		return result;
	}

	public static Acceleration operator -(Acceleration left, Acceleration right)
	{
		var result = new Acceleration(left._unitGroup);
		result.Init(left.Value() - right.Value(), left.Internal());
		return result;
	}

	public static Acceleration operator *(double scalar, Acceleration right)
	{
		var result = new Acceleration(right._unitGroup);
		result.Init(scalar * right.Value(), right.Internal());
		return result;
	}

	public static Acceleration operator *(Acceleration left, double scalar)
	{
		var result = new Acceleration(left._unitGroup);
		result.Init(left.Value() * scalar, left.Internal());
		return result;
	}

	public static Acceleration operator /(Acceleration left, double scalar)
	{
		var result = new Acceleration(left._unitGroup);
		result.Init(left.Value() / scalar, left.Internal());
		return result;
	}

	public static double operator /(Acceleration left, Acceleration right) => left.Value() / right.Value();

	// Cross-family: Acceleration × Time → Speed (v = a × t)
	public static Speed operator *(Acceleration left, Time right)
	{
		var speedValue = left.BaseValue() * right.BaseValue(); // (m/s²) × s = m/s
		var unitSystem = new UnitSystem(left._unitGroup.SystemType);
		return (Speed)unitSystem.CreateTypedMeasuredValue(UnitFamilyName.Speed, speedValue, "m/s");
	}
}
