using System;
using System.Collections.Generic;

namespace FoundryRulesAndUnits.Units.Specifications
{
    /// <summary>
    /// MKS (Meter-Kilogram-Second) Unit System Specification
    /// Base units: meters, kilograms, seconds, Celsius, radians, newtons
    /// </summary>
    public class MKSUnitSystemSpecification : IUnitSystemSpecification
    {
        public string SystemName => "MKS";
        
        public Dictionary<string, string> GetBaseUnits()
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
        
        public Dictionary<string, UnitDefinition> GetUnitDefinitions()
        {
            return new Dictionary<string, UnitDefinition>
            {
                // Length units
                ["m"] = new("m", "meters", "Length"),
                ["cm"] = new("cm", "centimeters", "Length"), 
                ["mm"] = new("mm", "millimeters", "Length"),
                ["km"] = new("km", "kilometers", "Length"),
                ["in"] = new("in", "inches", "Length"),
                ["ft"] = new("ft", "feet", "Length"),
                ["yd"] = new("yd", "yards", "Length"),
                ["mi"] = new("mi", "miles", "Length"),
                ["px"] = new("px", "pixels", "Length"),
                
                // Mass units
                ["kg"] = new("kg", "kilograms", "Mass"),
                ["g"] = new("g", "grams", "Mass"),
                ["mg"] = new("mg", "milligrams", "Mass"),
                ["lb"] = new("lb", "pounds", "Mass"),
                ["oz"] = new("oz", "ounces", "Mass"),
                
                // Force units
                ["N"] = new("N", "newtons", "Force"),
                ["kN"] = new("kN", "kilonewtons", "Force"),
                ["dyne"] = new("dyne", "dynes", "Force"),
                ["lbf"] = new("lbf", "pounds-force", "Force"),
                
                // Temperature units
                ["C"] = new("C", "Celsius", "Temperature"),
                ["F"] = new("F", "Fahrenheit", "Temperature"),
                ["K"] = new("K", "Kelvin", "Temperature"),
                
                // Angle units
                ["rad"] = new("rad", "radians", "Angle"),
                ["deg"] = new("deg", "degrees", "Angle"),
                ["mrad"] = new("mrad", "milliradians", "Angle"),
                
                // Time units
                ["s"] = new("s", "seconds", "Time"),
                ["ms"] = new("ms", "milliseconds", "Time"),
                ["min"] = new("min", "minutes", "Time"),
                ["hr"] = new("hr", "hours", "Time"),
                ["day"] = new("day", "days", "Time"),
                
                // Area units
                ["m2"] = new("m2", "square meters", "Area"),
                ["cm2"] = new("cm2", "square centimeters", "Area"),
                ["mm2"] = new("mm2", "square millimeters", "Area"),
                ["km2"] = new("km2", "square kilometers", "Area"),
                
                // Volume units
                ["m3"] = new("m3", "cubic meters", "Volume"),
                ["cm3"] = new("cm3", "cubic centimeters", "Volume"),
                ["mm3"] = new("mm3", "cubic millimeters", "Volume"),
                ["km3"] = new("km3", "cubic kilometers", "Volume"),
                
                // Speed units
                ["m/s"] = new("m/s", "meters per second", "Speed"),
                ["km/h"] = new("km/h", "kilometers per hour", "Speed"),
                ["mph"] = new("mph", "miles per hour", "Speed"),
                ["ft/s"] = new("ft/s", "feet per second", "Speed"),
                ["knot"] = new("knot", "knots", "Speed")
            };
        }
        
