# Modern Unit System Quick Reference

## Cheat Sheet for Developers (v9.1.0)

### Basic Usage Pattern (Recommended)
```csharp
// Create unit system once (application startup)
var unitSystem = IUnitSystem.MKS();  // or SI(), FPS(), IPS(), CGS()

// Type-safe creation with compile-time checking
Length length = unitSystem.CreateLength(5.0);        // 5 m (MKS base unit)
Mass mass = unitSystem.CreateMass(10.0);             // 10 kg (MKS base unit)
Temperature temp = unitSystem.CreateTemperature(20.0); // 20 °C (MKS base unit)

// With explicit units (automatic conversion to base units)
Length length = unitSystem.CreateLength(5.0, "ft");   // 5 ft → 1.524 m internally
Mass mass = unitSystem.CreateMass(10.0, "lb");        // 10 lb → 4.536 kg internally
Temperature temp = unitSystem.CreateTemperature(20.0, "°F"); // 20°F → -6.67°C internally
```

### Available Creation Methods (IUnitSystem Interface)
```csharp
// All methods follow pattern: unitSystem.CreateXxx(value, units?)
unitSystem.CreateLength(value, units?)        // Returns Length
unitSystem.CreateMass(value, units?)          // Returns Mass  
unitSystem.CreateTime(value, units?)          // Returns Time
unitSystem.CreateTemperature(value, units?)   // Returns Temperature
unitSystem.CreateAngle(value, units?)         // Returns Angle
unitSystem.CreateSpeed(value, units?)         // Returns Speed
unitSystem.CreateArea(value, units?)          // Returns Area
unitSystem.CreateVolume(value, units?)        // Returns Volume
unitSystem.CreateForce(value, units?)         // Returns Force
unitSystem.CreatePower(value, units?)         // Returns Power
unitSystem.CreateVoltage(value, units?)       // Returns Voltage
unitSystem.CreateCurrent(value, units?)       // Returns Current
unitSystem.CreateResistance(value, units?)    // Returns Resistance
unitSystem.CreateCapacitance(value, units?)   // Returns Capacitance
unitSystem.CreateFrequency(value, units?)     // Returns Frequency

// Generic creation for parser scenarios
unitSystem.CreateMeasuredValue(family, value, units?)  // Returns MeasuredValue
unitSystem.CreateUnit<T>(value, units?)               // Returns T where T : MeasuredValue
```

### System Setup and Configuration
```csharp
// Static factory methods for quick setup
var mksSystem = IUnitSystem.MKS();   // meters, kg, seconds, °C (engineering)
var fpsSystem = IUnitSystem.FPS();   // feet, pounds, seconds, °F (US imperial)
var siSystem = IUnitSystem.SI();     // meters, kg, seconds, K, Ampere (scientific)
var ipsSystem = IUnitSystem.IPS();   // inches, pounds, seconds, °F (manufacturing)
var cgsSystem = IUnitSystem.CGS();   // cm, grams, seconds, °C (physics)

// Alternative constructor approach
var customSystem = new UnitSystem(UnitSystemType.MKS);

// Switch systems dynamically
var system = IUnitSystem.MKS();
system.Apply(UnitSystemType.FPS);  // Switch to FPS
system.Apply(UnitSystemType.SI);   // Switch to SI

// Create measurements with specific system
var mksLength = mksSystem.CreateLength(5.0);    // 5 meters (base unit)
var fpsLength = fpsSystem.CreateLength(5.0);    // 5 feet (base unit)
var siLength = siSystem.CreateLength(5.0);      // 5 meters (base unit)
```

### Available Unit Systems
```csharp
IUnitSystem.MKS()  // meters, kg, seconds, °C (general engineering)
IUnitSystem.FPS()  // feet, pounds, seconds, °F (US construction/structural)
IUnitSystem.SI()   // meters, kg, seconds, K, Ampere (scientific)  
IUnitSystem.IPS()  // inches, pounds, seconds, °F (manufacturing/machining)
IUnitSystem.CGS()  // cm, grams, seconds, °C (physics/laboratory)
```

