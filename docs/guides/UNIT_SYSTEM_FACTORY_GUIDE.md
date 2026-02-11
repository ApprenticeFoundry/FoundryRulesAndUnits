# IUnitSystem Creation Methods Guide

## Overview

The modern unit system in FoundryRulesAndUnits v9.1.0 provides convenient creation methods through the `IUnitSystem` interface. These methods eliminate the need to manually work with separate factory classes or `UnitGroup` injection for common scenarios.

**Current Version**: 9.1.0 targeting .NET 9.0  
**Architecture Pattern**: Unified IUnitSystem interface with type-safe creation methods  
**Key Innovation**: Direct creation methods with automatic UnitGroup injection

## Quick Start

### Before (Complex - Deprecated)
```csharp
// Old way - required understanding of UnitFactory and UnitGroup injection  
var factory = new UnitFactory(unitSystem);
var length = factory.CreateLength(10.5, "m");
var unitGroup = unitGroups[UnitFamilyName.Mass];
var mass = new Mass(unitGroup);
mass.Init(2.3, "kg");
```

### After (Simple - Current)
```csharp
// New way - clean and intuitive
var system = IUnitSystem.SI();
var length = system.CreateLength(10.5, "m");
var mass = system.CreateMass(2.3, "kg");
```

## Available Factory Methods

### Static System Creation
```csharp
var siSystem = IUnitSystem.SI();        // International System of Units
var mksSystem = IUnitSystem.MKS();      // Meter-Kilogram-Second  
var fpsSystem = IUnitSystem.FPS();      // Foot-Pound-Second
var ipsSystem = IUnitSystem.IPS();      // Inch-Pound-Second
var cgsSystem = IUnitSystem.CGS();      // Centimeter-Gram-Second
```

## Available Creation Methods

### Static System Creation
```csharp
var siSystem = IUnitSystem.SI();        // International System of Units (scientific)
var mksSystem = IUnitSystem.MKS();      // Meter-Kilogram-Second (engineering)
var fpsSystem = IUnitSystem.FPS();      // Foot-Pound-Second (US construction)
var ipsSystem = IUnitSystem.IPS();      // Inch-Pound-Second (manufacturing)
var cgsSystem = IUnitSystem.CGS();      // Centimeter-Gram-Second (laboratory)
```

### Specific Type Creation Methods (Recommended)
```csharp
var system = IUnitSystem.SI();

// Basic measurements
Length length = system.CreateLength(10.5, "m");
Mass mass = system.CreateMass(2.3, "kg");  
Time time = system.CreateTime(5.0, "s");
Temperature temperature = system.CreateTemperature(25.0, "°C");
Angle angle = system.CreateAngle(45.0, "deg");

// Derived measurements  
Area area = system.CreateArea(100.0, "m²");
Volume volume = system.CreateVolume(50.0, "L");
Speed speed = system.CreateSpeed(100.0, "km/h");
Force force = system.CreateForce(250.0, "N");

// Electrical measurements
Voltage voltage = system.CreateVoltage(120.0, "V");
Current current = system.CreateCurrent(10.0, "A");
Resistance resistance = system.CreateResistance(12.0, "Ω");
Power power = system.CreatePower(1200.0, "W");
Capacitance capacitance = system.CreateCapacitance(100.0, "μF");
Frequency frequency = system.CreateFrequency(60.0, "Hz");
```

### Generic Creation Methods
```csharp
// Generic creation by family (for parser scenarios)
MeasuredValue generic = system.CreateMeasuredValue(UnitFamilyName.Length, 25.4, "mm");

// Strongly-typed generic creation (compile-time type safety)
Length length = system.CreateUnit<Length>(25.4, "mm");         // Returns Length
Mass mass = system.CreateUnit<Mass>(2.3, "kg");               // Returns Mass  
Voltage voltage = system.CreateUnit<Voltage>(120, "V");       // Returns Voltage
```

## Usage Patterns

