# Unit System Architecture Guide

## Overview

The modern unit system architecture in FoundryRulesAndUnits v9.1.0 follows the pattern: **IUnitSystem provides unified interface, UnitSystem manages UnitGroups, UnitGroups inject into MeasuredValue objects**. This eliminates the need for separate factory classes while providing three distinct methods for creating strongly-typed unit objects with automatic base unit conversion.

## Core Architecture

### Unified Interface Pattern
```csharp
public interface IUnitSystem
{
    // Core system management
    IUnitSystemSpecification Current { get; }
    UnitSystemType ActiveType { get; }
    
    // Type-safe creation methods
    T CreateUnit<T>(double value = 0, string? units = null) where T : MeasuredValue;
    Length CreateLength(double value = 0, string? units = null);
    Angle CreateAngle(double value = 0, string? units = null);
    Mass CreateMass(double value = 0, string? units = null);
    // ... and more
    
    // Static convenience methods
    static IUnitSystem MKS() => new UnitSystem(UnitSystemType.MKS);
    static IUnitSystem SI() => new UnitSystem(UnitSystemType.SI);
    static IUnitSystem FPS() => new UnitSystem(UnitSystemType.FPS);
}
```

**Key Benefits:**
- ✅ Single interface for all unit operations
- ✅ Dependency injection friendly
- ✅ Type-safe creation with compile-time checking
- ✅ Consistent patterns across all unit types
- ✅ Testable and maintainable

## Three Creation Methods - Complete API Coverage

### 1. `CreateUnit<T>()` - Compile-Time Type Safety ⭐ (Recommended)
**Purpose:** End-user APIs with maximum type safety and performance

```csharp
public T CreateUnit<T>(double value = 0, string? units = null) where T : MeasuredValue
```

**Usage:**
```csharp
var system = IUnitSystem.MKS();
Length length = system.CreateUnit<Length>(100, "cm");
Angle angle = system.CreateUnit<Angle>(90, "deg");
Mass mass = system.CreateUnit<Mass>(5.5, "kg");

// No casting needed - exact type returned
Length result = length + system.CreateUnit<Length>(1, "m");
```

**🚀 Performance Optimization:** Uses cached UnitTypeRegistry for O(1) type creation
**Advantages:**
- ✅ **Compile-time type safety** - impossible to get wrong type
- ✅ **IntelliSense support** - shows exact methods available
- ✅ **Automatic family discovery** - derives UnitFamilyName from type
- ✅ **No casting required** - returns exact type specified
- ✅ **Clean API** - most readable and maintainable

### 2. `CreateMeasuredValue()` - Generic Family Creation
**Purpose:** Parser integration and dynamic type creation

```csharp
public MeasuredValue CreateMeasuredValue(UnitFamilyName family, double value = 0, string? units = null)
```

**Usage:**
```csharp
// Parser determines family at runtime from unit string  
MeasuredValue length = system.CreateMeasuredValue(UnitFamilyName.Length, 100, "cm");
// Returns: MeasuredValue (base class for generic algorithms)
// Use case: Generic algorithms, legacy compatibility
```

### 3. `CreateTypedMeasuredValue()` - Runtime Type Discovery
**Purpose:** Internal use for parser integration requiring derived types

```csharp
public MeasuredValue CreateTypedMeasuredValue(UnitFamilyName family, double value = 0, string? units = null)
```

**Usage:**
```csharp
// Internal method - creates correct derived type at runtime
MeasuredValue length = system.CreateTypedMeasuredValue(UnitFamilyName.Length, 100, "cm");
// Returns: Length object (strongly-typed, not base MeasuredValue)
// Use case: Parser integration requiring specific derived types
```

**Critical for Parser Integration:**
- Parser extracts value and units from user input
- Parser determines UnitFamilyName from unit lookup
- System creates correct derived type (Length, Angle, Mass, etc.)
- Mathematical operations work immediately

## Automatic Base Unit Conversion with UnitGroup Injection

All three methods automatically convert values to the correct base units for the current unit system:

```csharp
// MKS System (base unit: meters)
var mksSystem = IUnitSystem.MKS();
var mksLength = mksSystem.CreateLength(100, "cm");
// Internal storage: 1.0 meters
// UnitGroup injection: UnitGroup(UnitFamilyName.Length, baseUnit="m", systemType=MKS)

// FPS System (base unit: feet) 
var fpsSystem = IUnitSystem.FPS();
var fpsLength = fpsSystem.CreateLength(100, "cm");
// Internal storage: 3.28084 feet
// UnitGroup injection: UnitGroup(UnitFamilyName.Length, baseUnit="ft", systemType=FPS)

// SI System (base unit: meters)
var siSystem = IUnitSystem.SI();
var siLength = siSystem.CreateLength(100, "cm");
// Internal storage: 1.0 meters
// UnitGroup injection: UnitGroup(UnitFamilyName.Length, baseUnit="m", systemType=SI)
```

