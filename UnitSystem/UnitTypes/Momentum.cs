using FoundryRulesAndUnits.Extensions;
using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units;

[System.Serializable]
[UnitType(UnitFamilyName.Momentum, Description = "Momentum measurement (Mass × Velocity)")]
public class Momentum : MeasuredValue
{
	// UnitFamily comes from UnitTypeAttribute - no need for redundant property override

	/// <summary>
	/// Constructor with UnitGroup injection - use UnitSystem to create instances
	/// </summary>
	public Momentum(UnitGroup unitGroup) : base(unitGroup)
	{
		if (unitGroup.Family != UnitFamilyName.Momentum)
			throw new ArgumentException($"UnitGroup must be for Momentum family, got {unitGroup.Family}");
	}

	public Momentum Assign(double value, string? units)
	{
		Init(value, units); // Base class handles everything!
		return this;
	}

	public Momentum Assign(Momentum source)
	{
		Init(source.Value(), source.U); // Base class handles everything!
		return this;
	}

	public Momentum Copy()
	{
		var copy = new Momentum(_unitGroup);
		copy.Init(Value(), Internal());
		return copy;
	}

	// Comparison operators
	public static bool operator <(Momentum left, Momentum right) => left.Value() < right.Value();
	public static bool operator >(Momentum left, Momentum right) => left.Value() > right.Value();
	public static bool operator ==(Momentum left, Momentum right) => Math.Abs(left.Value() - right.Value()) < 1e-10;
	public static bool operator !=(Momentum left, Momentum right) => !(left == right);
	
	// Override Equals and GetHashCode to be consistent with == operator
	public override bool Equals(object? obj) => obj is Momentum other && this == other;
	public override int GetHashCode() => Value().GetHashCode();

	// Arithmetic operators for same type
	public static Momentum operator +(Momentum left, Momentum right) 
	{
		var result = new Momentum(left._unitGroup);
		result.Init(left.Value() + right.Value(), left.Internal());
		return result;
	}
	
	public static Momentum operator -(Momentum left, Momentum right) 
	{
		var result = new Momentum(left._unitGroup);
		result.Init(left.Value() - right.Value(), left.Internal());
		return result;
	}
	
	// Scalar multiplication operators
	public static Momentum operator *(double scalar, Momentum momentum) 
	{
		var result = new Momentum(momentum._unitGroup);
		result.Init(scalar * momentum.Value(), momentum.Internal());
		return result;
	}

	public static Momentum operator *(Momentum momentum, double scalar)
	{
		var result = new Momentum(momentum._unitGroup);
		result.Init(momentum.Value() * scalar, momentum.Internal());
		return result;
	}
	
	public static Momentum operator /(Momentum momentum, double scalar) 
	{
		var result = new Momentum(momentum._unitGroup);
		result.Init(momentum.Value() / scalar, momentum.Internal());
		return result;
	}

	public static double operator /(Momentum left, Momentum right) => left.Value() / right.Value();

	// ============================================================================
	// PHYSICS OPERATIONS - Proper OOP approach
	// ============================================================================
	
	/// <summary>
	/// Momentum ÷ Mass → Speed (p = mv, so v = p/m)
	/// Example: 50 kg⋅m/s ÷ 10 kg = 5 m/s
	/// </summary>
	public static Speed operator /(Momentum momentum, Mass mass)
	{
		// Reuse existing unit system from the operands - no new instances!
		var speedValue = momentum.BaseValue() / mass.BaseValue(); // (kg⋅m/s) ÷ kg = m/s
		var speedUnit = momentum._unitGroup.CreateUnit<Speed>();
		speedUnit.Init(speedValue, speedUnit.Internal());
		return speedUnit;
	}
	
	/// <summary>
	/// Momentum ÷ Speed → Mass (p = mv, so m = p/v)
	/// Example: 50 kg⋅m/s ÷ 5 m/s = 10 kg
	/// </summary>
	public static Mass operator /(Momentum momentum, Speed speed)
	{
		// Reuse existing unit system from the operands - no new instances!
		var massValue = momentum.BaseValue() / speed.BaseValue(); // (kg⋅m/s) ÷ (m/s) = kg
		var massUnit = momentum._unitGroup.CreateUnit<Mass>();
		massUnit.Init(massValue, massUnit.Internal());
		return massUnit;
	}
}