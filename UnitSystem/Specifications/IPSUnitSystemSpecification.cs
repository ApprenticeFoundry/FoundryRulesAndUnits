using System;
using System.Collections.Generic;
using FoundryRulesAndUnits.Units;

namespace FoundryRulesAndUnits.Units.Specifications
{
    /// <summary>
    /// IPS (Inch-Pound-Second) Unit System Specification
    /// Base units: inches, pounds, seconds, Fahrenheit, radians, pound-force
    /// </summary>
    public class IPSUnitSystemSpecification : BaseUnitSystemSpecification
    {
        public override string SystemName => "IPS";
        
        public override Dictionary<string, string> GetBaseUnits()
        {
            return new Dictionary<string, string>
            {
                ["Length"] = "in",
                ["Mass"] = "lb", 
                ["Time"] = "s",
                ["Temperature"] = "F",
                ["Angle"] = "rad",
                ["Force"] = "lbf",
                ["Area"] = "in2",
                ["Volume"] = "in3",
                ["Speed"] = "in/s"
            };
        }
        
        public override Dictionary<string, UnitDefinition> GetUnitDefinitions()
        {
            return new Dictionary<string, UnitDefinition>
            {
                // Length units (inch as base)
                ["in"] = new UnitDefinition("in", "inches", UnitType.Length, isBaseUnit: true),
                ["ft"] = new UnitDefinition("ft", "feet", UnitType.Length, "in", 12.0),
                ["yd"] = new UnitDefinition("yd", "yards", UnitType.Length, "in", 36.0),
                ["mi"] = new UnitDefinition("mi", "miles", UnitType.Length, "in", 63360.0),
                
                // Metric length conversions (derived from in)
                ["m"] = new UnitDefinition("m", "meters", UnitType.Length, "in", 1.0/0.0254),
                ["cm"] = new UnitDefinition("cm", "centimeters", UnitType.Length, "in", 1.0/2.54),
                ["mm"] = new UnitDefinition("mm", "millimeters", UnitType.Length, "in", 1.0/25.4),
                ["km"] = new UnitDefinition("km", "kilometers", UnitType.Length, "in", 1.0/25400.0),
                
                // Pixels (standard 96 DPI)
                ["px"] = new UnitDefinition("px", "pixels", UnitType.Length, "in", 96.0),
                
                // Mass units (pound as base)
                ["lb"] = new UnitDefinition("lb", "pounds", UnitType.Mass, isBaseUnit: true),
                ["oz"] = new UnitDefinition("oz", "ounces", UnitType.Mass, "lb", 1.0/16.0),
                
                // Metric mass conversions
                ["kg"] = new UnitDefinition("kg", "kilograms", UnitType.Mass, "lb", 1.0/0.453592),
                ["g"] = new UnitDefinition("g", "grams", UnitType.Mass, "lb", 1.0/453.592),
                ["mg"] = new UnitDefinition("mg", "milligrams", UnitType.Mass, "lb", 1.0/453592.0),
                
                // Force units (pound-force as base)
                ["lbf"] = new UnitDefinition("lbf", "pounds-force", UnitType.Force, isBaseUnit: true),
                ["N"] = new UnitDefinition("N", "newtons", UnitType.Force, "lbf", 1.0/4.44822),
                ["kN"] = new UnitDefinition("kN", "kilonewtons", UnitType.Force, "lbf", 1.0/0.00444822),
                ["dyne"] = new UnitDefinition("dyne", "dynes", UnitType.Force, "lbf", 1.0/444822.0),
                
                // Temperature units (Fahrenheit as base)
                ["F"] = new UnitDefinition("F", "Fahrenheit", UnitType.Temperature, isBaseUnit: true),
                ["C"] = new UnitDefinition("C", "Celsius", UnitType.Temperature, "F", conversionFormula: "(F - 32) * 5/9"),
                ["K"] = new UnitDefinition("K", "Kelvin", UnitType.Temperature, "F", conversionFormula: "(F - 32) * 5/9 + 273.15"),
                
                // Angle units (radian as base)
                ["rad"] = new UnitDefinition("rad", "radians", UnitType.Angle, isBaseUnit: true),
                ["deg"] = new UnitDefinition("deg", "degrees", UnitType.Angle, "rad", Math.PI/180.0),
                ["mrad"] = new UnitDefinition("mrad", "milliradians", UnitType.Angle, "rad", 0.001),
                
                // Time units (second as base)
                ["s"] = new UnitDefinition("s", "seconds", UnitType.Time, isBaseUnit: true),
                ["ms"] = new UnitDefinition("ms", "milliseconds", UnitType.Time, "s", 0.001),
                ["min"] = new UnitDefinition("min", "minutes", UnitType.Time, "s", 60.0),
                ["hr"] = new UnitDefinition("hr", "hours", UnitType.Time, "s", 3600.0),
                ["day"] = new UnitDefinition("day", "days", UnitType.Time, "s", 86400.0),
                
                // Derived units
                ["in2"] = new UnitDefinition("in2", "square inches", UnitType.Area, "in", 1.0, exponent: 2),
                ["ft2"] = new UnitDefinition("ft2", "square feet", UnitType.Area, "in2", Math.Pow(12.0, 2)),
                ["yd2"] = new UnitDefinition("yd2", "square yards", UnitType.Area, "in2", Math.Pow(36.0, 2)),
                
                ["in3"] = new UnitDefinition("in3", "cubic inches", UnitType.Volume, "in", 1.0, exponent: 3),
                ["ft3"] = new UnitDefinition("ft3", "cubic feet", UnitType.Volume, "in3", Math.Pow(12.0, 3)),
                ["yd3"] = new UnitDefinition("yd3", "cubic yards", UnitType.Volume, "in3", Math.Pow(36.0, 3)),
                
                ["in/s"] = new UnitDefinition("in/s", "inches per second", UnitType.Speed, "in", 1.0, "s", -1),
                ["ft/s"] = new UnitDefinition("ft/s", "feet per second", UnitType.Speed, "in/s", 12.0),
                ["mph"] = new UnitDefinition("mph", "miles per hour", UnitType.Speed, "in/s", 63360.0/3600.0),
                ["m/s"] = new UnitDefinition("m/s", "meters per second", UnitType.Speed, "in/s", 1.0/0.0254),
                ["cm/s"] = new UnitDefinition("cm/s", "centimeters per second", UnitType.Speed, "in/s", 1.0/2.54),
                ["km/h"] = new UnitDefinition("km/h", "kilometers per hour", UnitType.Speed, "in/s", 1000.0/(25.4*3600.0))
            };
        }
        
        public override Dictionary<string, UnitConversion> GetBaseUnitConversions()
        {
            // 🎯 ZERO hard-coded conversions! Everything is auto-generated!
            return new Dictionary<string, UnitConversion>();
        }

        public override Dictionary<string, string> GetUnitDisplayNames()
        {
            return new Dictionary<string, string>
            {
                ["in"] = "inches",
                ["ft"] = "feet",
                ["yd"] = "yards",
                ["mi"] = "miles",
                ["m"] = "meters",
                ["cm"] = "centimeters",
                ["mm"] = "millimeters", 
                ["km"] = "kilometers",
                ["px"] = "pixels",
                ["lb"] = "pounds",
                ["oz"] = "ounces",
                ["kg"] = "kilograms",
                ["g"] = "grams",
                ["mg"] = "milligrams",
                ["lbf"] = "pounds-force",
                ["N"] = "newtons",
                ["kN"] = "kilonewtons",
                ["dyne"] = "dynes",
                ["F"] = "Fahrenheit",
                ["C"] = "Celsius", 
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