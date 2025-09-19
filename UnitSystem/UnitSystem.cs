using System;
using System.Collections.Generic;
using FoundryRulesAndUnits.Units;

namespace FoundryRulesAndUnits.Units
{
    /// <summary>
    /// Legacy UnitSystem class for backward compatibility
    /// Wraps the new UnitSystemService functionality
    /// </summary>
    public class UnitSystem : IUnitSystem
    {
        private readonly UnitSystemService _service;

        public UnitSystem()
        {
            _service = UnitSystemService.Instance;
        }

        public UnitSystem(UnitSystemType systemType)
        {
            _service = UnitSystemService.Instance;
            _service.SetUnitSystem(systemType);
        }

        /// <summary>
        /// Legacy Apply method - sets the unit system
        /// </summary>
        public void Apply(UnitSystemType systemType)
        {
            _service.SetUnitSystem(systemType);
        }

        /// <summary>
        /// Legacy Categories method - returns all unit categories
        /// </summary>
        public List<UnitCategory> Categories()
        {
            var categories = new List<UnitCategory>();
            foreach (var family in Enum.GetValues<UnitFamilyName>())
            {
                if (family != UnitFamilyName.None)
                {
                    categories.Add(new UnitCategory(family.ToString()));
                }
            }
            return categories;
        }

        /// <summary>
        /// Convert value from one unit to another
        /// </summary>
        public double Convert(double value, string fromUnit, string toUnit)
        {
            return _service.Convert(value, fromUnit, toUnit);
        }

        /// <summary>
        /// Check if a unit is valid
        /// </summary>
        public bool IsValidUnit(string unit)
        {
            return _service.IsValidUnit(unit);
        }

        /// <summary>
        /// Get units for a specific family
        /// </summary>
        public List<string> GetUnitsForFamily(UnitFamilyName family)
        {
            return _service.GetUnitsForFamily(family);
        }

        /// <summary>
        /// Get base unit for a family
        /// </summary>
        public string GetBaseUnitForFamily(UnitFamilyName family)
        {
            return _service.GetBaseUnitForFamily(family);
        }
    }

    /// <summary>
    /// Legacy IUnitSystem interface for backward compatibility
    /// </summary>
    public interface IUnitSystem
    {
        double Convert(double value, string fromUnit, string toUnit);
        bool IsValidUnit(string unit);
        List<string> GetUnitsForFamily(UnitFamilyName family);
        string GetBaseUnitForFamily(UnitFamilyName family);
    }
}