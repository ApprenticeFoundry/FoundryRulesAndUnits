# Foundry Rules and Units

## Overview

FoundryRulesAndUnits is a comprehensive, modernized unit system library providing type-safe unit conversions, measurement operations, and mathematical operations with automatic type inference. This library supports 6 complete unit systems (SI, MKS, CGS, FPS, IPS, mmNs) with 24+ unit families and advanced features for engineering and scientific applications.

**Current Version**: 9.2.0 | **Target**: .NET 9.0 | **Architecture**: UnitGroup injection with IUnitSystem interface

## 🚀 Key Features

### **Modern Architecture**
- **Unified IUnitSystem Interface**: Clean, dependency-injection friendly design
- **UnitGroup Injection**: Each MeasuredValue receives proper conversion logic via constructor
- **Type-Safe Creation**: Strongly-typed unit creation with compile-time safety
- **24+ Unit Types**: Complete coverage from Length/Mass to specialized units like Frequency/Resistance
- **Mathematical Operations**: Automatic type inference (Length × Length → Area, Mass × Acceleration → Force)
- **Zero Ambiguity Parser**: Two-tier unit family system eliminates parser conflicts

### **Unit Systems Supported**
- **SI** (International System of Units)
- **MKS** (Meter-Kilogram-Second)
- **CGS** (Centimeter-Gram-Second)  
- **FPS** (Foot-Pound-Second)
- **IPS** (Inch-Pound-Second)
- **mmNs** (Millimeter-Newton-Second)

### **24+ Unit Families**
- **Mechanical**: Length, Mass, Force, Speed, Power, Area, Volume
- **Thermal**: Temperature, Pressure  
- **Electrical**: Voltage, Current, Resistance, Capacitance
- **Digital**: DataStorage, DataFlow
- **Scientific**: Frequency, Time, Duration, Angle
- **Specialized**: Quantity, QuantityFlow, Percent, Dimensionless
- **Two-Tier Families**: Distance (function-only), Bearing (function-only), Time (function-only)

## 📦 Installation & Setup

### NuGet Package
```xml
<PackageReference Include="ApprenticeFoundryRulesAndUnits" Version="9.2.0" />
```

### Basic Setup
```csharp
using FoundryRulesAndUnits.Units;

// Create unit system (dependency injection friendly)
var unitSystem = IUnitSystem.MKS(); // or SI(), FPS(), IPS(), CGS()

// Alternative: Constructor approach
var unitSystem = new UnitSystem(UnitSystemType.MKS);

// Use throughout application via dependency injection or direct usage
```

## 💡 Quick Start Examples

### **Creating Measurements**
```csharp
var unitSystem = IUnitSystem.MKS(); // Create once, use everywhere

// Type-safe creation methods (recommended)
Length length = unitSystem.CreateLength(5.0, "m");
Temperature temp = unitSystem.CreateTemperature(25.0, "°C");  
Force force = unitSystem.CreateForce(100.0, "N");
Mass mass = unitSystem.CreateMass(50.0, "kg");
Speed speed = unitSystem.CreateSpeed(60.0, "mph");

// Generic creation (for parsers)
MeasuredValue parsed = unitSystem.CreateMeasuredValue(UnitFamilyName.Length, 5.0, "m");
```

### **Unit Conversions**
```csharp
var unitSystem = IUnitSystem.MKS();
var length = unitSystem.CreateLength(5.0, "m");

double feet = length.As("ft");        // Convert to feet
double inches = length.As("in");      // Convert to inches
string display = length.AsString("cm"); // "500 cm"

// Direct system conversion
double converted = unitSystem.Convert(100, "cm", "in"); // 39.37 inches
```

### **Arithmetic Operations**
```csharp
var unitSystem = IUnitSystem.MKS();
var length1 = unitSystem.CreateLength(10, "m");
var length2 = unitSystem.CreateLength(5, "ft");

var total = length1 + length2;         // Addition (same family)
var difference = length1 - length2;    // Subtraction  
var scaled = length1 * 2.0;           // Scalar multiplication
var ratio = length1 / length2;        // Ratio (returns double)
```

### **Advanced Operations with Type Inference**
```csharp
var unitSystem = IUnitSystem.MKS();

// Cross-family operations with automatic type inference
Length width = unitSystem.CreateLength(5, "m");
Length height = unitSystem.CreateLength(3, "m");
var area = width * height;              // Returns Area automatically!

Mass mass = unitSystem.CreateMass(100, "kg");
var acceleration = unitSystem.CreateAcceleration(9.8, "m/s2");  
var force = mass * acceleration;        // Returns Force automatically!

// Compare measurements
if (length1 > length2) {
    Console.WriteLine("Length1 is longer");
}
```

