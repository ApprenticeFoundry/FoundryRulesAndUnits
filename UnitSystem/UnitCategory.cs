using System;
using System.Collections.Generic;
using System.Linq;
using FoundryRulesAndUnits.Extensions;

namespace FoundryRulesAndUnits.Units;


public class UnitCategory
{
	protected string Name { get; set; }
	protected UnitSpec BaseUnit { get; set; }
	protected UnitFamilyName Family;
	protected Dictionary<string, UnitSpec> UnitLookup = new();
	protected Dictionary<string, UnitConversion> ConversionLookup = new();

	public UnitCategory(string title)
	{
		this.Name = title;
		this.BaseUnit = new UnitSpec(title);
		this.Family = BaseUnit.UnitFamily();
	}
	public UnitCategory(string title, UnitSpec baseUnit)
	{
		this.Name = title;
		this.BaseUnit = baseUnit;
		this.Family = baseUnit.UnitFamily();

		var u1 = baseUnit.Name();
		var found = new UnitConversion($"{u1}|{u1}", v => v);
		ConversionLookup.Add(found.Name(), found);
	}

	public UnitCategory Units(string name, string title)
	{
		var found = new UnitSpec(name, title, this.Family);
		UnitLookup.Add(name, found);
		return this;
	}

	public string Title() { return Name; }

	public UnitFamilyName UnitFamily() { return Family; }
	public UnitSpec BaseUnits() { return BaseUnit; }

	public UnitCategory Conversion(double v1, string u1, double v2, string u2)
	{
		var found = new UnitConversion($"{u1}|{u2}", (v) => (v * v2) / v1);
		if (ConversionLookup.ContainsKey(found.Name()))
			ConversionLookup.Remove(found.Name());

		ConversionLookup.Add(found.Name(), found);
		//$"Conv: Added Conversion {found.Name()}".WriteLine();

		found = new UnitConversion($"{u2}|{u1}", (v) => (v * v1) / v2);
		if (ConversionLookup.ContainsKey(found.Name()))
			ConversionLookup.Remove(found.Name());

		ConversionLookup.Add(found.Name(), found);
		//$"Conv: Added Conversion {found.Name()}".WriteLine();
		return this;
	}

	public UnitCategory Conversion(string u1, string u2, Func<double, double> convert)
	{
		var found = new UnitConversion($"{u1}|{u2}", convert);
		if (ConversionLookup.ContainsKey(found.Name()))
			ConversionLookup.Remove(found.Name());

		ConversionLookup.Add(found.Name(), found);
		//$"Conversion: Added Conversion {found.Name()}".WriteLine();
		return this;
	}

	public (bool success, double value) Convert(string u1, string u2, double v1)
	{
		var key = $"{u1}|{u2}";
		if (ConversionLookup.TryGetValue(key, out UnitConversion? found) && found != null)
		{
			//$"con key found {key}".WriteLine();
			return (true, found.Convert(v1));
		}
		$"UnitCategoryConvert: No Conversion found for {key}:  from [{u1}] to [{u2}]".WriteError();
		return (false, v1);
	}


	public (bool success, double value) ConvertFromBaseUnits(string u1, double v1)
	{
		var u2 = BaseUnit.Name();
		var result = Convert(u2, u1, v1);
		//$"ConvertFrom BaseUnits: {v1} {u1} to {u2} = {result}".WriteLine();
		return result;
	}


	public (bool success, double value) ConvertToBaseUnits(string u1, double v1)
	{
		var u2 = BaseUnit.Name();
		var result = Convert(u1, u2, v1);
		//$"ConvertTo BaseUnits: {v1} {u1} to {u2} = {result}".WriteLine();
		return result;
	}

	public List<UnitSpec> Units()
	{
		return UnitLookup.Values.ToList();
	}

	/// <summary>
	/// Legacy method for getting conversions
	/// </summary>
	public List<UnitConversion> Conversions()
	{
		return ConversionLookup.Values.ToList();
	}
}

// Legacy stub class for compatibility
public class UnitConversion
{
	private string _name;
	private Func<double, double> _converter;

	public UnitConversion(string name, Func<double, double> converter)
	{
		_name = name;
		_converter = converter;
	}

	public string Name() => _name;
	public double Convert(double value) => _converter(value);
}