### UnitGroup Injection Pattern
```csharp
// Each MeasuredValue receives a UnitGroup containing all conversion logic
public class Length : MeasuredValue
{
    public Length(UnitGroup unitGroup) : base(unitGroup)
    {
        // _unitGroup contains all conversion methods for Length family
        // Automatically validates family matches
        // Provides access to base unit and conversion functions
    }
    
    public override double As(string units)
    {
        return _unitGroup.Convert(V, I, units); // Delegate to injected UnitGroup
    }
}
```

## Usage Patterns by Scenario

### End-User APIs (Recommended: Specific Create Methods)
```csharp
// Engineering calculations
public static Force CalculateForce(double massKg, double accelerationMs2, IUnitSystem unitSystem)
{
    var mass = unitSystem.CreateMass(massKg, "kg");
    var acceleration = unitSystem.CreateAcceleration(accelerationMs2, "m/s²");
    return mass * acceleration;  // Returns Force - type-safe!
}

// Configuration and setup
var system = IUnitSystem.MKS();
var tolerance = system.CreateLength(0.1, "mm");
var maxSpeed = system.CreateSpeed(100, "mph");
var temperature = system.CreateTemperature(25, "°C");
```

### Parser Integration (Use `CreateMeasuredValueFromParsableUnit()`)
```csharp
// Parser processes user input: "100 cm + 1 m"
public MeasuredValue ParseExpression(string input, IUnitSystem unitSystem)
{
    var (value, units) = ExtractComponents(input);  // Parser logic
    
    // Creates correct derived type automatically
    return unitSystem.CreateMeasuredValueFromParsableUnit(units, value);
    // Returns: Length object ready for mathematical operations
}

// Ultimate integration test  
var left = system.CreateMeasuredValueFromParsableUnit("cm", 100);   // Length
var right = system.CreateMeasuredValueFromParsableUnit("m", 1);     // Length  
var result = (Length)left + (Length)right;  // 2 meters
```

### Generic Algorithms (Use `CreateMeasuredValue()`)
```csharp
// Generic algorithms that work with base MeasuredValue
public double ConvertValue(UnitFamilyName family, double value, string fromUnit, string toUnit, IUnitSystem unitSystem)
{
    var measured = unitSystem.CreateMeasuredValue(family, value, fromUnit);
    return measured.As(toUnit);
}
```

## Method Selection Guide

| Scenario | Method | Type Safety | Performance | Use When |
|----------|--------|-------------|-------------|----------|
| **End-User APIs** | `CreateLength()`, `CreateAngle()`, etc. | **Compile-time** | Best | You know the specific type needed |
| **Generic Creation** | `CreateUnit<T>()` | **Compile-time** | Best | You know the type at compile-time |
| **Parser Integration** | `CreateMeasuredValueFromParsableUnit()` | Runtime strong | Good | Parser determines type from unit string |
| **Legacy/Generic** | `CreateMeasuredValue()` | Base class only | Good | Working with base MeasuredValue class |

## Architecture Benefits

### 🎯 **Unified Interface Pattern**
- All unit operations through single IUnitSystem interface
- No separate factory classes to manage
- Easy to switch between unit systems
- Dependency injection friendly

### 🎯 **Type Safety Spectrum**
- `CreateMeasuredValue()`: Base class (legacy compatibility)
- `CreateMeasuredValueFromParsableUnit()`: Runtime strong typing (parser integration)
- `CreateLength()`, `CreateAngle()`, etc.: Compile-time strong typing (end-user APIs)
- `CreateUnit<T>()`: Generic compile-time strong typing (flexible APIs)

### 🎯 **Performance Optimized**
- Values stored in base units for zero-conversion mathematical operations
- UnitTypeRegistry caching eliminates reflection overhead
- UnitGroup injection provides O(1) conversion access
- Minimal object creation overhead

### 🎯 **Testability**
```csharp
// Easy to test with dependency injection
var mockUnitSystem = new Mock<IUnitSystem>();
var calculator = new Calculator(mockUnitSystem.Object);
// Test different unit system configurations
```

## Integration with Modern Architecture

### Parser → UnitSystem → Math Pipeline
```
Parser Input: "100 cm + 1 m"
    ↓
Parser extracts: (100, "cm"), (1, "m") 
    ↓
UnitSystem creates: Length(1.0 meters), Length(1.0 meters)
    ↓
Mathematical operation: 1.0 + 1.0 = 2.0 (in base units)
    ↓
Result: Length(2.0 meters) → Display as "2 m" or "200 cm"
```