### **Two-Tier Family System (Zero Ambiguity)**
```csharp
var unitSystem = IUnitSystem.MKS();

// Parser-accessible families (primary - used by parsers)
Length length = unitSystem.CreateLength(5.0, "ft");     // Length family  
Angle angle = unitSystem.CreateAngle(45.0, "deg");      // Angle family
Duration duration = unitSystem.CreateDuration(30, "s"); // Duration family

// Function-only families (secondary - via AS functions)
// These prevent parser ambiguity but provide specialized functionality
Distance distance = unitSystem.CreateDistance(5.0, "ft");   // Distance family
Bearing bearing = unitSystem.CreateBearing(45.0, "deg");    // Bearing family  
Time time = unitSystem.CreateTime(30, "s");                 // Time family

// Key insight: Parser sees "ft" → Length, "deg" → Angle, "s" → Duration
// No ambiguity! Distance/Bearing/Time accessed via specific creation methods
```

**Two-Tier Benefits:**
- **Zero Parser Ambiguity**: Each unit symbol maps to exactly one family
- **Specialized Context**: Length vs Distance, Angle vs Bearing, Duration vs Time
- **Same Input Units**: Both tiers accept the same unit symbols
- **Clear Separation**: Parser-accessible vs function-only families

**Available Two-Tier Families:**
- **Length/Distance**: Engineering measurements vs geographic distances
- **Duration/Time**: Event timing vs precise scientific time  
- **Angle/Bearing**: Mathematical angles vs navigation bearings

## 🏗️ Architecture Overview

### **Unit Classes with UnitGroup Injection Architecture**
**ALL unit classes follow the modern UnitGroup injection pattern:**

**✅ Physical & Mechanical Units:**
- `Length`, `Mass`, `Temperature`, `Volume`, `Force`
- `Speed`, `Power`, `Area`, `Duration`, `Distance`  
- `Frequency`, `Time`

**✅ Electrical Units:**
- `Voltage`, `Resistance`, `Current`, `Capacitance`

**✅ Digital & Computing Units:**
- `DataStorage`, `DataFlow`

**✅ Specialized Units:**
- `Quantity`, `QuantityFlow`, `Percent`, `Dimensionless`
- `Angle`, `Bearing`

**🚀 ALL CLASSES FEATURE:**
- UnitGroup injection via constructor
- UnitTypeAttribute for registry lookup  
- Enhanced operators (+, -, *, /, >, <, ==, !=, etc.)
- System.Text.Json serialization support (.NET 9.0)
- Cross-family mathematical operations (Length × Length → Area)

### **Unit System Management**
```csharp
// Create unit systems (dependency injection friendly)
var mksSystem = IUnitSystem.MKS();
var siSystem = IUnitSystem.SI();
var fpsSystem = IUnitSystem.FPS();

// Switch unit systems dynamically
var system = IUnitSystem.MKS();
system.Apply(UnitSystemType.FPS);  // Switch to Imperial
var converted = system.Convert(100, "m", "ft"); // 328.084 feet

// Validation and metadata
bool isValid = system.IsValidUnit("mph");  // true
var units = system.GetUnitsForFamily(UnitFamilyName.Length); // ["m", "cm", "km", ...]
string baseUnit = system.GetBaseUnitForFamily(UnitFamilyName.Mass); // "kg" (MKS)
```

## 🔄 Modern Architecture

### **Dependency Injection Pattern**
The modern architecture supports clean dependency injection:

```csharp
// Service registration (e.g., in Program.cs or Startup.cs)
services.AddSingleton<IUnitSystem>(_ => IUnitSystem.MKS());

// Usage in classes
public class Calculator
{
    private readonly IUnitSystem _unitSystem;
    
    public Calculator(IUnitSystem unitSystem)
    {
        _unitSystem = unitSystem;
    }
    
    public Force CalculateForce(double mass, double acceleration)
    {
        var m = _unitSystem.CreateMass(mass, "kg");
        var a = _unitSystem.CreateAcceleration(acceleration, "m/s2");
        return m * a; // Returns Force automatically
    }
}
```

## 🚀 Parser Integration

### **Zero-Ambiguity Parser Support**

The two-tier family system eliminates parser ambiguity:

```csharp
public class UnitParser
{
    private readonly IUnitSystem _unitSystem;
    
    public UnitParser(IUnitSystem unitSystem)
    {
        _unitSystem = unitSystem;
    }
    
    public MeasuredValue Parse(string input)
    {
        var (value, unit) = ExtractValueAndUnit(input); // "100 cm" → 100, "cm"
        
        // Creates correct derived type automatically (Length, Angle, Mass, etc.)
        // Zero ambiguity: "cm" → Length, "deg" → Angle, "s" → Duration
        return _unitSystem.CreateMeasuredValueFromParsableUnit(unit, value);
    }
}
```

### **Key Benefits of Modern Architecture**
1. **Dependency Injection**: Clean, testable architecture via IUnitSystem interface
2. **Type Safety**: Compile-time checking with generic creation methods
3. **Performance**: UnitTypeRegistry caching eliminates reflection overhead
4. **Mathematical Operations**: Automatic type inference (Length × Length → Area)
5. **Zero Ambiguity**: Two-tier family system prevents parser conflicts