### Mathematical Operations (Type-Safe)
```csharp
var system = IUnitSystem.MKS();

// Same-family operations
Length a = system.CreateLength(5, "m");
Length b = system.CreateLength(3, "m");
Length sum = a + b;  // 8 meters

// Cross-family operations (automatic type inference)
Length width = system.CreateLength(5, "m");
Length height = system.CreateLength(3, "m");
var area = width * height;  // Returns Area object (15 m²)

Mass mass = system.CreateMass(100, "kg");
var acceleration = system.CreateAcceleration(9.8, "m/s2");
var force = mass * acceleration;  // Returns Force object

// Unit conversion
double meters = length.As("m");      // Convert to meters
double feet = length.As("ft");       // Convert to feet  
string display = length.AsString("cm"); // "500 cm"
```

### Adding New Unit Types (Template)
```csharp
// 1. Create MeasuredValue derived class with UnitTypeAttribute
[UnitType(UnitFamilyName.YourFamily, Description = "Your unit description")]
public class YourType : MeasuredValue
{
    public YourType(UnitGroup unitGroup) : base(unitGroup)
    {
        if (unitGroup.Family != UnitFamilyName.YourFamily)
            throw new ArgumentException($"UnitGroup family must be {UnitFamilyName.YourFamily}");
    }
    
    // Implement required methods: Assign, Copy, operators
    // Follow gold standard pattern from Length.cs and Angle.cs
}

// 2. Add to IUnitSystem interface
YourType CreateYourType(double value = 0, string? units = null);

// 3. Implement in UnitSystem class
public YourType CreateYourType(double value = 0, string? units = null)
{
    return CreateUnit<YourType>(value, units);
}

// 4. Add to unit system specifications as needed
```

### Migration from Old Patterns
```csharp
// OLD patterns (deprecated)
var length = new Length(5.0, "m");              // Manual constructor
var mass = UnitFactory.CreateMass(10.0, "kg");  // Static factory
var temp = KnBase.UnitService.CreateTemp(20.0); // Global service

// NEW pattern (recommended)
var unitSystem = IUnitSystem.MKS();  // Create once, use everywhere
var length = unitSystem.CreateLength(5.0, "m");      // Type-safe creation
var mass = unitSystem.CreateMass(10.0, "kg");        // Consistent pattern
var temp = unitSystem.CreateTemperature(20.0, "°C"); // Full interface support
```

### Advanced Usage Patterns
```csharp
// Dependency injection pattern
public class Calculator
{
    private readonly IUnitSystem _unitSystem;
    
    public Calculator(IUnitSystem unitSystem)
    {
        _unitSystem = unitSystem ?? IUnitSystem.MKS();
    }
    
    public Force CalculateForce(double mass, double acceleration)
    {
        var m = _unitSystem.CreateMass(mass, "kg");
        var a = _unitSystem.CreateAcceleration(acceleration, "m/s2");
        return m * a; // Returns Force automatically
    }
}

// Parser integration
public class UnitParser
{
    private readonly IUnitSystem _unitSystem;
    
    public MeasuredValue Parse(string input)
    {
        var (value, unit) = ExtractValueAndUnit(input); // "100 cm" → 100, "cm"
        return _unitSystem.CreateMeasuredValueFromParsableUnit(unit, value);
        // Returns correct derived type (Length, Angle, Mass, etc.)
    }
}

// Generic type-safe creation
public T CreateTypedMeasurement<T>(double value, string? units = null) 
    where T : MeasuredValue
{
    return _unitSystem.CreateUnit<T>(value, units);
}
```

### Key Benefits (v9.1.0)
- ✅ **Type safety** - Compile-time checking with generic methods
- ✅ **Zero ambiguity** - Two-tier unit family system for parsers
- ✅ **Performance** - O(1) lookups with UnitTypeRegistry caching
- ✅ **Flexibility** - Switch unit systems without code changes
- ✅ **Mathematical operations** - Automatic type inference (Length × Length → Area)
- ✅ **Unit validation** - Automatic validation and conversion
- ✅ **System consistency** - All measurements use same unit system base units
- ✅ **Cross-family compatibility** - Length + Distance, Time + Duration work seamlessly
- ✅ **Dependency injection** - Testable and configurable via IUnitSystem interface

### Remember (Updated Guidelines)
- Use `IUnitSystem` interface instead of direct constructors or static factories
- Create unit system once and inject/pass it where needed
- Let type inference work - `CreateLength(5.0)` uses base units automatically  
- Use specific creation methods for compile-time safety: `CreateLength()`, `CreateAngle()`, etc.
- Use `CreateMeasuredValue()` or `CreateUnit<T>()` for generic/parser scenarios
- Mathematical operations return correct types automatically
- All units are stored internally in base units for consistent calculations
- Interface provides both type-safe creation and unit system flexibility