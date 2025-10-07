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
        private bool _allCachesBuilt = false;

        /// <summary>
        /// Ensures all caches are built in a single pass over UnitDefinitions for optimal performance.
        /// This replaces 6 separate loops with one O(n) operation.
        /// </summary>
        private void EnsureAllCachesBuilt()
        {
            if (_allCachesBuilt) return;

            // Initialize all caches
            _cachedUnitSymbols = new List<string>();
            _cachedBaseUnits = new List<UnitDefinition>();
            _cachedBaseUnitsByFamily = new Dictionary<UnitFamilyName, UnitDefinition>();
            _cachedUnitsByFamily = new Dictionary<UnitFamilyName, List<UnitDefinition>>();
            _cachedSymbolToFamily = new Dictionary<string, UnitFamilyName>();

            // Single loop to build all caches simultaneously
            foreach (var unit in UnitDefinitions)
            {
                // Build symbol cache
                _cachedUnitSymbols.Add(unit.Symbol);

                // Build base units cache
                if (unit.IsBaseUnit)
                {
                    _cachedBaseUnits.Add(unit);
                    _cachedBaseUnitsByFamily[unit.Family] = unit;
                }

                // Build units by family cache
                if (!_cachedUnitsByFamily.ContainsKey(unit.Family))
                {
                    _cachedUnitsByFamily[unit.Family] = new List<UnitDefinition>();
                }
                _cachedUnitsByFamily[unit.Family].Add(unit);

                // Build symbol to family mapping cache
                _cachedSymbolToFamily[unit.Symbol] = unit.Family;
            }

            // Build unit groups cache using the already-built caches
            _cachedUnitGroups = new Dictionary<UnitFamilyName, UnitGroup>();
            foreach (var family in _cachedUnitsByFamily.Keys)
            {
                if (_cachedBaseUnitsByFamily.TryGetValue(family, out var baseUnit))
                {
                    var members = _cachedUnitsByFamily[family];
                    
                    // Parser accessibility and alternative family rules:
                    // Parser-accessible families can be used directly (e.g., "5m")
                    // Non-parser-accessible families require functions (e.g., ASDISTANCE())
                    // Alternative families are bidirectional relationships for compatible operations
                    var (isParserAccessible, alternativeFamily) = family switch
                    {
                        UnitFamilyName.Length => (true, UnitFamilyName.Distance),   // Length ↔ Distance
                        UnitFamilyName.Distance => (false, UnitFamilyName.Length), // Distance ↔ Length
                        UnitFamilyName.Angle => (true, UnitFamilyName.Bearing),    // Angle ↔ Bearing  
                        UnitFamilyName.Bearing => (false, UnitFamilyName.Angle),   // Bearing ↔ Angle
                        UnitFamilyName.Time => (false, (UnitFamilyName?)null),     // Function-only, no alternative
                        _ => (true, (UnitFamilyName?)null)                          // Most families are parser-accessible
                    };
                    
                    _cachedUnitGroups[family] = new UnitGroup(family, SystemType, baseUnit, members, isParserAccessible, alternativeFamily);
                }
            }

            _allCachesBuilt = true;
        }

        /// <summary>
        /// Gets all unit symbols in the system (cached for performance)
        /// </summary>
        public virtual List<string> GetAllUnitSymbols()
        {
            EnsureAllCachesBuilt();
            return _cachedUnitSymbols!;
        }

        /// <summary>
        /// Gets all base units in the system (cached for performance)
        /// </summary>
        public virtual List<UnitDefinition> GetAllBaseUnits()
        {
            EnsureAllCachesBuilt();
            return _cachedBaseUnits!;
        }

        /// <summary>
        /// Gets unit groups organized by family, with each group containing a base unit and all related units
        /// Enhanced with parser accessibility configuration for two-tier family system
        /// </summary>
        public virtual Dictionary<UnitFamilyName, UnitGroup> GetUnitGroups()
        {
            EnsureAllCachesBuilt();
            return _cachedUnitGroups!;
        }

        /// <summary>
        /// Gets a dictionary mapping each unit family to its base unit (cached for performance)
        /// </summary>
        public virtual Dictionary<UnitFamilyName, UnitDefinition> GetBaseUnitsByFamily()
        {
            EnsureAllCachesBuilt();
            return _cachedBaseUnitsByFamily!;
        }

        /// <summary>
        /// Gets a dictionary mapping each unit family to all units in that family (cached for performance)
        /// </summary>
        public virtual Dictionary<UnitFamilyName, List<UnitDefinition>> GetAllUnitsByFamily()
        {
            EnsureAllCachesBuilt();
            return _cachedUnitsByFamily!;
        }

        /// <summary>
        /// Gets a dictionary mapping each unit symbol to its unit family (cached for performance)
        /// </summary>
        public virtual Dictionary<string, UnitFamilyName> GetSymbolToFamilyMap()
        {
            EnsureAllCachesBuilt();
            return _cachedSymbolToFamily!;
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
            _allCachesBuilt = false;
        }
    }
}