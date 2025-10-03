using FoundryRulesAndUnits.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FoundryRulesAndUnits.Units;


public interface IMeasuredValue
{
	void SetValue(double value);
	void SetDisplayUnits(string units);
	string Debug();
	string AsString(string units);
	string Format(string format);
	string ToString();

	// Unit inspection methods
	double BaseValue();
	string BaseUnits();
	string DisplayUnits();
	string InternalRepresentation();
	string BaseUnitInfo();
	string DisplayInfo();
	string ConversionInfo();
	string DetailedDebug();
}


[JsonDerivedType(typeof(Angle))]
[JsonDerivedType(typeof(Area))]
[JsonDerivedType(typeof(Capacitance))]
[JsonDerivedType(typeof(Current))]
[JsonDerivedType(typeof(DataFlow))]
[JsonDerivedType(typeof(DataStorage))]
[JsonDerivedType(typeof(Dimensionless))]
[JsonDerivedType(typeof(Distance))]
[JsonDerivedType(typeof(Duration))]
[JsonDerivedType(typeof(Force))]
[JsonDerivedType(typeof(Frequency))]
[JsonDerivedType(typeof(Bearing))]
[JsonDerivedType(typeof(Length))]
[JsonDerivedType(typeof(Mass))]
[JsonDerivedType(typeof(Percent))]
[JsonDerivedType(typeof(Power))]
[JsonDerivedType(typeof(Quantity))]
[JsonDerivedType(typeof(QuantityFlow))]
[JsonDerivedType(typeof(Resistance))]
[JsonDerivedType(typeof(Speed))]
[JsonDerivedType(typeof(Temperature))]
[JsonDerivedType(typeof(Time))]
[JsonDerivedType(typeof(Voltage))]
[JsonDerivedType(typeof(Volume))]
[System.Serializable]
public class MeasuredValue : IMeasuredValue
{
	[JsonPropertyName("V")]
	public double V = 0.0;
	[JsonPropertyName("I")]
	public string I = "";      //internal storage units
	[JsonIgnore]
	public string U = "";  //reporting  input and output units
	protected UnitFamilyName F = UnitFamilyName.None;

	// UnitGroup injection - contains all unit conversion logic for this family
	protected UnitGroup _unitGroup;

	/// <summary>
	/// Gets the UnitFamily for this measurement type from UnitTypeAttribute
	/// Uses reflection to read the attribute - single source of truth
	/// </summary>
	public virtual UnitFamilyName UnitFamily
	{
		get
		{
			var attribute = UnitTypeRegistry.GetAttributeForType(this.GetType());
			return attribute?.Family ?? UnitFamilyName.None;
		}
	}

	/// <summary>
	/// Constructor with UnitGroup injection - preferred for new code
	/// Use UnitFactory to create instances with proper UnitGroup injection
	/// </summary>
	public MeasuredValue(UnitGroup unitGroup)
	{
		_unitGroup = unitGroup ?? throw new ArgumentNullException(nameof(unitGroup));
		F = unitGroup.Family;
		V = default!;
		// Set internal units to base unit from injected UnitGroup
		I = unitGroup.BaseUnit.Symbol;
		U = I; // Default display units to base units
	}

	/// <summary>
	/// Parameterless constructor for JSON deserialization only
	/// WARNING: UnitGroup will be null - conversion methods will have limited functionality
	/// </summary>
	protected MeasuredValue()
	{
		_unitGroup = null!; // Will be null for JSON deserialization
		F = UnitFamilyName.None;
		V = default!;
		I = "";
		U = "";
	}
	/// <summary>
	/// Initialize with value and units using injected UnitGroup
	/// </summary>
	public double Init(double value, string? units = null)
	{
		// Use injected UnitGroup - all family conversions handled here
		units = units ?? _unitGroup.BaseUnit.Symbol;

		// Validate unit belongs to this family
		if (!_unitGroup.IsValidUnit(units))
			throw new ArgumentException($"{units} is not a valid unit for {F}");

		U = units;
		I = _unitGroup.BaseUnit.Symbol;

		// Convert to base units for internal storage using UnitGroup
		if (I != U)
		{
			V = _unitGroup.Convert(value, U, I);
		}
		else
		{
			V = value;
		}
		return V;
	}