        public Dictionary<string, UnitConversion> GetConversions()
        {
            return new Dictionary<string, UnitConversion>
            {
                // Length conversions (all relative to meter base)
                ["m|cm"] = UnitConversion.Linear(100.0, "1m = 100cm"),
                ["cm|m"] = UnitConversion.Linear(0.01, "1cm = 0.01m"),
                ["m|mm"] = UnitConversion.Linear(1000.0, "1m = 1000mm"),
                ["mm|m"] = UnitConversion.Linear(0.001, "1mm = 0.001m"),
                ["m|km"] = UnitConversion.Linear(0.001, "1m = 0.001km"),
                ["km|m"] = UnitConversion.Linear(1000.0, "1km = 1000m"),
                ["m|in"] = UnitConversion.Linear(39.3701, "EXACT: 1m = 39.3701in"),
                ["in|m"] = UnitConversion.Linear(0.0254, "EXACT: 1in = 0.0254m"),
                ["m|ft"] = UnitConversion.Linear(3.28084, "EXACT: 1m = 3.28084ft"),
                ["ft|m"] = UnitConversion.Linear(0.3048, "EXACT: 1ft = 0.3048m"),
                ["m|yd"] = UnitConversion.Linear(1.09361, "EXACT: 1m = 1.09361yd"),
                ["yd|m"] = UnitConversion.Linear(0.9144, "EXACT: 1yd = 0.9144m"),
                ["m|mi"] = UnitConversion.Linear(0.000621371, "EXACT: 1m = 0.000621371mi"),
                ["mi|m"] = UnitConversion.Linear(1609.344, "EXACT: 1mi = 1609.344m")
                
                // Imperial-to-Imperial conversions (direct)
                ["in|ft"] = UnitConversion.Linear(1.0/12.0, "EXACT: 12in = 1ft"),
                ["ft|in"] = UnitConversion.Linear(12.0, "EXACT: 1ft = 12in"),
                ["ft|yd"] = UnitConversion.Linear(1.0/3.0, "EXACT: 3ft = 1yd"),
                ["yd|ft"] = UnitConversion.Linear(3.0, "EXACT: 1yd = 3ft"),
                ["in|yd"] = UnitConversion.Linear(1.0/36.0, "EXACT: 36in = 1yd"),
                ["yd|in"] = UnitConversion.Linear(36.0, "EXACT: 1yd = 36in"),
                ["ft|mi"] = UnitConversion.Linear(1.0/5280.0, "EXACT: 5280ft = 1mi"),
                ["mi|ft"] = UnitConversion.Linear(5280.0, "EXACT: 1mi = 5280ft"),
                ["in|mi"] = UnitConversion.Linear(1.0/63360.0, "EXACT: 63360in = 1mi"),
                ["mi|in"] = UnitConversion.Linear(63360.0, "EXACT: 1mi = 63360in"),
                ["yd|mi"] = UnitConversion.Linear(1.0/1760.0, "EXACT: 1760yd = 1mi"),
                ["mi|yd"] = UnitConversion.Linear(1760.0, "EXACT: 1mi = 1760yd")
                
                // Metric-to-Metric conversions (direct)
                ["cm|mm"] = new(10.0),
                ["mm|cm"] = new(0.1),
                ["cm|km"] = new(0.00001),
                ["km|cm"] = new(100000.0),
                ["mm|km"] = new(0.000001),
                ["km|mm"] = new(1000000.0),
                
                // Mass conversions (all relative to kilogram base)
                ["kg|g"] = new(1000.0),
                ["g|kg"] = new(0.001),
                ["kg|mg"] = new(1000000.0),
                ["mg|kg"] = new(0.000001),
                ["kg|lb"] = new(2.20462),       // EXACT: 1kg = 2.20462lb
                ["lb|kg"] = new(0.453592),      // EXACT: 1lb = 0.453592kg
                ["kg|oz"] = new(35.274),        // EXACT: 1kg = 35.274oz
                ["oz|kg"] = new(0.0283495),     // EXACT: 1oz = 0.0283495kg
                ["lb|oz"] = new(16.0),          // EXACT: 1lb = 16oz
                ["oz|lb"] = new(0.0625),        // EXACT: 1oz = 0.0625lb
                ["g|mg"] = new(1000.0),
                ["mg|g"] = new(0.001),
                
                // Force conversions (all relative to newton base)
                ["N|kN"] = new(0.001),
                ["kN|N"] = new(1000.0),
                ["N|dyne"] = new(100000.0),     // EXACT: 1N = 100000dyne
                ["dyne|N"] = new(0.00001),      // EXACT: 1dyne = 0.00001N
                ["N|lbf"] = new(0.224809),      // EXACT: 1N = 0.224809lbf
                ["lbf|N"] = new(4.44822),       // EXACT: 1lbf = 4.44822N
                
                // Temperature conversions (non-linear functions)
                ["C|F"] = UnitConversion.Function(c => c * 9.0/5.0 + 32.0, "°C to °F: F = C × 9/5 + 32"),
                ["F|C"] = UnitConversion.Function(f => (f - 32.0) * 5.0/9.0, "°F to °C: C = (F - 32) × 5/9"),
                ["C|K"] = UnitConversion.Function(c => c + 273.15, "°C to K: K = C + 273.15"),
                ["K|C"] = UnitConversion.Function(k => k - 273.15, "K to °C: C = K - 273.15"),
                ["F|K"] = UnitConversion.Function(f => (f - 32.0) * 5.0/9.0 + 273.15, "°F to K: K = (F - 32) × 5/9 + 273.15"),
                ["K|F"] = UnitConversion.Function(k => (k - 273.15) * 9.0/5.0 + 32.0, "K to °F: F = (K - 273.15) × 9/5 + 32"),
                
                // Angle conversions (using functions for precision)
                ["rad|deg"] = UnitConversion.Function(rad => rad * 180.0 / Math.PI, "rad to deg: deg = rad × 180/π"),
                ["deg|rad"] = UnitConversion.Function(deg => deg * Math.PI / 180.0, "deg to rad: rad = deg × π/180"),
                ["rad|mrad"] = UnitConversion.Linear(1000.0, "1 rad = 1000 mrad"),
                ["mrad|rad"] = UnitConversion.Linear(0.001, "1 mrad = 0.001 rad"),
                ["deg|mrad"] = UnitConversion.Function(deg => deg * 1000.0 * Math.PI / 180.0, "deg to mrad: mrad = deg × 1000π/180"),
                ["mrad|deg"] = UnitConversion.Function(mrad => mrad * 180.0 / (1000.0 * Math.PI), "mrad to deg: deg = mrad × 180/(1000π)")
                
                // Time conversions
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
                ["m2|cm2"] = new(10000.0),      // (100)^2
                ["cm2|m2"] = new(0.0001),       // (0.01)^2
                ["m2|mm2"] = new(1000000.0),    // (1000)^2
                ["mm2|m2"] = new(0.000001),     // (0.001)^2
                ["m2|km2"] = new(0.000001),     // (0.001)^2
                ["km2|m2"] = new(1000000.0),    // (1000)^2
                
                // Volume conversions (cube of length conversions)
                ["m3|cm3"] = new(1000000.0),    // (100)^3
                ["cm3|m3"] = new(0.000001),     // (0.01)^3
                ["m3|mm3"] = new(1000000000.0), // (1000)^3
                ["mm3|m3"] = new(0.000000001),  // (0.001)^3
                ["m3|km3"] = new(0.000000001),  // (0.001)^3
                ["km3|m3"] = new(1000000000.0), // (1000)^3
                
                // Speed conversions
                ["m/s|km/h"] = new(3.6),        // 1 m/s = 3.6 km/h
                ["km/h|m/s"] = new(1.0/3.6),    // 1 km/h = 0.2778 m/s
                ["m/s|mph"] = new(2.23694),     // 1 m/s = 2.23694 mph
                ["mph|m/s"] = new(0.44704),     // 1 mph = 0.44704 m/s
                ["m/s|ft/s"] = new(3.28084),    // Same as m|ft
                ["ft/s|m/s"] = new(0.3048),     // Same as ft|m
                ["m/s|knot"] = new(1.94384),    // 1 m/s = 1.94384 knots
                ["knot|m/s"] = new(0.514444),   // 1 knot = 0.514444 m/s
                
                // Pixels (display dependent - default 5000 px/m)
                ["m|px"] = UnitConversion.Linear(5000.0, "Default: 5000 pixels per meter"),
                ["px|m"] = UnitConversion.Linear(0.0002, "Default: 0.0002 meters per pixel"),
                
                // Example of more complex function-based conversions that could be added:
                // Wind chill, heat index, pressure altitude, etc.
                // ["temp_windchill"] = UnitConversion.Function(data => 
                //     35.74 + 0.6215*data.temp - 35.75*Math.Pow(data.wind, 0.16) + 0.4275*data.temp*Math.Pow(data.wind, 0.16),
                //     "Wind chill calculation based on temperature and wind speed"),
                
                // Logarithmic conversions (decibels, pH, Richter scale, etc.)
                // ["linear|db"] = UnitConversion.Function(linear => 20.0 * Math.Log10(linear), "Linear to decibels: dB = 20×log₁₀(linear)"),
                // ["db|linear"] = UnitConversion.Function(db => Math.Pow(10.0, db / 20.0), "Decibels to linear: linear = 10^(dB/20)")
                
                // Conditional conversions (different formulas based on ranges)
                // ["pressure|altitude"] = UnitConversion.Function(pressure => 
                //     pressure > 1013.25 ? /* sea level formula */ : /* high altitude formula */,
                //     "Pressure to altitude with different formulas based on altitude range")
            };
        }
        
        public Dictionary<string, string> GetUnitDisplayNames()
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