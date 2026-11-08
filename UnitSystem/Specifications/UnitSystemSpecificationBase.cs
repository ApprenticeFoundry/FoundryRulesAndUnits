using System;
using System.Collections.Generic;
using System.Linq;
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

        /// <summary>The units this system states for itself: its base units, and what only it has.</summary>
        protected abstract IReadOnlyList<UnitDefinition> OwnDefinitions { get; }

        private IReadOnlyList<UnitDefinition>? _unitDefinitions;

        /// <summary>
        /// Every unit this system can read. MKS is the one full list; another system is that list restated on its own base units, so a model means
        /// the same thing whichever base the host chose, and a family this system never declared (seconds, once) is there rather than missing.
        /// </summary>
        public IReadOnlyList<UnitDefinition> UnitDefinitions =>
            _unitDefinitions ??= SystemType == UnitSystemType.MKS ? OwnDefinitions : RestatedOnOwnBaseUnits();

        // A unit whose own definition differs from MKS by more than this means something else (ton: 2000 lb here, 1000 kg there), and keeps its meaning.
        // Below it, the difference is a rounded constant, and the systems must not disagree over rounding.
        private const double DifferentMeaning = 1e-3;

        private IReadOnlyList<UnitDefinition> RestatedOnOwnBaseUnits()
        {
            var own = OwnDefinitions.GroupBy(u => u.Family).ToDictionary(g => g.Key, g => g.ToList());
            var all = new List<UnitDefinition>();

            // In MKS order: a bare symbol two families register resolves as it does in MKS.
            foreach (var family in new MKSUnitSystemSpecification().OwnDefinitions.GroupBy(u => u.Family))
            {
                var ownUnits = own.GetValueOrDefault(family.Key);
                var ownBase = ownUnits?.FirstOrDefault(u => u.IsBaseUnit);
                if (ownBase is null) { all.AddRange(family); own.Remove(family.Key); continue; }

                var (toMks, fromMks) = BridgeToMks(ownBase, ownUnits!, family.ToList());
                all.Add(ownBase);
                foreach (var unit in family.Where(u => u.Symbol != ownBase.Symbol))
                {
                    var restated = UnitDefinition.DerivedUnit(unit.Symbol, unit.Name, unit.Family,
                        x => fromMks(unit.ToBaseUnit!(x)), x => unit.FromBaseUnit!(toMks(x)), unit.UnicodeSymbol);
                    var stated = ownUnits!.FirstOrDefault(u => u.Symbol == unit.Symbol);
                    all.Add(stated is not null && MeansSomethingElse(stated, restated) ? stated : restated);
                }
                all.AddRange(ownUnits!.Where(u => !u.IsBaseUnit && family.All(m => m.Symbol != u.Symbol)));
                own.Remove(family.Key);
            }

            all.AddRange(own.Values.SelectMany(units => units));   // families MKS does not have
            return all;
        }

        /// <summary>Between this system's base unit and the MKS base unit of the family: by MKS's own definition when it has one, or by this system's.</summary>
        private static (Func<double, double> ToMks, Func<double, double> FromMks) BridgeToMks(UnitDefinition ownBase, List<UnitDefinition> ownUnits, List<UnitDefinition> mksUnits)
        {
            var known = mksUnits.FirstOrDefault(u => u.Symbol == ownBase.Symbol);
            if (known is not null) return (known.ToBaseUnit!, known.FromBaseUnit!);

            var mksBase = mksUnits.First(u => u.IsBaseUnit).Symbol;
            var stated = ownUnits.FirstOrDefault(u => u.Symbol == mksBase)
                ?? throw new InvalidOperationException($"{ownBase.Family}: neither {ownBase.Symbol} nor {mksBase} is defined in terms of the other.");
            return (stated.FromBaseUnit!, stated.ToBaseUnit!);
        }

        private static bool MeansSomethingElse(UnitDefinition a, UnitDefinition b)
        {
            var (x, y) = (a.ToBaseUnit!(1.0), b.ToBaseUnit!(1.0));
            return Math.Abs(x - y) > DifferentMeaning * Math.Max(Math.Abs(x), Math.Abs(y));
        }

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