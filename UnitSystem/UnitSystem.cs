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
    private IUnitSystemSpecification _currentSystem = null!;
    private Dictionary<string, UnitLookupInfo>? _cachedUnitLookup = null;

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
    public IUnitSystemSpecification Apply(UnitSystemType systemType)
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

        // Clear cached lookup when system changes
        _cachedUnitLookup = null;
        return _currentSystem;
    }

    /// <summary>
    /// Build efficient unit lookup cache for O(1) validation and metadata access
    /// Uses CreateTypedMeasuredValue for parser compatibility
    /// 
    /// PHASE 1 ENHANCEMENT: Two-Tier Unit Family System
    /// Only includes parser-accessible families in lookup cache to eliminate ambiguity.
    /// Function-only families (Distance, Time, Bearing, etc.) must be created via AS functions.
    /// 
    /// ELIMINATED AMBIGUITY CASES:
    /// - Time Domain: "s" now only maps to Duration (parser-accessible), Time requires ASTIME()
    /// - Angular Domain: "deg" now only maps to Angle (parser-accessible), Bearing requires ASBEARING()
    /// - Length Domain: "m" now only maps to Length (parser-accessible), Distance requires ASDISTANCE()
    /// 
    /// PARSER BENEFITS:
    /// - Zero ambiguity: Each unit symbol maps to exactly one family
    /// - Predictable behavior: 5m→Length, 45deg→Angle, 30s→Duration
    /// - Performance: No runtime disambiguation needed
    /// </summary>
    private Dictionary<string, UnitLookupInfo> BuildUnitLookupCache()
    {
        var lookup = new Dictionary<string, UnitLookupInfo>();
        var unitGroups = _currentSystem.GetUnitGroups();
        
        foreach (var unitGroup in unitGroups.Values)
        {
            // CRITICAL FILTER: Only include parser-accessible families in lookup cache
            // This eliminates the "last one wins" ambiguity problem completely!
            if (unitGroup.IsParserAccessible)
            {
                // Add all units from this parser-accessible family
                foreach (var unitDef in unitGroup.Members)
                {
                    // Create factory function using reflection-based method for correct derived types
                    // This is CRITICAL for parser integration that expects specific types (Angle, Length, Mass, etc.)
                    Func<double, MeasuredValue> createFunc = (value) => 
                        CreateTypedMeasuredValue(unitDef.Family, value, unitDef.Symbol);
                    
                    lookup[unitDef.Symbol] = new UnitLookupInfo(
                        unitDef.Family,
                        unitDef,
                        createFunc
                    );
                }
                
                // Also add the base unit if it's not already included
                if (!lookup.ContainsKey(unitGroup.BaseUnit.Symbol))
                {
                    Func<double, MeasuredValue> createFunc = (value) => 
                        CreateTypedMeasuredValue(unitGroup.BaseUnit.Family, value, unitGroup.BaseUnit.Symbol);
                    
                    lookup[unitGroup.BaseUnit.Symbol] = new UnitLookupInfo(
                        unitGroup.BaseUnit.Family,
                        unitGroup.BaseUnit,
                        createFunc
                    );
                }
            }
            // Function-only families are excluded from parser cache
            // They can only be created via AS functions (ASDISTANCE, ASTIME, ASBEARING, etc.)
        }
        
        return lookup;
    }

    /// <summary>
    /// Get cached unit lookup dictionary for efficient operations
    /// </summary>
    private Dictionary<string, UnitLookupInfo> GetUnitLookup()
    {
        if (_cachedUnitLookup == null)
        {
            _cachedUnitLookup = BuildUnitLookupCache();
        }
        return _cachedUnitLookup;
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
    /// Check if a unit is valid in the current system (O(1) lookup)
    /// </summary>
    public bool IsValidUnit(string unit) => GetUnitLookup().ContainsKey(unit);

    /// <summary>
    /// Check if a unit belongs to the specified family (O(1) lookup)
    /// </summary>
    public bool IsValidUnit(string unit, UnitFamilyName family)
    {
        // Use the complete specification data, not just parser-accessible cache
        // This ensures all families (including non-parser-accessible ones like Distance) work correctly
        var allUnitsByFamily = GetAllUnitsByFamily();
        if (!allUnitsByFamily.TryGetValue(family, out var unitsInFamily))
        {
            return false;
        }
        
        return unitsInFamily.Any(u => u.Symbol == unit);
    }

    /// <summary>
    /// Get unit family for a given unit symbol (O(1) lookup)
    /// Returns UnitFamilyName.None if unit is not found
    /// </summary>
    public UnitFamilyName GetUnitFamily(string unit)
    {
        var lookup = GetUnitLookup();
        return lookup.ContainsKey(unit) ? lookup[unit].Family : UnitFamilyName.None;
    }

    /// <summary>
    /// Try to get complete unit information for efficient operations
    /// Returns true if unit exists, false otherwise
    /// </summary>
    public bool TryGetUnitInfo(string unit, out UnitLookupInfo? unitInfo)
    {
        var lookup = GetUnitLookup();
        return lookup.TryGetValue(unit, out unitInfo);
    }

    /// <summary>
    /// Determine the unit family from a parsable unit symbol, then create the MeasuredValue
    /// Step 1: Look up unit to determine family (centralized lookup logic)
    /// Step 2: Call CreateMeasuredValue(family, value, units) - single creation path
    /// Throws ArgumentException if unit is not valid in parser-accessible families
    /// </summary>
    public MeasuredValue CreateMeasuredValueFromParsableUnit(string unit, double value)
    {
        // Step 1: Determine family from unit symbol (centralized lookup)
        var family = DetermineUnitFamilyFromUnit(unit);
        
        // Step 2: Delegate to family-based creation - single code path
        return CreateMeasuredValue(family, value, unit);
    }

    /// <summary>
    /// Determine the unit family from a unit symbol
    /// Centralizes all unit lookup logic in UnitSystem class
    /// This will help eliminate magic strings over time by keeping lookups in one place
    /// </summary>
    public UnitFamilyName DetermineUnitFamilyFromUnit(string unit)
    {
        var family = GetUnitFamily(unit);
        if (family == UnitFamilyName.None)
        {
            throw new ArgumentException($"Invalid unit symbol: {unit}");
        }
        return family;
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

    /// <summary>
    /// Create a strongly typed unit object with compile-time type safety
    /// Uses attribute-based reflection to create the correct derived type
    /// </summary>
    public T CreateUnit<T>(double value = 0, string? units = null) where T : MeasuredValue
    {
        var type = typeof(T);
        var attribute = UnitTypeRegistry.GetAttributeForType(type);
        if (attribute == null)
            throw new ArgumentException($"Type {type.Name} is not registered with UnitTypeAttribute");

        // Create typed measured value directly instead of using factory
        var instance = CreateTypedMeasuredValue(attribute.Family, value, units);
        
        // Safe cast since UnitTypeRegistry guarantees correct type for family
        return (T)instance;
    }

    /// <summary>
    /// Create the correct derived MeasuredValue type using attribute-based reflection
    /// This is CRITICAL for parser integration that expects specific types (Angle, Length, Mass, etc.)
    /// </summary>
    public MeasuredValue CreateTypedMeasuredValue(UnitFamilyName family, double value = 0, string? units = null)
    {
        // Get the authoritative UnitGroup from the unit system specification
        var unitGroups = Current.GetUnitGroups();
        if (!unitGroups.TryGetValue(family, out var unitGroup))
        {
            throw new ArgumentException($"No unit group found for family {family} in current unit system");
        }
        
        // Use attribute-based registry to create the correct derived type
        var instance = UnitTypeRegistry.CreateInstance(family, unitGroup);
        if (instance == null)
        {
            // Fallback to creating via base MeasuredValue
            var baseMeasuredValue = new MeasuredValue(unitGroup);
            baseMeasuredValue.Init(value, units);
            return baseMeasuredValue;
        }
        
        // Initialize the derived instance with the original units parameter to preserve display units
        instance.Init(value, units);
        
        return instance;
    }

    // Quick measurement creation methods using the current system

    /// <summary>
    /// Create a Length measurement with the current unit system
    /// </summary>
    public Length CreateLength(double value = 0, string? units = null)
    {
        return CreateUnit<Length>(value, units);
    }

    /// <summary>
    /// Create an Angle measurement with the current unit system
    /// </summary>
    public Angle CreateAngle(double value = 0, string? units = null)
    {
        return CreateUnit<Angle>(value, units);
    }

    /// <summary>
    /// Create a Temperature measurement with the current unit system
    /// </summary>
    public Temperature CreateTemperature(double value = 0, string? units = null)
    {
        return CreateUnit<Temperature>(value, units);
    }

    /// <summary>
    /// Create a Mass measurement with the current unit system
    /// </summary>
    public Mass CreateMass(double value = 0, string? units = null)
    {
        return CreateUnit<Mass>(value, units);
    }

    /// <summary>
    /// Create a Time measurement with the current unit system
    /// </summary>
    public Time CreateTime(double value = 0, string? units = null)
    {
        return CreateUnit<Time>(value, units);
    }

    /// <summary>
    /// Create a Speed measurement with the current unit system
    /// </summary>
    public Speed CreateSpeed(double value = 0, string? units = null)
    {
        return CreateUnit<Speed>(value, units);
    }

    /// <summary>
    /// Create an Area measurement with the current unit system
    /// </summary>
    public Area CreateArea(double value = 0, string? units = null)
    {
        return CreateUnit<Area>(value, units);
    }

    /// <summary>
    /// Create a Volume measurement with the current unit system
    /// </summary>
    public Volume CreateVolume(double value = 0, string? units = null)
    {
        return CreateUnit<Volume>(value, units);
    }

    /// <summary>
    /// Create a Force measurement with the current unit system
    /// </summary>
    public Force CreateForce(double value = 0, string? units = null)
    {
        return CreateUnit<Force>(value, units);
    }

    /// <summary>
    /// Create a Power measurement with the current unit system
    /// </summary>
    public Power CreatePower(double value = 0, string? units = null)
    {
        return CreateUnit<Power>(value, units);
    }

    /// <summary>
    /// Create a Voltage measurement with the current unit system
    /// </summary>
    public Voltage CreateVoltage(double value = 0, string? units = null)
    {
        return CreateUnit<Voltage>(value, units);
    }

    /// <summary>
    /// Create a Current measurement with the current unit system
    /// </summary>
    public Current CreateCurrent(double value = 0, string? units = null)
    {
        return CreateUnit<Current>(value, units);
    }

    /// <summary>
    /// Create a Resistance measurement with the current unit system
    /// </summary>
    public Resistance CreateResistance(double value = 0, string? units = null)
    {
        return CreateUnit<Resistance>(value, units);
    }

    /// <summary>
    /// Create a Capacitance measurement with the current unit system
    /// </summary>
    public Capacitance CreateCapacitance(double value = 0, string? units = null)
    {
        return CreateUnit<Capacitance>(value, units);
    }

    /// <summary>
    /// Create a Frequency measurement with the current unit system
    /// </summary>
    public Frequency CreateFrequency(double value = 0, string? units = null)
    {
        return CreateUnit<Frequency>(value, units);
    }

    /// <summary>
    /// Create a generic MeasuredValue for any unit family
    /// </summary>
    public MeasuredValue CreateMeasuredValue(UnitFamilyName family, double value = 0, string? units = null)
    {
        return CreateTypedMeasuredValue(family, value, units);
    }



}


