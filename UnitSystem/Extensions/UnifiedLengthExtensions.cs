using System;

namespace FoundryRulesAndUnits.Units
{
    /// <summary>
    /// Unified length extension method that provides ALL length units regardless of base unit choice.
    /// This solves the category overwriting problem by ensuring complete unit coverage.
    /// </summary>
    public static class UnifiedLengthExtensions
    {
        /// <summary>
        /// Extension method to add ALL length units to a UnitCategory with exact conversions.
        /// This unified approach ensures all length units are available regardless of base unit choice.
        /// No more "cross system conversions" - just one complete Length category.
        /// </summary>
        public static UnitCategory AddAllLengthUnits(this UnitCategory category)
        {
            var baseUnit = category.BaseUnits().Name();
            
            // METRIC UNITS: Always add with exact conversions to ANY base unit
            if (baseUnit == "m")
            {
                // Meter base - direct metric conversions
                category.Units("mm", "millimeters").Conversion(1.0, "mm", 0.001, "m");     // 1 mm = 0.001 m
                category.Units("cm", "centimeters").Conversion(1.0, "cm", 0.01, "m");      // 1 cm = 0.01 m
                category.Units("km", "kilometers").Conversion(1.0, "km", 1000.0, "m");     // 1 km = 1000 m
            }
            else if (baseUnit == "cm")
            {
                // Centimeter base - direct metric conversions
                category.Units("mm", "millimeters").Conversion(1.0, "mm", 0.1, "cm");      // 1 mm = 0.1 cm
                category.Units("m", "meters").Conversion(1.0, "m", 100.0, "cm");           // 1 m = 100 cm
                category.Units("km", "kilometers").Conversion(1.0, "km", 100000.0, "cm");  // 1 km = 100000 cm
            }
            else if (baseUnit == "mm")
            {
                // Millimeter base - direct metric conversions
                category.Units("cm", "centimeters").Conversion(1.0, "cm", 10.0, "mm");     // 1 cm = 10 mm
                category.Units("m", "meters").Conversion(1.0, "m", 1000.0, "mm");          // 1 m = 1000 mm
                category.Units("km", "kilometers").Conversion(1.0, "km", 1000000.0, "mm"); // 1 km = 1000000 mm
            }
            else if (baseUnit == "in")
            {
                // Inch base - metric conversions via exact definitions
                category.Units("mm", "millimeters").Conversion(1.0, "mm", 1.0/25.4, "in");     // EXACT: 25.4 mm = 1 in
                category.Units("cm", "centimeters").Conversion(1.0, "cm", 1.0/2.54, "in");      // EXACT: 2.54 cm = 1 in
                category.Units("m", "meters").Conversion(1.0, "m", 1.0/0.0254, "in");           // EXACT: 0.0254 m = 1 in
                category.Units("km", "kilometers").Conversion(1.0, "km", 1000.0/0.0254, "in");  // EXACT
            }
            else if (baseUnit == "ft")
            {
                // Foot base - metric conversions via exact definitions
                category.Units("mm", "millimeters").Conversion(1.0, "mm", 1.0/304.8, "ft");    // EXACT: 304.8 mm = 1 ft
                category.Units("cm", "centimeters").Conversion(1.0, "cm", 1.0/30.48, "ft");     // EXACT: 30.48 cm = 1 ft
                category.Units("m", "meters").Conversion(1.0, "m", 1.0/0.3048, "ft");           // EXACT: 0.3048 m = 1 ft
                category.Units("km", "kilometers").Conversion(1.0, "km", 1000.0/0.3048, "ft");  // EXACT
            }
            
            // IMPERIAL UNITS: Always add with exact conversions to ANY base unit
            if (baseUnit == "in")
            {
                // Inch base - direct imperial conversions
                category.Units("ft", "feet").Conversion(1.0, "ft", 12.0, "in");         // 1 ft = 12 in
                category.Units("yd", "yards").Conversion(1.0, "yd", 36.0, "in");        // 1 yd = 36 in
                category.Units("mi", "miles").Conversion(1.0, "mi", 63360.0, "in");     // 1 mi = 63360 in
            }
            else if (baseUnit == "ft")
            {
                // Foot base - direct imperial conversions
                category.Units("in", "inches").Conversion(1.0, "in", 1.0/12.0, "ft");   // 12 in = 1 ft
                category.Units("yd", "yards").Conversion(1.0, "yd", 3.0, "ft");         // 1 yd = 3 ft
                category.Units("mi", "miles").Conversion(1.0, "mi", 5280.0, "ft");      // 1 mi = 5280 ft
            }
            else
            {
                // Metric base - imperial conversions via exact definitions
                if (baseUnit == "m")
                {
                    category.Units("in", "inches").Conversion(1.0, "in", 0.0254, "m");     // EXACT by definition
                    category.Units("ft", "feet").Conversion(1.0, "ft", 0.3048, "m");       // EXACT by definition
                    category.Units("yd", "yards").Conversion(1.0, "yd", 0.9144, "m");      // EXACT by definition
                    category.Units("mi", "miles").Conversion(1.0, "mi", 1609.344, "m");    // EXACT by definition
                }
                else if (baseUnit == "cm")
                {
                    category.Units("in", "inches").Conversion(1.0, "in", 2.54, "cm");      // EXACT by definition
                    category.Units("ft", "feet").Conversion(1.0, "ft", 30.48, "cm");       // EXACT by definition
                    category.Units("yd", "yards").Conversion(1.0, "yd", 91.44, "cm");      // EXACT by definition
                    category.Units("mi", "miles").Conversion(1.0, "mi", 160934.4, "cm");   // EXACT by definition
                }
                else if (baseUnit == "mm")
                {
                    category.Units("in", "inches").Conversion(1.0, "in", 25.4, "mm");      // EXACT by definition
                    category.Units("ft", "feet").Conversion(1.0, "ft", 304.8, "mm");       // EXACT by definition
                    category.Units("yd", "yards").Conversion(1.0, "yd", 914.4, "mm");      // EXACT by definition
                    category.Units("mi", "miles").Conversion(1.0, "mi", 1609344.0, "mm");  // EXACT by definition
                }
                
                // Add DIRECT imperial-to-imperial conversions for ALL metric base units
                category.Units("ft", "feet").Conversion(12.0, "in", 1.0, "ft");         // DIRECT: 12 in = 1 ft
                category.Units("yd", "yards").Conversion(3.0, "ft", 1.0, "yd");         // DIRECT: 3 ft = 1 yd
                category.Units("yd", "yards").Conversion(36.0, "in", 1.0, "yd");        // DIRECT: 36 in = 1 yd
                category.Units("mi", "miles").Conversion(5280.0, "ft", 1.0, "mi");      // DIRECT: 5280 ft = 1 mi
                category.Units("mi", "miles").Conversion(63360.0, "in", 1.0, "mi");     // DIRECT: 63360 in = 1 mi
            }
            
            return category;
        }
    }
}