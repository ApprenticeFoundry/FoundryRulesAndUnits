using System;
using System.Collections.Generic;
using FoundryRulesAndUnits.Units;
using FoundryRulesAndUnits.Units.Specifications;

namespace FoundryRulesAndUnits.Units;

/// <summary>
/// Unit lookup information for efficient validation and creation
/// </summary>
public record UnitLookupInfo(
    UnitFamilyName Family,
    UnitDefinition Definition,
    Func<double, MeasuredValue> CreateMeasuredValue
);

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
    /// Get unit family for a given unit symbol (efficient O(1) lookup)
    /// Returns UnitFamilyName.None if unit is not found
    /// </summary>
    UnitFamilyName GetUnitFamily(string unit);

    /// <summary>
    /// Try to get complete unit information for efficient operations
    /// Returns true if unit exists, false otherwise
    /// </summary>
    bool TryGetUnitInfo(string unit, out UnitLookupInfo? unitInfo);

    /// <summary>
    /// Create a MeasuredValue directly from unit symbol and value (efficient)
    /// Throws ArgumentException if unit is not valid
    /// </summary>
    MeasuredValue CreateMeasuredValueFromUnit(string unit, double value);

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

    // NEW: Static factory methods for simplified measurement creation
    
    /// <summary>
    /// Create a UnitFactory for this unit system for advanced usage
    /// </summary>
    UnitFactory GetFactory();

    // Quick measurement creation methods using the current system

    /// <summary>
    /// Create a Length measurement with the current unit system
    /// </summary>
    Length CreateLength(double value = 0, string? units = null);

    /// <summary>
    /// Create an Angle measurement with the current unit system
    /// </summary>
    Angle CreateAngle(double value = 0, string? units = null);

    /// <summary>
    /// Create a Temperature measurement with the current unit system
    /// </summary>
    Temperature CreateTemperature(double value = 0, string? units = null);

    /// <summary>
    /// Create a Mass measurement with the current unit system
    /// </summary>
    Mass CreateMass(double value = 0, string? units = null);

    /// <summary>
    /// Create a Time measurement with the current unit system
    /// </summary>
    Time CreateTime(double value = 0, string? units = null);

    /// <summary>
    /// Create a Speed measurement with the current unit system
    /// </summary>
    Speed CreateSpeed(double value = 0, string? units = null);

    /// <summary>
    /// Create an Area measurement with the current unit system
    /// </summary>
    Area CreateArea(double value = 0, string? units = null);

    /// <summary>
    /// Create a Volume measurement with the current unit system
    /// </summary>
    Volume CreateVolume(double value = 0, string? units = null);

    /// <summary>
    /// Create a Force measurement with the current unit system
    /// </summary>
    Force CreateForce(double value = 0, string? units = null);

    /// <summary>
    /// Create a Power measurement with the current unit system
    /// </summary>
    Power CreatePower(double value = 0, string? units = null);

    /// <summary>
    /// Create a Voltage measurement with the current unit system
    /// </summary>
    Voltage CreateVoltage(double value = 0, string? units = null);

    /// <summary>
    /// Create a Current measurement with the current unit system
    /// </summary>
    Current CreateCurrent(double value = 0, string? units = null);

    /// <summary>
    /// Create a Resistance measurement with the current unit system
    /// </summary>
    Resistance CreateResistance(double value = 0, string? units = null);

    /// <summary>
    /// Create a Capacitance measurement with the current unit system
    /// </summary>
    Capacitance CreateCapacitance(double value = 0, string? units = null);

    /// <summary>
    /// Create a Frequency measurement with the current unit system
    /// </summary>
    Frequency CreateFrequency(double value = 0, string? units = null);

    /// <summary>
    /// Create a generic MeasuredValue for any unit family
    /// </summary>
    MeasuredValue CreateMeasuredValue(UnitFamilyName family, double value = 0, string? units = null);



    // Static convenience methods for quick unit system creation and measurement creation

    /// <summary>
    /// Create a UnitSystem with SI units
    /// </summary>
    static IUnitSystem SI() => new UnitSystem(UnitSystemType.SI);

    /// <summary>
    /// Create a UnitSystem with MKS units
    /// </summary>
    static IUnitSystem MKS() => new UnitSystem(UnitSystemType.MKS);

    /// <summary>
    /// Create a UnitSystem with FPS units
    /// </summary>
    static IUnitSystem FPS() => new UnitSystem(UnitSystemType.FPS);

    /// <summary>
    /// Create a UnitSystem with IPS units
    /// </summary>
    static IUnitSystem IPS() => new UnitSystem(UnitSystemType.IPS);

    /// <summary>
    /// Create a UnitSystem with CGS units
    /// </summary>
    static IUnitSystem CGS() => new UnitSystem(UnitSystemType.CGS);
}