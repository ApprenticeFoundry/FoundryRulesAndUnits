using System;
using System.Collections.Concurrent;

namespace FoundryRulesAndUnits.Units
{
    /// <summary>
    /// Extension methods for UnitCategory to enable code reuse across different unit systems
    /// while maintaining base unit accuracy and exact conversions.
    /// 
    /// PERFORMANCE NOTE: Consider adding caching for frequently-used unit categories
    /// to avoid recreating conversion tables on each Apply() call.
    /// </summary>
    public static class UnitCategoryExtensions
    {
        // TODO: Add static caching dictionary for common unit configurations
        // private static readonly ConcurrentDictionary<string, UnitCategory> _categoryCache = new();
        
        /// <summary>
        /// Adds common metric length units with exact conversions relative to the specified base unit.
        /// </summary>
        /// <param name="category">The UnitCategory to extend</param>
        /// <param name="baseUnit">The base metric unit ("m", "cm", "mm")</param>
        /// <returns>The UnitCategory with metric length units added</returns>
        public static UnitCategory AddMetricLengthUnits(this UnitCategory category, string baseUnit = "m")
        {
            if (baseUnit == "m")
            {
                return category
                    .Units("mm", "millimeters").Conversion(1000, "mm", 1, "m")     // EXACT: 1000 mm = 1 m
                    .Units("cm", "centimeters").Conversion(100, "cm", 1, "m")     // EXACT: 100 cm = 1 m
                    .Units("km", "kilometers").Conversion(1, "km", 1000, "m");    // EXACT: 1 km = 1000 m
            }
            else if (baseUnit == "mm")
            {
                return category
                    .Units("cm", "centimeters").Conversion(1, "cm", 10, "mm")     // EXACT: 1 cm = 10 mm
                    .Units("m", "meters").Conversion(0.001, "m", 1, "mm")         // EXACT: 0.001 m = 1 mm
                    .Units("km", "kilometers").Conversion(0.000001, "km", 1, "mm"); // EXACT: 0.000001 km = 1 mm
            }
            else if (baseUnit == "cm")
            {
                return category
                    .Units("mm", "millimeters").Conversion(10, "mm", 1, "cm")     // EXACT: 10 mm = 1 cm
                    .Units("m", "meters").Conversion(0.01, "m", 1, "cm")          // EXACT: 0.01 m = 1 cm
                    .Units("km", "kilometers").Conversion(0.00001, "km", 1, "cm"); // EXACT: 0.00001 km = 1 cm
            }
            else
            {
                throw new ArgumentException($"Unsupported metric base unit: '{baseUnit}'. Supported units: m, mm, cm", nameof(baseUnit));
            }
        }

        /// <summary>
        /// Adds common imperial length units with exact conversions relative to the specified base unit.
        /// </summary>
        /// <param name="category">The UnitCategory to extend</param>
        /// <param name="baseUnit">The base imperial unit ("in", "ft")</param>
        /// <returns>The UnitCategory with imperial length units added</returns>
        public static UnitCategory AddImperialLengthUnits(this UnitCategory category, string baseUnit = "in")
        {
            if (baseUnit == "in")
            {
                return category
                    .Units("ft", "feet").Conversion(1, "ft", 12, "in")           // EXACT: 1 ft = 12 in
                    .Units("yd", "yards").Conversion(1, "yd", 36, "in")          // EXACT: 1 yd = 36 in
                    .Units("mi", "miles").Conversion(1, "mi", 63360, "in")       // EXACT: 1 mi = 63360 in
                    .Units("mil", "mils").Conversion(1000, "mil", 1, "in");      // EXACT: 1000 mil = 1 in
            }
            else if (baseUnit == "ft")
            {
                return category
                    .Units("in", "inches").Conversion(12, "in", 1, "ft")         // EXACT: 12 in = 1 ft
                    .Units("yd", "yards").Conversion(1, "yd", 3, "ft")           // EXACT: 1 yd = 3 ft
                    .Units("mi", "miles").Conversion(1, "mi", 5280, "ft");       // EXACT: 1 mi = 5280 ft
            }
            
            return category;
        }

