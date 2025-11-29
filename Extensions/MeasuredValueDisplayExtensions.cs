using System;
using System.Collections.Generic;
using FoundryRulesAndUnits.Units;

namespace FoundryRulesAndUnits.Extensions
{
    /// <summary>
    /// Extension methods for enhanced display formatting of MeasuredValue objects
    /// Provides Unicode-friendly display options while maintaining ASCII parsing compatibility
    /// </summary>
    public static class MeasuredValueDisplayExtensions
    {
        /// <summary>
        /// Unicode unit symbol mapping for pretty printing
        /// Maps ASCII unit symbols back to their proper Unicode representations
        /// </summary>
        private static readonly Dictionary<string, string> UnicodeUnitMap = new()
        {
            // Temperature units
            ["C"] = "°C",      // SI specification uses "C", not "degC"
            ["F"] = "°F",      // SI specification uses "F", not "degF"
            ["K"] = "K",       // Kelvin stays the same
            ["R"] = "°R",      // Rankine
            ["degC"] = "°C",   // Legacy support for ASCII parser output
            ["degF"] = "°F",   // Legacy support for ASCII parser output
            ["degK"] = "°K",   // Legacy support
            ["degR"] = "°R",   // Legacy support
            
            // Micro units (μ prefix)
            ["um"] = "μm",
            ["ug"] = "μg",
            ["uA"] = "μA",
            ["uV"] = "μV",
            ["uF"] = "μF",
            ["uH"] = "μH",
            ["us"] = "μs",
            ["umol"] = "μmol",
            ["urad"] = "μrad",
            
            // Area units (superscript 2)
            ["m2"] = "m²",
            ["cm2"] = "cm²",
            ["mm2"] = "mm²",
            ["km2"] = "km²",
            ["ft2"] = "ft²",
            ["in2"] = "in²",
            ["yd2"] = "yd²",
            ["mi2"] = "mi²",
            ["ha"] = "ha", // hectare stays the same
            ["acre"] = "acre", // acre stays the same
            
            // Volume units (superscript 3)
            ["m3"] = "m³",
            ["cm3"] = "cm³",
            ["mm3"] = "mm³",
            ["km3"] = "km³",
            ["ft3"] = "ft³",
            ["in3"] = "in³",
            ["yd3"] = "yd³",
            
            // Speed units
            ["kph"] = "km/h",
            ["mph"] = "mph", // already ASCII-friendly
            
            // Electrical resistance
            ["ohm"] = "Ω",
            
            // Angular units
            ["deg"] = "°", // Just the degree symbol for angle
            
            // Special characters
            ["Angstrom"] = "Å",
            ["angstrom"] = "Å",
            
            // Compound units with superscripts in denominators
            ["m/s2"] = "m/s²",
            ["ft/s2"] = "ft/s²",
            ["cm/s2"] = "cm/s²",
            ["km/h2"] = "km/h²",
            
            // Power and energy compound units
            ["W/m2"] = "W/m²",
            ["J/m2"] = "J/m²",
            ["cal/cm2"] = "cal/cm²"
        };

        /// <summary>
        /// Display the measured value with Unicode-formatted units for beautiful output
        /// Converts ASCII unit symbols to proper Unicode representations
        /// </summary>
        /// <param name="measuredValue">The measured value to format</param>
        /// <param name="precision">Number of decimal places (default: 3)</param>
        /// <returns>Formatted string with Unicode units</returns>
        public static string PrettyPrint(this MeasuredValue measuredValue, int precision = 3)
        {
            if (measuredValue == null)
                return "null";

            var displayUnits = measuredValue.DisplayUnits() ?? measuredValue.BaseUnits();
            // CRITICAL FIX: Convert internal value to display units using As() method
            // Previously used Value() which returns V (internal/base value), causing "1 mm" instead of "1000 mm"
            var displayValue = measuredValue.As(displayUnits);
            
            // Convert ASCII units to Unicode
            var prettyUnits = ConvertToUnicodeUnits(displayUnits);
            
            // Format the number with specified precision
            var formattedValue = Math.Round(displayValue, precision).ToString($"F{precision}").TrimEnd('0').TrimEnd('.');
            
            return $"{formattedValue} {prettyUnits}";
        }

