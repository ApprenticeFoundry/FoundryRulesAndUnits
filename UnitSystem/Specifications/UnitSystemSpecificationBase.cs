using System;
using System.Collections.Generic;
using FoundryRulesAndUnits.Units;

namespace FoundryRulesAndUnits.Units.Specifications
{
    /// <summary>
    /// Abstract base class for unit system specifications that provides common caching and method implementations.
    /// Concrete implementations only need to provide SystemName, SystemDescription, SystemType, and UnitDefinitions.
    /// All other interface methods are implemented here with proper caching for performance.
    /// </summary>
    public abstract class UnitSystemSpecificationBase : IUnitSystemSpecification
    {
        // Abstract properties that concrete classes must implement
        public abstract string SystemName { get; }
        public abstract string SystemDescription { get; }
        public abstract UnitSystemType SystemType { get; }
        public abstract IReadOnlyList<UnitDefinition> UnitDefinitions { get; }

        // Common cached fields for performance optimization
        private Dictionary<UnitFamilyName, UnitGroup>? _cachedUnitGroups = null;
        private List<string>? _cachedUnitSymbols = null;
        private List<UnitDefinition>? _cachedBaseUnits = null;
        private Dictionary<UnitFamilyName, UnitDefinition>? _cachedBaseUnitsByFamily = null;
        private Dictionary<UnitFamilyName, List<UnitDefinition>>? _cachedUnitsByFamily = null;
        private Dictionary<string, UnitFamilyName>? _cachedSymbolToFamily = null;

        /// <summary>
        /// Gets all unit symbols in the system (cached for performance)
        /// </summary>
        public virtual List<string> GetAllUnitSymbols()
        {
            if (_cachedUnitSymbols == null)
            {
                _cachedUnitSymbols = new List<string>();
                foreach (var unit in UnitDefinitions)
                {
                    _cachedUnitSymbols.Add(unit.Symbol);
                }
            }
            return _cachedUnitSymbols;
        }

        /// <summary>
        /// Gets all base units in the system (cached for performance)
        /// </summary>
        public virtual List<UnitDefinition> GetAllBaseUnits()
        {
            if (_cachedBaseUnits == null)
            {
                _cachedBaseUnits = new List<UnitDefinition>();
                foreach (var unit in UnitDefinitions)
                {
                    if (unit.IsBaseUnit)
                    {
                        _cachedBaseUnits.Add(unit);
                    }
                }
            }
            return _cachedBaseUnits;
        }

        /// <summary>
        /// Gets unit groups organized by family, with each group containing a base unit and all related units
        /// Enhanced with parser accessibility configuration for two-tier family system
        /// </summary>
        public virtual Dictionary<UnitFamilyName, UnitGroup> GetUnitGroups()
        {
            if (_cachedUnitGroups == null)
            {
                _cachedUnitGroups = new Dictionary<UnitFamilyName, UnitGroup>();
                var unitsByFamily = GetAllUnitsByFamily();
                var baseUnitsByFamily = GetBaseUnitsByFamily();
                var parserAccessibleFamilies = GetParserAccessibleFamilies();

                foreach (var family in unitsByFamily.Keys)
                {
                    if (baseUnitsByFamily.TryGetValue(family, out var baseUnit))
                    {
                        var members = unitsByFamily[family];
                        var isParserAccessible = parserAccessibleFamilies.Contains(family);
                        _cachedUnitGroups[family] = new UnitGroup(family, SystemType, baseUnit, members, isParserAccessible);
                    }
                }
            }
            return _cachedUnitGroups;
        }

