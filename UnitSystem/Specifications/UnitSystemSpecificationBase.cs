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
        private volatile bool _allCachesBuilt = false;
        private readonly object _cacheBuildLock = new();

        /// <summary>
        /// Ensures all caches are built in a single pass over UnitDefinitions for optimal performance.
        /// This replaces 6 separate loops with one O(n) operation.
        ///
        /// Thread-safety (bug 032): this instance is shared by every consumer of the
        /// process-wide unit service, including concurrent agents. The caches are built
        /// into locals and published only when complete, under a lock — concurrent first
        /// readers used to see a half-built dictionary and report valid units
        /// ("Unknown unit: m") as unknown.
        /// </summary>
        private void EnsureAllCachesBuilt()
        {
            if (_allCachesBuilt) return;

            lock (_cacheBuildLock)
            {
                if (_allCachesBuilt) return;

                var unitSymbols       = new List<string>();
                var baseUnits         = new List<UnitDefinition>();
                var baseUnitsByFamily = new Dictionary<UnitFamilyName, UnitDefinition>();
                var unitsByFamily     = new Dictionary<UnitFamilyName, List<UnitDefinition>>();
                var symbolToFamily    = new Dictionary<string, UnitFamilyName>();

                // Single loop to build all caches simultaneously
                foreach (var unit in UnitDefinitions)
                {
                    // Build symbol cache
                    unitSymbols.Add(unit.Symbol);

                    // Build base units cache
                    if (unit.IsBaseUnit)
                    {
                        baseUnits.Add(unit);
                        baseUnitsByFamily[unit.Family] = unit;
                    }

                    // Build units by family cache
                    if (!unitsByFamily.ContainsKey(unit.Family))
                    {
                        unitsByFamily[unit.Family] = new List<UnitDefinition>();
                    }
                    unitsByFamily[unit.Family].Add(unit);

                    // Build symbol to family mapping cache
                    symbolToFamily[unit.Symbol] = unit.Family;
                }

                // Build unit groups cache using the already-built caches
                var unitGroups = new Dictionary<UnitFamilyName, UnitGroup>();
                foreach (var family in unitsByFamily.Keys)
                {
                    if (baseUnitsByFamily.TryGetValue(family, out var baseUnit))
                    {
                        var members = unitsByFamily[family];

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
                            // WorkTime (a business day = 8 hr) is nominal — it has no dimensional
                            // signature and never composes. While it was parser-accessible it also
                            // CLAIMED the bare symbol `hr` (last group registered wins), so every
                            // `|hr` parameter became WorkTime and hr × EUR/hr could not compose.
                            // Function-only since 2026-08-30: a bare `hr` is Duration (coherent,
                            // seconds-based); a work-week model asks for WorkTime explicitly.
                            UnitFamilyName.WorkTime => (false, (UnitFamilyName?)null),
                            _ => (true, (UnitFamilyName?)null)                          // Most families are parser-accessible
                        };

                        unitGroups[family] = new UnitGroup(family, SystemType, baseUnit, members, isParserAccessible, alternativeFamily);
                    }
                }

                // Publish complete caches only — no reader ever sees a partial build.
                _cachedUnitSymbols       = unitSymbols;
                _cachedBaseUnits         = baseUnits;
                _cachedBaseUnitsByFamily = baseUnitsByFamily;
                _cachedUnitsByFamily     = unitsByFamily;
                _cachedSymbolToFamily    = symbolToFamily;
                _cachedUnitGroups        = unitGroups;

                _allCachesBuilt = true;   // volatile write last — release-publishes the caches
            }
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
            lock (_cacheBuildLock)
            {
                _allCachesBuilt = false;
                _cachedUnitGroups = null;
                _cachedUnitSymbols = null;
                _cachedBaseUnits = null;
                _cachedBaseUnitsByFamily = null;
                _cachedUnitsByFamily = null;
                _cachedSymbolToFamily = null;
            }
        }
    }
}