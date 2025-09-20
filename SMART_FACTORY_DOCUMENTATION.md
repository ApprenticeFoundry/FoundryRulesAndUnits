# Smart Unit Factory System Documentation

## Overview

The Smart Unit Factory System provides a clean, dependency-injection-based approach to creating measurement objects with proper unit system integration. This replaces the previous global singleton pattern with a more maintainable and testable architecture.

## Core Architecture

### Key Components

1. **IUnitSystem Interface** - Service contract for unit system operations
2. **UnitSystem Class** - Implementation with factory method delegation
3. **UnitFactory Class** - Core factory for creating typed measurements
4. **KnBase Static Service** - Global access point for application-wide unit system

### Dependency Flow

```
Application
    ↓
KnBase.UnitService (Static Global)
    ↓
IUnitSystem.CreateXxx() methods
    ↓
UnitFactory.CreateXxx() methods
    ↓
Typed Measurement Objects
```

## Factory Method Pattern

### Standard Pattern

Every factory method follows this consistent 4-step pattern:

```csharp
public SomeType CreateSomeType(double value = 0, string? units = null)
{
    var unitGroup = _unitGroups[UnitFamilyName.SomeFamily];
    var someType = new SomeType(unitGroup);
    var defaultUnit = units ?? unitGroup.BaseUnit.Symbol;
    someType.Init(value, defaultUnit);
    return someType;
}
```

### Pattern Benefits

- **Explicit Default Logic**: `units ?? unitGroup.BaseUnit.Symbol` makes default unit resolution visible
- **Consistent Structure**: Every method follows identical steps
- **Easy Debugging**: Clear breakpoint locations for troubleshooting
- **Self-Documenting**: Pattern immediately shows default behavior

## Usage Examples

### Basic Usage with Defaults

```csharp
// Uses base units for the active system
var length = KnBase.UnitService.CreateLength(5.0);    // 5 m (MKS), 5 ft (FPS)
var mass = KnBase.UnitService.CreateMass(10.0);       // 10 kg (MKS), 10 lb (FPS)
var temp = KnBase.UnitService.CreateTemperature(20.0); // 20 K (MKS), 20 °R (FPS)
```

### Usage with Explicit Units

```csharp
// Explicit units, converted to base units for storage
var length = KnBase.UnitService.CreateLength(5.0, "ft");    // Stored as meters internally
var mass = KnBase.UnitService.CreateMass(10.0, "kg");       // Stored as base mass unit
var temp = KnBase.UnitService.CreateTemperature(20.0, "°C"); // Converted to base temp unit
```

### System-Specific Usage

```csharp
// Create specific unit systems
var mksSystem = IUnitSystem.MKS();
var fpsSystem = IUnitSystem.FPS();

var lengthMKS = mksSystem.CreateLength(5.0);  // 5 m
var lengthFPS = fpsSystem.CreateLength(5.0);  // 5 ft
```

## Unit System Types

### Available Systems

| System | Base Units | Use Case |
|--------|------------|----------|
| **MKS** | m, kg, s | Engineering, general purpose |
| **SI** | m, kg, s, K, A, mol, cd | Scientific applications |
| **FPS** | ft, lb, s | US engineering |
| **IPS** | in, lb, s | Manufacturing, machining |
| **CGS** | cm, g, s | Physics, older scientific |
| **mmNs** | mm, N, s | Mechanical design |

### Default Unit Examples

```csharp
// MKS System Defaults
Length: meters (m)
Mass: kilograms (kg)
Time: seconds (s)
Temperature: Kelvin (K)
Force: Newtons (N)
Power: Watts (W)

// FPS System Defaults
Length: feet (ft)
Mass: pounds (lb)
Time: seconds (s)
Temperature: Rankine (°R)
Force: pound-force (lbf)
Power: horsepower (hp)
```

## Integration Guidelines

### Application Setup

1. **Choose Default System**: Set application-wide default in startup
```csharp
// In application startup/configuration
KnBase.UnitService = IUnitSystem.MKS(); // or FPS, SI, etc.
```

2. **Service Injection**: For dependency injection frameworks
```csharp
// In DI container setup
services.AddSingleton<IUnitSystem>(_ => IUnitSystem.MKS());
```

### Creating New Measurement Types

When adding new measurement types, follow this checklist:

1. **Add to UnitFamilyName enum**
```csharp
public enum UnitFamilyName
{
    // ... existing values
    YourNewFamily
}
```

