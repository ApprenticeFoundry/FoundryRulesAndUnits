# IUnitSystem Factory Methods

## Overview

We've added convenient static factory methods to `IUnitSystem` to dramatically simplify the creation of measurement instances. These methods eliminate the need to manually work with `UnitFactory` or `UnitGroup` injection for common scenarios.

## Quick Start

### Before (Complex)
```csharp
// Old way - required understanding of UnitFactory and UnitGroup injection  
var factory = new UnitFactory(UnitSystemType.SI);
var length = factory.CreateLength(10.5, "m");
var unitGroup = factory.GetUnitGroup(UnitFamilyName.Mass);
var mass = new Mass(unitGroup);
mass.Init(2.3, "kg");
```

### After (Simple)
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

### Measurement Creation Methods
```csharp
var system = IUnitSystem.SI();

// Basic measurements
var length = system.CreateLength(10.5, "m");
var mass = system.CreateMass(2.3, "kg");  
var time = system.CreateTime(5.0, "s");
var temperature = system.CreateTemperature(25.0, "°C");
var angle = system.CreateAngle(45.0, "deg");

// Derived measurements  
var area = system.CreateArea(100.0, "m²");
var volume = system.CreateVolume(50.0, "L");
var speed = system.CreateSpeed(100.0, "km/h");
var force = system.CreateForce(250.0, "N");

// Electrical measurements
var voltage = system.CreateVoltage(120.0, "V");
var current = system.CreateCurrent(10.0, "A");
var resistance = system.CreateResistance(12.0, "Ω");
var power = system.CreatePower(1200.0, "W");
var capacitance = system.CreateCapacitance(100.0, "μF");
var frequency = system.CreateFrequency(60.0, "Hz");

// Generic measurement creation
var genericMeasurement = system.CreateMeasuredValue(UnitFamilyName.Length, 25.4, "mm");

// NEW: Strongly-typed generic creation
var length = system.Create<Length>(25.4, "mm");         // Returns Length, not MeasuredValue
var mass = system.Create<Mass>(2.3, "kg");              // Returns Mass, not MeasuredValue  
var voltage = system.Create<Voltage>(120, "V");         // Returns Voltage, not MeasuredValue
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

// NEW: Strongly-typed generic creation - best of both worlds!
var length = system.Create<Length>(10.5, "m");          // Returns Length, not MeasuredValue
var mass = system.Create<Mass>(2.3, "kg");              // Returns Mass, not MeasuredValue
var temperature = system.Create<Temperature>(25, "°C");  // Returns Temperature, not MeasuredValue

// No casting needed - already strongly typed!
Console.WriteLine($"Length: {length.As("ft")} ft");
```

### 4. Advanced Factory Access
```csharp
var system = IUnitSystem.SI();
var factory = system.GetFactory();  // Get underlying factory for advanced usage

// Use factory directly for specialized operations
var distance1 = factory.CreateDistance(100, "km");
var distance2 = factory.CreateDistance(50, "mi");

// Generic creation also works with factory
var speed = factory.Create<Speed>(100, "km/h");
var force = factory.Create<Force>(250, "N");
```

## Benefits

1. **Simplified API**: No need to understand UnitGroup injection details
2. **Consistent Interface**: Same pattern across all measurement types  
3. **Type Safety**: Returns strongly-typed measurement instances
4. **Performance**: Internal caching prevents recreation of UnitGroups
5. **Flexibility**: Can still access underlying factory for advanced scenarios
6. **Backward Compatibility**: Old UnitFactory approach still works

## Architecture

The factory methods internally:
1. Create and cache a `UnitFactory` for the current system type
2. Use the factory's `Create*` methods to generate properly injected measurements
3. Initialize measurements with the provided value and units
4. Return fully functional measurement instances with UnitGroup injection

## Examples

See `Examples/UnitSystemFactoryExamples.cs` for comprehensive usage examples including:
- Basic factory usage  
- Different unit systems
- Electrical measurements
- Persistent system usage
- Advanced factory operations
- Before/after comparisons

## Migration Guide

### From UnitFactory Direct Usage
```csharp
// Old
var factory = new UnitFactory(UnitSystemType.SI);
var length = factory.CreateLength(10, "m");

// New  
var system = IUnitSystem.SI();
var length = system.CreateLength(10, "m");
```

### From Manual UnitGroup Injection
```csharp
// Old - complex and error-prone
var specification = new SIUnitSystemSpecification();
var unitGroups = specification.GetUnitGroups();
var lengthGroup = unitGroups[UnitFamilyName.Length];
var length = new Length(lengthGroup);
length.Init(10, "m");

// New - simple and clean
var system = IUnitSystem.SI();
var length = system.CreateLength(10, "m");
```

The new factory methods provide a much cleaner, more intuitive API while maintaining all the power and flexibility of the underlying UnitGroup injection architecture.