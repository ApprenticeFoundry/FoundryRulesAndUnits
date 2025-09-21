using System;

namespace FoundryRulesAndUnits.Units
{
    /// <summary>
    /// Attribute to mark unit types with their intrinsic metadata for factory creation
    /// This enables reflection-based factory methods to create correct derived types
    /// without hardcoded switch statements. Default units are system-dependent and
    /// should be handled by unit system specifications, not this attribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class UnitTypeAttribute : Attribute
    {
        /// <summary>
        /// The unit family this type represents
        /// </summary>
        public UnitFamilyName Family { get; }
        
        /// <summary>
        /// The primary constructor signature to use for factory creation
        /// Default is "UnitGroup" (constructor that takes only UnitGroup parameter)
        /// </summary>
        public string ConstructorSignature { get; set; } = "UnitGroup";
        
        /// <summary>
        /// Optional: Human-readable description of this unit type
        /// </summary>
        public string? Description { get; set; }
        
        /// <summary>
        /// Whether this is a derived unit (calculated from other units)
        /// vs a fundamental unit (base measurement)
        /// </summary>
        public bool IsDerived { get; set; } = false;
        
        /// <summary>
        /// Initialize unit type attribute with required family
        /// </summary>
        /// <param name="family">The unit family this type represents</param>
        public UnitTypeAttribute(UnitFamilyName family)
        {
            Family = family;
        }
    }
}