        /// <summary>
        /// Display the measured value with Unicode units and custom formatting
        /// </summary>
        /// <param name="measuredValue">The measured value to format</param>
        /// <param name="format">Custom number format string (e.g., "F2", "G", "E2")</param>
        /// <returns>Formatted string with Unicode units</returns>
        public static string PrettyPrint(this MeasuredValue measuredValue, string format)
        {
            if (measuredValue == null)
                return "null";

            var displayUnits = measuredValue.DisplayUnits() ?? measuredValue.BaseUnits();
            // CRITICAL FIX: Convert internal value to display units using As() method
            var displayValue = measuredValue.As(displayUnits);
            
            // Convert ASCII units to Unicode
            var prettyUnits = ConvertToUnicodeUnits(displayUnits);
            
            // Apply custom formatting
            var formattedValue = displayValue.ToString(format);
            
            return $"{formattedValue} {prettyUnits}";
        }

        /// <summary>
        /// Get just the pretty-formatted units without the value
        /// Useful for labels, headers, or unit-only displays
        /// </summary>
        /// <param name="measuredValue">The measured value to get units from</param>
        /// <returns>Unicode-formatted unit string</returns>
        public static string PrettyUnits(this MeasuredValue measuredValue)
        {
            if (measuredValue == null)
                return "";

            var displayUnits = measuredValue.DisplayUnits() ?? measuredValue.BaseUnits();
            return ConvertToUnicodeUnits(displayUnits);
        }

        /// <summary>
        /// Convert ASCII unit symbols to their Unicode equivalents
        /// Handles compound units and complex expressions
        /// </summary>
        /// <param name="asciiUnits">ASCII unit string</param>
        /// <returns>Unicode-formatted unit string</returns>
        private static string ConvertToUnicodeUnits(string asciiUnits)
        {
            if (string.IsNullOrEmpty(asciiUnits))
                return "";

            // Direct lookup for simple units
            if (UnicodeUnitMap.TryGetValue(asciiUnits, out var unicodeUnit))
                return unicodeUnit;

            // Handle compound units with forward slashes (e.g., "m/s", "km/h")
            if (asciiUnits.Contains('/'))
            {
                return ConvertCompoundUnits(asciiUnits);
            }

            // Handle multiplied units with asterisks (e.g., "N*m")
            if (asciiUnits.Contains('*'))
            {
                return ConvertMultipliedUnits(asciiUnits);
            }

            // Handle units with spaces (e.g., "ft lb")
            if (asciiUnits.Contains(' '))
            {
                return ConvertSpacedUnits(asciiUnits);
            }

            // If no mapping found, return as-is (already Unicode-friendly)
            return asciiUnits;
        }

        /// <summary>
        /// Convert compound units with division (e.g., "m/s2" → "m/s²")
        /// </summary>
        private static string ConvertCompoundUnits(string asciiUnits)
        {
            var parts = asciiUnits.Split('/');
            var numerator = ConvertToUnicodeUnits(parts[0]);
            var denominator = parts.Length > 1 ? ConvertToUnicodeUnits(parts[1]) : "";
            
            return $"{numerator}/{denominator}";
        }

        /// <summary>
        /// Convert multiplied units (e.g., "N*m" → "N⋅m")
        /// </summary>
        private static string ConvertMultipliedUnits(string asciiUnits)
        {
            var parts = asciiUnits.Split('*');
            var convertedParts = new List<string>();
            
            foreach (var part in parts)
            {
                convertedParts.Add(ConvertToUnicodeUnits(part.Trim()));
            }
            
            return string.Join("⋅", convertedParts); // Use Unicode multiplication dot
        }

        /// <summary>
        /// Convert space-separated units (e.g., "ft lb" → "ft⋅lb")
        /// </summary>
        private static string ConvertSpacedUnits(string asciiUnits)
        {
            var parts = asciiUnits.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var convertedParts = new List<string>();
            
            foreach (var part in parts)
            {
                convertedParts.Add(ConvertToUnicodeUnits(part));
            }
            
            return string.Join("⋅", convertedParts); // Use Unicode multiplication dot
        }

        /// <summary>
        /// Add a new ASCII-to-Unicode unit mapping
        /// Allows extending the Unicode mapping at runtime
        /// </summary>
        /// <param name="asciiUnit">ASCII unit symbol</param>
        /// <param name="unicodeUnit">Unicode representation</param>
        public static void AddUnicodeMapping(string asciiUnit, string unicodeUnit)
        {
            UnicodeUnitMap[asciiUnit] = unicodeUnit;
        }

        /// <summary>
        /// Get all available ASCII-to-Unicode mappings
        /// Useful for debugging or UI display
        /// </summary>
        /// <returns>Dictionary of ASCII to Unicode unit mappings</returns>
        public static Dictionary<string, string> GetUnicodeMappings()
        {
            return new Dictionary<string, string>(UnicodeUnitMap);
        }
    }
}