## 🧪 Testing & Validation

### **Build Status**
- ✅ **FoundryRulesAndUnits**: Builds successfully (3 warnings)
- ✅ **FoundryMentorModeler**: Builds successfully (3 warnings)  
- ✅ **All Tests Passing**: Legacy compatibility maintained

### **Validation Examples**
```csharp
// Unit system validation
var unitSystem = IUnitSystem.MKS();
Debug.Assert(unitSystem.IsValidUnit("kg"));
Debug.Assert(unitSystem.Convert(1000, "g", "kg") == 1.0);

// Measurement validation  
var length = unitSystem.CreateLength(1.0, "km");
Debug.Assert(Math.Abs(length.As("m") - 1000.0) < 0.001);

// Cross-family operations
var width = unitSystem.CreateLength(5, "m");
var height = unitSystem.CreateLength(3, "m"); 
var area = width * height; // Should be Area with 15 m² base value
Debug.Assert(area.GetType() == typeof(Area));
```

## 📁 Project Structure

```
FoundryRulesAndUnits/
├── UnitSystem/
│   ├── MeasuredValue.cs              # Base class with UnitGroup injection
│   ├── UnitSystem.cs                 # Complete IUnitSystem implementation  
│   ├── IUnitSystem.cs                # Modern interface with static factory methods
│   ├── UnitTypeRegistry.cs           # Performance-optimized type cache
│   ├── UnitFamilyName.cs             # Enum defining all unit families
│   ├── UnitTypeAttribute.cs          # Attribute for type registration
│   ├── Specifications/               # Unit system definitions  
│   │   ├── IUnitSystemSpecification.cs
│   │   ├── SIUnitSystemSpecification.cs    # SI unit definitions
│   │   ├── MKSUnitSystemSpecification.cs   # MKS unit definitions
│   │   └── [Other system specifications...]
│   └── UnitTypes/                    # Individual unit classes
│       ├── Length.cs                 # ✅ UnitGroup injection + UnitTypeAttribute
│       ├── Mass.cs                   # ✅ Enhanced operators + type safety
│       ├── Temperature.cs            # ✅ Cross-family operations
│       ├── Force.cs                  # ✅ Mathematical operations
│       └── [24+ other unit types...]
├── Extensions/
│   ├── BasicMath.cs                  # Mathematical utilities
│   ├── JsonUtilities.cs              # System.Text.Json support
│   └── [Other utility extensions...]
└── DataModels/                       # Supporting data structures
    ├── ContextWrapper.cs             # API response wrapper
    └── [Other data models...]
```

## 🤝 Contributing

### **Adding New Unit Types**
Follow the UnitGroup injection pattern:

```csharp
[System.Serializable]
[UnitType(UnitFamilyName.MyFamily, Description = "My unit description")]
public class MyUnit : MeasuredValue
{
    // UnitFamily comes from UnitTypeAttribute - no need for redundant property override
    
    /// <summary>
    /// Constructor with UnitGroup injection - preferred for factory pattern
    /// </summary>
    public MyUnit(UnitGroup unitGroup) : base(unitGroup)
    {
        if (unitGroup.Family != UnitFamilyName.MyFamily)
            throw new ArgumentException($"UnitGroup family must be {UnitFamilyName.MyFamily}");
    }
    
    // Required methods: Assign, Copy, operators
    public MyUnit Assign(double value, string? units)
    {
        Init(value, units);
        return this;
    }
    
    public MyUnit Copy()
    {
        var copy = new MyUnit(_unitGroup);
        copy.Init(Value(), Internal());
        return copy;
    }
    
    // Operators using UnitGroup pattern
    public static MyUnit operator +(MyUnit left, MyUnit right)
    {
        var result = new MyUnit(left._unitGroup);
        result.Init(left.Value() + right.Value(), left.Internal());
        return result;
    }
}
```

### **Adding to IUnitSystem Interface**
```csharp
// Add creation method to interface
MyUnit CreateMyUnit(double value = 0, string? units = null);

// Implement in UnitSystem class
public MyUnit CreateMyUnit(double value = 0, string? units = null)
{
    return CreateUnit<MyUnit>(value, units);
}
```

## 📄 License

MIT License - See LICENSE file for details.

## 🏢 Authors

- **Stephen Strong** - Apprentice Foundry
- **Company**: Stephen Strong  
- **Project URL**: https://apprenticefoundry.github.io/

---

### 🔗 Related Projects
- **FoundryMentorModeler**: Advanced modeling toolkit using this unit system
- **TRISoC Dashboard**: Digital twin dashboard with unit system integration

**Version**: 9.2.0 | **Target**: .NET 9.0 | **Updated**: October 2025