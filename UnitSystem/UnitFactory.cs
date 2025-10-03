using System;
using System.Linq;

namespace FoundryRulesAndUnits.Units
{
    /// <summary>
    /// Clean, simple factory service for creating MeasuredValue instances with proper UnitGroup injection
    /// Uses IUnitSystem interface to get current unit specifications - no hardcoded dependencies
    /// Follows the architectural principle: Factory takes unit system, gets current config, creates objects
    /// </summary>
    public class UnitFactory
    {
        private readonly IUnitSystem _unitSystem;

        /// <summary>
        /// Create a factory that uses the provided unit system for all operations
        /// This is the clean, architectural approach - one dependency, all configuration comes through IUnitSystem
        /// </summary>
        public UnitFactory(IUnitSystem unitSystem)
        {
            _unitSystem = unitSystem ?? throw new ArgumentNullException(nameof(unitSystem));
        }

        /// <summary>
        /// Create a factory with the specified unit system type (compatibility constructor)
        /// This constructor provides backward compatibility with existing code
        /// </summary>
        public UnitFactory(UnitSystemType systemType)
        {
            _unitSystem = new UnitSystem(systemType);
        }

        /// <summary>
        /// Get the unit system this factory uses
        /// </summary>
        public IUnitSystem UnitSystem => _unitSystem;

        /// <summary>
        /// Get the system type for compatibility with existing caching logic
        /// </summary>
        public UnitSystemType SystemType => _unitSystem.ActiveType;

        // Clean factory methods - leverage existing IUnitSystem functionality

        /// <summary>
        /// Create a generic MeasuredValue for any unit family (base class only)
        /// NOTE: This creates base MeasuredValue objects, not specific derived types
        /// For parser integration, use CreateTypedMeasuredValue() instead
        /// </summary>
        public MeasuredValue CreateMeasuredValue(UnitFamilyName family, double value = 0, string? units = null)
        {
            // Create UnitGroup directly to avoid circular dependency
            var unitGroup = CreateUnitGroupForFamily(family);
            var measuredValue = new MeasuredValue(unitGroup);
            
            // Initialize with the provided value and units
            var baseUnit = _unitSystem.GetBaseUnitForFamily(family);
            var finalUnit = units ?? baseUnit;
            measuredValue.Init(value, finalUnit);
            
            return measuredValue;
        }

        /// <summary>
        /// Create the correct derived MeasuredValue type using attribute-based reflection
        /// This is CRITICAL for parser integration that expects specific types (Angle, Length, Mass, etc.)
        /// Uses UnitTypeAttribute metadata and current unit system configuration
        /// ARCHITECTURAL PRINCIPLE: Factory gets current config from IUnitSystem, creates appropriate objects
        /// </summary>
        public MeasuredValue CreateTypedMeasuredValue(UnitFamilyName family, double value = 0, string? units = null)
        {
            // Create UnitGroup from current unit system specification
            var unitGroup = CreateUnitGroupForFamily(family);
            
            // Use attribute-based registry to create the correct derived type
            var instance = UnitTypeRegistry.CreateInstance(family, unitGroup);
            if (instance == null)
            {
                // Fallback to creating via the unit system
                return CreateMeasuredValue(family, value, units);
            }
            
            // Initialize the instance using the unit system's conversion capabilities
            var baseUnit = _unitSystem.GetBaseUnitForFamily(family);
            var finalUnit = units ?? baseUnit;
            
            // CRITICAL FIX: Always pass the original units parameter to preserve display units
            // The Init method will handle conversion to base units internally while preserving U field
            instance.Init(value, units);
            
            return instance;
        }

        /// <summary>
        /// Create a UnitGroup for the specified family using current unit system configuration
        /// This is the bridge between the unit system and the unit type constructors
        /// </summary>
        private UnitGroup CreateUnitGroupForFamily(UnitFamilyName family)
        {
            var allUnitsByFamily = _unitSystem.GetAllUnitsByFamily();
            if (!allUnitsByFamily.TryGetValue(family, out var units))
            {
                throw new ArgumentException($"No units found for family {family} in current unit system");
            }

            var baseUnitSymbol = _unitSystem.GetBaseUnitForFamily(family);
            var baseUnit = units.FirstOrDefault(u => u.Symbol == baseUnitSymbol);
            if (baseUnit == null)
            {
                throw new InvalidOperationException($"Base unit {baseUnitSymbol} not found for family {family}");
            }

            return new UnitGroup(family, _unitSystem.ActiveType, baseUnit, units);
        }


        /// <summary>
        /// Create a strongly typed unit object with compile-time type safety
        /// Uses the optimized CreateTypedMeasuredValue() path with cached reflection
        /// PERFORMANCE: Leverages UnitTypeRegistry cache instead of expensive constructor lookup
        /// </summary>
        public T CreateUnit<T>(double value = 0, string? units = null) where T : MeasuredValue
        {
            var type = typeof(T);
            var attribute = UnitTypeRegistry.GetAttributeForType(type);
            if (attribute == null)
                throw new ArgumentException($"Type {type.Name} is not registered with UnitTypeAttribute");

            // Use the optimized cached path instead of duplicating reflection logic!
            var instance = CreateTypedMeasuredValue(attribute.Family, value, units);
            
            // Safe cast since UnitTypeRegistry guarantees correct type for family
            return (T)instance;
        }


        // No static factory methods - use proper dependency injection instead
        // Example: var factory = new UnitFactory(myUnitSystem);
        // This respects the architectural principle of dependency injection and testability
    }
}