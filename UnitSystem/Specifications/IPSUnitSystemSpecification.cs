using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units.Specifications
{
    /// <summary>
    /// IPS (Inch-Pound-Second) Unit System Specification
    /// Base units: inches, pounds, seconds, Fahrenheit, radians, pound-force
    /// </summary>
    public class IPSUnitSystemSpecification : IUnitSystemSpecification
    {
        public string SystemName => "IPS";
        
        public Dictionary<string, string> GetBaseUnits()
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
        
        public Dictionary<string, UnitDefinition> GetUnitDefinitions()
        {
            return new Dictionary<string, UnitDefinition>
            {
                // Length units (inches as base)
                ["in"] = new("in", "inches", "Length"),
                ["ft"] = new("ft", "feet", "Length"),
                ["yd"] = new("yd", "yards", "Length"),
                ["mi"] = new("mi", "miles", "Length"),
                ["m"] = new("m", "meters", "Length"),
                ["cm"] = new("cm", "centimeters", "Length"), 
                ["mm"] = new("mm", "millimeters", "Length"),
                ["km"] = new("km", "kilometers", "Length"),
                ["px"] = new("px", "pixels", "Length"),
                
                // Mass units (pounds as base)
                ["lb"] = new("lb", "pounds", "Mass"),
                ["oz"] = new("oz", "ounces", "Mass"),
                ["kg"] = new("kg", "kilograms", "Mass"),
                ["g"] = new("g", "grams", "Mass"),
                ["mg"] = new("mg", "milligrams", "Mass"),
                
                // Force units (pound-force as base)
                ["lbf"] = new("lbf", "pounds-force", "Force"),
                ["N"] = new("N", "newtons", "Force"),
                ["kN"] = new("kN", "kilonewtons", "Force"),
                ["dyne"] = new("dyne", "dynes", "Force"),
                
                // Temperature units (Fahrenheit as base)
                ["F"] = new("F", "Fahrenheit", "Temperature"),
                ["C"] = new("C", "Celsius", "Temperature"),
                ["K"] = new("K", "Kelvin", "Temperature"),
                
                // Angle units (same as MKS)
                ["rad"] = new("rad", "radians", "Angle"),
                ["deg"] = new("deg", "degrees", "Angle"),
                ["mrad"] = new("mrad", "milliradians", "Angle"),
                
                // Time units (same as MKS)
                ["s"] = new("s", "seconds", "Time"),
                ["ms"] = new("ms", "milliseconds", "Time"),
                ["min"] = new("min", "minutes", "Time"),
                ["hr"] = new("hr", "hours", "Time"),
                ["day"] = new("day", "days", "Time"),
                
                // Area units
                ["in2"] = new("in2", "square inches", "Area"),
                ["ft2"] = new("ft2", "square feet", "Area"),
                ["yd2"] = new("yd2", "square yards", "Area"),
                
                // Volume units
                ["in3"] = new("in3", "cubic inches", "Volume"),
                ["ft3"] = new("ft3", "cubic feet", "Volume"),
                ["yd3"] = new("yd3", "cubic yards", "Volume"),
                
                // Speed units
                ["in/s"] = new("in/s", "inches per second", "Speed"),
                ["ft/s"] = new("ft/s", "feet per second", "Speed"),
                ["mph"] = new("mph", "miles per hour", "Speed"),
                ["m/s"] = new("m/s", "meters per second", "Speed"),
                ["km/h"] = new("km/h", "kilometers per hour", "Speed")
            };
        }
        
        public Dictionary<string, UnitConversion> GetConversions()
        {
            return new Dictionary<string, UnitConversion>
            {
                // Length conversions (all relative to inch base)
                ["in|ft"] = new(1.0/12.0),      // EXACT: 12in = 1ft
                ["ft|in"] = new(12.0),          // EXACT: 1ft = 12in
                ["in|yd"] = new(1.0/36.0),      // EXACT: 36in = 1yd
                ["yd|in"] = new(36.0),          // EXACT: 1yd = 36in
                ["in|mi"] = new(1.0/63360.0),   // EXACT: 63360in = 1mi
                ["mi|in"] = new(63360.0),       // EXACT: 1mi = 63360in
                ["ft|yd"] = new(1.0/3.0),       // EXACT: 3ft = 1yd
                ["yd|ft"] = new(3.0),           // EXACT: 1yd = 3ft
                ["ft|mi"] = new(1.0/5280.0),    // EXACT: 5280ft = 1mi
                ["mi|ft"] = new(5280.0),        // EXACT: 1mi = 5280ft
                ["yd|mi"] = new(1.0/1760.0),    // EXACT: 1760yd = 1mi
                ["mi|yd"] = new(1760.0),        // EXACT: 1mi = 1760yd
                
                // Metric conversions (via inch base)
                ["in|m"] = new(0.0254),         // EXACT: 1in = 0.0254m
                ["m|in"] = new(39.3701),        // EXACT: 1m = 39.3701in
                ["in|cm"] = new(2.54),          // EXACT: 1in = 2.54cm
                ["cm|in"] = new(1.0/2.54),      // EXACT: 1cm = 0.393701in
                ["in|mm"] = new(25.4),          // EXACT: 1in = 25.4mm
                ["mm|in"] = new(1.0/25.4),      // EXACT: 1mm = 0.0393701in
                ["in|km"] = new(0.0000254),     // EXACT: 1in = 0.0000254km
                ["km|in"] = new(39370.1),       // EXACT: 1km = 39370.1in
                
                // Metric-to-Imperial (derived)
                ["m|ft"] = new(3.28084),        // EXACT: 1m = 3.28084ft
                ["ft|m"] = new(0.3048),         // EXACT: 1ft = 0.3048m
                ["m|yd"] = new(1.09361),        // EXACT: 1m = 1.09361yd
                ["yd|m"] = new(0.9144),         // EXACT: 1yd = 0.9144m
                ["km|mi"] = new(0.621371),      // EXACT: 1km = 0.621371mi
                ["mi|km"] = new(1.609344),      // EXACT: 1mi = 1.609344km
                
                // Metric-to-Metric (derived)
                ["cm|mm"] = new(10.0),
                ["mm|cm"] = new(0.1),
                ["m|cm"] = new(100.0),
                ["cm|m"] = new(0.01),
                ["m|mm"] = new(1000.0),
                ["mm|m"] = new(0.001),
                ["m|km"] = new(0.001),
                ["km|m"] = new(1000.0),
                
                // Mass conversions (all relative to pound base)
                ["lb|oz"] = new(16.0),          // EXACT: 1lb = 16oz
                ["oz|lb"] = new(0.0625),        // EXACT: 1oz = 0.0625lb
                ["lb|kg"] = new(0.453592),      // EXACT: 1lb = 0.453592kg
                ["kg|lb"] = new(2.20462),       // EXACT: 1kg = 2.20462lb
                ["lb|g"] = new(453.592),        // EXACT: 1lb = 453.592g
                ["g|lb"] = new(0.00220462),     // EXACT: 1g = 0.00220462lb
                ["lb|mg"] = new(453592.0),      // EXACT: 1lb = 453592mg
                ["mg|lb"] = new(0.00000220462), // EXACT: 1mg = 0.00000220462lb
                ["oz|kg"] = new(0.0283495),     // EXACT: 1oz = 0.0283495kg
                ["kg|oz"] = new(35.274),        // EXACT: 1kg = 35.274oz
                ["g|kg"] = new(0.001),
                ["kg|g"] = new(1000.0),
                ["g|mg"] = new(1000.0),
                ["mg|g"] = new(0.001),
                
                // Force conversions (all relative to pound-force base)
                ["lbf|N"] = new(4.44822),       // EXACT: 1lbf = 4.44822N
                ["N|lbf"] = new(0.224809),      // EXACT: 1N = 0.224809lbf
                ["lbf|kN"] = new(0.00444822),   // EXACT: 1lbf = 0.00444822kN
                ["kN|lbf"] = new(224.809),      // EXACT: 1kN = 224.809lbf
                ["lbf|dyne"] = new(444822.0),   // EXACT: 1lbf = 444822dyne
                ["dyne|lbf"] = new(0.00000224809), // EXACT: 1dyne = 0.00000224809lbf
                ["N|kN"] = new(0.001),
                ["kN|N"] = new(1000.0),
                ["N|dyne"] = new(100000.0),
                ["dyne|N"] = new(0.00001),
                
                // Temperature conversions (formulas needed, Fahrenheit as base)
                ["F|C"] = new(0, "(F - 32) * 5/9"),
                ["C|F"] = new(0, "C * 9/5 + 32"),
                ["F|K"] = new(0, "(F - 32) * 5/9 + 273.15"),
                ["K|F"] = new(0, "(K - 273.15) * 9/5 + 32"),
                ["C|K"] = new(0, "C + 273.15"),
                ["K|C"] = new(0, "K - 273.15"),
                
                // Angle conversions (same as MKS)
                ["rad|deg"] = new(180.0 / Math.PI),
                ["deg|rad"] = new(Math.PI / 180.0),
                ["rad|mrad"] = new(1000.0),
                ["mrad|rad"] = new(0.001),
                ["deg|mrad"] = new(1000.0 * Math.PI / 180.0),
                ["mrad|deg"] = new(180.0 / (1000.0 * Math.PI)),
                
                // Time conversions (same as MKS)
                ["s|ms"] = new(1000.0),
                ["ms|s"] = new(0.001),
                ["s|min"] = new(1.0/60.0),
                ["min|s"] = new(60.0),
                ["s|hr"] = new(1.0/3600.0),
                ["hr|s"] = new(3600.0),
                ["s|day"] = new(1.0/86400.0),
                ["day|s"] = new(86400.0),
                ["min|hr"] = new(1.0/60.0),
                ["hr|min"] = new(60.0),
                ["hr|day"] = new(1.0/24.0),
                ["day|hr"] = new(24.0),
                
                // Area conversions (square of length conversions)
                ["in2|ft2"] = new(1.0/144.0),   // (1/12)^2
                ["ft2|in2"] = new(144.0),       // (12)^2
                ["in2|yd2"] = new(1.0/1296.0),  // (1/36)^2
                ["yd2|in2"] = new(1296.0),      // (36)^2
                ["ft2|yd2"] = new(1.0/9.0),     // (1/3)^2
                ["yd2|ft2"] = new(9.0),         // (3)^2
                
                // Volume conversions (cube of length conversions)
                ["in3|ft3"] = new(1.0/1728.0),  // (1/12)^3
                ["ft3|in3"] = new(1728.0),      // (12)^3
                ["in3|yd3"] = new(1.0/46656.0), // (1/36)^3
                ["yd3|in3"] = new(46656.0),     // (36)^3
                ["ft3|yd3"] = new(1.0/27.0),    // (1/3)^3
                ["yd3|ft3"] = new(27.0),        // (3)^3
                
                // Speed conversions
                ["in/s|ft/s"] = new(1.0/12.0),  // Same as in|ft
                ["ft/s|in/s"] = new(12.0),      // Same as ft|in
                ["in/s|mph"] = new(0.0568182),  // 1 in/s = 0.0568182 mph
                ["mph|in/s"] = new(17.6),       // 1 mph = 17.6 in/s
                ["ft/s|mph"] = new(0.681818),   // 1 ft/s = 0.681818 mph
                ["mph|ft/s"] = new(1.46667),    // 1 mph = 1.46667 ft/s
                ["in/s|m/s"] = new(0.0254),     // Same as in|m
                ["m/s|in/s"] = new(39.3701),    // Same as m|in
                ["ft/s|m/s"] = new(0.3048),     // Same as ft|m
                ["m/s|ft/s"] = new(3.28084),    // Same as m|ft
                ["m/s|km/h"] = new(3.6),
                ["km/h|m/s"] = new(1.0/3.6),
                ["mph|km/h"] = new(1.609344),   // Same as mi|km
                ["km/h|mph"] = new(0.621371),   // Same as km|mi
                
                // Pixels (96 DPI standard for inches)
                ["in|px"] = new(96.0),
                ["px|in"] = new(1.0/96.0)
            };
        }
        
        public Dictionary<string, string> GetUnitDisplayNames()
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