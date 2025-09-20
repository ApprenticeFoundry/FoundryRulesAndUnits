using System;
using System.Collections.Generic;
using System.Linq;
using FoundryRulesAndUnits.Units;
using FoundryRulesAndUnits.Units.Specifications;

namespace FoundryRulesAndUnits.Units;

/// <summary>
/// Complete UnitSystem implementation with all conversion functionality
/// No longer depends on UnitSystemService - everything is self-contained
/// </summary>
public class UnitSystem : IUnitSystem
{
    private IUnitSystemSpecification _currentSystem;

    /// <summary>
    /// Current active unit system specification
    /// </summary>
    public IUnitSystemSpecification Current => _currentSystem;

    /// <summary>
    /// Currently active system type
    /// </summary>
    public UnitSystemType ActiveType { get; private set; }

    public UnitSystem()
    {
        // Default to MKS system
        Apply(UnitSystemType.MKS);
    }

    public UnitSystem(UnitSystemType systemType)
    {
        Apply(systemType);
    }

    public static List<UnitFamilyName> GetAllUnitFamilies()
    {
        return Enum.GetValues<UnitFamilyName>().Where(f => f != UnitFamilyName.None).ToList();
    }

    /// <summary>
    /// Set/change the unit system type
    /// </summary>
    public void Apply(UnitSystemType systemType)
    {
        _currentSystem = systemType switch
        {
            UnitSystemType.MKS => new MKSUnitSystemSpecification(),
            UnitSystemType.SI => new SIUnitSystemSpecification(),
            UnitSystemType.CGS => new CGSUnitSystemSpecification(),
            UnitSystemType.FPS => new FPSUnitSystemSpecification(),
            UnitSystemType.IPS => new IPSUnitSystemSpecification(),
            UnitSystemType.mmNs => new mmNsUnitSystemSpecification(),
            _ => new MKSUnitSystemSpecification()
        };

        ActiveType = systemType;
    }

    /// <summary>
    /// Convert value between any two units in the current system
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

    // NEW: Enhanced services replacing UnitCategory functionality

    /// <summary>
    /// Get detailed unit metadata for a specific unit symbol
    /// Replaces UnitCategory unit lookup functionality
    /// </summary>
    public UnitDefinition? GetUnitMetadata(string unitSymbol)
    {
        return _currentSystem.UnitDefinitions.FirstOrDefault(u => u.Symbol == unitSymbol);
    }

    /// <summary>
    /// Get all known units in the current system
    /// Replaces UnitCategory.Units() functionality
    /// </summary>
    public List<UnitDefinition> GetAllKnownUnits()
    {
        return _currentSystem.UnitDefinitions.ToList();
    }

    /// <summary>
    /// Get all units organized by family
    /// Replaces complex UnitCategory traversal
    /// </summary>
    public Dictionary<UnitFamilyName, List<UnitDefinition>> GetAllUnitsByFamily()
    {
        return _currentSystem.GetAllUnitsByFamily();
    }

    /// <summary>
    /// Get all valid unit symbols in the current system
    /// Useful for validation and UI population
    /// </summary>
    public List<string> GetAllUnitSymbols()
    {
        return _currentSystem.GetAllUnitSymbols();
    }

    // NEW: Factory methods for simplified measurement creation

    private UnitFactory? _cachedFactory;

    /// <summary>
    /// Create a UnitFactory for this unit system for advanced usage
    /// </summary>
    public UnitFactory GetFactory()
    {
        // Cache the factory to avoid recreating UnitGroups repeatedly
        if (_cachedFactory == null || _cachedFactory.SystemType != ActiveType)
        {
            _cachedFactory = new UnitFactory(ActiveType);
        }
        return _cachedFactory;
    }

    // Quick measurement creation methods using the current system

    /// <summary>
    /// Create a Length measurement with the current unit system
    /// </summary>
    public Length CreateLength(double value = 0, string? units = null)
    {
        return GetFactory().CreateLength(value, units);
    }

    /// <summary>
    /// Create an Angle measurement with the current unit system
    /// </summary>
    public Angle CreateAngle(double value = 0, string? units = null)
    {
        return GetFactory().CreateAngle(value, units);
    }

    /// <summary>
    /// Create a Temperature measurement with the current unit system
    /// </summary>
    public Temperature CreateTemperature(double value = 0, string? units = null)
    {
        return GetFactory().CreateTemperature(value, units);
    }

    /// <summary>
    /// Create a Mass measurement with the current unit system
    /// </summary>
    public Mass CreateMass(double value = 0, string? units = null)
    {
        return GetFactory().CreateMass(value, units);
    }

    /// <summary>
    /// Create a Time measurement with the current unit system
    /// </summary>
    public Time CreateTime(double value = 0, string? units = null)
    {
        return GetFactory().CreateTime(value, units);
    }

    /// <summary>
    /// Create a Speed measurement with the current unit system
    /// </summary>
    public Speed CreateSpeed(double value = 0, string? units = null)
    {
        return GetFactory().CreateSpeed(value, units);
    }

    /// <summary>
    /// Create an Area measurement with the current unit system
    /// </summary>
    public Area CreateArea(double value = 0, string? units = null)
    {
        return GetFactory().CreateArea(value, units);
    }

    /// <summary>
    /// Create a Volume measurement with the current unit system
    /// </summary>
    public Volume CreateVolume(double value = 0, string? units = null)
    {
        return GetFactory().CreateVolume(value, units);
    }

    /// <summary>
    /// Create a Force measurement with the current unit system
    /// </summary>
    public Force CreateForce(double value = 0, string? units = null)
    {
        return GetFactory().CreateForce(value, units);
    }

    /// <summary>
    /// Create a Power measurement with the current unit system
    /// </summary>
    public Power CreatePower(double value = 0, string? units = null)
    {
        return GetFactory().CreatePower(value, units);
    }

    /// <summary>
    /// Create a Voltage measurement with the current unit system
    /// </summary>
    public Voltage CreateVoltage(double value = 0, string? units = null)
    {
        return GetFactory().CreateVoltage(value, units);
    }

    /// <summary>
    /// Create a Current measurement with the current unit system
    /// </summary>
    public Current CreateCurrent(double value = 0, string? units = null)
    {
        return GetFactory().CreateCurrent(value, units);
    }

    /// <summary>
    /// Create a Resistance measurement with the current unit system
    /// </summary>
    public Resistance CreateResistance(double value = 0, string? units = null)
    {
        return GetFactory().CreateResistance(value, units);
    }

    /// <summary>
    /// Create a Capacitance measurement with the current unit system
    /// </summary>
    public Capacitance CreateCapacitance(double value = 0, string? units = null)
    {
        return GetFactory().CreateCapacitance(value, units);
    }

    /// <summary>
    /// Create a Frequency measurement with the current unit system
    /// </summary>
    public Frequency CreateFrequency(double value = 0, string? units = null)
    {
        return GetFactory().CreateFrequency(value, units);
    }

    /// <summary>
    /// Create a generic MeasuredValue for any unit family
    /// </summary>
    public MeasuredValue CreateMeasuredValue(UnitFamilyName family, double value = 0, string? units = null)
    {
        return GetFactory().CreateMeasuredValue(family, value, units);
    }



}


