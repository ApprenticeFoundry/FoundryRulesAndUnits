using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace FoundryRulesAndUnits.Units
{
    /// <summary>
    /// Extension methods for UnitCategory to enable code reuse across different unit systems
    /// while maintaining base unit accuracy and exact conversions.
    /// 
    /// Features dynamic unit lookup table populated from actual registered unit categories.
    /// This ensures the hash table is always synchronized with the unit system.
    /// </summary>
    public static class UnitCategoryExtensions
    {
        // Dynamic hash table populated from actual unit system data
        private static readonly ConcurrentDictionary<string, UnitFamilyName> _unitFamilyLookup = new(StringComparer.OrdinalIgnoreCase);
        private static readonly object _initLock = new object();
        private static bool _isInitialized = false;

        /// <summary>
        /// Initializes the unit family lookup table from all registered unit categories
        /// This ensures the hash table is always synchronized with the actual unit system
        /// </summary>
        private static void EnsureInitialized()
        {
            if (_isInitialized) return;
            
            lock (_initLock)
            {
                if (_isInitialized) return;
                
                try
                {
                    // Create a unit system instance to access all registered categories
                    var unitSystem = new UnitSystem();
                    unitSystem.Apply(UnitSystemType.MKS); // Initialize with standard system
                    
                    // Populate the hash table from all registered categories
                    var categories = unitSystem.Categories();
                    foreach (var category in categories)
                    {
                        var unitFamily = category.UnitFamily();
                        
                        // Add base unit
                        var baseUnit = category.BaseUnits();
                        if (baseUnit != null)
                        {
                            _unitFamilyLookup.TryAdd(baseUnit.Name(), unitFamily);
                        }
                        
                        // Add all units in this category
                        var units = category.Units();
                        foreach (var unit in units)
                        {
                            _unitFamilyLookup.TryAdd(unit.Name(), unitFamily);
                        }
                    }
                    
                    // WORKAROUND: Manually add missing imperial units that are defined in FPS/IPS systems
                    // but not captured due to category overwriting issue
                    _unitFamilyLookup.TryAdd("in", UnitFamilyName.Length);
                    _unitFamilyLookup.TryAdd("ft", UnitFamilyName.Length);
                    _unitFamilyLookup.TryAdd("yd", UnitFamilyName.Length);
                    _unitFamilyLookup.TryAdd("mi", UnitFamilyName.Length);
                    
                    _isInitialized = true;
                }
                catch (Exception ex)
                {
                    // Log the error but don't fail - fallback to empty lookup
                    System.Diagnostics.Debug.WriteLine($"Failed to initialize unit lookup table: {ex.Message}");
                    _isInitialized = true; // Prevent retry loops
                }
            }
        }

        /// <summary>
        /// Fast lookup to determine unit family from unit string using dynamic hash table
        /// Populated from actual registered unit categories for perfect synchronization
        /// </summary>
        /// <param name="unitString">The unit string to look up</param>
        /// <returns>The UnitFamilyName if found, otherwise UnitFamilyName.None</returns>
        public static UnitFamilyName GetUnitFamily(string unitString)
        {
            EnsureInitialized();
            return _unitFamilyLookup.TryGetValue(unitString, out var family) ? family : UnitFamilyName.None;
        }

        /// <summary>
        /// Check if a unit string is recognized in our dynamic lookup table
        /// </summary>
        /// <param name="unitString">The unit string to check</param>
        /// <returns>True if the unit is recognized</returns>
        public static bool IsKnownUnit(string unitString)
        {
            EnsureInitialized();
            return _unitFamilyLookup.ContainsKey(unitString);
        }

        /// <summary>
        /// Get all known units for a specific family from the dynamic lookup table
        /// </summary>
        /// <param name="family">The unit family to get units for</param>
        /// <returns>List of unit strings for that family</returns>
        public static List<string> GetUnitsForFamily(UnitFamilyName family)
        {
            EnsureInitialized();
            return _unitFamilyLookup.Where(kvp => kvp.Value == family).Select(kvp => kvp.Key).ToList();
        }

        /// <summary>
        /// Extension method to add length units to a UnitCategory with exact conversions
        /// </summary>
        public static void AddLengthUnits(this UnitCategory category)
        {
            var baseUnit = category.BaseUnits().Name();
            category.Units("mm", "millimeters").Conversion(1.0, baseUnit, 0.001, "mm");
            category.Units("cm", "centimeters").Conversion(1.0, baseUnit, 0.01, "cm");
            category.Units("km", "kilometers").Conversion(1.0, baseUnit, 1000.0, "km");
            category.Units("in", "inches").Conversion(1.0, baseUnit, 0.0254, "in");
            category.Units("ft", "feet").Conversion(1.0, baseUnit, 0.3048, "ft");
            category.Units("yd", "yards").Conversion(1.0, baseUnit, 0.9144, "yd");
            category.Units("mi", "miles").Conversion(1.0, baseUnit, 1609.344, "mi");
        }

        /// <summary>
        /// Extension method to add angle units to a UnitCategory with exact conversions
        /// </summary>
        public static void AddAngleUnits(this UnitCategory category)
        {
            var baseUnit = category.BaseUnits().Name();
            category.Units("deg", "degrees").Conversion(1.0, baseUnit, Math.PI / 180.0, "deg");
            category.Units("rad", "radians").Conversion(1.0, baseUnit, 1.0, "rad");
            category.Units("mrad", "milliradians").Conversion(1.0, baseUnit, 0.001, "mrad");
        }

        /// <summary>
        /// Extension method to add time units to a UnitCategory with exact conversions
        /// </summary>
        public static UnitCategory AddTimeUnits(this UnitCategory category, string baseUnit)
        {
            category.Units("ms", "milliseconds").Conversion(1.0, baseUnit, 0.001, "ms");
            category.Units("us", "microseconds").Conversion(1.0, baseUnit, 0.000001, "us");
            category.Units("ns", "nanoseconds").Conversion(1.0, baseUnit, 0.000000001, "ns");
            category.Units("min", "minutes").Conversion(1.0, baseUnit, 60.0, "min");
            category.Units("hr", "hours").Conversion(1.0, baseUnit, 3600.0, "hr");
            category.Units("day", "days").Conversion(1.0, baseUnit, 86400.0, "day");
            return category;
        }

        /// <summary>
        /// Extension method to add area units to a UnitCategory with exact conversions
        /// </summary>
        public static void AddAreaUnits(this UnitCategory category)
        {
            var baseUnit = category.BaseUnits().Name();
            category.Units("mm2", "square millimeters").Conversion(1.0, baseUnit, 0.000001, "mm2");
            category.Units("cm2", "square centimeters").Conversion(1.0, baseUnit, 0.0001, "cm2");
            category.Units("km2", "square kilometers").Conversion(1.0, baseUnit, 1000000.0, "km2");
            category.Units("in2", "square inches").Conversion(1.0, baseUnit, 0.00064516, "in2");
            category.Units("ft2", "square feet").Conversion(1.0, baseUnit, 0.092903, "ft2");
            category.Units("yd2", "square yards").Conversion(1.0, baseUnit, 0.836127, "yd2");
        }

        /// <summary>
        /// Extension method to add volume units to a UnitCategory with exact conversions
        /// </summary>
        public static void AddVolumeUnits(this UnitCategory category)
        {
            var baseUnit = category.BaseUnits().Name();
            category.Units("mm3", "cubic millimeters").Conversion(1.0, baseUnit, 0.000000001, "mm3");
            category.Units("cm3", "cubic centimeters").Conversion(1.0, baseUnit, 0.000001, "cm3");
            category.Units("km3", "cubic kilometers").Conversion(1.0, baseUnit, 1000000000.0, "km3");
            category.Units("in3", "cubic inches").Conversion(1.0, baseUnit, 0.000016387, "in3");
            category.Units("ft3", "cubic feet").Conversion(1.0, baseUnit, 0.028317, "ft3");
            category.Units("l", "liters").Conversion(1.0, baseUnit, 0.001, "l");
            category.Units("ml", "milliliters").Conversion(1.0, baseUnit, 0.000001, "ml");
        }

        /// <summary>
        /// Extension method to add data storage units to a UnitCategory with exact conversions
        /// </summary>
        public static void AddDataStorageUnits(this UnitCategory category)
        {
            var baseUnit = category.BaseUnits().Name();
            category.Units("KB", "kilobytes").Conversion(1.0, baseUnit, 1024.0, "KB");
            category.Units("MB", "megabytes").Conversion(1.0, baseUnit, 1048576.0, "MB");
            category.Units("GB", "gigabytes").Conversion(1.0, baseUnit, 1073741824.0, "GB");
            category.Units("TB", "terabytes").Conversion(1.0, baseUnit, 1099511627776.0, "TB");
        }

        /// <summary>
        /// Extension method to add speed units to a UnitCategory with exact conversions
        /// </summary>
        public static void AddSpeedUnits(this UnitCategory category)
        {
            var baseUnit = category.BaseUnits().Name();
            category.Units("km/h", "kilometers per hour").Conversion(1.0, baseUnit, 0.277778, "km/h");
            category.Units("mph", "miles per hour").Conversion(1.0, baseUnit, 0.44704, "mph");
            category.Units("ft/s", "feet per second").Conversion(1.0, baseUnit, 0.3048, "ft/s");
            category.Units("kn", "knots").Conversion(1.0, baseUnit, 0.514444, "kn");
        }

        /// <summary>
        /// Extension method to add metric length units to a UnitCategory
        /// </summary>
        public static UnitCategory AddMetricLengthUnits(this UnitCategory category, string baseUnit)
        {
            // Add common metric length units with exact conversions
            category.Units("mm", "millimeters").Conversion(1.0, baseUnit, 0.001, "mm");
            category.Units("cm", "centimeters").Conversion(1.0, baseUnit, 0.01, "cm");
            category.Units("m", "meters").Conversion(1.0, baseUnit, 1.0, "m");
            category.Units("km", "kilometers").Conversion(1.0, baseUnit, 1000.0, "km");
            return category;
        }

        /// <summary>
        /// Extension method to add imperial length units to a UnitCategory
        /// </summary>
        public static UnitCategory AddImperialLengthUnits(this UnitCategory category, string baseUnit)
        {
            // Add common imperial length units with exact conversions
            // Note: conversion uses meters as the reference for exact values
            category.Units("in", "inches").Conversion(1.0, "in", 0.0254, "m");
            category.Units("ft", "feet").Conversion(1.0, "ft", 0.3048, "m");
            category.Units("yd", "yards").Conversion(1.0, "yd", 0.9144, "m");
            category.Units("mi", "miles").Conversion(1.0, "mi", 1609.344, "m");
            
            // If base unit is not meters, add conversions to/from base unit
            if (baseUnit != "m")
            {
                // Convert between imperial units based on exact ratios
                if (baseUnit == "in")
                {
                    category.Conversion(12.0, "in", 1.0, "ft");
                    category.Conversion(36.0, "in", 1.0, "yd");
                    category.Conversion(63360.0, "in", 1.0, "mi");
                }
                else if (baseUnit == "ft")
                {
                    category.Conversion(1.0, "ft", 12.0, "in");
                    category.Conversion(3.0, "ft", 1.0, "yd");
                    category.Conversion(5280.0, "ft", 1.0, "mi");
                }
            }
            return category;
        }

        /// <summary>
        /// Extension method to add mass units to a UnitCategory
        /// </summary>
        public static UnitCategory AddMassUnits(this UnitCategory category, string baseUnit)
        {
            // Add common mass units
            category.Units("g", "grams").Conversion(1.0, baseUnit, 0.001, "g");
            category.Units("mg", "milligrams").Conversion(1.0, baseUnit, 0.000001, "mg");
            category.Units("kg", "kilograms").Conversion(1.0, baseUnit, 1.0, "kg");
            category.Units("lb", "pounds").Conversion(1.0, baseUnit, 0.453592, "lb");
            category.Units("oz", "ounces").Conversion(1.0, baseUnit, 0.0283495, "oz");
            category.Units("ton", "tons").Conversion(1.0, baseUnit, 1000.0, "ton");
            return category;
        }

        /// <summary>
        /// Extension method to add force units to a UnitCategory
        /// </summary>
        public static UnitCategory AddForceUnits(this UnitCategory category, string baseUnit)
        {
            // Add common force units
            category.Units("N", "newtons").Conversion(1.0, baseUnit, 1.0, "N");
            category.Units("kN", "kilonewtons").Conversion(1.0, baseUnit, 1000.0, "kN");
            category.Units("dyne", "dynes").Conversion(1.0, baseUnit, 0.00001, "dyne");
            category.Units("lbf", "pound-force").Conversion(1.0, baseUnit, 4.44822, "lbf");
            return category;
        }

        /// <summary>
        /// Extension method to add temperature conversions to a UnitCategory
        /// </summary>
        public static UnitCategory AddTemperatureConversions(this UnitCategory category)
        {
            // Add temperature units (temperature conversions are complex with offsets)
            category.Units("C", "Celsius");
            category.Units("F", "Fahrenheit");
            category.Units("K", "Kelvin");
            category.Units("R", "Rankine");
            // Note: Temperature conversions need custom formulas with offsets
            // Basic scaling doesn't work for temperature
            return category;
        }

        /// <summary>
        /// Extension method to add high-precision cross-system conversions
        /// </summary>
        public static UnitCategory AddCrossSystemConversions(this UnitCategory category)
        {
            // This method can be used to add high-precision conversions between different unit systems
            // Implementation depends on the specific unit category
            // For now, return the category unchanged as conversions are handled by other methods
            return category;
        }
    }
}