using System;

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
        /// Get the unit system this factory uses
        /// </summary>
        public IUnitSystem UnitSystem => _unitSystem;

        /// <summary>
        /// Get a specific UnitGroup from the current unit system configuration
        /// </summary>
        public UnitGroup GetUnitGroup(UnitFamilyName family)
        {
            return _unitSystem.GetUnitGroup(family);
        }

        // Clean factory methods - just two methods handle everything through reflection

        /// <summary>
        /// Create a generic MeasuredValue for any unit family (base class only)
        /// NOTE: This creates base MeasuredValue objects, not specific derived types
        /// For parser integration, use CreateTypedMeasuredValue() instead
        /// </summary>
        public MeasuredValue CreateMeasuredValue(UnitFamilyName family, double value = 0, string? units = null)
        {
            var unitGroup = _unitSystem.GetUnitGroup(family);
            var measuredValue = new MeasuredValue(unitGroup);
            var defaultUnit = units ?? unitGroup.BaseUnit.Symbol;
            measuredValue.Init(value, defaultUnit);
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
            var unitGroup = _unitSystem.GetUnitGroup(family);

            // Use attribute-based registry to create the correct derived type
            var instance = UnitTypeRegistry.CreateInstance(family, unitGroup);
            if (instance == null)
            {
                // Fallback to Dimensionless if no specific type is registered
                instance = new Dimensionless(unitGroup);
            }
            
            var defaultUnit = units ?? unitGroup.BaseUnit.Symbol;
            instance.Init(value, defaultUnit);
            return instance;
        }


        public T CreateUnit<T>(double value = 0, string? units = null) where T : MeasuredValue
        {
            var type = typeof(T);
            var attribute = UnitTypeRegistry.GetAttributeForType(type);
            if (attribute == null)
                throw new ArgumentException($"Type {type.Name} is not registered with UnitTypeAttribute");

            var unitGroup = _unitSystem.GetUnitGroup(attribute.Family);

            // Use reflection to invoke the constructor with UnitGroup parameter
            var constructor = type.GetConstructor(new[] { typeof(UnitGroup) });
            if (constructor == null)
                throw new InvalidOperationException($"Type {type.Name} does not have a constructor with UnitGroup parameter");

            var instance = (T)constructor.Invoke(new object[] { unitGroup });
            var defaultUnit = units ?? unitGroup.BaseUnit.Symbol;
            instance.Init(value, defaultUnit);
            return instance;
        }


        // No static factory methods - use proper dependency injection instead
        // Example: var factory = new UnitFactory(myUnitSystem);
        // This respects the architectural principle of dependency injection and testability
    }
}