### Unit System Switching
```csharp
// Same API works with different unit systems
var mksSystem = IUnitSystem.MKS();
var fpsSystem = IUnitSystem.FPS();

// Same code, different internal base units
var mksLength = mksSystem.CreateLength(100, "cm");    // 1.0 meters internally
var fpsLength = fpsSystem.CreateLength(100, "cm");    // 3.28084 feet internally

// Mathematical operations work identically
var mksArea = mksLength * mksLength;  // Area in m²
var fpsArea = fpsLength * fpsLength;  // Area in ft²
```

## Best Practices

### ✅ **Do: Use specific CreateXxx() methods for end-user APIs**
```csharp
Length length = unitSystem.CreateLength(100, "cm");
Angle angle = unitSystem.CreateAngle(90, "deg");
// Compile-time safety, IntelliSense support, no casting
```

### ✅ **Do: Use CreateMeasuredValueFromParsableUnit() for parsers**
```csharp
var result = unitSystem.CreateMeasuredValueFromParsableUnit("cm", 100);
// Runtime type discovery, strongly-typed results, zero ambiguity
```

### ✅ **Do: Use CreateUnit<T>() for generic APIs**
```csharp
public T CreateTypedMeasurement<T>(double value, string? units = null) 
    where T : MeasuredValue
{
    return unitSystem.CreateUnit<T>(value, units);
}
```

### ✅ **Do: Use dependency injection pattern**
```csharp
public class Calculator
{
    private readonly IUnitSystem _unitSystem;
    
    public Calculator(IUnitSystem unitSystem)
    {
        _unitSystem = unitSystem ?? IUnitSystem.MKS(); // Default fallback
    }
}
```

### ❌ **Don't: Use manual constructors directly**
```csharp
// Bad: Requires manual UnitGroup creation, error-prone
var unitGroup = // complex UnitGroup setup...
var length = new Length(unitGroup);

// Good: Uses unit system configuration automatically
var length = unitSystem.CreateLength(100, "cm");
```

### ❌ **Don't: Create multiple unit systems unnecessarily**
```csharp
// Bad: Multiple instances, potential inconsistency
var system1 = IUnitSystem.MKS();
var system2 = IUnitSystem.MKS();

// Good: Single system instance, consistent configuration
var unitSystem = IUnitSystem.MKS();
// Use same system instance throughout application scope
```

## Summary

The modern unit system architecture provides a clean, type-safe, and flexible approach for creating unit objects with automatic base unit conversion. The unified IUnitSystem interface covers all use cases from legacy compatibility to modern type-safe APIs, while maintaining architectural principles of dependency injection and UnitGroup-based conversion.

**Key Achievement:** Single interface provides compile-time type safety, runtime flexibility, seamless parser integration, and zero-conversion mathematical operations across all unit systems.

## UnitTypeRegistry: The Performance Engine

### **Why UnitTypeRegistry is Essential**

The `UnitTypeRegistry` is **critical** for production performance in the attribute-based system.

**The Performance Problem Without Caching:**
```csharp
// BAD: What every CreateUnit<T>() call would need to do
foreach(var type in Assembly.GetTypes()) // Scan 24+ types every time!
{
    var attr = type.GetCustomAttribute<UnitTypeAttribute>();
    if (attr?.Family == targetFamily) 
        return Activator.CreateInstance(type, unitGroup);
}
// Result: 50-100x slower than cached approach
```

**The Performance Solution With UnitTypeRegistry:**
```csharp
// GOOD: What actually happens with caching
return UnitTypeRegistry.CreateInstance(family, unitGroup); // O(1) dictionary lookup
```

### **UnitTypeRegistry Integration**

**Method Dependencies:**
- `CreateUnit<T>()` → `UnitTypeRegistry.GetAttributeForType()`
- `CreateTypedMeasuredValue()` → `UnitTypeRegistry.CreateInstance()` 
- `MeasuredValue.UnitFamily` → `UnitTypeRegistry.GetAttributeForType()`

**Performance Strategy:**
- **One-time cost:** Assembly scanning at application startup to build reflection cache
- **Runtime benefit:** O(1) dictionary lookups for all creation operations
- **Memory trade-off:** Small cache size for massive performance improvement

### **Critical Architectural Insight**

The `UnitTypeRegistry` makes the entire attribute-based unit system **production-ready**. It transforms expensive reflection operations into fast cached lookups, enabling:

✅ **High-frequency parser operations** - No performance penalty for repeated unit creation  
✅ **Responsive user interfaces** - Instant unit object creation  
✅ **Scalable server applications** - No reflection bottlenecks under load  

**Without UnitTypeRegistry:** Elegant design, unacceptable performance  
**With UnitTypeRegistry:** Elegant design, high performance - **production ready** 🎯