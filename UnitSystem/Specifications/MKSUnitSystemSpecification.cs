using System;
using System.Collections.Generic;
using FoundryRulesAndUnits.Units;

namespace FoundryRulesAndUnits.Units.Specifications
{
    /// <summary>
    /// MKS (Meter-Kilogram-Second) Unit System Specification
    /// Base units: meters, kilograms, seconds, Celsius, radians, newtons
    /// Uses base unit hub architecture - all conversions go through the base unit
    /// </summary>
    public class MKSUnitSystemSpecification : BaseUnitSystemSpecification
    {
        public override string SystemName => "MKS";
        
        public override Dictionary<string, string> GetBaseUnits()
        {
            return new Dictionary<string, string>
            {
                ["Length"] = "m",
                ["Mass"] = "kg", 
                ["Time"] = "s",
                ["Temperature"] = "C",
                ["Angle"] = "rad",
                ["Force"] = "N",
                ["Area"] = "m2",
                ["Volume"] = "m3",
                ["Speed"] = "m/s"
            };
        }
        
        public override Dictionary<string, UnitDefinition> GetUnitDefinitions()
        {
            return new Dictionary<string, UnitDefinition>
            {
                // Length units (meters as base) - using enhanced approach with UnitFamilyName enum!
                ["m"] = UnitDefinition.BaseUnit("m", "meters", UnitFamilyName.Length),
                ["cm"] = UnitDefinition.LinearUnit("cm", "centimeters", UnitFamilyName.Length, 0.01),         // 1 cm = 0.01 m
                ["mm"] = UnitDefinition.LinearUnit("mm", "millimeters", UnitFamilyName.Length, 0.001),        // 1 mm = 0.001 m
                ["km"] = UnitDefinition.LinearUnit("km", "kilometers", UnitFamilyName.Length, 1000.0),        // 1 km = 1000 m
                ["in"] = UnitDefinition.LinearUnit("in", "inches", UnitFamilyName.Length, 0.0254),            // 1 in = 0.0254 m
                ["ft"] = UnitDefinition.LinearUnit("ft", "feet", UnitFamilyName.Length, 0.3048),              // 1 ft = 0.3048 m
                ["yd"] = UnitDefinition.LinearUnit("yd", "yards", UnitFamilyName.Length, 0.9144),             // 1 yd = 0.9144 m
                ["mi"] = UnitDefinition.LinearUnit("mi", "miles", UnitFamilyName.Length, 1609.344),           // 1 mi = 1609.344 m
                ["px"] = UnitDefinition.LinearUnit("px", "pixels", UnitFamilyName.Length, 1.0/96.0 * 0.0254), // 96 DPI
                
                // Mass units (kilograms as base) - using enhanced approach with UnitFamilyName enum!
                ["kg"] = UnitDefinition.BaseUnit("kg", "kilograms", UnitFamilyName.Mass),
                ["g"] = UnitDefinition.LinearUnit("g", "grams", UnitFamilyName.Mass, 0.001),                  // 1 g = 0.001 kg
                ["mg"] = UnitDefinition.LinearUnit("mg", "milligrams", UnitFamilyName.Mass, 0.000001),        // 1 mg = 0.000001 kg
                ["lb"] = UnitDefinition.LinearUnit("lb", "pounds", UnitFamilyName.Mass, 0.453592),            // 1 lb = 0.453592 kg
                ["oz"] = UnitDefinition.LinearUnit("oz", "ounces", UnitFamilyName.Mass, 0.0283495),           // 1 oz = 0.0283495 kg
                
                // Force units (newtons as base) - using enhanced approach with UnitFamilyName enum!
                ["N"] = UnitDefinition.BaseUnit("N", "newtons", UnitFamilyName.Force),
                ["kN"] = UnitDefinition.LinearUnit("kN", "kilonewtons", UnitFamilyName.Force, 1000.0),        // 1 kN = 1000 N
                ["dyne"] = UnitDefinition.LinearUnit("dyne", "dynes", UnitFamilyName.Force, 0.00001),         // 1 dyne = 0.00001 N
                ["lbf"] = UnitDefinition.LinearUnit("lbf", "pounds-force", UnitFamilyName.Force, 4.44822),    // 1 lbf = 4.44822 N
                
                // Temperature units (Kelvin as base for absolute temperature scale) - using enhanced approach with UnitFamilyName enum!
                ["K"] = UnitDefinition.BaseUnit("K", "Kelvin", UnitFamilyName.Temperature),
                ["C"] = UnitDefinition.DerivedUnit("C", "Celsius", UnitFamilyName.Temperature, 
                    c => c + 273.15,          // C to K: add 273.15
                    k => k - 273.15),         // K to C: subtract 273.15
                ["F"] = UnitDefinition.DerivedUnit("F", "Fahrenheit", UnitFamilyName.Temperature,
                    f => (f - 32.0) * 5.0/9.0 + 273.15,    // F to K: (F-32)*5/9 + 273.15
                    k => (k - 273.15) * 9.0/5.0 + 32.0),   // K to F: (K-273.15)*9/5 + 32
                
                // Angle units (radians as base) - using enhanced approach with UnitFamilyName enum!
                ["rad"] = UnitDefinition.BaseUnit("rad", "radians", UnitFamilyName.Angle),
                ["deg"] = UnitDefinition.LinearUnit("deg", "degrees", UnitFamilyName.Angle, Math.PI / 180.0),  // 1 deg = π/180 rad
                ["mrad"] = UnitDefinition.LinearUnit("mrad", "milliradians", UnitFamilyName.Angle, 0.001),     // 1 mrad = 0.001 rad
                
                // Time units (seconds as base) - using enhanced approach with UnitFamilyName enum!
                ["s"] = UnitDefinition.BaseUnit("s", "seconds", UnitFamilyName.Time),
                ["ms"] = UnitDefinition.LinearUnit("ms", "milliseconds", UnitFamilyName.Time, 0.001),          // 1 ms = 0.001 s
                ["min"] = UnitDefinition.LinearUnit("min", "minutes", UnitFamilyName.Time, 60.0),              // 1 min = 60 s
                ["hr"] = UnitDefinition.LinearUnit("hr", "hours", UnitFamilyName.Time, 3600.0),                // 1 hr = 3600 s
                ["day"] = UnitDefinition.LinearUnit("day", "days", UnitFamilyName.Time, 86400.0),              // 1 day = 86400 s
                
                // Area units (square meters as base) - using enhanced approach with UnitFamilyName enum!
                ["m2"] = UnitDefinition.BaseUnit("m2", "square meters", UnitFamilyName.Area),
                ["cm2"] = UnitDefinition.LinearUnit("cm2", "square centimeters", UnitFamilyName.Area, 0.0001), // 1 cm² = 0.0001 m²
                ["mm2"] = UnitDefinition.LinearUnit("mm2", "square millimeters", UnitFamilyName.Area, 0.000001), // 1 mm² = 0.000001 m²
                ["km2"] = UnitDefinition.LinearUnit("km2", "square kilometers", UnitFamilyName.Area, 1000000.0), // 1 km² = 1,000,000 m²
                
                // Volume units (cubic meters as base) - using enhanced approach with UnitFamilyName enum!
                ["m3"] = UnitDefinition.BaseUnit("m3", "cubic meters", UnitFamilyName.Volume),
                ["cm3"] = UnitDefinition.LinearUnit("cm3", "cubic centimeters", UnitFamilyName.Volume, 0.000001), // 1 cm³ = 0.000001 m³
                ["mm3"] = UnitDefinition.LinearUnit("mm3", "cubic millimeters", UnitFamilyName.Volume, 0.000000001), // 1 mm³ = 1e-9 m³
                ["km3"] = UnitDefinition.LinearUnit("km3", "cubic kilometers", UnitFamilyName.Volume, 1000000000.0), // 1 km³ = 1e9 m³
                
                // Speed units (meters per second as base) - using enhanced approach with UnitFamilyName enum!
                ["m/s"] = UnitDefinition.BaseUnit("m/s", "meters per second", UnitFamilyName.Speed),
                ["km/h"] = UnitDefinition.LinearUnit("km/h", "kilometers per hour", UnitFamilyName.Speed, 1000.0/3600.0), // 1 km/h = 1000/3600 m/s
                ["mph"] = UnitDefinition.LinearUnit("mph", "miles per hour", UnitFamilyName.Speed, 0.44704),              // 1 mph = 0.44704 m/s
                ["ft/s"] = UnitDefinition.LinearUnit("ft/s", "feet per second", UnitFamilyName.Speed, 0.3048),           // 1 ft/s = 0.3048 m/s
                ["knot"] = UnitDefinition.LinearUnit("knot", "knots", UnitFamilyName.Speed, 0.514444)                    // 1 knot = 0.514444 m/s
            };
        }
        
        /// <summary>
        /// NO HARD-CODED CONVERSION MATRICES! All conversions auto-generated from UnitDefinition records.
        /// This eliminates 150+ lines of redundant conversion code!
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
                ["m"] = "meters",
                ["cm"] = "centimeters",
                ["mm"] = "millimeters", 
                ["km"] = "kilometers",
                ["in"] = "inches",
                ["ft"] = "feet",
                ["yd"] = "yards",
                ["mi"] = "miles",
                ["px"] = "pixels",
                ["kg"] = "kilograms",
                ["g"] = "grams",
                ["mg"] = "milligrams",
                ["lb"] = "pounds",
                ["oz"] = "ounces",
                ["N"] = "newtons",
                ["kN"] = "kilonewtons",
                ["dyne"] = "dynes",
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