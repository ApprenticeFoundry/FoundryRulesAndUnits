using System;
using System.Collections.Generic;
using FoundryRulesAndUnits.Units;

namespace FoundryRulesAndUnits.Units.Specifications
{
    /// <summary>
    /// CGS (Centimeter-Gram-Second) Unit System Specification
    /// Base units: centimeters, grams, seconds, Celsius, radians, dynes
    /// </summary>
    public class CGSUnitSystemSpecification : BaseUnitSystemSpecification
    {
        public override string SystemName => "CGS";
        
        public override Dictionary<UnitFamilyName, string> GetBaseUnits()
        {
            return new Dictionary<UnitFamilyName, string>
            {
                [UnitFamilyName.Length] = "cm",
                [UnitFamilyName.Mass] = "g", 
                [UnitFamilyName.Time] = "s",
                [UnitFamilyName.Temperature] = "C",
                [UnitFamilyName.Angle] = "rad",
                [UnitFamilyName.Force] = "dyne",
                [UnitFamilyName.Area] = "cm2",
                [UnitFamilyName.Volume] = "cm3",
                [UnitFamilyName.Speed] = "cm/s"
            };
        }
        
        public override Dictionary<string, UnitDefinition> GetUnitDefinitions()
        {
            return new Dictionary<string, UnitDefinition>
            {
                // Length units (centimeters as base) - using enhanced approach with UnitFamilyName enum!
                ["cm"] = UnitDefinition.BaseUnit("cm", "centimeters", UnitFamilyName.Length),
                ["mm"] = UnitDefinition.LinearUnit("mm", "millimeters", UnitFamilyName.Length, 0.1),           // 1 mm = 0.1 cm
                ["m"] = UnitDefinition.LinearUnit("m", "meters", UnitFamilyName.Length, 100.0),                // 1 m = 100 cm
                ["km"] = UnitDefinition.LinearUnit("km", "kilometers", UnitFamilyName.Length, 100000.0),       // 1 km = 100,000 cm
                ["in"] = UnitDefinition.LinearUnit("in", "inches", UnitFamilyName.Length, 2.54),               // 1 in = 2.54 cm
                ["ft"] = UnitDefinition.LinearUnit("ft", "feet", UnitFamilyName.Length, 30.48),                // 1 ft = 30.48 cm
                ["yd"] = UnitDefinition.LinearUnit("yd", "yards", UnitFamilyName.Length, 91.44),               // 1 yd = 91.44 cm
                ["mi"] = UnitDefinition.LinearUnit("mi", "miles", UnitFamilyName.Length, 160934.0),            // 1 mi = 160,934 cm
                ["px"] = UnitDefinition.LinearUnit("px", "pixels", UnitFamilyName.Length, 0.0352778),          // 1 px = 0.0352778 cm (72 DPI)
                
                // Mass units (grams as base) - using enhanced approach with UnitFamilyName enum!
                ["g"] = UnitDefinition.BaseUnit("g", "grams", UnitFamilyName.Mass),
                ["mg"] = UnitDefinition.LinearUnit("mg", "milligrams", UnitFamilyName.Mass, 0.001),            // 1 mg = 0.001 g
                ["kg"] = UnitDefinition.LinearUnit("kg", "kilograms", UnitFamilyName.Mass, 1000.0),            // 1 kg = 1000 g
                ["lb"] = UnitDefinition.LinearUnit("lb", "pounds", UnitFamilyName.Mass, 453.592),              // 1 lb = 453.592 g
                ["oz"] = UnitDefinition.LinearUnit("oz", "ounces", UnitFamilyName.Mass, 28.3495),              // 1 oz = 28.3495 g
                
                // Force units (dynes as base) - using enhanced approach with UnitFamilyName enum!
                ["dyne"] = UnitDefinition.BaseUnit("dyne", "dynes", UnitFamilyName.Force),
                ["N"] = UnitDefinition.LinearUnit("N", "newtons", UnitFamilyName.Force, 100000.0),            // 1 N = 100,000 dynes
                ["kN"] = UnitDefinition.LinearUnit("kN", "kilonewtons", UnitFamilyName.Force, 100000000.0),   // 1 kN = 1e8 dynes
                ["lbf"] = UnitDefinition.LinearUnit("lbf", "pounds-force", UnitFamilyName.Force, 444822.0),   // 1 lbf = 444,822 dynes
                
                // Temperature units (Celsius as base) - using enhanced approach with UnitFamilyName enum!
                ["C"] = UnitDefinition.BaseUnit("C", "Celsius", UnitFamilyName.Temperature),
                ["F"] = UnitDefinition.DerivedUnit("F", "Fahrenheit", UnitFamilyName.Temperature,
                    f => (f - 32.0) * 5.0/9.0,       // F to C: (F-32)*5/9
                    c => c * 9.0/5.0 + 32.0),        // C to F: C*9/5 + 32
                ["K"] = UnitDefinition.DerivedUnit("K", "Kelvin", UnitFamilyName.Temperature,
                    k => k - 273.15,                 // K to C: subtract 273.15
                    c => c + 273.15),                // C to K: add 273.15
                
                // Angle units (radians as base) - using enhanced approach with UnitFamilyName enum!
                ["rad"] = UnitDefinition.BaseUnit("rad", "radians", UnitFamilyName.Angle),
                ["deg"] = UnitDefinition.LinearUnit("deg", "degrees", UnitFamilyName.Angle, Math.PI / 180.0), // 1 deg = π/180 rad
                ["mrad"] = UnitDefinition.LinearUnit("mrad", "milliradians", UnitFamilyName.Angle, 0.001),    // 1 mrad = 0.001 rad
                
                // Time units (seconds as base) - using enhanced approach with UnitFamilyName enum!
                ["s"] = UnitDefinition.BaseUnit("s", "seconds", UnitFamilyName.Time),
                ["ms"] = UnitDefinition.LinearUnit("ms", "milliseconds", UnitFamilyName.Time, 0.001),         // 1 ms = 0.001 s
                ["min"] = UnitDefinition.LinearUnit("min", "minutes", UnitFamilyName.Time, 60.0),             // 1 min = 60 s
                ["hr"] = UnitDefinition.LinearUnit("hr", "hours", UnitFamilyName.Time, 3600.0),               // 1 hr = 3600 s
                ["day"] = UnitDefinition.LinearUnit("day", "days", UnitFamilyName.Time, 86400.0),             // 1 day = 86400 s
                
                // Area units (square centimeters as base) - using enhanced approach with UnitFamilyName enum!
                ["cm2"] = UnitDefinition.BaseUnit("cm2", "square centimeters", UnitFamilyName.Area),
                ["mm2"] = UnitDefinition.LinearUnit("mm2", "square millimeters", UnitFamilyName.Area, 0.01),   // 1 mm² = 0.01 cm²
                ["m2"] = UnitDefinition.LinearUnit("m2", "square meters", UnitFamilyName.Area, 10000.0),       // 1 m² = 10,000 cm²
                
                // Volume units (cubic centimeters as base) - using enhanced approach with UnitFamilyName enum!
                ["cm3"] = UnitDefinition.BaseUnit("cm3", "cubic centimeters", UnitFamilyName.Volume),
                ["mm3"] = UnitDefinition.LinearUnit("mm3", "cubic millimeters", UnitFamilyName.Volume, 0.001), // 1 mm³ = 0.001 cm³
                ["m3"] = UnitDefinition.LinearUnit("m3", "cubic meters", UnitFamilyName.Volume, 1000000.0),    // 1 m³ = 1,000,000 cm³
                
                // Speed units (centimeters per second as base) - using enhanced approach with UnitFamilyName enum!
                ["cm/s"] = UnitDefinition.BaseUnit("cm/s", "centimeters per second", UnitFamilyName.Speed),
                ["m/s"] = UnitDefinition.LinearUnit("m/s", "meters per second", UnitFamilyName.Speed, 100.0),  // 1 m/s = 100 cm/s
                ["km/h"] = UnitDefinition.LinearUnit("km/h", "kilometers per hour", UnitFamilyName.Speed, 100000.0/3600.0) // 1 km/h = 27.778 cm/s
            };
        }
        
        /// <summary>
        /// NO HARD-CODED CONVERSION MATRICES! All conversions auto-generated from UnitDefinition records.
        /// This eliminates 200+ lines of redundant conversion code!
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
                ["in"] = "inches",
                ["ft"] = "feet",
                ["yd"] = "yards",
                ["mi"] = "miles",
                ["px"] = "pixels",
                ["g"] = "grams",
                ["mg"] = "milligrams",
                ["kg"] = "kilograms",
                ["lb"] = "pounds",
                ["oz"] = "ounces",
                ["dyne"] = "dynes",
                ["N"] = "newtons",
                ["kN"] = "kilonewtons",
                ["lbf"] = "pounds-force",
                ["C"] = "Celsius",
                ["F"] = "Fahrenheit", 
                ["K"] = "Kelvin",
                ["rad"] = "radians",
                ["deg"] = "degrees",
                ["mrad"] = "milliradians",
                ["s"] = "seconds",
                ["ms"] = "milliseconds",
                ["min"] = "minutes",
                ["hr"] = "hours",
                ["day"] = "days"
            };
        }
    }
}