using System;
using System.Collections.Generic;
using FoundryRulesAndUnits.Units;

namespace FoundryRulesAndUnits.Units.Specifications
{
    public record UnitConversion(
        double Factor = 1.0, 
        Func<double, double>? ConversionFunction = null,
        string? FormulaDescription = null
    )
    {
        /// <summary>
        /// Performs the conversion using function if provided, otherwise uses simple multiplication
        /// </summary>
        public double Convert(double value) => 
            ConversionFunction?.Invoke(value) ?? (value * Factor);
            
        /// <summary>
        /// Creates a simple linear conversion (multiplication by factor)
        /// </summary>
        public static UnitConversion Linear(double factor, string? description = null) =>
            new(Factor: factor, FormulaDescription: description);
            
        /// <summary>
        /// Creates a function-based conversion for non-linear relationships
        /// </summary>
        public static UnitConversion Function(Func<double, double> func, string description) =>
            new(ConversionFunction: func, FormulaDescription: description);
    }
    
    /// <summary>
    /// Enhanced unit definition with embedded conversion functions
    /// Eliminates the need for massive conversion matrices by storing conversions in each unit
    /// Uses UnitFamilyName enum for type safety and consistency
    /// </summary>
    public record UnitDefinition(
        string Symbol, 
        string Name, 
        UnitFamilyName Family,
        Func<double, double>? ToBaseUnit = null,     // Convert FROM this unit TO base unit
        Func<double, double>? FromBaseUnit = null    // Convert FROM base unit TO this unit
    )
    {
        /// <summary>
        /// For base units, both functions are identity (x => x)
        /// </summary>
        public static UnitDefinition BaseUnit(string symbol, string name, UnitFamilyName family) =>
            new(symbol, name, family, x => x, x => x);
            
        /// <summary>
        /// For derived units, provide conversion to/from base unit
        /// </summary>
        public static UnitDefinition DerivedUnit(string symbol, string name, UnitFamilyName family, 
            Func<double, double> toBase, Func<double, double> fromBase) =>
            new(symbol, name, family, toBase, fromBase);
            
        /// <summary>
        /// Simple linear conversion (multiplication/division)
        /// Most common case - automatically generates inverse function
        /// </summary>
        public static UnitDefinition LinearUnit(string symbol, string name, UnitFamilyName family, double factor) =>
            new(symbol, name, family, 
                toBase: x => x * factor,      // e.g., inches to feet: x * (1/12)
                fromBase: x => x / factor);   // e.g., feet to inches: x / (1/12) = x * 12
                
        /// <summary>
        /// Convert any value from this unit to any other unit in the same family
        /// This replaces the entire conversion matrix lookup system!
        /// </summary>
        public double ConvertTo(double value, UnitDefinition targetUnit)
        {
            if (Family != targetUnit.Family)
                throw new InvalidOperationException($"Cannot convert from {Family} to {targetUnit.Family}");
                
            // The magic: thisUnit -> baseUnit -> targetUnit
            var baseValue = ToBaseUnit?.Invoke(value) ?? value;
            return targetUnit.FromBaseUnit?.Invoke(baseValue) ?? baseValue;
        }
        
        /// <summary>
        /// Check if this unit can convert to the target unit (same family)
        /// </summary>
        public bool CanConvertTo(UnitDefinition targetUnit) => Family == targetUnit.Family;
        
        /// <summary>
        /// Get the conversion factor to base unit (for linear conversions only)
        /// </summary>
        public double? GetLinearFactorToBase()
        {
            if (ToBaseUnit == null) return null;
            try
            {
                // Test with value 1.0 to get the factor
                return ToBaseUnit(1.0);
            }
            catch
            {
                // Non-linear conversion
                return null;
            }
        }
    };
    
    public interface IUnitSystemSpecification
    {
        string SystemName { get; }
        
        /// <summary>
        /// Gets the base unit for each unit family in this system
        /// </summary>
        Dictionary<UnitFamilyName, string> GetBaseUnits();
        
        /// <summary>
        /// Gets all unit definitions in this system
        /// </summary>
        Dictionary<string, UnitDefinition> GetUnitDefinitions();
        
        /// <summary>
        /// Gets conversions TO and FROM base units only (base unit hub architecture)
        /// Key format: "unit|baseUnit" or "baseUnit|unit"
        /// </summary>
        Dictionary<string, UnitConversion> GetBaseUnitConversions();
        
        /// <summary>
        /// Gets display information for units
        /// </summary>
        Dictionary<string, string> GetUnitDisplayNames();
        
        /// <summary>
        /// Gets the unit family for a given unit symbol
        /// </summary>
        string GetUnitFamily(string unitSymbol);
        
        /// <summary>
        /// Universal conversion method: converts any unit to any other unit via base unit hub
        /// </summary>
        double Convert(double value, string fromUnit, string toUnit);
        
        /// <summary>
        /// Gets all possible conversions (generated dynamically from base unit conversions)
        /// </summary>
        Dictionary<string, UnitConversion> GetAllPossibleConversions();
    }
}