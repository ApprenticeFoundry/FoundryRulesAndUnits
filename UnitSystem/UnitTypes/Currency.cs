using FoundryRulesAndUnits.Extensions;
using System;

namespace FoundryRulesAndUnits.Units;

[System.Serializable]
[UnitType(UnitFamilyName.Currency, Description = "Monetary value")]
public class Currency : MeasuredValue
{
	/// <summary>
	/// Constructor with UnitGroup injection - preferred for factory pattern
	/// </summary>
	public Currency(UnitGroup unitGroup) : base(unitGroup)
	{
		if (unitGroup.Family != UnitFamilyName.Currency)
			throw new ArgumentException($"UnitGroup family must be {UnitFamilyName.Currency}", nameof(unitGroup));
	}

	public Currency Assign(double value, string? units)
	{
		Init(value, units); // Base class handles everything!
		return this;
	}

	public Currency Assign(Currency source)
	{
		Init(source.Value(), source.U); // Base class handles everything!
		return this;
	}

	public Currency Copy()
	{
		var copy = new Currency(_unitGroup);
		copy.Init(Value(), Internal());
		return copy;
	}

	/// <summary>
	/// Format currency with appropriate symbol and decimal places
	/// </summary>
	public string FormatCurrency(int decimalPlaces = 2)
	{
		var symbol = GetCurrencySymbol(U);
		return $"{symbol}{Value().ToString($"F{decimalPlaces}")}";
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
	public static Currency operator +(Currency left, Currency right)
	{
		var result = new Currency(left._unitGroup);
		result.Init(left.Value() + right.Value(), left.Internal());
		return result;
	}

	public static Currency operator -(Currency left, Currency right)
	{
		var result = new Currency(left._unitGroup);
		result.Init(left.Value() - right.Value(), left.Internal());
		return result;
	}

	public static Currency operator *(double scalar, Currency right)
	{
		var result = new Currency(right._unitGroup);
		result.Init(scalar * right.Value(), right.Internal());
		return result;
	}

	public static Currency operator *(Currency left, double scalar)
	{
		var result = new Currency(left._unitGroup);
		result.Init(left.Value() * scalar, left.Internal());
		return result;
	}

	public static Currency operator /(Currency left, double scalar)
	{
		var result = new Currency(left._unitGroup);
		result.Init(left.Value() / scalar, left.Internal());
		return result;
	}

	// Comparison operators
	public static bool operator <(Currency left, Currency right) => left.Value() < right.Value();
	public static bool operator >(Currency left, Currency right) => left.Value() > right.Value();
	public static bool operator ==(Currency left, Currency right) => Math.Abs(left.Value() - right.Value()) < 1e-4; // 0.01 cent precision
	public static bool operator !=(Currency left, Currency right) => !(left == right);
	
	// Override Equals and GetHashCode to be consistent with == operator
	public override bool Equals(object? obj) => obj is Currency other && this == other;
	public override int GetHashCode() => Value().GetHashCode();
}
