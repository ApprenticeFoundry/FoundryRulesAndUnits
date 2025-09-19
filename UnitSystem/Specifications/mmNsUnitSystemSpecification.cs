using System;
using System.Collections.Generic;
using FoundryRulesAndUnits.Units;

namespace FoundryRulesAndUnits.Units.Specifications
{
    /// <summary>
    /// mmNs (Millimeter-Newton-Second) Unit System Specification
    /// Base units: millimeters, grams, seconds, Celsius, radians, newtons
    /// </summary>
    public class mmNsUnitSystemSpecification : BaseUnitSystemSpecification
    {
        public override string SystemName => "mmNs";
        
        public override Dictionary<string, string> GetBaseUnits()
        {
            return new Dictionary<string, string>
            {
                ["Length"] = "mm",
                ["Mass"] = "g", 
                ["Time"] = "s",
                ["Temperature"] = "C",
                ["Angle"] = "rad",
                ["Force"] = "N",
                ["Area"] = "mm2",
                ["Volume"] = "mm3",
                ["Speed"] = "mm/s"
            };
        }
        
        public override Dictionary<string, UnitDefinition> GetUnitDefinitions()
        {
            return new Dictionary<string, UnitDefinition>
            {
                // Length units (millimeter as base)
                ["mm"] = new UnitDefinition("mm", "millimeters", UnitType.Length, isBaseUnit: true),
                ["μm"] = new UnitDefinition("μm", "micrometers", UnitType.Length, "mm", 0.001),
                ["cm"] = new UnitDefinition("cm", "centimeters", UnitType.Length, "mm", 10.0),
                ["m"] = new UnitDefinition("m", "meters", UnitType.Length, "mm", 1000.0),
                ["km"] = new UnitDefinition("km", "kilometers", UnitType.Length, "mm", 1000000.0),
                
                // Imperial length conversions (derived from mm)
                ["in"] = new UnitDefinition("in", "inches", UnitType.Length, "mm", 25.4),
                ["ft"] = new UnitDefinition("ft", "feet", UnitType.Length, "mm", 304.8),
                ["yd"] = new UnitDefinition("yd", "yards", UnitType.Length, "mm", 914.4),
                ["mi"] = new UnitDefinition("mi", "miles", UnitType.Length, "mm", 1609344.0),
                
                // Pixels (standard 96 DPI)
                ["px"] = new UnitDefinition("px", "pixels", UnitType.Length, "mm", 25.4/96.0),
                
                // Mass units (gram as base)
                ["g"] = new UnitDefinition("g", "grams", UnitType.Mass, isBaseUnit: true),
                ["mg"] = new UnitDefinition("mg", "milligrams", UnitType.Mass, "g", 0.001),
                ["kg"] = new UnitDefinition("kg", "kilograms", UnitType.Mass, "g", 1000.0),
                
                // Imperial mass conversions
                ["lb"] = new UnitDefinition("lb", "pounds", UnitType.Mass, "g", 453.592),
                ["oz"] = new UnitDefinition("oz", "ounces", UnitType.Mass, "g", 28.3495),
                
                // Force units (newton as base)
                ["N"] = new UnitDefinition("N", "newtons", UnitType.Force, isBaseUnit: true),
                ["kN"] = new UnitDefinition("kN", "kilonewtons", UnitType.Force, "N", 1000.0),
                ["dyne"] = new UnitDefinition("dyne", "dynes", UnitType.Force, "N", 0.00001),
                ["lbf"] = new UnitDefinition("lbf", "pounds-force", UnitType.Force, "N", 4.44822),
                
                // Temperature units (Celsius as base)
                ["C"] = new UnitDefinition("C", "Celsius", UnitType.Temperature, isBaseUnit: true),
                ["F"] = new UnitDefinition("F", "Fahrenheit", UnitType.Temperature, "C", conversionFormula: "C * 9/5 + 32"),
                ["K"] = new UnitDefinition("K", "Kelvin", UnitType.Temperature, "C", conversionFormula: "C + 273.15"),
                
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
                ["mm2"] = new UnitDefinition("mm2", "square millimeters", UnitType.Area, "mm", 1.0, exponent: 2),
                ["cm2"] = new UnitDefinition("cm2", "square centimeters", UnitType.Area, "mm2", Math.Pow(10.0, 2)),
                ["m2"] = new UnitDefinition("m2", "square meters", UnitType.Area, "mm2", Math.Pow(1000.0, 2)),
                
                ["mm3"] = new UnitDefinition("mm3", "cubic millimeters", UnitType.Volume, "mm", 1.0, exponent: 3),
                ["cm3"] = new UnitDefinition("cm3", "cubic centimeters", UnitType.Volume, "mm3", Math.Pow(10.0, 3)),
                ["m3"] = new UnitDefinition("m3", "cubic meters", UnitType.Volume, "mm3", Math.Pow(1000.0, 3)),
                
                ["mm/s"] = new UnitDefinition("mm/s", "millimeters per second", UnitType.Speed, "mm", 1.0, "s", -1),
                ["cm/s"] = new UnitDefinition("cm/s", "centimeters per second", UnitType.Speed, "mm/s", 10.0),
                ["m/s"] = new UnitDefinition("m/s", "meters per second", UnitType.Speed, "mm/s", 1000.0),
                ["km/h"] = new UnitDefinition("km/h", "kilometers per hour", UnitType.Speed, "mm/s", 1000000.0/3600.0)
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
                ["mm"] = "millimeters",
                ["μm"] = "micrometers",
                ["cm"] = "centimeters",
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