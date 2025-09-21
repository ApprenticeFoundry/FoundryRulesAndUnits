# UnitFactory Architecture Guide

## Overview

The `UnitFactory` is the cornerstone of the unit system architecture, providing three distinct methods for creating strongly-typed unit objects with automatic base unit conversion. It follows the architectural principle: **Factory takes unit system, gets current configuration, creates objects**.

## Core Architecture

### Single Dependency Pattern
```csharp
public class UnitFactory
{
    private readonly IUnitSystem _unitSystem;
    
    public UnitFactory(IUnitSystem unitSystem)
    {
        _unitSystem = unitSystem ?? throw new ArgumentNullException(nameof(unitSystem));
    }
}
```

**Key Benefits:**
- ✅ No hardcoded unit specifications
- ✅ Always uses current unit system configuration
- ✅ Proper dependency injection pattern
- ✅ Testable and maintainable

## Three Factory Methods - Complete API Coverage

### 1. `CreateMeasuredValue()` - Base Class Creation
**Purpose:** Legacy support and generic scenarios

```csharp
public MeasuredValue CreateMeasuredValue(UnitFamilyName family, double value = 0, string? units = null)
```

**Usage:**
```csharp
MeasuredValue length = factory.CreateMeasuredValue(UnitFamilyName.Length, 100, "cm");
// Returns: base MeasuredValue object
// Use case: Legacy code, generic algorithms
```

### 2. `CreateTypedMeasuredValue()` - Runtime Type Discovery
**Purpose:** Parser integration and dynamic type creation

```csharp
public MeasuredValue CreateTypedMeasuredValue(UnitFamilyName family, double value = 0, string? units = null)
```

**Usage:**
```csharp
// Parser determines family at runtime from unit string
MeasuredValue length = factory.CreateTypedMeasuredValue(UnitFamilyName.Length, 100, "cm");
// Returns: Length object (strongly-typed, not base MeasuredValue)
// Use case: Parser integration, runtime family determination
```

**Critical for Parser Integration:**
- Parser extracts value and units from user input
- Parser determines UnitFamilyName from unit lookup
- Factory creates correct derived type (Length, Angle, Mass, etc.)
- Mathematical operations work immediately

### 3. `CreateUnit<T>()` - Compile-Time Type Safety ⭐
**Purpose:** End-user APIs with maximum type safety  
**Performance:** Uses optimized `CreateTypedMeasuredValue()` internally with cached UnitTypeRegistry

```csharp
public T CreateUnit<T>(double value = 0, string? units = null) where T : MeasuredValue
```

**Usage:**
```csharp
Length length = factory.CreateUnit<Length>(100, "cm");
Angle angle = factory.CreateUnit<Angle>(90, "deg");
Mass mass = factory.CreateUnit<Mass>(5.5, "kg");

// No casting needed - exact type returned
Length result = length + factory.CreateUnit<Length>(1, "m");
```

**🚀 Performance Optimization:** This method now leverages the cached UnitTypeRegistry instead of doing expensive constructor reflection on every call. Both compile-time type safety AND runtime performance!
```

**Advantages:**
- ✅ **Compile-time type safety** - impossible to get wrong type
- ✅ **IntelliSense support** - shows exact methods available
- ✅ **Automatic family discovery** - derives UnitFamilyName from type
- ✅ **No casting required** - returns exact type specified
- ✅ **Clean API** - most readable and maintainable

## Automatic Base Unit Conversion

All three methods automatically convert values to the correct base units for the current unit system:

```csharp
// SI System (base unit: meters)
var siSystem = new UnitSystem(new SIUnitSystemSpecification());
var siFactory = new UnitFactory(siSystem);
var siLength = siFactory.CreateUnit<Length>(100, "cm");
// Internal storage: 1.0 meters

// FPS System (base unit: feet) 
var fpsSystem = new UnitSystem(new FPSUnitSystemSpecification());
var fpsFactory = new UnitFactory(fpsSystem);
var fpsLength = fpsFactory.CreateUnit<Length>(100, "cm");
// Internal storage: 3.28084 feet

