# Smart Factory Quick Reference

## Cheat Sheet for Developers

### Basic Usage
```csharp
// Use defaults (recommended)
var length = KnBase.UnitService.CreateLength(5.0);        // 5 m or 5 ft depending on system
var mass = KnBase.UnitService.CreateMass(10.0);           // 10 kg or 10 lb
var temperature = KnBase.UnitService.CreateTemperature(20.0); // 20 K or 20 °R

// With explicit units
var length = KnBase.UnitService.CreateLength(5.0, "ft");   // Always 5 ft, converted internally
var mass = KnBase.UnitService.CreateMass(10.0, "kg");     // Always 10 kg
var temperature = KnBase.UnitService.CreateTemperature(20.0, "°C"); // 20°C, converted
```

### Available Factory Methods
```csharp
KnBase.UnitService.CreateLength(value, units?)
KnBase.UnitService.CreateMass(value, units?)
KnBase.UnitService.CreateTime(value, units?)
KnBase.UnitService.CreateTemperature(value, units?)
KnBase.UnitService.CreateAngle(value, units?)
KnBase.UnitService.CreateSpeed(value, units?)
KnBase.UnitService.CreateArea(value, units?)
KnBase.UnitService.CreateVolume(value, units?)
KnBase.UnitService.CreateForce(value, units?)
KnBase.UnitService.CreatePower(value, units?)
KnBase.UnitService.CreateVoltage(value, units?)
KnBase.UnitService.CreateCurrent(value, units?)
KnBase.UnitService.CreateResistance(value, units?)
KnBase.UnitService.CreateCapacitance(value, units?)
KnBase.UnitService.CreateFrequency(value, units?)
KnBase.UnitService.CreateDimensionless(value, units?)
// ... and more
```

### System Setup
```csharp
// Set application default (do this once at startup)
KnBase.UnitService = IUnitSystem.MKS();  // or FPS(), SI(), CGS(), IPS()

// Create specific systems
var mksSystem = IUnitSystem.MKS();
var fpsSystem = IUnitSystem.FPS();
var measurement = mksSystem.CreateLength(5.0); // Always uses MKS base units
```

### Common Unit Systems
```csharp
IUnitSystem.MKS()  // meters, kg, seconds (engineering)
IUnitSystem.FPS()  // feet, pounds, seconds (US)
IUnitSystem.SI()   // meters, kg, seconds, Kelvin, Ampere (scientific)
IUnitSystem.IPS()  // inches, pounds, seconds (manufacturing)
IUnitSystem.CGS()  // cm, grams, seconds (physics)
```

### Adding New Factory Method (Template)
```csharp
// 1. In UnitFactory.cs
public YourType CreateYourType(double value = 0, string? units = null)
{
    var unitGroup = _unitGroups[UnitFamilyName.YourFamily];
    var instance = new YourType(unitGroup);
    var defaultUnit = units ?? unitGroup.BaseUnit.Symbol;
    instance.Init(value, defaultUnit);
    return instance;
}

// 2. In IUnitSystem.cs
YourType CreateYourType(double value = 0, string? units = null);

// 3. In UnitSystem.cs
public YourType CreateYourType(double value = 0, string? units = null)
{
    return GetFactory().CreateYourType(value, units);
}
```

### Migration from Old Code
```csharp
// OLD (still works, but not recommended)
var length = new Length(5.0, "m");

// NEW (recommended)
var length = KnBase.UnitService.CreateLength(5.0); // Auto-uses base unit

// NEW with explicit units
var length = KnBase.UnitService.CreateLength(5.0, "m");
```

### Key Benefits
- ✅ **Auto-defaults to base units** - no need to specify units if using system defaults
- ✅ **Type safety** - compile-time checking of factory methods
- ✅ **Unit validation** - automatic validation of units for measurement type
- ✅ **System consistency** - all measurements use same unit system
- ✅ **Easy testing** - can switch unit systems for different test scenarios
- ✅ **Clear patterns** - consistent factory method structure

### Remember
- Use `KnBase.UnitService.CreateXxx()` instead of `new Xxx()`
- Let defaults work - `CreateLength(5.0)` is often better than `CreateLength(5.0, "m")`
- Set your application's unit system once at startup
- Factory methods handle all the unit system complexity for you