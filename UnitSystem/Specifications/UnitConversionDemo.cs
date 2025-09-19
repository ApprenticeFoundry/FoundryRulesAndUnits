using System;
using FoundryRulesAndUnits.Units.Specifications;

namespace FoundryRulesAndUnits.Units.Specifications
{
    /// <summary>
    /// Test program demonstrating function-based unit conversions
    /// </summary>
    public class UnitConversionDemo
    {
        public static void RunDemo()
        {
            Console.WriteLine("=== Function-Based Unit Conversion Demo ===\n");
            
            var mks = new MKSUnitSystemSpecification();
            var conversions = mks.GetConversions();
            
            // Test linear conversions
            Console.WriteLine("Linear Conversions:");
            TestConversion(conversions, "m|ft", 1.0, "1 meter to feet");
            TestConversion(conversions, "ft|m", 1.0, "1 foot to meters");
            TestConversion(conversions, "in|ft", 12.0, "12 inches to feet");
            TestConversion(conversions, "ft|in", 1.0, "1 foot to inches");
            Console.WriteLine();
            
            // Test temperature conversions (non-linear)
            Console.WriteLine("Temperature Conversions (Non-Linear):");
            TestConversion(conversions, "C|F", 0.0, "0°C to Fahrenheit (freezing point)");
            TestConversion(conversions, "C|F", 100.0, "100°C to Fahrenheit (boiling point)");
            TestConversion(conversions, "F|C", 32.0, "32°F to Celsius (freezing point)");
            TestConversion(conversions, "F|C", 212.0, "212°F to Celsius (boiling point)");
            TestConversion(conversions, "C|K", 0.0, "0°C to Kelvin (absolute zero offset)");
            TestConversion(conversions, "K|C", 273.15, "273.15K to Celsius (freezing point)");
            Console.WriteLine();
            
            // Test angle conversions
            Console.WriteLine("Angle Conversions:");
            TestConversion(conversions, "deg|rad", 90.0, "90° to radians (right angle)");
            TestConversion(conversions, "rad|deg", Math.PI, "π radians to degrees (180°)");
            TestConversion(conversions, "deg|rad", 180.0, "180° to radians (π)");
            TestConversion(conversions, "rad|deg", Math.PI/2, "π/2 radians to degrees (90°)");
            Console.WriteLine();
            
            // Show formula descriptions
            Console.WriteLine("Conversion Formulas:");
            ShowFormula(conversions, "C|F");
            ShowFormula(conversions, "F|C");
            ShowFormula(conversions, "deg|rad");
            ShowFormula(conversions, "m|ft");
            ShowFormula(conversions, "in|ft");
        }
        
        private static void TestConversion(Dictionary<string, UnitConversion> conversions, 
            string conversionKey, double inputValue, string description)
        {
            if (conversions.TryGetValue(conversionKey, out var conversion))
            {
                var result = conversion.Convert(inputValue);
                var units = conversionKey.Split('|');
                Console.WriteLine($"  ✅ {description}: {inputValue:F3} {units[0]} = {result:F6} {units[1]}");
            }
            else
            {
                Console.WriteLine($"  ❌ {description}: Conversion '{conversionKey}' not found");
            }
        }
        
        private static void ShowFormula(Dictionary<string, UnitConversion> conversions, string conversionKey)
        {
            if (conversions.TryGetValue(conversionKey, out var conversion) && 
                !string.IsNullOrEmpty(conversion.FormulaDescription))
            {
                Console.WriteLine($"  📝 {conversionKey}: {conversion.FormulaDescription}");
            }
        }
    }
}