        /// <summary>
        /// Define which unit families are parser-accessible (can be created from shorthand syntax)
        /// Override in derived classes to customize the two-tier family system
        /// </summary>
        protected virtual HashSet<UnitFamilyName> GetParserAccessibleFamilies()
        {
            // Default implementation: Most common engineering families are parser-accessible
            return new HashSet<UnitFamilyName>
            {
                // PARSER-ACCESSIBLE FAMILIES (shorthand syntax works: "5m", "45deg", "30s")
                UnitFamilyName.Length,        // Object dimensions, mechanical parts
                UnitFamilyName.Distance,      // Large-scale measurements (dual family with Length)
                UnitFamilyName.Angle,         // Rotations, orientations, geometric angles
                UnitFamilyName.Duration,      // Time intervals, processing times
                UnitFamilyName.Mass,          // Material properties, object weight
                UnitFamilyName.Temperature,   // Environmental, process temperatures
                UnitFamilyName.Speed,         // Velocity, rate measurements
                UnitFamilyName.Force,         // Applied forces, loads
                UnitFamilyName.Power,         // Energy consumption, output
                UnitFamilyName.Voltage,       // Electrical measurements
                UnitFamilyName.Current,       // Electrical measurements
                UnitFamilyName.Resistance,    // Electrical measurements
                UnitFamilyName.Capacitance,   // Electrical measurements
                UnitFamilyName.Frequency,     // Signal processing, mechanical vibration
                UnitFamilyName.Quantity,      // Count, discrete items
                UnitFamilyName.QuantityFlow,  // Flow rates
                UnitFamilyName.Percent,       // Ratios, efficiency
                UnitFamilyName.DataStorage,   // File sizes, memory
                UnitFamilyName.DataFlow,      // Network speeds
                UnitFamilyName.None           // Dimensionless values
                
                // FUNCTION-ONLY FAMILIES (require explicit AS functions):
                // UnitFamilyName.Time          → ASTIME() required  
                // UnitFamilyName.Bearing       → ASBEARING() required
                // UnitFamilyName.Area          → ASAREA() required (when explicit semantics needed)
                // UnitFamilyName.Volume        → ASVOLUME() required (when explicit semantics needed)
            };
        }

        /// <summary>
        /// Gets a dictionary mapping each unit family to its base unit (cached for performance)
        /// </summary>
        public virtual Dictionary<UnitFamilyName, UnitDefinition> GetBaseUnitsByFamily()
        {
            if (_cachedBaseUnitsByFamily == null)
            {
                _cachedBaseUnitsByFamily = new Dictionary<UnitFamilyName, UnitDefinition>();
                foreach (var unit in UnitDefinitions)
                {
                    if (unit.IsBaseUnit)
                    {
                        _cachedBaseUnitsByFamily[unit.Family] = unit;
                    }
                }
            }
            return _cachedBaseUnitsByFamily;
        }

        /// <summary>
        /// Gets a dictionary mapping each unit family to all units in that family (cached for performance)
        /// </summary>
        public virtual Dictionary<UnitFamilyName, List<UnitDefinition>> GetAllUnitsByFamily()
        {
            if (_cachedUnitsByFamily == null)
            {
                _cachedUnitsByFamily = new Dictionary<UnitFamilyName, List<UnitDefinition>>();
                foreach (var unit in UnitDefinitions)
                {
                    if (!_cachedUnitsByFamily.ContainsKey(unit.Family))
                    {
                        _cachedUnitsByFamily[unit.Family] = new List<UnitDefinition>();
                    }
                    _cachedUnitsByFamily[unit.Family].Add(unit);
                }
            }
            return _cachedUnitsByFamily;
        }

        /// <summary>
        /// Gets a dictionary mapping each unit symbol to its unit family (cached for performance)
        /// </summary>
        public virtual Dictionary<string, UnitFamilyName> GetSymbolToFamilyMap()
        {
            if (_cachedSymbolToFamily == null)
            {
                _cachedSymbolToFamily = new Dictionary<string, UnitFamilyName>();
                foreach (var unit in UnitDefinitions)
                {
                    _cachedSymbolToFamily[unit.Symbol] = unit.Family;
                }
            }
            return _cachedSymbolToFamily;
        }

        /// <summary>
        /// Clears all cached data, forcing regeneration on next access.
        /// Useful for testing or if UnitDefinitions change at runtime.
        /// </summary>
        protected virtual void ClearCache()
        {
            _cachedUnitGroups = null;
            _cachedUnitSymbols = null;
            _cachedBaseUnits = null;
            _cachedBaseUnitsByFamily = null;
            _cachedUnitsByFamily = null;
            _cachedSymbolToFamily = null;
        }
    }
}