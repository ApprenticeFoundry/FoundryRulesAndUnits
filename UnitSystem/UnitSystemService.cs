using System;
using System.Collections.Generic;
using System.Linq;
using FoundryRulesAndUnits.Units;
using FoundryRulesAndUnits.Units.Specifications;

namespace FoundryRulesAndUnits.Units
{
	public enum UnitSystemType
	{
		IPS,
		FPS,
		MKS,
		CGS,
		mmNs,
		SI
	}

	/// <summary>
	/// Global Unit System Service - Set once, use everywhere
	/// Provides application-wide consistent unit system access
	/// </summary>
	public class UnitSystemService
	{
		private static UnitSystemService? _instance;
		private IUnitSystemSpecification _currentSystem;

		/// <summary>
		/// Singleton instance - globally accessible throughout application
		/// </summary>
		public static UnitSystemService Instance => _instance ??= new UnitSystemService();

		/// <summary>
		/// Current active unit system - all conversions use this
		/// </summary>
		public IUnitSystemSpecification Current => _currentSystem;

		/// <summary>
		/// Currently active system type
		/// </summary>
		public UnitSystemType ActiveType { get; private set; }

		private UnitSystemService()
		{
			// Default to MKS system
			SetUnitSystem(UnitSystemType.MKS);
		}

		/// <summary>
		/// Set the unit system for the entire application
		/// Call once at startup, everything else just works
		/// </summary>
		public void SetUnitSystem(UnitSystemType type)
		{
			_currentSystem = type switch
			{
				UnitSystemType.MKS => new MKSUnitSystemSpecification(),
				UnitSystemType.SI => new SIUnitSystemSpecification(),
				UnitSystemType.CGS => new CGSUnitSystemSpecification(),
				UnitSystemType.FPS => new FPSUnitSystemSpecification(),
				UnitSystemType.IPS => new IPSUnitSystemSpecification(),
				UnitSystemType.mmNs => new mmNsUnitSystemSpecification(),
				_ => new MKSUnitSystemSpecification()
			};

			ActiveType = type;
		}

		/// <summary>
		/// Convert between any two units in the current system
		/// </summary>
		public double Convert(double value, string fromUnit, string toUnit)
		{
			var fromDef = _currentSystem.UnitDefinitions.FirstOrDefault(u => u.Symbol == fromUnit);
			var toDef = _currentSystem.UnitDefinitions.FirstOrDefault(u => u.Symbol == toUnit);

			if (fromDef == null) throw new ArgumentException($"Unknown unit: {fromUnit}");
			if (toDef == null) throw new ArgumentException($"Unknown unit: {toUnit}");
			if (fromDef.Family != toDef.Family) throw new ArgumentException($"Cannot convert {fromUnit} to {toUnit} - different unit families");

			// Hub-and-spoke conversion: from → base → to
			var baseValue = fromDef.ConvertToBase(value);
			return toDef.ConvertFromBase(baseValue);
		}

		/// <summary>
		/// Check if a unit is valid in the current system
		/// </summary>
		public bool IsValidUnit(string unit) => _currentSystem.GetAllUnitSymbols().Contains(unit);

		/// <summary>
		/// Check if a unit belongs to the specified family
		/// </summary>
		public bool IsValidUnit(string unit, UnitFamilyName family)
		{
			var symbolToFamily = _currentSystem.GetSymbolToFamilyMap();
			return symbolToFamily.ContainsKey(unit) && symbolToFamily[unit] == family;
		}

		/// <summary>
		/// Get all units for a specific family in the current system
		/// </summary>
		public List<string> GetUnitsForFamily(UnitFamilyName family)
		{
			var unitsByFamily = _currentSystem.GetAllUnitsByFamily();
			return unitsByFamily.ContainsKey(family) 
				? unitsByFamily[family].Select(u => u.Symbol).ToList()
				: new List<string>();
		}

		/// <summary>
		/// Get the base unit for a family in the current system
		/// </summary>
		public string GetBaseUnitForFamily(UnitFamilyName family)
		{
			var baseUnits = _currentSystem.GetBaseUnitsByFamily();
			return baseUnits.ContainsKey(family) ? baseUnits[family].Symbol : "";
		}
	}
}
