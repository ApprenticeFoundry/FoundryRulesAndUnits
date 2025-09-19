using System;
using System.Collections.Generic;
using System.Linq;
using FoundryRulesAndUnits.Units.Specifications;

namespace FoundryRulesAndUnits.Units
{
    /// <summary>
    /// Adapter that bridges the new UnitSystemSpecification architecture with the existing UnitCategory system
    /// Allows gradual migration from the old manual system to the new specification-driven system
    /// </summary>
    public static class UnitSystemAdapter
    {
        /// <summary>
        /// Creates UnitCategory instances from a UnitSystemSpecification
        /// This bridges the new specification architecture with the existing UnitCategory system
        /// </summary>
        /// <param name="specification">The unit system specification to convert</param>
        /// <returns>Dictionary of UnitCategory instances organized by family</returns>
        public static Dictionary<UnitFamilyName, UnitCategory> CreateCategoriesFromSpecification(IUnitSystemSpecification specification)
        {
            var categories = new Dictionary<UnitFamilyName, UnitCategory>();
            var baseUnits = specification.GetBaseUnits();
            var unitDefinitions = specification.GetUnitDefinitions();
            var displayNames = specification.GetUnitDisplayNames();

            // Group units by family
            var unitsByFamily = unitDefinitions.Values.GroupBy(u => u.Family);

            foreach (var familyGroup in unitsByFamily)
            {
                var family = familyGroup.Key;
                var familyUnits = familyGroup.ToList();

                // Find the base unit for this family
                var baseUnitSymbol = baseUnits.ContainsKey(family.ToString()) 
                    ? baseUnits[family.ToString()] 
                    : familyUnits.FirstOrDefault(u => u.ToBaseUnit != null && u.ToBaseUnit(1.0) == 1.0)?.Symbol ?? familyUnits.First().Symbol;

                var baseUnit = familyUnits.FirstOrDefault(u => u.Symbol == baseUnitSymbol) ?? familyUnits.First();

                // Get display name for base unit
                var baseDisplayName = displayNames.ContainsKey(baseUnitSymbol) ? displayNames[baseUnitSymbol] : baseUnitSymbol;

                // Create the UnitCategory with the base unit
                var category = new UnitCategory(family.ToString(), new UnitSpec(baseUnitSymbol, baseDisplayName, family));

                // Add all other units in this family
                foreach (var unit in familyUnits.Where(u => u.Symbol != baseUnitSymbol))
                {
                    var unitDisplayName = displayNames.ContainsKey(unit.Symbol) ? displayNames[unit.Symbol] : unit.Name;
                    category.Units(unit.Symbol, unitDisplayName);

                    // Add conversion if this is a linear unit
                    if (unit.ToBaseUnit != null && unit.FromBaseUnit != null)
                    {
                        try
                        {
                            // Test if it's a linear conversion by checking if ToBaseUnit(1) gives us the factor
                            var testValue = 1.0;
                            var convertedValue = unit.ToBaseUnit(testValue);
                            var backConverted = unit.FromBaseUnit(convertedValue);
                            
                            // If back-conversion works, it's likely linear
                            if (Math.Abs(backConverted - testValue) < 1e-10)
                            {
                                // Use the conversion functions
                                category.Conversion(unit.Symbol, baseUnitSymbol, unit.ToBaseUnit);
                                category.Conversion(baseUnitSymbol, unit.Symbol, unit.FromBaseUnit);
                            }
                        }
                        catch
                        {
                            // If conversion functions fail, skip for now
                            // Complex conversions like temperature may need special handling
                        }
                    }
                }

                categories[family] = category;
            }

            return categories;
        }