### 1. One-off Measurements
```csharp
// Quick measurement creation and conversion
var length = IUnitSystem.SI().CreateLength(100, "cm");
Console.WriteLine($"Length in inches: {length.As("in")} in");
```

### 2. Persistent System Usage
```csharp
var system = IUnitSystem.FPS();  // Create once, use many times

var measurements = new[]
{
    system.CreateLength(12.0, "ft"),
    system.CreateMass(150.0, "lb"), 
    system.CreateForce(1000.0, "lbf")
};

foreach (var measurement in measurements)
{
    Console.WriteLine($"{measurement.GetType().Name}: {measurement.Value()} {measurement.Units()}");
}
```

### 3. Strongly-Typed Generic Creation
```csharp
var system = IUnitSystem.SI();

// Strongly-typed generic creation - compile-time type safety
Length length = system.CreateUnit<Length>(10.5, "m");          // Returns Length
Mass mass = system.CreateUnit<Mass>(2.3, "kg");               // Returns Mass
Temperature temperature = system.CreateUnit<Temperature>(25, "°C");  // Returns Temperature

// No casting needed - already strongly typed!
Console.WriteLine($"Length: {length.As("ft")} ft");
```

### 4. Parser Integration
```csharp
var system = IUnitSystem.SI();

// Parse user input and create appropriate type
public MeasuredValue ParseInput(string input)
{
    var (value, unit) = ExtractValueAndUnit(input); // "100 cm" → 100, "cm"
    
    // Creates correct derived type automatically (Length, Angle, Mass, etc.)
    return system.CreateMeasuredValueFromParsableUnit(unit, value);
}
```

## Benefits

1. **Unified Interface**: Single interface for all unit operations (no separate factory classes)
2. **Type Safety**: Returns strongly-typed measurement instances with compile-time checking
3. **Consistent Patterns**: Same creation pattern across all measurement types  
4. **Performance**: Internal UnitGroup injection with cached operations
5. **Flexibility**: Multiple creation methods for different scenarios (specific, generic, parser)
6. **Dependency Injection**: Easy to inject and test with IUnitSystem interface

## Architecture

The creation methods internally:
1. Use the current unit system specification to get appropriate UnitGroups
2. Create MeasuredValue instances with proper UnitGroup injection
3. Initialize measurements with the provided value and units (automatic base unit conversion)
4. Return fully functional measurement instances ready for mathematical operations

**Key Innovation**: No separate factory classes needed - everything through unified IUnitSystem interface

## Migration Guide

### From Manual Constructor Usage
```csharp
// Old - manual constructor with UnitGroup setup
var unitGroup = // complex UnitGroup setup...
var length = new Length(unitGroup);
length.Init(10, "m");

// New - simple and clean
var system = IUnitSystem.SI();
var length = system.CreateLength(10, "m");
```

### From Static Methods
```csharp
// Old - static factory methods (if they existed)
var length = Length.Create(10, "m");    // Static approach

// New - instance methods with system configuration
var system = IUnitSystem.SI();
var length = system.CreateLength(10, "m");  // Configurable system
```

### For Different Unit Systems
```csharp
// Engineering calculations - use MKS
var mksSystem = IUnitSystem.MKS();
var distance = mksSystem.CreateLength(100, "m");
var mass = mksSystem.CreateMass(1000, "kg");

// US construction - use FPS  
var fpsSystem = IUnitSystem.FPS();
var height = fpsSystem.CreateLength(20, "ft");
var weight = fpsSystem.CreateMass(2000, "lb");

// Scientific work - use SI
var siSystem = IUnitSystem.SI();
var temperature = siSystem.CreateTemperature(298.15, "K");
var current = siSystem.CreateCurrent(0.5, "A");
```

The modern creation methods provide a much cleaner, more intuitive API while maintaining all the power and flexibility of the underlying UnitGroup injection architecture. The unified IUnitSystem interface eliminates the need for separate factory classes and provides consistent patterns across all measurement types.