	/// <summary>
	/// Convert current value to specified units using injected UnitGroup
	/// </summary>
	public virtual double As(string units)
	{
		if (_unitGroup != null)
		{
			// Use injected UnitGroup - all family conversions handled here
			return _unitGroup.Convert(V, I, units);
		}
		else
		{
			// Enhanced fallback for common conversions when UnitGroup not available
			if (units == I) return V;

			// Try to create a temporary unit system for conversion
			try
			{
				var unitSystem = new UnitSystem();
				return unitSystem.Convert(V, I, units);
			}
			catch
			{
				// If all else fails, throw the original error
				throw new InvalidOperationException($"Unit conversion requires UnitGroup injection. Cannot convert from {I} to {units}");
			}
		}
	}

	public int ValueAsInt() { return (int)V; }
	public double Value() { return V; }
	public string Units() { return U; }
	public string Internal() { return I; }

	/// <summary>
	/// Get the base unit value (V field) - the numeric value stored in base units
	/// </summary>
	/// <returns>The numeric value in base units for mental math calculations</returns>
	public double BaseValue() { return V; }

	/// <summary>
	/// Get the base unit symbol (I field) - the unit symbol for internal storage
	/// </summary>
	/// <returns>The base unit symbol (e.g., 'm', 'km', 'ft', 'in')</returns>
	public string BaseUnits() { return I; }

	/// <summary>
	/// Get the display unit symbol (U field) - the unit symbol for user display
	/// </summary>
	/// <returns>The display unit symbol as originally entered</returns>
	public string DisplayUnits() { return U; }

	/// <summary>
	/// Get a detailed internal representation showing all V/I/U values
	/// </summary>
	/// <returns>Formatted string showing Value, Internal units, and User units</returns>
	public string InternalRepresentation()
	{
		return $"V={V:G}, I='{I}', U='{U}'";
	}

	/// <summary>
	/// Get base unit information for mental math - shows value and base unit clearly
	/// </summary>
	/// <returns>Human-readable string for doing mental calculations</returns>
	public string BaseUnitInfo()
	{
		return $"{V:G} {I}";
	}

	/// <summary>
	/// Get display information - shows value and display unit clearly
	/// </summary>
	/// <returns>Human-readable string as entered by user</returns>
	public string DisplayInfo()
	{
		return $"{As(U):G} {U}";
	}

	/// <summary>
	/// Get conversion information showing the relationship between display and base units
	/// </summary>
	/// <returns>Conversion details for understanding unit transformations</returns>
	public string ConversionInfo()
	{
		if (U == I)
			return $"{As(U):G} {U} (no conversion needed)";

		return $"{As(U):G} {U} = {V:G} {I} (base units)";
	}

	/// <summary>
	/// Get comprehensive debug information including unit family
	/// </summary>
	/// <returns>Complete internal state for debugging</returns>
	public string DetailedDebug()
	{
		return $"Display: {As(U):G} {U} | Base: {V:G} {I} | Family: {GetUnitFamily()} | V/I/U: {V:G}/'{I}'/'{U}'";
	}

	public void SetInternal(string units)
	{
		I = units;
	}

	public void SetValue(double value)
	{
		V = value;
	}


	public void SetDisplayUnits(string units)
	{
		U = units;
	}

	// public virtual double As(string units)
	// {
	// 	return default!;
	// }

	/// <summary>
	/// Public property to access the injected UnitGroup
	/// </summary>
	public UnitGroup UnitGroup
	{
		get
		{
			return _unitGroup ?? throw new InvalidOperationException("MeasuredValue must have an injected UnitGroup. Use UnitFactory to create instances.");
		}
	}

