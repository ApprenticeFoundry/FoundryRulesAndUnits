using FoundryRulesAndUnits.Extensions;
using System;

namespace FoundryRulesAndUnits.Units;

[System.Serializable]
[UnitType(UnitFamilyName.CostPerQuantity, Description = "Cost per item/unit/piece")]
public class CostPerQuantity : MeasuredValue
{
	/// <summary>
	/// Constructor with UnitGroup injection - preferred for factory pattern
	/// </summary>
	public CostPerQuantity(UnitGroup unitGroup) : base(unitGroup)
	{
		if (unitGroup.Family != UnitFamilyName.CostPerQuantity)
			throw new ArgumentException($"UnitGroup family must be {UnitFamilyName.CostPerQuantity}", nameof(unitGroup));
	}

	public CostPerQuantity Assign(double value, string? units)
	{
		Init(value, units); // Base class handles everything!
		return this;
	}

	public CostPerQuantity Assign(CostPerQuantity source)
	{
		Init(source.Value(), source.U); // Base class handles everything!
		return this;
	}

	public CostPerQuantity Copy()
	{
		var copy = new CostPerQuantity(_unitGroup);
		copy.Init(Value(), Internal());
		return copy;
	}

	/// <summary>
	/// Calculate total cost for a given quantity
	/// </summary>
	public Currency CalculateCost(double quantity, UnitGroup currencyGroup)
	{
		var costPerUnit = Value(); // Get value in current units
		var totalValue = costPerUnit * quantity;
		
		// Extract currency code from unit string (e.g., "USD/ea" -> "USD")
		var currencyCode = ExtractCurrencyCode(U);
		
		// Create currency using provided currency unit group
		var currency = new Currency(currencyGroup);
		currency.Assign(totalValue, currencyCode);
		return currency;
	}

	/// <summary>
	/// Apply volume discount percentage
	/// </summary>
	/// <param name="discountPercent">Discount percentage (e.g., 15 for 15% off)</param>
	public CostPerQuantity ApplyVolumeDiscount(double discountPercent)
	{
		if (discountPercent < 0 || discountPercent > 100)
			throw new ArgumentException("Discount must be between 0 and 100", nameof(discountPercent));
		
		var discountedValue = Value() * (1 - discountPercent / 100.0);
		var result = new CostPerQuantity(_unitGroup);
		result.Init(discountedValue, Internal());
		return result;
	}

	/// <summary>
	/// Extract currency code from cost unit string (e.g., "USD/ea" -> "USD")
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
		return $"{symbol}{Value().ToString($"F{decimalPlaces}")}/{GetQuantityUnit(U)}";
	}

	/// <summary>
	/// Get quantity unit from cost unit string (e.g., "USD/ea" -> "ea")
	/// </summary>
	private string GetQuantityUnit(string costUnit)
	{
		var parts = costUnit.Split('/');
		return parts.Length > 1 ? parts[1] : "unit";
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
	public static CostPerQuantity operator +(CostPerQuantity left, CostPerQuantity right)
	{
		var result = new CostPerQuantity(left._unitGroup);
		result.Init(left.Value() + right.Value(), left.Internal());
		return result;
	}

	public static CostPerQuantity operator -(CostPerQuantity left, CostPerQuantity right)
	{
		var result = new CostPerQuantity(left._unitGroup);
		result.Init(left.Value() - right.Value(), left.Internal());
		return result;
	}

	public static CostPerQuantity operator *(double scalar, CostPerQuantity right)
	{
		var result = new CostPerQuantity(right._unitGroup);
		result.Init(scalar * right.Value(), right.Internal());
		return result;
	}

	public static CostPerQuantity operator *(CostPerQuantity left, double scalar)
	{
		var result = new CostPerQuantity(left._unitGroup);
		result.Init(left.Value() * scalar, left.Internal());
		return result;
	}

	public static CostPerQuantity operator /(CostPerQuantity left, double scalar)
	{
		var result = new CostPerQuantity(left._unitGroup);
		result.Init(left.Value() / scalar, left.Internal());
		return result;
	}

	// Comparison operators
	public static bool operator <(CostPerQuantity left, CostPerQuantity right) => left.Value() < right.Value();
	public static bool operator >(CostPerQuantity left, CostPerQuantity right) => left.Value() > right.Value();
	public static bool operator ==(CostPerQuantity left, CostPerQuantity right) => Math.Abs(left.Value() - right.Value()) < 1e-4;
	public static bool operator !=(CostPerQuantity left, CostPerQuantity right) => !(left == right);
	
	// Override Equals and GetHashCode to be consistent with == operator
	public override bool Equals(object? obj) => obj is CostPerQuantity other && this == other;
	public override int GetHashCode() => Value().GetHashCode();
}
