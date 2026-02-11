# Unit System Architecture Guide

## Overview

The FoundryRulesAndUnits unit system implements a revolutionary **hub-and-spoke architecture** that eliminates the traditional N² complexity of unit conversions while providing type-safe, performant, and extensible unit management across multiple engineering domains.

**Current Version**: 9.1.0 targeting .NET 9.0  
**Architecture Pattern**: UnitGroup-based dependency injection with MeasuredValue base classes  
**Key Innovation**: Two-tier unit family system eliminating parser ambiguity

## 🏗️ Core Architecture Principles

### Hub-and-Spoke Conversion Model
Instead of maintaining N² conversion matrices, each unit knows how to convert to/from its base unit:

```csharp
// Traditional N² approach (BAD):
Dictionary<(string from, string to), double> conversions = {
    ("ft", "in") => 12.0,
    ("in", "ft") => 1/12.0,
    ("ft", "m") => 0.3048,
    ("m", "ft") => 1/0.3048,
    // ... grows exponentially!
};

// Modern UnitGroup approach (GOOD):
// Each unit family has a UnitGroup with base unit and conversion definitions
var lengthGroup = new UnitGroup(UnitFamilyName.Length, baseUnit, members, systemType);
// All conversions: Value → Base Unit → Target Unit (max 2 operations)
```

### Performance Through UnitGroup Injection and Caching
All unit systems implement identical caching patterns for O(1) lookups:

```csharp
// Modern MeasuredValue with UnitGroup injection
public class MeasuredValue
{
    protected UnitGroup _unitGroup; // Injected dependency
    
    public MeasuredValue(UnitGroup unitGroup)
    {
        _unitGroup = unitGroup ?? throw new ArgumentNullException(nameof(unitGroup));
        // All conversion logic centralized in UnitGroup
    }
    
    public virtual double As(string units)
    {
        return _unitGroup.Convert(V, I, units); // O(1) with caching
    }
}

// UnitSystem provides O(1) cached lookups
private Dictionary<string, UnitLookupInfo>? _cachedUnitLookup = null;

public Dictionary<string, UnitLookupInfo> GetUnitLookup()
{
    if (_cachedUnitLookup == null)  // Lazy evaluation
    {
        _cachedUnitLookup = BuildUnitLookupCache();
    }
    return _cachedUnitLookup;  // O(1) lookups after initialization
}
```

## 📋 Current Interface Architecture

The unified interface pattern provides complete unit system functionality:

```csharp
public interface IUnitSystem
{
    // Core system management
    IUnitSystemSpecification Current { get; }
    UnitSystemType ActiveType { get; }
    IUnitSystemSpecification Apply(UnitSystemType systemType);

    // Conversion and validation
    double Convert(double value, string fromUnit, string toUnit);
    bool IsValidUnit(string unit);
    bool IsValidUnit(string unit, UnitFamilyName family);

    // Efficient lookup services
    UnitFamilyName GetUnitFamily(string unit);
    bool TryGetUnitInfo(string unit, out UnitLookupInfo? unitInfo);

    // Modern creation methods with type safety
    T CreateUnit<T>(double value = 0, string? units = null) where T : MeasuredValue;
    MeasuredValue CreateMeasuredValue(UnitFamilyName family, double value = 0, string? units = null);
    
    // Specific typed creation methods
    Length CreateLength(double value = 0, string? units = null);
    Angle CreateAngle(double value = 0, string? units = null);
    Mass CreateMass(double value = 0, string? units = null);
    Temperature CreateTemperature(double value = 0, string? units = null);
    // ... and more
    
    // Static convenience methods
    static IUnitSystem MKS() => new UnitSystem(UnitSystemType.MKS);
    static IUnitSystem SI() => new UnitSystem(UnitSystemType.SI);
    static IUnitSystem FPS() => new UnitSystem(UnitSystemType.FPS);
    // ... and more
}
```

## 🎯 Available Unit Systems

### 1. **MKS (Meter-Kilogram-Second)**
- **Base Units**: meters, kilograms, seconds, Celsius, radians, newtons
- **Target**: General engineering applications
- **Key Features**: Standard metric base units

### 2. **SI (International System)**
- **Base Units**: meters, kilograms, seconds, Kelvin, amperes, moles, candela
- **Target**: Scientific applications with complete SI coverage
- **Key Features**: All 7 SI base units, electrical, radioactivity, dose units

### 3. **CGS (Centimeter-Gram-Second)**
- **Base Units**: centimeters, grams, seconds, Celsius, degrees, dynes
- **Target**: Laboratory and scientific precision work
- **Key Features**: Uses dynes (force), barye (pressure), ergs (energy)

