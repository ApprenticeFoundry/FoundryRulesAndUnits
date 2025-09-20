# Foundry Rules and Units

## Overview

FoundryRulesAndUnits is a comprehensive, modernized unit system library providing type-safe unit conversions, measurement operations, and full backward compatibility. This library supports 6 complete unit systems (SI, MKS, CGS, FPS, IPS, mmNs) with 39+ unit families and advanced features for engineering and scientific applications.

## 🚀 Key Features

### **Modern Architecture**
- **Global Unit System Service**: Centralized, singleton-based unit management
- **Type-Safe Conversions**: Strongly-typed unit operations with compile-time safety
- **24+ Unit Types**: Complete coverage from Length/Mass to specialized units like Frequency/Resistance
- **Factory Methods**: Intuitive construction with `Length.FromMeters(5.0)`, `Temperature.FromCelsius(25)`
- **Full Operator Support**: Natural arithmetic with `length1 + length2`, `force * scalar`

### **Unit Systems Supported**
- **SI** (International System of Units)
- **MKS** (Meter-Kilogram-Second)
- **CGS** (Centimeter-Gram-Second)  
- **FPS** (Foot-Pound-Second)
- **IPS** (Inch-Pound-Second)
- **mmNs** (Millimeter-Newton-Second)

### **39+ Unit Families**
- **Mechanical**: Length, Mass, Force, Speed, Power, Area, Volume
- **Thermal**: Temperature, Pressure
- **Electrical**: Voltage, Current, Resistance, Capacitance
- **Digital**: DataStorage, DataFlow
- **Scientific**: Frequency, Time, Duration, Angle
- **Specialized**: Quantity, QuantityFlow, Percent, Dimensionless

## 📦 Installation & Setup

### NuGet Package
```xml
<PackageReference Include="ApprenticeFoundryRulesAndUnits" Version="8.0.0" />
```

### Basic Setup
```csharp
using FoundryRulesAndUnits.Units;

// Initialize the unit system (do this once at app startup)
var unitService = UnitSystemService.Instance;
unitService.SetUnitSystem(UnitSystemType.SI); // or MKS, CGS, FPS, IPS, mmNs
```

## 💡 Quick Start Examples

### **Creating Measurements**
```csharp
// Factory methods (recommended)
var length = Length.FromMeters(5.0);
var temp = Temperature.FromCelsius(25.0);
var force = Force.FromNewtons(100.0);

// Constructor approach
var mass = new Mass(50.0, "kg");
var speed = new Speed(60.0, "mph");
```

### **Unit Conversions**
```csharp
var length = Length.FromMeters(5.0);
double feet = length.As("ft");        // Convert to feet
double inches = length.As("in");      // Convert to inches
double pixels = length.AsPixels();    // Special conversion for UI
```

### **Arithmetic Operations**
```csharp
var length1 = Length.FromMeters(10);
var length2 = Length.FromFeet(5);

var total = length1 + length2;         // Addition
var difference = length1 - length2;    // Subtraction
var scaled = length1 * 2.0;           // Scalar multiplication
var ratio = length1 / length2;        // Ratio (returns double)
```

### **Advanced Operations**
```csharp
// Create flow rates from quantities and time
var quantity = Quantity.FromEach(1000);
var time = Time.FromSeconds(60);
var flow = quantity / time;            // QuantityFlow in "ea/s"

// Compare measurements
if (length1 > length2) {
    Console.WriteLine("Length1 is longer");
}
```

## 🏗️ Architecture Overview

### **Modern Unit Classes (24/24 FULLY MODERNIZED) 🎉**
**ALL unit classes now follow the clean, modern pattern:**

**✅ Physical & Mechanical Units:**
- `Length`, `Mass`, `Temperature`, `Volume`, `Force`
- `Speed`, `Power`, `Area`, `Time`, `Duration`
- `Distance`, `Frequency`

**✅ Electrical Units:**
- `Voltage`, `Resistance`, `Current`, `Capacitance`

**✅ Digital & Computing Units:**
- `DataStorage`, `DataFlow`

**✅ Specialized Units:**
- `Quantity`, `QuantityFlow`, `Percent`, `Heading`, `Dimensionless`

**🚀 ALL CLASSES FEATURE:**
- Factory methods (`FromMeters()`, `FromVolts()`, etc.)
- Enhanced operators (+, -, *, /, >, <, etc.)
- JSON serialization support
- Full backward compatibility

