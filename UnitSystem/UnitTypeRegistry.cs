using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FoundryRulesAndUnits.Units
{
    /// <summary>
    /// ESSENTIAL performance-critical registry for attribute-based unit type discovery
    /// Caches reflection results to avoid expensive type scanning on every factory call
    /// WITHOUT this caching, CreateInstance() would be very slow (scanning all types repeatedly)
    /// </summary>
    public static class UnitTypeRegistry
    {
        private static Dictionary<UnitFamilyName, Type>? _familyToTypeMap;
        private static Dictionary<Type, UnitTypeAttribute>? _typeToAttributeMap;
        
        /// <summary>
        /// CRITICAL: Get the UnitType attribute for a specific type (cached for performance)
        /// Used by MeasuredValue.UnitFamily and UnitSystem.CreateUnit<T>()
        /// </summary>
        public static UnitTypeAttribute? GetAttributeForType(Type type)
        {
            EnsureMapsBuilt();
            return _typeToAttributeMap!.TryGetValue(type, out var attr) ? attr : null;
        }
        
        /// <summary>
        /// CRITICAL: Create an instance of the correct unit type for the given family (cached for performance)
        /// Used by UnitSystem.CreateTypedMeasuredValue() - this is why the registry exists!
        /// Without caching, we'd scan all types on every factory call (very expensive)
        /// </summary>
        public static MeasuredValue? CreateInstance(UnitFamilyName family, UnitGroup unitGroup)
        {
            EnsureMapsBuilt();
            if (!_familyToTypeMap!.TryGetValue(family, out var type)) 
                return null;
            
            // All unit types use UnitGroup constructor (enforced by gold standard)
            return (MeasuredValue?)Activator.CreateInstance(type, unitGroup);
        }
        
        /// <summary>
        /// Build the performance-critical reflection maps by scanning for UnitType attributes
        /// This expensive operation happens ONCE at startup, then results are cached
        /// </summary>
        private static void EnsureMapsBuilt()
        {
            if (_familyToTypeMap != null && _typeToAttributeMap != null)
                return;
                
            _familyToTypeMap = new Dictionary<UnitFamilyName, Type>();
            _typeToAttributeMap = new Dictionary<Type, UnitTypeAttribute>();
            
            // Scan the current assembly for classes with UnitType attributes
            var assembly = Assembly.GetExecutingAssembly();
            var unitTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(MeasuredValue)))
                .Where(t => t.GetCustomAttribute<UnitTypeAttribute>() != null);
            
            foreach (var type in unitTypes)
            {
                var attribute = type.GetCustomAttribute<UnitTypeAttribute>()!;
                
                // Build both mappings for fast lookup
                _familyToTypeMap[attribute.Family] = type;
                _typeToAttributeMap[type] = attribute;
            }
        }
    }
}