### 4. **FPS (Foot-Pound-Second)**
- **Base Units**: feet, pounds, seconds, Fahrenheit, degrees
- **Target**: Structural and civil engineering
- **Key Features**: Uses psf (pressure), optimized for construction

### 5. **IPS (Inch-Pound-Second)**
- **Base Units**: inches, pounds, seconds, Fahrenheit, degrees
- **Target**: Imperial engineering applications
- **Key Features**: Inch-based measurements for precision machining

### 6. **mmNs (Millimeter-Newton-Second)**
- **Base Units**: millimeters, newtons, seconds, Celsius, degrees
- **Target**: Precision mechanical engineering and CAD
- **Key Features**: kPa pressure, mJ energy scales for mechanical design

## 🚀 Modern Usage Patterns

### Basic Unit Creation (Recommended)
```csharp
// Create unit system 
var unitSystem = IUnitSystem.MKS(); // or SI(), FPS(), etc.

// Type-safe creation with compile-time checking
Length distance = unitSystem.CreateLength(100, "cm");  // Returns Length object
Angle rotation = unitSystem.CreateAngle(90, "deg");    // Returns Angle object
Mass weight = unitSystem.CreateMass(5.5, "kg");        // Returns Mass object

// Generic creation for parser scenarios
MeasuredValue parsed = unitSystem.CreateMeasuredValue(UnitFamilyName.Length, 100, "cm");
```

### Unit System Switching
```csharp
// Same API works with any unit system
var mksSystem = IUnitSystem.MKS();    // meters, kg, seconds
var fpsSystem = IUnitSystem.FPS();    // feet, pounds, seconds  
var siSystem = IUnitSystem.SI();      // full scientific SI

// Switch systems dynamically
var engineeringCalc = new Calculator(mksSystem);
var scientificCalc = new Calculator(siSystem);
```

### Mathematical Operations with Type Safety
```csharp
// Strongly-typed mathematical operations
Length width = unitSystem.CreateLength(5, "m");
Length height = unitSystem.CreateLength(3, "m");
var area = width * height;  // Returns Area object automatically

Mass mass = unitSystem.CreateMass(100, "kg");
var acceleration = unitSystem.CreateAcceleration(9.8, "m/s2");
var force = mass * acceleration;  // Returns Force object automatically

// Operations maintain unit consistency
Length total = unitSystem.CreateLength(100, "cm") + unitSystem.CreateLength(1, "m");
// Result: 2.0 meters (automatic unit conversion)
```

### Parser Integration Pattern
```csharp
public class UnitParser
{
    private readonly IUnitSystem _unitSystem;
    
    public UnitParser(IUnitSystem unitSystem)
    {
        _unitSystem = unitSystem;
    }
    
    public MeasuredValue ParseExpression(string input)
    {
        // Parse: "100 cm" → value=100, unit="cm"
        var (value, units) = ExtractValueAndUnit(input);
        
        // Create strongly-typed object automatically
        return _unitSystem.CreateMeasuredValueFromParsableUnit(units, value);
        // Returns: Length, Angle, Mass, etc. (correct derived type)
    }
}
```

### Multi-Domain Engineering Calculator
```csharp
public class EngineeringCalculator
{
    private IUnitSystem currentSystem;
    
    public void SwitchToSystem(string domain)
    {
        currentSystem = domain switch
        {
            "structural" => IUnitSystem.FPS(),     // feet, pounds, psi
            "precision"  => IUnitSystem.MKS(),     // mm, newtons, kPa  
            "laboratory" => IUnitSystem.CGS(),     // cm, grams, dynes
            "scientific" => IUnitSystem.SI(),      // complete SI
            "imperial"   => IUnitSystem.IPS(),     // inches, pounds
            _ => IUnitSystem.MKS()                 // default metric
        };
    }
    
    public MeasuredValue CalculateStress(Force force, Area area)
    {
        // Works with ANY unit system automatically!
        var forceValue = force.BaseValue();     // Get base unit value
        var areaValue = area.BaseValue();       // Get base unit value
        
        // Calculate in base units
        var stressValue = forceValue / areaValue;
        
        // Return pressure object in current system
        return currentSystem.CreateMeasuredValue(UnitFamilyName.Pressure, stressValue);
    }
}
```

### Type-Safe Unit Family Validation with UnitTypeRegistry
```csharp
// UnitFamilyName enum prevents runtime errors
public enum UnitFamilyName 
{
    Length, Mass, Force, Temperature, Time, Area, Volume, Speed, 
    Pressure, Energy, Power, Frequency, Voltage, Current,
    Angle, Distance, Duration, Bearing, // Two-tier system
    AmountOfSubstance, LuminousIntensity, // ... and more
}

// UnitTypeAttribute ensures compile-time correctness:
[UnitType(UnitFamilyName.Length, Description = "Length measurement")]
public class Length : MeasuredValue { ... }

[UnitType(UnitFamilyName.Mass, Description = "Mass measurement")]
public class Mass : MeasuredValue { ... }

// UnitTypeRegistry provides fast lookups:
var instance = UnitTypeRegistry.CreateInstance(UnitFamilyName.Length, unitGroup);
// Returns: Length object (correct derived type)
```