// IPS System (base unit: inches)
var ipsSystem = new UnitSystem(new IPSUnitSystemSpecification());
var ipsFactory = new UnitFactory(ipsSystem);
var ipsLength = ipsFactory.CreateUnit<Length>(100, "cm");
// Internal storage: 39.3701 inches
```

## Usage Patterns by Scenario

### End-User APIs (Recommended: `CreateUnit<T>()`)
```csharp
// Engineering calculations
public static Force CalculateForce(double massKg, double accelerationMs2)
{
    var mass = factory.CreateUnit<Mass>(massKg, "kg");
    var acceleration = factory.CreateUnit<Acceleration>(accelerationMs2, "m/s²");
    return mass * acceleration;  // Returns Force - type-safe!
}

// Configuration and setup
var tolerance = factory.CreateUnit<Length>(0.1, "mm");
var maxSpeed = factory.CreateUnit<Speed>(100, "mph");
var temperature = factory.CreateUnit<Temperature>(25, "°C");
```

### Parser Integration (`CreateTypedMeasuredValue()`)
```csharp
// Parser processes user input: "100 cm + 1 m"
public MeasuredValue ParseExpression(string input)
{
    var (value, units, family) = ExtractComponents(input);  // Parser logic
    return factory.CreateTypedMeasuredValue(family, value, units);
    // Returns: Length object ready for mathematical operations
}

// Ultimate integration test
var left = factory.CreateTypedMeasuredValue(UnitFamilyName.Length, 100, "cm");   // Length
var right = factory.CreateTypedMeasuredValue(UnitFamilyName.Length, 1, "m");     // Length  
var result = (Length)left + (Length)right;  // 2 meters
```

### Legacy Code (`CreateMeasuredValue()`)
```csharp
// Generic algorithms that work with base MeasuredValue
public double ConvertValue(UnitFamilyName family, double value, string fromUnit, string toUnit)
{
    var measured = factory.CreateMeasuredValue(family, value, fromUnit);
    return measured.As(toUnit);
}
```

## Method Selection Guide

| Scenario | Method | Type Safety | Performance | Use When |
|----------|--------|-------------|-------------|----------|
| **End-User APIs** | `CreateUnit<T>()` | **Compile-time** | Best | You know the type at compile-time |
| **Parser Integration** | `CreateTypedMeasuredValue()` | Runtime strong | Good | You determine type at runtime |
| **Legacy/Generic** | `CreateMeasuredValue()` | Base class only | Good | Working with base MeasuredValue |

## Architecture Benefits

### 🎯 **Single Source of Truth**
- All configuration comes through `IUnitSystem`
- No hardcoded unit specifications in factory
- Easy to switch between unit systems

### 🎯 **Type Safety Spectrum**
- `CreateMeasuredValue()`: Base class (legacy compatibility)
- `CreateTypedMeasuredValue()`: Runtime strong typing (parser integration)
- `CreateUnit<T>()`: Compile-time strong typing (end-user APIs)

### 🎯 **Performance Optimized**
- Values stored in base units for zero-conversion mathematical operations
- Reflection cached in `UnitTypeRegistry` for performance
- Minimal object creation overhead

### 🎯 **Testability**
```csharp
// Easy to test with dependency injection
var mockUnitSystem = new Mock<IUnitSystem>();
var factory = new UnitFactory(mockUnitSystem.Object);
// Test different unit system configurations
```

## Integration with Architecture

### Parser → Factory → Math Pipeline
```
Parser Input: "100 cm + 1 m"
    ↓
Parser extracts: (100, "cm", Length), (1, "m", Length) 
    ↓
Factory creates: Length(1.0 meters), Length(1.0 meters)
    ↓
Mathematical operation: 1.0 + 1.0 = 2.0 (in base units)
    ↓