        /// <summary>
        /// Updates an existing UnitSystem instance to use specifications from a given system type
        /// This allows the existing UnitSystem service to benefit from the new specification architecture
        /// </summary>
        /// <param name="unitSystem">The UnitSystem instance to update</param>
        /// <param name="systemType">The specification type to apply</param>
        /// <returns>True if successful</returns>
        public static bool ApplySpecificationToUnitSystem(UnitSystem unitSystem, UnitSystemType systemType)
        {
            try
            {
                var specification = UnitSystemFactory.GetSpecification(systemType);
                var categories = CreateCategoriesFromSpecification(specification);

                // Clear existing categories
                unitSystem.UnitCategories = new UnitCategoryService();

                // Map common families to UnitSystem properties
                if (categories.TryGetValue(UnitFamilyName.Length, out var lengthCategory))
                {
                    unitSystem.length = lengthCategory;
                    unitSystem.UnitCategories.Category(lengthCategory);
                    Length.Category = () => lengthCategory;
                }

                if (categories.TryGetValue(UnitFamilyName.Mass, out var massCategory))
                {
                    unitSystem.mass = massCategory;
                    unitSystem.UnitCategories.Category(massCategory);
                    Mass.Category = () => massCategory;
                }

                if (categories.TryGetValue(UnitFamilyName.Force, out var forceCategory))
                {
                    unitSystem.force = forceCategory;
                    unitSystem.UnitCategories.Category(forceCategory);
                    Force.Category = () => forceCategory;
                }

                if (categories.TryGetValue(UnitFamilyName.Temperature, out var temperatureCategory))
                {
                    unitSystem.temperature = temperatureCategory;
                    unitSystem.UnitCategories.Category(temperatureCategory);
                    Temperature.Category = () => temperatureCategory;
                }

                if (categories.TryGetValue(UnitFamilyName.Angle, out var angleCategory))
                {
                    unitSystem.angle = angleCategory;
                    unitSystem.UnitCategories.Category(angleCategory);
                    Angle.Category = () => angleCategory;
                }

                if (categories.TryGetValue(UnitFamilyName.Time, out var timeCategory))
                {
                    unitSystem.time = timeCategory;
                    unitSystem.UnitCategories.Category(timeCategory);
                    Time.Category = () => timeCategory;
                }

                // Add all other categories
                foreach (var kvp in categories)
                {
                    if (!IsAlreadyAdded(kvp.Key))
                    {
                        unitSystem.UnitCategories.Category(kvp.Value);
                    }
                }

                // Update active system
                unitSystem.ActiveSystem = systemType;

                return true;
            }
            catch (Exception ex)
            {
                // Log error if logging is available
                Console.WriteLine($"Error applying specification {systemType}: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Helper to check if a family has already been added to avoid duplicates
        /// </summary>
        private static bool IsAlreadyAdded(UnitFamilyName family)
        {
            return family == UnitFamilyName.Length || 
                   family == UnitFamilyName.Mass || 
                   family == UnitFamilyName.Force || 
                   family == UnitFamilyName.Temperature || 
                   family == UnitFamilyName.Angle || 
                   family == UnitFamilyName.Time;
        }

        /// <summary>
        /// Gets all available conversions for a specific unit system
        /// This provides a unified interface to access all conversion capabilities
        /// </summary>
        /// <param name="systemType">The unit system type</param>
        /// <returns>Dictionary of all possible conversions</returns>
        public static Dictionary<string, UnitConversion> GetAllConversions(UnitSystemType systemType)
        {
            var specification = UnitSystemFactory.GetSpecification(systemType);
            
            // Get auto-generated conversions from the specification
            if (specification is BaseUnitSystemSpecification baseSpec)
            {
                return baseSpec.GetEffectiveConversions();
            }

            // Fallback to base conversions
            return specification.GetBaseUnitConversions();
        }

        /// <summary>
        /// Performs a unit conversion using the specification architecture
        /// This can be used as a drop-in replacement for manual conversion logic
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="fromUnit">Source unit</param>
        /// <param name="toUnit">Target unit</param>
        /// <param name="systemType">Unit system to use</param>
        /// <returns>Converted value</returns>
        public static double ConvertUnits(double value, string fromUnit, string toUnit, UnitSystemType systemType)
        {
            return UnitSystemFactory.Convert(value, fromUnit, toUnit, systemType);
        }
    }
}