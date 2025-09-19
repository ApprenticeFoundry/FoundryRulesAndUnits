using System;
using System.Collections.Generic;

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
    
    public record UnitDefinition(string Symbol, string Name, string Family);
    
    public interface IUnitSystemSpecification
    {
        string SystemName { get; }
        
        /// <summary>
        /// Gets the base unit for each unit family in this system
        /// </summary>
        Dictionary<string, string> GetBaseUnits();
        
        /// <summary>
        /// Gets all unit definitions in this system
        /// </summary>
        Dictionary<string, UnitDefinition> GetUnitDefinitions();
        
        /// <summary>
        /// Gets all conversions as from|to -> conversion factor
        /// </summary>
        Dictionary<string, UnitConversion> GetConversions();
        
        /// <summary>
        /// Gets display information for units
        /// </summary>
        Dictionary<string, string> GetUnitDisplayNames();
    }
}