Result: Length(2.0 meters) → Display as "2 m" or "200 cm"
```

### Unit System Switching
```csharp
// Same factory code works with different unit systems
var siFactory = new UnitFactory(siSystem);
var fpsFactory = new UnitFactory(fpsSystem);

// Same API, different internal base units
var siLength = siFactory.CreateUnit<Length>(100, "cm");    // 1.0 meters
var fpsLength = fpsFactory.CreateUnit<Length>(100, "cm");  // 3.28084 feet
```

## Best Practices

### ✅ **Do: Use `CreateUnit<T>()` for end-user APIs**
```csharp
Length length = factory.CreateUnit<Length>(100, "cm");
// Compile-time safety, IntelliSense support, no casting
```

### ✅ **Do: Use `CreateTypedMeasuredValue()` for parsers**
```csharp
var result = factory.CreateTypedMeasuredValue(family, value, units);
// Runtime type discovery, strongly-typed results
```

### ✅ **Do: Inject `IUnitSystem` for flexibility**
```csharp
public class Calculator
{
    private readonly UnitFactory _factory;
    
    public Calculator(IUnitSystem unitSystem)
    {
        _factory = new UnitFactory(unitSystem);
    }
}
```

### ❌ **Don't: Use static factory methods**
```csharp
// Bad: Hardcoded dependencies, not testable
var factory = UnitFactory.SI();

// Good: Dependency injection, configurable
var factory = new UnitFactory(unitSystem);
```

### ❌ **Don't: Create multiple factories for same unit system**
```csharp
// Bad: Multiple instances, inconsistent state
var factory1 = new UnitFactory(unitSystem);
var factory2 = new UnitFactory(unitSystem);

// Good: Single factory, consistent configuration
var factory = new UnitFactory(unitSystem);
// Use same factory instance throughout application scope
```

## Summary

The `UnitFactory` provides a clean, type-safe, and flexible API for creating unit objects with automatic base unit conversion. The three-method approach covers all use cases from legacy compatibility to modern type-safe APIs, while maintaining architectural principles of dependency injection and single source of truth.

**Key Achievement:** Same simple API works across all unit systems, provides both runtime and compile-time type safety, and enables seamless parser integration with zero-conversion mathematical operations.

## UnitTypeRegistry: The Hidden Performance Hero

### **Why UnitTypeRegistry Exists**

The `UnitTypeRegistry` is **absolutely essential** for the `UnitFactory` architecture. Without it, the attribute-based system would be impractically slow.

**The Performance Problem Without Caching:**
```csharp
// BAD: What every CreateTypedMeasuredValue() call would need to do
foreach(var type in Assembly.GetTypes()) // Scan 24+ types every time!
{
    var attr = type.GetCustomAttribute<UnitTypeAttribute>();
    if (attr?.Family == family) 
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

**Factory Method Dependencies:**
- `CreateTypedMeasuredValue()` → `UnitTypeRegistry.CreateInstance()` 
- `CreateUnit<T>()` → `UnitTypeRegistry.GetAttributeForType()`
- `MeasuredValue.UnitFamily` → `UnitTypeRegistry.GetAttributeForType()`

**Performance Strategy:**
- **One-time cost:** Assembly scanning at startup to build reflection cache
- **Runtime benefit:** O(1) dictionary lookups for all factory operations
- **Memory trade-off:** Small cache size for massive performance improvement

### **Critical Architectural Insight**

The `UnitTypeRegistry` is what makes the entire attribute-based factory system **production-ready**. It transforms expensive reflection operations into fast cached lookups, enabling:

✅ **High-frequency parser operations** - No performance penalty for repeated factory calls  
✅ **Responsive user interfaces** - Instant unit object creation  
✅ **Scalable server applications** - No reflection bottlenecks under load  

**Without UnitTypeRegistry:** Elegant design, unacceptable performance  
**With UnitTypeRegistry:** Elegant design, high performance - **production ready** 🎯