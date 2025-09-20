using System;
using System.Collections.Generic;
using System.Linq;
using FoundryRulesAndUnits.Units;
using FoundryRulesAndUnits.Units.Specifications;

namespace FoundryRulesAndUnits.Units;

/// <summary>
/// Unified IUnitSystem interface providing complete unit system functionality
/// Replaces the previous UnitSystemService singleton pattern with a cleaner approach
/// </summary>
public interface IUnitSystem
{
    /// <summary>
    /// Current active unit system specification
    /// </summary>
    IUnitSystemSpecification Current { get; }

    /// <summary>
    /// Currently active system type
    /// </summary>
    UnitSystemType ActiveType { get; }

    /// <summary>
    /// Set/change the unit system type
    /// </summary>
    void Apply(UnitSystemType systemType);

    /// <summary>
    /// Convert value between any two units in the current system
    /// </summary>
    double Convert(double value, string fromUnit, string toUnit);

    /// <summary>
    /// Check if a unit is valid in the current system
    /// </summary>
    bool IsValidUnit(string unit);

    /// <summary>
    /// Check if a unit belongs to the specified family
    /// </summary>
    bool IsValidUnit(string unit, UnitFamilyName family);

    /// <summary>
    /// Get all units for a specific family in the current system
    /// </summary>
    List<string> GetUnitsForFamily(UnitFamilyName family);

    /// <summary>
    /// Get the base unit for a family in the current system
    /// </summary>
    string GetBaseUnitForFamily(UnitFamilyName family);

    // NEW: Enhanced services replacing UnitCategory functionality
    
    /// <summary>
    /// Get detailed unit metadata for a specific unit symbol
    /// Replaces UnitCategory unit lookup functionality
    /// </summary>
    UnitDefinition? GetUnitMetadata(string unitSymbol);

    /// <summary>
    /// Get all known units in the current system
    /// Replaces UnitCategory.Units() functionality
    /// </summary>
    List<UnitDefinition> GetAllKnownUnits();

    /// <summary>
    /// Get all units organized by family
    /// Replaces complex UnitCategory traversal
    /// </summary>
    Dictionary<UnitFamilyName, List<UnitDefinition>> GetAllUnitsByFamily();

    /// <summary>
    /// Get all valid unit symbols in the current system
    /// Useful for validation and UI population
    /// </summary>
    List<string> GetAllUnitSymbols();
}


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


}