## 🔧 Integration Guidelines

### Modern Creation Pattern (Recommended)
```csharp
// Use IUnitSystem interface for all unit creation
public class Calculator
{
    private readonly IUnitSystem _unitSystem;
    
    public Calculator(IUnitSystem unitSystem)
    {
        _unitSystem = unitSystem ?? IUnitSystem.MKS(); // Default to MKS
    }
    
    public Force CalculateForce(double mass, double acceleration)
    {
        var massObj = _unitSystem.CreateMass(mass, "kg");
        var accelObj = _unitSystem.CreateAcceleration(acceleration, "m/s2");
        return massObj * accelObj; // Type-safe multiplication
    }
}
```

### Adding New Unit Types
1. **Create MeasuredValue derived class** with UnitTypeAttribute
2. **Register in UnitTypeRegistry** (automatic via attribute)
3. **Add to IUnitSystem interface** for direct creation
4. **Implement in UnitSystem class**
5. **Add to unit specifications** as needed

```csharp
[UnitType(UnitFamilyName.Torque, Description = "Torque measurement")]
public class Torque : MeasuredValue
{
    public Torque(UnitGroup unitGroup) : base(unitGroup)
    {
        if (unitGroup.Family != UnitFamilyName.Torque)
            throw new ArgumentException($"UnitGroup family must be {UnitFamilyName.Torque}");
    }
    
    // Implement required methods (Assign, Copy, operators)
    // Follow gold standard pattern from Length.cs and Angle.cs
}
```

### Extending Unit Families
Add new unit families to the `UnitFamilyName` enum:

```csharp
public enum UnitFamilyName 
{
    // Existing families...
    Length, Mass, Force, Temperature,
    
    // New families for your domain
    Torque, MomentOfInertia, Density, Viscosity
}
```

### Creating Conversion Utilities
```csharp
public static class UnitConversionExtensions
{
    public static double ConvertBetweenSystems(this MeasuredValue value, 
        IUnitSystem fromSystem, IUnitSystem toSystem)
    {
        // Get base value from source system
        var baseValue = value.BaseValue();
        var baseUnit = fromSystem.GetBaseUnitForFamily(value.UnitFamily);
        
        // Convert to target system
        var targetUnit = toSystem.GetBaseUnitForFamily(value.UnitFamily);
        return toSystem.Convert(baseValue, baseUnit, targetUnit);
    }
}
```

## 📊 Performance Characteristics

### Scalability Comparison
| Approach | Adding 1 Unit | Adding N Units | Lookup Performance | Memory Usage |
|----------|---------------|----------------|-------------------|--------------|
| Traditional Matrix | O(N) conversions | O(N²) conversions | O(1) | N² space |
| Hub-and-Spoke + Injection | O(1) definition | O(N) definitions | O(1) with caching | O(N) space |
| UnitGroup Pattern | O(1) definition | O(N) definitions | O(1) with injection | O(N) space |

### Modern Architecture Benefits
- **UnitGroup Injection**: Each MeasuredValue has direct access to conversion logic
- **UnitTypeRegistry Caching**: O(1) type creation via reflection cache
- **Lazy Evaluation**: Cached lookups initialized only when needed
- **Type Safety**: Compile-time checking prevents runtime errors

## 🎯 Best Practices

### 1. **Always Use IUnitSystem Interface**
```csharp
// Good - flexible and testable
public void ProcessUnits(IUnitSystem unitSystem) 
{
    var length = unitSystem.CreateLength(100, "cm");
}

// Bad - tightly coupled to specific implementation
public void ProcessUnits(UnitSystem unitSystem) { }
```

### 2. **Leverage Type-Safe Creation Methods**
```csharp
// Good - compile-time type safety
Length distance = unitSystem.CreateLength(100, "cm");
Angle rotation = unitSystem.CreateAngle(90, "deg");

// Acceptable for parser scenarios
MeasuredValue parsed = unitSystem.CreateMeasuredValue(family, value, units);

// Bad - no type safety
var distance = new Length(unitGroup); // Requires manual UnitGroup creation
```

### 3. **Use Static Factory Methods for Quick Setup**
```csharp
// Good - quick and readable
var engineeringSystem = IUnitSystem.MKS();
var scientificSystem = IUnitSystem.SI();
var imperialSystem = IUnitSystem.FPS();

// Also good - explicit configuration
var customSystem = new UnitSystem(UnitSystemType.CGS);
```