### **Unit System Service**
```csharp
// Global service manages all conversions
var service = UnitSystemService.Instance;

// Switch unit systems at runtime
service.SetUnitSystem(UnitSystemType.FPS);  // Switch to Imperial
var converted = service.Convert(100, "m", "ft"); // 328.084 feet

// Validation
bool isValid = service.IsValidUnit("mph");  // true
var units = service.GetUnitsForFamily(UnitFamilyName.Length); // ["m", "cm", "km", ...]
```

## 🔄 Backward Compatibility

### **Legacy Support Classes**
For existing codebases, we provide full compatibility:

```csharp
// Legacy UnitSystem class (wraps modern service)
var unitSystem = new UnitSystem(UnitSystemType.MKS);
unitSystem.Apply(UnitSystemType.SI);
var categories = unitSystem.Categories();

// Legacy extension methods
var family = UnitCategoryExtensions.GetUnitFamily("kg");  // UnitFamilyName.Mass
bool known = UnitCategoryExtensions.IsKnownUnit("mph");   // true

// Legacy Length methods
var length = new Length(100, "m");
var category = Length.Category();        // UnitCategory
double pixels = length.AsPixels();       // UI conversion
```

## 🚀 Migration Guide

### **From Legacy to Modern**

**Old Pattern:**
```csharp
// Legacy approach
var length = new Length(5.0, "m");
var convertedValue = length.As("ft");
```

**New Pattern:**
```csharp
// Modern approach
var length = Length.FromMeters(5.0);    // Factory method
var convertedValue = length.As("ft");   // Same conversion API
```

### **Key Benefits of Modern Classes**
1. **Factory Methods**: `Length.FromMeters()` vs `new Length(value, "m")`
2. **Enhanced Operators**: Full arithmetic support including scalar operations
3. **Better Performance**: Direct integration with UnitSystemService
4. **JSON Serialization**: Built-in JSON converter support
5. **Cleaner Code**: No UnitCategory dependencies

## 🧪 Testing & Validation

### **Build Status**
- ✅ **FoundryRulesAndUnits**: Builds successfully (3 warnings)
- ✅ **FoundryMentorModeler**: Builds successfully (3 warnings)  
- ✅ **All Tests Passing**: Legacy compatibility maintained

### **Validation Examples**
```csharp
// Unit system validation
var service = UnitSystemService.Instance;
Debug.Assert(service.IsValidUnit("kg"));
Debug.Assert(service.Convert(1000, "g", "kg") == 1.0);

// Measurement validation  
var length = Length.FromKilometers(1.0);
Debug.Assert(Math.Abs(length.As("m") - 1000.0) < 0.001);
```

## 📁 Project Structure

```
FoundryRulesAndUnits/
├── UnitSystem/
│   ├── UnitSystemService.cs          # Global conversion service
│   ├── MeasuredValue.cs              # Base class for all units
│   ├── UnitCategory.cs               # Legacy compatibility (+ enum)
│   ├── UnitSystem.cs                 # Legacy wrapper class
│   ├── Specifications/               # Unit system definitions
│   │   ├── IUnitSystemSpecification.cs
│   │   ├── SISpecification.cs        # SI unit definitions
│   │   ├── MKSSpecification.cs       # MKS unit definitions
│   │   └── [Other system specs...]
│   └── UnitTypes/                    # Individual unit classes
│       ├── Length.cs                 # ✅ Modernized
│       ├── Mass.cs                   # ✅ Modernized  
│       ├── Temperature.cs            # ✅ Modernized
│       ├── Force.cs                  # ✅ Modernized
│       └── [24+ other unit types...]
├── Extensions/
│   ├── UnitCategoryExtensions.cs     # Legacy compatibility helpers
│   └── [Other utility extensions...]
└── DataModels/                       # Supporting data structures
```

## 🤝 Contributing

### **Adding New Unit Types**
Follow the modernized pattern:

```csharp
[System.Serializable]
[JsonConverter(typeof(MyUnitJsonConverter))]
public class MyUnit : MeasuredValue
{
    public MyUnit() : base(UnitFamilyName.MyFamily) { }
    
    public MyUnit(double value, string? units = null) : base(UnitFamilyName.MyFamily)
    {
        Init(value, units);
    }
    
    // Factory methods
    public static MyUnit FromBaseUnit(double value) => new(value, "base");
    
    // Operators
    public static MyUnit operator +(MyUnit left, MyUnit right) => 
        new(left.Value() + right.Value(), left.Internal());
}
```

### **Extending Unit Systems**
Add new specifications implementing `IUnitSystemSpecification`:

```csharp
public class MyCustomSpecification : IUnitSystemSpecification
{
    public string SystemName => "MySystem";
    // Implement interface methods...
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

**Version**: 8.0.0 | **Target**: .NET 9.0 | **Updated**: September 2025