2. **Create measurement class with UnitGroup constructor**
```csharp
public class YourMeasurement : MeasuredValue
{
    public override UnitFamilyName UnitFamily => UnitFamilyName.YourNewFamily;
    
    public YourMeasurement(UnitGroup unitGroup) : base(unitGroup)
    {
        if (unitGroup.Family != UnitFamilyName.YourNewFamily)
            throw new ArgumentException($"UnitGroup family must be {UnitFamilyName.YourNewFamily}");
    }
}
```

3. **Add factory method to UnitFactory**
```csharp
public YourMeasurement CreateYourMeasurement(double value = 0, string? units = null)
{
    var unitGroup = _unitGroups[UnitFamilyName.YourNewFamily];
    var measurement = new YourMeasurement(unitGroup);
    var defaultUnit = units ?? unitGroup.BaseUnit.Symbol;
    measurement.Init(value, defaultUnit);
    return measurement;
}
```

4. **Add to IUnitSystem interface**
```csharp
YourMeasurement CreateYourMeasurement(double value = 0, string? units = null);
```

5. **Add to UnitSystem implementation**
```csharp
public YourMeasurement CreateYourMeasurement(double value = 0, string? units = null)
{
    return GetFactory().CreateYourMeasurement(value, units);
}
```

### Best Practices

#### Do ✅

- **Use factory methods**: `KnBase.UnitService.CreateLength(5.0)` instead of `new Length(5.0, "m")`
- **Let defaults work**: `CreateLength(5.0)` automatically uses correct base unit
- **Explicit when needed**: `CreateLength(5.0, "ft")` when specific units required
- **Consistent patterns**: Follow the 4-step factory method pattern
- **Unit validation**: Factory handles unit validation automatically

#### Don't ❌

- **Direct constructors**: Avoid `new Length(5.0, "m")` - bypasses unit system
- **Global singletons**: Don't use old `UnitCategory.Units()` patterns
- **Manual unit lookup**: Let factory handle unit group injection
- **Mixed patterns**: Stick to consistent factory method structure

### Testing Integration

```csharp
[Test]
public void TestWithDifferentUnitSystems()
{
    // Test with MKS
    var originalSystem = KnBase.UnitService;
    KnBase.UnitService = IUnitSystem.MKS();
    
    var lengthMKS = KnBase.UnitService.CreateLength(5.0);
    Assert.AreEqual("m", lengthMKS.Internal());
    
    // Test with FPS
    KnBase.UnitService = IUnitSystem.FPS();
    
    var lengthFPS = KnBase.UnitService.CreateLength(5.0);
    Assert.AreEqual("ft", lengthFPS.Internal());
    
    // Restore original
    KnBase.UnitService = originalSystem;
}
```

### Error Handling

The factory system provides clear error messages:

```csharp
// Invalid unit for family
try {
    var length = KnBase.UnitService.CreateLength(5.0, "kg"); // Wrong unit type
} catch (ArgumentException ex) {
    // "kg is not a valid unit for Length"
}

// Unsupported unit family
try {
    var factory = new UnitFactory(UnitSystemType.MKS);
    var measurement = factory.CreateMeasuredValue(UnitFamilyName.SomeUnsupported, 5.0);
} catch (ArgumentException ex) {
    // "Unit family SomeUnsupported not available in MKS system"
}
```

## Migration Guide

### From Old Pattern

**Before (avoid):**
```csharp
var length = new Length(5.0, "m");
var mass = new Mass(10.0, "kg");
```

**After (preferred):**
```csharp
var length = KnBase.UnitService.CreateLength(5.0);  // Auto-defaults to base unit
var mass = KnBase.UnitService.CreateMass(10.0);     // Auto-defaults to base unit
```

### Backwards Compatibility

The system maintains backwards compatibility:
- Old constructors still work for existing code
- New factory methods provide enhanced functionality
- Gradual migration is supported

## Performance Considerations

- **Factory Caching**: `UnitSystem` caches `UnitFactory` instances
- **UnitGroup Reuse**: UnitGroups are created once per system type
- **Minimal Overhead**: Factory pattern adds negligible performance cost
- **Memory Efficient**: Shared UnitGroup instances across measurements

## Conclusion

The Smart Unit Factory System provides a clean, maintainable approach to unit management that:

1. **Eliminates global dependencies** through dependency injection
2. **Provides consistent patterns** for measurement creation
3. **Handles defaults intelligently** based on active unit system
4. **Maintains type safety** with compile-time checking
5. **Supports multiple unit systems** seamlessly
6. **Enables easy testing** with system switching

This architecture scales well across applications and provides a solid foundation for unit-aware calculations.