### 4. **Handle Unit Family Compatibility**
```csharp
// Use built-in compatibility checking
public bool CanAdd(MeasuredValue a, MeasuredValue b)
{
    return a.IsCompatibleWith(b); // Handles Length + Distance, Time + Duration, etc.
}

// Use safe mathematical operations
public MeasuredValue AddSafely(MeasuredValue a, MeasuredValue b, IUnitSystem unitSystem)
{
    return a.AddCompatible(b, unitSystem); // Throws on incompatible types
}
```

## 🔄 Integration with Modern Architecture

### Parser Integration (Zero-Ambiguity Pattern)
```csharp
// Two-tier system eliminates parser ambiguity
public class ModernParser
{
    private readonly IUnitSystem _unitSystem;
    
    public MeasuredValue Parse(string input)
    {
        var (value, unit) = ExtractValueAndUnit(input); // "100 cm" → 100, "cm"
        
        // Direct creation - no ambiguity!
        // "cm" always maps to Length (parser-accessible)
        // Distance requires explicit ASDISTANCE() function
        return _unitSystem.CreateMeasuredValueFromParsableUnit(unit, value);
    }
}
```

### Calculation Engine Integration
```csharp
// Modern calculation with automatic type handling
public class CalculationEngine
{
    private readonly IUnitSystem _unitSystem;
    
    public MeasuredValue Evaluate(string expression)
    {
        // Parse: "5m * 3m" → Length(5, "m"), Length(3, "m")
        var terms = ParseTerms(expression);
        
        // Mathematical operations return correct types automatically
        if (terms.Count == 2 && IsMultiplication(expression))
        {
            var result = terms[0] * terms[1]; // Length * Length = Area
            return result; // Returns Area object
        }
        
        return terms[0];
    }
}
```

### Validation and Error Handling
```csharp
public class UnitValidator
{
    private readonly IUnitSystem _unitSystem;
    
    public ValidationResult ValidateExpression(string expression)
    {
        var errors = new List<string>();
        var units = ExtractUnits(expression);
        
        foreach(var unit in units)
        {
            if (!_unitSystem.IsValidUnit(unit))
                errors.Add($"Unknown unit: {unit}");
        }
        
        return new ValidationResult(errors.Count == 0, errors);
    }
    
    public bool ValidateMathematicalOperation(MeasuredValue a, MeasuredValue b, string operation)
    {
        return operation switch
        {
            "+" or "-" => a.IsCompatibleWith(b), // Length + Distance = OK
            "*" or "/" => true, // Cross-family operations allowed
            _ => false
        };
    }
}
```

## 🚀 Future Enhancements

### 1. **Dynamic Unit System Configuration**
```csharp
public class UnitSystemConfiguration
{
    public static IUnitSystem LoadFromJson(string configPath)
    {
        // Load unit definitions from configuration
        // Generate unit system at runtime with custom base units
    }
}
```

### 2. **Enhanced Cross-Family Operations**
```csharp
// Already implemented in current version:
// Length × Length → Area
// Mass × Acceleration → Force  
// Length ÷ Time → Speed

// Future enhancements:
// Volume × Density → Mass
// Force × Distance → Energy
// Power × Time → Energy
```

### 3. **AI-Powered Unit Inference**
```csharp
public class IntelligentUnitParser
{
    public MeasuredValue ParseWithContext(string input, string context)
    {
        // "5" in context "distance to target" → infer Length
        // "5" in context "rotation angle" → infer Angle
        // Use machine learning for context-aware parsing
    }
}
```

## 📝 Summary

The FoundryRulesAndUnits unit system architecture (v9.1.0) provides:

- **Modern Architecture**: UnitGroup injection pattern with MeasuredValue base classes
- **Type Safety**: UnitTypeRegistry with compile-time and runtime type checking
- **Zero Ambiguity**: Two-tier unit family system for parser integration
- **Performance**: O(1) lookups with lazy caching and reflection optimization
- **Flexibility**: Drop-in unit system replacement via IUnitSystem interface
- **Maintainability**: Single source of truth per unit family with hub-and-spoke conversions
- **Extensibility**: Easy addition of new units and mathematical operations
- **Cross-Family Operations**: Automatic type inference (Length × Length → Area)

The hub-and-spoke model with UnitGroup injection and interface-based design creates a robust foundation for building sophisticated engineering calculation tools while maintaining simplicity, performance, and type safety.

**Key Achievement**: Seamless integration of parser requirements, mathematical operations, and unit system flexibility in a production-ready .NET 9.0 library.