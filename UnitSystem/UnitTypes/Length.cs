using FoundryRulesAndUnits.Extensions;
using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units;

[System.Serializable]
[UnitType(UnitFamilyName.Length, Description = "Length/distance measurement")]
public class Length : MeasuredValue
{
	// UnitFamily comes from UnitTypeAttribute - no need for redundant property override


	/// <summary>
	/// Constructor with UnitGroup injection - preferred for factory pattern
	/// </summary>
	public Length(UnitGroup unitGroup) : base(unitGroup)
	{
		if (unitGroup.Family != UnitFamilyName.Length)
			throw new ArgumentException($"UnitGroup family must be {UnitFamilyName.Length}", nameof(unitGroup));
	}

	public Length Assign(double value, string? units)
	{
		Init(value, units); // Base class handles everything!
		return this;
	}

	public Length Assign(Length source)
	{
		Init(source.Value(), source.U); // Base class handles everything!
		return this;
	}

	public Length Copy()
	{
		var copy = new Length(_unitGroup);
		copy.Init(Value(), Internal());
		return copy;
	}

	// Static factory methods removed - use UnitSystem.CreateUnit<T>() instead
	// Example: factory.CreateLength(1000, "m") for kilometers

	// As() method inherited from MeasuredValue - no override needed!

	// Legacy compatibility methods
	public int AsPixels()
	{
		// Handle pixel conversion with manual fallback when UnitGroup injection unavailable
		try
		{
			return (int)Math.Round(As("px"));
		}
		catch (InvalidOperationException)
		{
			// Manual conversion fallback for common units to pixels (96 DPI standard)
			// This handles cases where UnitGroup injection is not available
			var valueInMeters = I switch
			{
				"m" => V,
				"cm" => V * 0.01,
				"mm" => V * 0.001,
				"in" => V * 0.0254,
				"ft" => V * 0.3048,
				"px" => V / (96.0 / 0.0254), // Convert px back to meters first, then to px (identity)
				_ => throw new InvalidOperationException($"Cannot convert {I} to pixels without UnitGroup injection")
			};

			// Convert meters to pixels (96 DPI: 96 pixels per inch, 0.0254 meters per inch)
			var pixelsPerMeter = 96.0 / 0.0254;
			return (int)Math.Round(valueInMeters * pixelsPerMeter);
		}
	}

	public static bool operator <(Length left, Length right) => left.Value() < right.Value();
	public static bool operator >(Length left, Length right) => left.Value() > right.Value();
	public static bool operator ==(Length left, Length right) => Math.Abs(left.Value() - right.Value()) < 1e-10;
	public static bool operator !=(Length left, Length right) => !(left == right);
	
	// Override Equals and GetHashCode to be consistent with == operator
	public override bool Equals(object? obj) => obj is Length other && this == other;
	public override int GetHashCode() => Value().GetHashCode();

	public static Length operator +(Length left, Length right)
	{
		var result = new Length(left._unitGroup);
		result.Init(left.Value() + right.Value(), left.Internal());
		return result;
	}

	public static Length operator -(Length left, Length right)
	{
		var result = new Length(left._unitGroup);
		result.Init(left.Value() - right.Value(), left.Internal());
		return result;
	}

	public static Length operator *(double left, Length right)
	{
		var result = new Length(right._unitGroup);
		result.Init(left * right.Value(), right.Internal());
		return result;
	}

	public static Length operator *(Length left, double right)
	{
		var result = new Length(left._unitGroup);
		result.Init(left.Value() * right, left.Internal());
		return result;
	}

	public static Length operator /(Length left, double right)
	{
		var result = new Length(left._unitGroup);
		result.Init(left.Value() / right, left.Internal());
		return result;
	}

	public static double operator /(Length left, Length right) => left.Value() / right.Value();

	// ============================================================================
	// PHASE 4A: CROSS-FAMILY OPERATIONS - Length-specific operators
	// ============================================================================
	
	/// <summary>
	/// Length × Length → Area
	/// Example: 5m * 3m = 15 m²
	/// </summary>
	public static MeasuredValue operator *(Length left, Length right)
	{
		var unitSystem = new UnitSystem(left._unitGroup.SystemType);
		var areaValue = left.BaseValue() * right.BaseValue(); // m × m = m²
		return unitSystem.CreateMeasuredValue(UnitFamilyName.Area, areaValue, "m2");
	}
	
	/// <summary>
	/// Length ÷ Time → Speed
	/// Example: 100m / 10s = 10 m/s
	/// </summary>
	public static MeasuredValue operator /(Length left, Time right)
	{
		var unitSystem = new UnitSystem(left._unitGroup.SystemType);
		var speedValue = left.BaseValue() / right.BaseValue(); // m ÷ s = m/s
		return unitSystem.CreateMeasuredValue(UnitFamilyName.Speed, speedValue, "m/s");
	}
	
	/// <summary>
	/// Length ÷ Duration → Speed
	/// Example: 50m / 5s = 10 m/s
	/// </summary>
	public static MeasuredValue operator /(Length left, Duration right)
	{
		var unitSystem = new UnitSystem(left._unitGroup.SystemType);
		var speedValue = left.BaseValue() / right.BaseValue(); // m ÷ s = m/s
		return unitSystem.CreateMeasuredValue(UnitFamilyName.Speed, speedValue, "m/s");
	}
}




