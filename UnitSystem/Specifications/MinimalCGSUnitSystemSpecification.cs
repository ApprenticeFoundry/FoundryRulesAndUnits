using System;
using System.Collections.Generic;
using FoundryRulesAndUnits.Units;

namespace FoundryRulesAndUnits.Units.Specifications
{
    /// <summary>
    /// MINIMAL CGS Unit System Specification - NO HARD-CODED CONVERSION MATRICES!
    /// Auto-generates all conversions from UnitDefinition records
    /// This demonstrates the elimination of massive conversion matrices
    /// </summary>
    public class MinimalCGSUnitSystemSpecification : BaseUnitSystemSpecification
    {
        public override string SystemName => "Minimal-CGS";
        
        public override Dictionary<string, string> GetBaseUnits()
        {
            return new Dictionary<string, string>
            {
                ["Length"] = "cm",
                ["Mass"] = "g", 
                ["Force"] = "dyne",
                ["Temperature"] = "C"
            };
        }
        
        public override Dictionary<string, UnitDefinition> GetUnitDefinitions()
        {
            return new Dictionary<string, UnitDefinition>
            {
                // Length units (centimeters as base) - ALL CONVERSIONS AUTO-GENERATED!
                ["cm"] = UnitDefinition.BaseUnit("cm", "centimeters", UnitFamilyName.Length),
                ["mm"] = UnitDefinition.LinearUnit("mm", "millimeters", UnitFamilyName.Length, 0.1),
                ["m"] = UnitDefinition.LinearUnit("m", "meters", UnitFamilyName.Length, 100.0),
                ["km"] = UnitDefinition.LinearUnit("km", "kilometers", UnitFamilyName.Length, 100000.0),
                
                // Mass units (grams as base) - ALL CONVERSIONS AUTO-GENERATED!
                ["g"] = UnitDefinition.BaseUnit("g", "grams", UnitFamilyName.Mass),
                ["mg"] = UnitDefinition.LinearUnit("mg", "milligrams", UnitFamilyName.Mass, 0.001),
                ["kg"] = UnitDefinition.LinearUnit("kg", "kilograms", UnitFamilyName.Mass, 1000.0),
                
                // Force units (dynes as base) - ALL CONVERSIONS AUTO-GENERATED!
                ["dyne"] = UnitDefinition.BaseUnit("dyne", "dynes", UnitFamilyName.Force),
                ["N"] = UnitDefinition.LinearUnit("N", "newtons", UnitFamilyName.Force, 100000.0),
                
                // Temperature units (Celsius as base) - ALL CONVERSIONS AUTO-GENERATED!
                ["C"] = UnitDefinition.BaseUnit("C", "Celsius", UnitFamilyName.Temperature),
                ["F"] = UnitDefinition.DerivedUnit("F", "Fahrenheit", UnitFamilyName.Temperature,
                    f => (f - 32.0) * 5.0/9.0,       // F to C
                    c => c * 9.0/5.0 + 32.0),        // C to F
                ["K"] = UnitDefinition.DerivedUnit("K", "Kelvin", UnitFamilyName.Temperature,
                    k => k - 273.15,                 // K to C
                    c => c + 273.15),                // C to K
            };
        }
        
        /// <summary>
        /// NO HARD-CODED CONVERSION MATRICES! 
        /// Returns empty dictionary - all conversions are auto-generated from UnitDefinition records
        /// </summary>
        public override Dictionary<string, UnitConversion> GetBaseUnitConversions()
        {
            // 🎯 ZERO hard-coded conversions! Everything is auto-generated!
            return new Dictionary<string, UnitConversion>();
        }
        
        public override Dictionary<string, string> GetUnitDisplayNames()
        {
            return new Dictionary<string, string>
            {
                ["cm"] = "centimeters",
                ["mm"] = "millimeters", 
                ["m"] = "meters",
                ["km"] = "kilometers",
                ["g"] = "grams",
                ["mg"] = "milligrams",
                ["kg"] = "kilograms",
                ["dyne"] = "dynes",
                ["N"] = "newtons",
                ["C"] = "Celsius",
                ["F"] = "Fahrenheit",
                ["K"] = "Kelvin"
            };
        }
    }
}