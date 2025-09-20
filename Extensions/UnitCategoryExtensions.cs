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
        /// Note: With dependency injection, prefer using UnitGroup directly
        /// </summary>
        public static UnitFamilyName GetUnitFamily(string unit)
        {
            if (string.IsNullOrEmpty(unit))
                return UnitFamilyName.None;

            // TODO: This method is deprecated in favor of UnitGroup injection
            // For now, return None to encourage proper dependency injection usage
            return UnitFamilyName.None;
        }

        /// <summary>
        /// Check if a unit string is known/recognized by the system
        /// DEPRECATED: Use UnitGroup injection for proper unit validation
        /// </summary>
        [Obsolete("Use UnitGroup injection for proper unit validation")]
        public static bool IsKnownUnit(string unit)
        {
            if (string.IsNullOrEmpty(unit))
                return false;

            // Fallback - assume non-empty units are valid
            // Proper validation requires UnitGroup injection
            return true;
        }

        /// <summary>
        /// Get all known units for a specific unit family
        /// DEPRECATED: Use UnitGroup injection for proper unit enumeration
        /// </summary>
        [Obsolete("Use UnitGroup injection for proper unit enumeration")]
        public static List<string> GetUnitsForFamily(UnitFamilyName family)
        {
            // Cannot provide accurate list without UnitGroup injection
            // Return empty list to prevent runtime errors
            return new List<string>();
        }

        /// <summary>
        /// Get all known unit families
        /// </summary>

    }
}