	/// <summary>
	/// Checks if this MeasuredValue is compatible with another for mathematical operations
	/// Uses UnitFamilyCompatibility to allow mixing related families (Length + Distance, etc.)
	/// </summary>
	/// <param name="other">The other MeasuredValue to check compatibility with</param>
	/// <returns>True if the units are compatible (same family or compatible families)</returns>
	public bool IsCompatibleWith(MeasuredValue other)
	{
		if (other == null) return false;
		
		// Check if they're compatible via the family compatibility matrix
		var thisFamily = this.UnitFamily;
		var otherFamily = other.UnitFamily;
		
		return UnitFamilyCompatibility.AreCompatible(thisFamily, otherFamily);
	}

	/// <summary>
	/// Add two compatible MeasuredValues together
	/// Automatically handles unit conversion and family resolution
	/// </summary>
	/// <param name="other">The other MeasuredValue to add</param>
	/// <param name="unitSystem">Unit system to use for result creation</param>
	/// <returns>A new MeasuredValue with the result</returns>
	/// <exception cref="InvalidOperationException">Thrown when units are incompatible</exception>
	public MeasuredValue AddCompatible(MeasuredValue other, UnitSystem unitSystem)
	{
		if (!IsCompatibleWith(other))
		{
			throw new InvalidOperationException(
				$"Cannot add incompatible unit families: {this.UnitFamily} and {other.UnitFamily}");
		}

		// Convert both to their base values for math
		var thisBaseValue = this.V;
		var otherBaseValue = other.V;
		
		// Determine result family using compatibility rules
		var resultFamily = UnitFamilyCompatibility.GetResultFamily(this.UnitFamily, other.UnitFamily);
		
		// Create result using factory to ensure proper type and initialization
		var factory = unitSystem.GetFactory();
		var result = factory.CreateMeasuredValue(resultFamily, thisBaseValue + otherBaseValue, this.I);
		
		return result;
	}

	/// <summary>
	/// Subtract two compatible MeasuredValues
	/// Automatically handles unit conversion and family resolution
	/// </summary>
	/// <param name="other">The other MeasuredValue to subtract</param>
	/// <param name="unitSystem">Unit system to use for result creation</param>
	/// <returns>A new MeasuredValue with the result</returns>
	/// <exception cref="InvalidOperationException">Thrown when units are incompatible</exception>
	public MeasuredValue SubtractCompatible(MeasuredValue other, UnitSystem unitSystem)
	{
		if (!IsCompatibleWith(other))
		{
			throw new InvalidOperationException(
				$"Cannot subtract incompatible unit families: {this.UnitFamily} and {other.UnitFamily}");
		}

		// Convert both to their base values for math
		var thisBaseValue = this.V;
		var otherBaseValue = other.V;
		
		// Determine result family using compatibility rules
		var resultFamily = UnitFamilyCompatibility.GetResultFamily(this.UnitFamily, other.UnitFamily);
		
		// Create result using factory to ensure proper type and initialization
		var factory = unitSystem.GetFactory();
		var result = factory.CreateMeasuredValue(resultFamily, thisBaseValue - otherBaseValue, this.I);
		
		return result;
	}

	/// <summary>
	/// Gets the unit family name for this measured value
	/// </summary>
	/// <returns>The unit family (Angle, Length, Mass, etc.)</returns>
	public string GetUnitFamily()
	{
		return this.GetType().Name;
	}


	public override string ToString()
	{
		return AsString(Units());
	}

	public string Format(string format)
	{
		var value = As(Units());
		return $"{value.ToString(format, CultureInfo.CurrentCulture)} {Units()}";
	}

	public string AsString(string units)
	{
		return $"{As(units)} {units}";
	}

	public string Debug()
	{
		return $"{Value()}({Internal()}) {Units()}";
	}



}


