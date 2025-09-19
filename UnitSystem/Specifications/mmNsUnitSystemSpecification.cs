using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units.Specifications
{
    /// <summary>
    /// mmNs (Millimeter-Newton-Second) Unit System Specification
    /// Base units: millimeters, grams, seconds, Celsius, radians, newtons
    /// </summary>
    public class mmNsUnitSystemSpecification : IUnitSystemSpecification
    {
        public string SystemName => "mmNs";
        
        public Dictionary<string, string> GetBaseUnits()
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
        
        public Dictionary<string, UnitDefinition> GetUnitDefinitions()
        {
            return new Dictionary<string, UnitDefinition>
            {
                // Length units (millimeters as base)
                ["mm"] = new("mm", "millimeters", "Length"),
                ["μm"] = new("μm", "micrometers", "Length"),
                ["cm"] = new("cm", "centimeters", "Length"),
                ["m"] = new("m", "meters", "Length"),
                ["km"] = new("km", "kilometers", "Length"),
                ["in"] = new("in", "inches", "Length"),
                ["ft"] = new("ft", "feet", "Length"),
                ["yd"] = new("yd", "yards", "Length"),
                ["mi"] = new("mi", "miles", "Length"),
                ["px"] = new("px", "pixels", "Length"),
                
                // Mass units (grams as base - derived from F=ma with mm, N, s)
                ["g"] = new("g", "grams", "Mass"),
                ["mg"] = new("mg", "milligrams", "Mass"),
                ["kg"] = new("kg", "kilograms", "Mass"),
                ["lb"] = new("lb", "pounds", "Mass"),
                ["oz"] = new("oz", "ounces", "Mass"),
                
                // Force units (newtons as base)
                ["N"] = new("N", "newtons", "Force"),
                ["kN"] = new("kN", "kilonewtons", "Force"),
                ["dyne"] = new("dyne", "dynes", "Force"),
                ["lbf"] = new("lbf", "pounds-force", "Force"),
                
                // Temperature units (Celsius as base)
                ["C"] = new("C", "Celsius", "Temperature"),
                ["F"] = new("F", "Fahrenheit", "Temperature"),
                ["K"] = new("K", "Kelvin", "Temperature"),
                
                // Angle units (same as others)
                ["rad"] = new("rad", "radians", "Angle"),
                ["deg"] = new("deg", "degrees", "Angle"),
                ["mrad"] = new("mrad", "milliradians", "Angle"),
                
                // Time units (same as others)
                ["s"] = new("s", "seconds", "Time"),
                ["ms"] = new("ms", "milliseconds", "Time"),
                ["min"] = new("min", "minutes", "Time"),
                ["hr"] = new("hr", "hours", "Time"),
                ["day"] = new("day", "days", "Time"),
                
                // Area units
                ["mm2"] = new("mm2", "square millimeters", "Area"),
                ["cm2"] = new("cm2", "square centimeters", "Area"),
                ["m2"] = new("m2", "square meters", "Area"),
                
                // Volume units
                ["mm3"] = new("mm3", "cubic millimeters", "Volume"),
                ["cm3"] = new("cm3", "cubic centimeters", "Volume"),
                ["m3"] = new("m3", "cubic meters", "Volume"),
                
                // Speed units
                ["mm/s"] = new("mm/s", "millimeters per second", "Speed"),
                ["cm/s"] = new("cm/s", "centimeters per second", "Speed"),
                ["m/s"] = new("m/s", "meters per second", "Speed"),
                ["km/h"] = new("km/h", "kilometers per hour", "Speed")
            };
        }
        
        public Dictionary<string, UnitConversion> GetConversions()
        {
            return new Dictionary<string, UnitConversion>
            {
                // Length conversions (all relative to millimeter base)
                ["mm|μm"] = new(1000.0),        // EXACT: 1mm = 1000μm
                ["μm|mm"] = new(0.001),         // EXACT: 1μm = 0.001mm
                ["mm|cm"] = new(0.1),           // EXACT: 1mm = 0.1cm
                ["cm|mm"] = new(10.0),          // EXACT: 1cm = 10mm
                ["mm|m"] = new(0.001),          // EXACT: 1mm = 0.001m
                ["m|mm"] = new(1000.0),         // EXACT: 1m = 1000mm
                ["mm|km"] = new(0.000001),      // EXACT: 1mm = 0.000001km
                ["km|mm"] = new(1000000.0),     // EXACT: 1km = 1000000mm
                ["cm|m"] = new(0.01),           // EXACT: 1cm = 0.01m
                ["m|cm"] = new(100.0),          // EXACT: 1m = 100cm
                ["cm|km"] = new(0.00001),       // EXACT: 1cm = 0.00001km
                ["km|cm"] = new(100000.0),      // EXACT: 1km = 100000cm
                ["m|km"] = new(0.001),          // EXACT: 1m = 0.001km
                ["km|m"] = new(1000.0),         // EXACT: 1km = 1000m
                
                // Imperial conversions (via millimeter base)
                ["mm|in"] = new(1.0/25.4),      // EXACT: 1mm = 0.0393701in
                ["in|mm"] = new(25.4),          // EXACT: 1in = 25.4mm
                ["mm|ft"] = new(1.0/304.8),     // EXACT: 1mm = 0.00328084ft
                ["ft|mm"] = new(304.8),         // EXACT: 1ft = 304.8mm
                ["mm|yd"] = new(1.0/914.4),     // EXACT: 1mm = 0.00109361yd
                ["yd|mm"] = new(914.4),         // EXACT: 1yd = 914.4mm
                ["mm|mi"] = new(1.0/1609344.0), // EXACT: 1mm = 0.000000621371mi
                ["mi|mm"] = new(1609344.0),     // EXACT: 1mi = 1609344mm
                
                // Imperial-to-Imperial (derived)
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
                
                // Metric-to-Imperial (derived)
                ["cm|in"] = new(1.0/2.54),      // EXACT: 1cm = 0.393701in
                ["in|cm"] = new(2.54),          // EXACT: 1in = 2.54cm
                ["cm|ft"] = new(1.0/30.48),     // EXACT: 1cm = 0.0328084ft
                ["ft|cm"] = new(30.48),         // EXACT: 1ft = 30.48cm
                ["m|in"] = new(39.3701),        // EXACT: 1m = 39.3701in
                ["in|m"] = new(0.0254),         // EXACT: 1in = 0.0254m
                ["m|ft"] = new(3.28084),        // EXACT: 1m = 3.28084ft
                ["ft|m"] = new(0.3048),         // EXACT: 1ft = 0.3048m
                ["m|yd"] = new(1.09361),        // EXACT: 1m = 1.09361yd
                ["yd|m"] = new(0.9144),         // EXACT: 1yd = 0.9144m
                ["km|mi"] = new(0.621371),      // EXACT: 1km = 0.621371mi
                ["mi|km"] = new(1.609344),      // EXACT: 1mi = 1.609344km
                
                // Mass conversions (same as CGS - grams as base)
                ["g|mg"] = new(1000.0),         // EXACT: 1g = 1000mg
                ["mg|g"] = new(0.001),          // EXACT: 1mg = 0.001g
                ["g|kg"] = new(0.001),          // EXACT: 1g = 0.001kg
                ["kg|g"] = new(1000.0),         // EXACT: 1kg = 1000g
                ["g|lb"] = new(0.00220462),     // EXACT: 1g = 0.00220462lb
                ["lb|g"] = new(453.592),        // EXACT: 1lb = 453.592g
                ["g|oz"] = new(0.035274),       // EXACT: 1g = 0.035274oz
                ["oz|g"] = new(28.3495),        // EXACT: 1oz = 28.3495g
                ["mg|kg"] = new(0.000001),      // EXACT: 1mg = 0.000001kg
                ["kg|mg"] = new(1000000.0),     // EXACT: 1kg = 1000000mg
                ["lb|oz"] = new(16.0),          // EXACT: 1lb = 16oz
                ["oz|lb"] = new(0.0625),        // EXACT: 1oz = 0.0625lb
                ["kg|lb"] = new(2.20462),       // EXACT: 1kg = 2.20462lb
                ["lb|kg"] = new(0.453592),      // EXACT: 1lb = 0.453592kg
                ["kg|oz"] = new(35.274),        // EXACT: 1kg = 35.274oz
                ["oz|kg"] = new(0.0283495),     // EXACT: 1oz = 0.0283495kg
                
                // Force conversions (same as MKS - newtons as base)
                ["N|kN"] = new(0.001),          // EXACT: 1N = 0.001kN
                ["kN|N"] = new(1000.0),         // EXACT: 1kN = 1000N
                ["N|dyne"] = new(100000.0),     // EXACT: 1N = 100000dyne
                ["dyne|N"] = new(0.00001),      // EXACT: 1dyne = 0.00001N
                ["N|lbf"] = new(0.224809),      // EXACT: 1N = 0.224809lbf
                ["lbf|N"] = new(4.44822),       // EXACT: 1lbf = 4.44822N
                ["kN|dyne"] = new(100000000.0), // EXACT: 1kN = 100000000dyne
                ["dyne|kN"] = new(0.00000001),  // EXACT: 1dyne = 0.00000001kN
                ["kN|lbf"] = new(224.809),      // EXACT: 1kN = 224.809lbf
                ["lbf|kN"] = new(0.00444822),   // EXACT: 1lbf = 0.00444822kN
                ["dyne|lbf"] = new(0.00000224809), // EXACT: 1dyne = 0.00000224809lbf
                ["lbf|dyne"] = new(444822.0),   // EXACT: 1lbf = 444822dyne
                
                // Temperature conversions (same as MKS and CGS - Celsius as base)
                ["C|F"] = new(0, "C * 9/5 + 32"),
                ["F|C"] = new(0, "(F - 32) * 5/9"),
                ["C|K"] = new(0, "C + 273.15"),
                ["K|C"] = new(0, "K - 273.15"),
                ["F|K"] = new(0, "(F - 32) * 5/9 + 273.15"),
                ["K|F"] = new(0, "(K - 273.15) * 9/5 + 32"),
                
                // Angle conversions (same as others)
                ["rad|deg"] = new(180.0 / Math.PI),
                ["deg|rad"] = new(Math.PI / 180.0),
                ["rad|mrad"] = new(1000.0),
                ["mrad|rad"] = new(0.001),
                ["deg|mrad"] = new(1000.0 * Math.PI / 180.0),
                ["mrad|deg"] = new(180.0 / (1000.0 * Math.PI)),
                
                // Time conversions (same as others)
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
                ["mm2|cm2"] = new(0.01),        // (0.1)^2
                ["cm2|mm2"] = new(100.0),       // (10)^2
                ["mm2|m2"] = new(0.000001),     // (0.001)^2
                ["m2|mm2"] = new(1000000.0),    // (1000)^2
                ["cm2|m2"] = new(0.0001),       // (0.01)^2
                ["m2|cm2"] = new(10000.0),      // (100)^2
                
                // Volume conversions (cube of length conversions)
                ["mm3|cm3"] = new(0.001),       // (0.1)^3
                ["cm3|mm3"] = new(1000.0),      // (10)^3
                ["mm3|m3"] = new(0.000000001),  // (0.001)^3
                ["m3|mm3"] = new(1000000000.0), // (1000)^3
                ["cm3|m3"] = new(0.000001),     // (0.01)^3
                ["m3|cm3"] = new(1000000.0),    // (100)^3
                
                // Speed conversions
                ["mm/s|cm/s"] = new(0.1),       // Same as mm|cm
                ["cm/s|mm/s"] = new(10.0),      // Same as cm|mm
                ["mm/s|m/s"] = new(0.001),      // Same as mm|m
                ["m/s|mm/s"] = new(1000.0),     // Same as m|mm
                ["cm/s|m/s"] = new(0.01),       // Same as cm|m
                ["m/s|cm/s"] = new(100.0),      // Same as m|cm
                ["m/s|km/h"] = new(3.6),        // 1 m/s = 3.6 km/h
                ["km/h|m/s"] = new(1.0/3.6),    // 1 km/h = 0.2778 m/s
                ["mm/s|km/h"] = new(0.0036),    // 1 mm/s = 0.0036 km/h
                ["km/h|mm/s"] = new(277.778),   // 1 km/h = 277.778 mm/s
                ["cm/s|km/h"] = new(0.036),     // 1 cm/s = 0.036 km/h
                ["km/h|cm/s"] = new(27.7778),   // 1 km/h = 27.7778 cm/s
                
                // Pixels (assuming high precision - 25.4 pixels per mm for 1:1 display)
                ["mm|px"] = new(25.4),
                ["px|mm"] = new(1.0/25.4)
            };
        }
        
        public Dictionary<string, string> GetUnitDisplayNames()
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