using FoundryRulesAndUnits.Extensions;
using System;

namespace FoundryRulesAndUnits.Units;

[System.Serializable]
[UnitType(UnitFamilyName.CostPerTime, Description = "Cost per unit time (hourly rates, etc.)")]
public class CostPerTime : MeasuredValue
{
	/// <summary>
	/// Constructor with UnitGroup injection - preferred for factory pattern
	/// </summary>
	public CostPerTime(UnitGroup unitGroup) : base(unitGroup)
	{
		if (unitGroup.Family != UnitFamilyName.CostPerTime)
			throw new ArgumentException($"UnitGroup family must be {UnitFamilyName.CostPerTime}", nameof(unitGroup));
	}

	public CostPerTime Assign(double value, string? units)
	{
		Init(value, units); // Base class handles everything!
		return this;
	}

	public CostPerTime Assign(CostPerTime source)
	{
		Init(source.Value(), source.U); // Base class handles everything!
		return this;
	}

	public CostPerTime Copy()
	{
		var copy = new CostPerTime(_unitGroup);
		copy.Init(Value(), Internal());
		return copy;
	}

	/// <summary>
	/// Calculate total cost for a given duration
	/// </summary>
	/// <param name="duration">Duration in hours</param>
	/// <param name="currencyGroup">Currency unit group for result</param>
	public Currency CalculateCost(double duration, UnitGroup currencyGroup)
	{
		var costPerHour = Value(); // Get value in current units (assuming USD/hr base)
		var totalValue = costPerHour * duration;
		
		// Extract currency code from unit string (e.g., "USD/hr" -> "USD")
		var currencyCode = ExtractCurrencyCode(U);
		
		// Create currency using provided currency unit group
		var currency = new Currency(currencyGroup);
		currency.Assign(totalValue, currencyCode);
		return currency;
	}

	/// <summary>
	/// Calculate total cost for a given Time duration
	/// </summary>
	public Currency CalculateCost(Time duration, UnitGroup currencyGroup)
	{
		var hours = duration.Value(); // Get hours from Time (assuming hours as base)
		return CalculateCost(hours, currencyGroup);
	}

	/// <summary>
	/// Apply markup or discount percentage
	/// </summary>
	/// <param name="adjustmentPercent">Adjustment percentage (positive for markup, negative for discount, e.g., 20 for 20% markup, -10 for 10% discount)</param>
	public CostPerTime ApplyAdjustment(double adjustmentPercent)
	{
		var adjustedValue = Value() * (1 + adjustmentPercent / 100.0);
		var result = new CostPerTime(_unitGroup);
		result.Init(adjustedValue, Internal());
		return result;
	}

	/// <summary>
	/// Extract currency code from cost unit string (e.g., "USD/hr" -> "USD")
	/// </summary>
	private string ExtractCurrencyCode(string costUnit)
	{
		var parts = costUnit.Split('/');
		return parts.Length > 0 ? parts[0] : "USD";
	}

	/// <summary>
	/// Format cost with currency symbol
	/// </summary>
	public string FormatCost(int decimalPlaces = 2)
	{
		var symbol = GetCurrencySymbol(ExtractCurrencyCode(U));
		return $"{symbol}{Value().ToString($"F{decimalPlaces}")}/{GetTimeUnit(U)}";
	}

	/// <summary>
	/// Get time unit from cost unit string (e.g., "USD/hr" -> "hr")
	/// </summary>
	private string GetTimeUnit(string costUnit)
	{
		var parts = costUnit.Split('/');
		return parts.Length > 1 ? parts[1] : "hr";
	}

	/// <summary>
	/// Get currency symbol for display
	/// </summary>
	private string GetCurrencySymbol(string currencyCode)
	{
		return currencyCode switch
		{
			"USD" => "$",
			"EUR" => "€",
			"GBP" => "£",
			"JPY" => "¥",
			"CNY" => "¥",
			"CAD" => "C$",
			"AUD" => "A$",
			"CHF" => "CHF ",
			"INR" => "₹",
			"MXN" => "MX$",
			"BRL" => "R$",
			"KRW" => "₩",
			"SGD" => "S$",
			"HKD" => "HK$",
			_ => currencyCode + " "
		};
	}

	// Arithmetic operators
	public static CostPerTime operator +(CostPerTime left, CostPerTime right)
	{
		var result = new CostPerTime(left._unitGroup);
		result.Init(left.Value() + right.Value(), left.Internal());
		return result;
	}

	public static CostPerTime operator -(CostPerTime left, CostPerTime right)
	{
		var result = new CostPerTime(left._unitGroup);
		result.Init(left.Value() - right.Value(), left.Internal());
		return result;
	}

	public static CostPerTime operator *(double scalar, CostPerTime right)
	{
		var result = new CostPerTime(right._unitGroup);
		result.Init(scalar * right.Value(), right.Internal());
		return result;
	}

	public static CostPerTime operator *(CostPerTime left, double scalar)
	{
		var result = new CostPerTime(left._unitGroup);
		result.Init(left.Value() * scalar, left.Internal());
		return result;
	}

	public static CostPerTime operator /(CostPerTime left, double scalar)
	{
		var result = new CostPerTime(left._unitGroup);
		result.Init(left.Value() / scalar, left.Internal());
		return result;
	}

	// Comparison operators
	public static bool operator <(CostPerTime left, CostPerTime right) => left.Value() < right.Value();
	public static bool operator >(CostPerTime left, CostPerTime right) => left.Value() > right.Value();
	public static bool operator ==(CostPerTime left, CostPerTime right) => Math.Abs(left.Value() - right.Value()) < 1e-4;
	public static bool operator !=(CostPerTime left, CostPerTime right) => !(left == right);
	
	// Override Equals and GetHashCode to be consistent with == operator
	public override bool Equals(object? obj) => obj is CostPerTime other && this == other;
	public override int GetHashCode() => Value().GetHashCode();
}
