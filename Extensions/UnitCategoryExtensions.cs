using System;
using System.Collections.Generic;
using System.Linq;
using FoundryRulesAndUnits.Units;

namespace FoundryRulesAndUnits.Extensions
{
    /// <summary>
    /// Legacy compatibility extensions for unit category operations
    /// Provides backward compatibility for existing code while using the new unit system
    /// </summary>
    public static class UnitCategoryExtensions
    {
        /// <summary>
        /// Get the unit family for a given unit string
        /// </summary>
        public static UnitFamilyName GetUnitFamily(string unit)
        {
            if (string.IsNullOrEmpty(unit))
                return UnitFamilyName.None;

            // Use the global unit system to determine unit family
            var globalSystem = MeasuredValue.GlobalSystem;
            var symbolToFamilyMap = globalSystem.Current.GetSymbolToFamilyMap();
            
            if (symbolToFamilyMap.TryGetValue(unit, out var family))
            {
                return family;
            }

            return UnitFamilyName.None;
        }

        /// <summary>
        /// Check if a unit string is known/recognized by the system
        /// </summary>
        public static bool IsKnownUnit(string unit)
        {
            if (string.IsNullOrEmpty(unit))
                return false;

            var globalSystem = MeasuredValue.GlobalSystem;
            return globalSystem.IsValidUnit(unit);
        }

        /// <summary>
        /// Get all known units for a specific unit family
        /// </summary>
        public static List<string> GetUnitsForFamily(UnitFamilyName family)
        {
            var globalSystem = MeasuredValue.GlobalSystem;
            return globalSystem.GetUnitsForFamily(family);
        }

        /// <summary>
        /// Get all known unit families
        /// </summary>
        public static List<UnitFamilyName> GetAllUnitFamilies()
        {
            return Enum.GetValues<UnitFamilyName>().Where(f => f != UnitFamilyName.None).ToList();
        }
    }
}