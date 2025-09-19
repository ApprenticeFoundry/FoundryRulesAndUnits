using System;
using System.Collections.Generic;
using FoundryRulesAndUnits.Units.Specifications;

namespace FoundryRulesAndUnits.Units
{
    /// <summary>
    /// Factory for creating and managing unit system specifications
    /// Bridges the new specification architecture with the existing UnitSystem service
    /// </summary>
    public static class UnitSystemFactory
    {
        /// <summary>
        /// Registry of all available unit system specifications
        /// </summary>
        private static readonly Dictionary<UnitSystemType, Func<IUnitSystemSpecification>> SpecificationRegistry = 
            new Dictionary<UnitSystemType, Func<IUnitSystemSpecification>>
        {
            [UnitSystemType.MKS] = () => new MKSUnitSystemSpecification(),
            [UnitSystemType.CGS] = () => new CGSUnitSystemSpecification(),
            [UnitSystemType.FPS] = () => new FPSUnitSystemSpecification(),
            [UnitSystemType.IPS] = () => new IPSUnitSystemSpecification(),
            [UnitSystemType.mmNs] = () => new mmNsUnitSystemSpecification()
        };

        /// <summary>
        /// Gets the specification for a given unit system type
        /// </summary>
        /// <param name="systemType">The unit system type to get</param>
        /// <returns>The unit system specification instance</returns>
        public static IUnitSystemSpecification GetSpecification(UnitSystemType systemType)
        {
            if (SpecificationRegistry.TryGetValue(systemType, out var factory))
            {
                return factory();
            }
            throw new ArgumentException($"No specification registered for unit system type: {systemType}");
        }

        /// <summary>
        /// Gets all available unit system types
        /// </summary>
        /// <returns>Array of all supported unit system types</returns>
        public static UnitSystemType[] GetAvailableSystemTypes()
        {
            return new[] { UnitSystemType.MKS, UnitSystemType.CGS, UnitSystemType.FPS, UnitSystemType.IPS, UnitSystemType.mmNs };
        }

        /// <summary>
        /// Gets all available unit system specifications
        /// </summary>
        /// <returns>Dictionary mapping system types to their specifications</returns>
        public static Dictionary<UnitSystemType, IUnitSystemSpecification> GetAllSpecifications()
        {
            var result = new Dictionary<UnitSystemType, IUnitSystemSpecification>();
            foreach (var kvp in SpecificationRegistry)
            {
                result[kvp.Key] = kvp.Value();
            }
            return result;
        }

        /// <summary>
        /// Performs a universal unit conversion using the appropriate specification
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="fromUnit">Source unit symbol</param>
        /// <param name="toUnit">Target unit symbol</param>
        /// <param name="systemType">Unit system to use for conversion (defaults to MKS)</param>
        /// <returns>Converted value</returns>
        public static double Convert(double value, string fromUnit, string toUnit, UnitSystemType systemType = UnitSystemType.MKS)
        {
            var specification = GetSpecification(systemType);
            return specification.Convert(value, fromUnit, toUnit);
        }

        /// <summary>
        /// Gets the display name for a unit from any system
        /// </summary>
        /// <param name="unitSymbol">Unit symbol to look up</param>
        /// <param name="systemType">Unit system to check (defaults to MKS)</param>
        /// <returns>Display name or the symbol if not found</returns>
        public static string GetUnitDisplayName(string unitSymbol, UnitSystemType systemType = UnitSystemType.MKS)
        {
            var specification = GetSpecification(systemType);
            var displayNames = specification.GetUnitDisplayNames();
            return displayNames.TryGetValue(unitSymbol, out var displayName) ? displayName : unitSymbol;
        }

        /// <summary>
        /// Gets the unit family for a given unit symbol
        /// </summary>
        /// <param name="unitSymbol">Unit symbol to check</param>
        /// <param name="systemType">Unit system to use (defaults to MKS)</param>
        /// <returns>Unit family name</returns>
        public static string GetUnitFamily(string unitSymbol, UnitSystemType systemType = UnitSystemType.MKS)
        {
            var specification = GetSpecification(systemType);
            return specification.GetUnitFamily(unitSymbol);
        }

        /// <summary>
        /// Validates that a unit conversion is possible between two units
        /// </summary>
        /// <param name="fromUnit">Source unit</param>
        /// <param name="toUnit">Target unit</param>
        /// <param name="systemType">Unit system to check</param>
        /// <returns>True if conversion is possible</returns>
        public static bool CanConvert(string fromUnit, string toUnit, UnitSystemType systemType = UnitSystemType.MKS)
        {
            try
            {
                var specification = GetSpecification(systemType);
                var fromFamily = specification.GetUnitFamily(fromUnit);
                var toFamily = specification.GetUnitFamily(toUnit);
                return fromFamily == toFamily && fromFamily != "Unknown";
            }
            catch
            {
                return false;
            }
        }
    }
}