        /// <summary>
        /// Adds high-precision cross-system conversions based on international definitions.
        /// </summary>
        /// <param name="category">The UnitCategory to extend</param>
        /// <returns>The UnitCategory with cross-system conversions added</returns>
        public static UnitCategory AddCrossSystemConversions(this UnitCategory category)
        {
            var baseUnit = category.BaseUnits().Name();

            if (baseUnit == "m") // Metric base - add imperial conversions
            {
                return category
                    .Units("in", "inches").Conversion(39.3700787402, "in", 1, "m")  // HIGH PRECISION: 1000/25.4
                    .Units("ft", "feet").Conversion(3.28083989501, "ft", 1, "m");   // HIGH PRECISION: 12/3.28083989501
            }
            else if (baseUnit == "in") // Imperial base - add metric conversions
            {
                return category
                    .Units("mm", "millimeters").Conversion(25.4, "mm", 1, "in")     // EXACT by international definition (1959)
                    .Units("cm", "centimeters").Conversion(2.54, "cm", 1, "in")     // EXACT by international definition
                    .Units("m", "meters").Conversion(0.0254, "m", 1, "in");         // EXACT by international definition
            }
            else if (baseUnit == "ft") // Foot base - add metric conversions
            {
                return category
                    .Units("m", "meters").Conversion(0.3048, "m", 1, "ft")          // EXACT: 12 × 0.0254
                    .Units("cm", "centimeters").Conversion(30.48, "cm", 1, "ft");   // EXACT: 12 × 2.54
            }
            else if (baseUnit == "mm") // Millimeter base - add imperial conversions
            {
                return category
                    .Units("in", "inches").Conversion(1.0/25.4, "in", 1, "mm")      // EXACT: 1/25.4
                    .Units("ft", "feet").Conversion(1.0/(25.4*12), "ft", 1, "mm");  // EXACT: 1/(25.4*12)
            }
            else if (baseUnit == "cm") // Centimeter base - add imperial conversions
            {
                return category
                    .Units("in", "inches").Conversion(1.0/2.54, "in", 1, "cm")      // EXACT: 1/2.54
                    .Units("ft", "feet").Conversion(1.0/30.48, "ft", 1, "cm");      // EXACT: 1/30.48
            }
            
            return category;
        }

        /// <summary>
        /// Adds temperature conversions with exact formulas for all temperature scales.
        /// </summary>
        /// <param name="category">The UnitCategory to extend</param>
        /// <returns>The UnitCategory with temperature conversions added</returns>
        public static UnitCategory AddTemperatureConversions(this UnitCategory category)
        {
            var baseUnit = category.BaseUnits().Name();
            
            if (baseUnit == "C") // Celsius base
            {
                return category
                    .Units("F", "Fahrenheit")
                    .Conversion("C", "F", c => c * 9.0 / 5.0 + 32.0)        // EXACT formula
                    .Conversion("F", "C", f => (f - 32.0) * 5.0 / 9.0)      // EXACT formula
                    .Units("K", "Kelvin")
                    .Conversion("C", "K", c => c + 273.15)                   // EXACT by definition
                    .Conversion("K", "C", k => k - 273.15);                  // EXACT by definition
            }
            else if (baseUnit == "F") // Fahrenheit base
            {
                return category
                    .Units("C", "Celsius")
                    .Conversion("F", "C", f => (f - 32.0) * 5.0 / 9.0)      // EXACT formula
                    .Conversion("C", "F", c => c * 9.0 / 5.0 + 32.0)        // EXACT formula
                    .Units("K", "Kelvin")
                    .Conversion("F", "K", f => (f - 32.0) * 5.0 / 9.0 + 273.15) // EXACT formula
                    .Units("R", "Rankine")
                    .Conversion("F", "R", f => f + 459.67)                   // EXACT formula
                    .Conversion("R", "F", r => r - 459.67);                  // EXACT formula
            }
            else if (baseUnit == "K") // Kelvin base
            {
                return category
                    .Units("C", "Celsius")
                    .Conversion("K", "C", k => k - 273.15)                   // EXACT by definition
                    .Conversion("C", "K", c => c + 273.15)                   // EXACT by definition
                    .Units("F", "Fahrenheit")
                    .Conversion("K", "F", k => (k - 273.15) * 9.0 / 5.0 + 32.0); // EXACT formula
            }
            
            return category;
        }

        /// <summary>
        /// Adds mass/weight units with exact conversions relative to the specified base unit.
        /// </summary>
        /// <param name="category">The UnitCategory to extend</param>
        /// <param name="baseUnit">The base mass unit ("kg", "g", "lb")</param>
        /// <returns>The UnitCategory with mass units added</returns>
        public static UnitCategory AddMassUnits(this UnitCategory category, string baseUnit = "kg")
        {
            if (baseUnit == "kg") // Kilogram base (MKS)
            {
                return category
                    .Units("g", "grams").Conversion(1000, "g", 1, "kg")          // EXACT: 1000 g = 1 kg
                    .Units("mg", "milligrams").Conversion(1000000, "mg", 1, "kg") // EXACT: 1,000,000 mg = 1 kg
                    .Units("lb", "pounds").Conversion(2.20462262185, "lb", 1, "kg") // HIGH PRECISION
                    .Units("oz", "ounces").Conversion(35.2739619496, "oz", 1, "kg"); // HIGH PRECISION
            }
            else if (baseUnit == "g") // Gram base (CGS)
            {
                return category
                    .Units("kg", "kilograms").Conversion(0.001, "kg", 1, "g")    // EXACT: 0.001 kg = 1 g
                    .Units("mg", "milligrams").Conversion(1000, "mg", 1, "g")    // EXACT: 1000 mg = 1 g
                    .Units("lb", "pounds").Conversion(0.00220462262, "lb", 1, "g") // HIGH PRECISION
                    .Units("oz", "ounces").Conversion(0.035273962, "oz", 1, "g"); // HIGH PRECISION
            }
            else if (baseUnit == "lb") // Pound base (IPS/FPS)
            {
                return category
                    .Units("oz", "ounces").Conversion(16, "oz", 1, "lb")         // EXACT: 16 oz = 1 lb
                    .Units("ton", "tons").Conversion(1, "ton", 2000, "lb")       // EXACT: 1 ton = 2000 lb (US)
                    .Units("kg", "kilograms").Conversion(0.45359237, "kg", 1, "lb") // EXACT by international definition
                    .Units("g", "grams").Conversion(453.59237, "g", 1, "lb");    // EXACT by international definition
            }
            
            return category;
        }

        /// <summary>
        /// Adds force units with exact conversions relative to the specified base unit.
        /// </summary>
        /// <param name="category">The UnitCategory to extend</param>
        /// <param name="baseUnit">The base force unit ("N", "dyne", "lbf")</param>
        /// <returns>The UnitCategory with force units added</returns>
        public static UnitCategory AddForceUnits(this UnitCategory category, string baseUnit = "N")
        {
            if (baseUnit == "N") // Newton base (MKS/mmNs)
            {
                return category
                    .Units("kN", "kilonewtons").Conversion(1, "kN", 1000, "N")   // EXACT: 1 kN = 1000 N
                    .Units("dyne", "dynes").Conversion(100000, "dyne", 1, "N")   // EXACT: 100,000 dyne = 1 N
                    .Units("lbf", "pounds-force").Conversion(0.224808943, "lbf", 1, "N"); // HIGH PRECISION
            }
            else if (baseUnit == "dyne") // Dyne base (CGS)
            {
                return category
                    .Units("N", "newtons").Conversion(0.00001, "N", 1, "dyne")   // EXACT: 1 N = 100,000 dyne
                    .Units("lbf", "pounds-force").Conversion(2.24808943e-6, "lbf", 1, "dyne"); // HIGH PRECISION
            }
            else if (baseUnit == "lbf") // Pound-force base (IPS/FPS)
            {
                return category
                    .Units("N", "newtons").Conversion(4.4482216152605, "N", 1, "lbf") // EXACT by definition
                    .Units("dyne", "dynes").Conversion(444822.16152605, "dyne", 1, "lbf"); // EXACT
            }
            
            return category;
        }
    }
}
