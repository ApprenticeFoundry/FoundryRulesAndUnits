# FoundryRulesAndUnits 10.10.0 Release Notes

**Release Date**: December 28, 2025  
**Package**: `ApprenticeFoundryRulesAndUnits` version 10.10.0  
**Target Framework**: .NET 9.0

---

## 🎯 What's New in 10.10.0

### **📏 New Unit: Rack Units (RU)**

Added support for **Rack Units (RU)** to the Length unit family across all six unit systems. Rack units are the standard measurement for server and network equipment height in data centers and telecommunications.

**Standard Conversion:**
- **1 RU = 1.75 inches = 44.45 mm**

---

## ✨ Key Features

### **Rack Unit Support**
Full integration of rack units into the unit system architecture:

```csharp
var unitSystem = new UnitSystem(UnitSystemType.SI);

// Create rack unit measurements
var serverHeight = unitSystem.CreateLength(2, "RU");      // 2U server
var totalRackSpace = unitSystem.CreateLength(42, "RU");   // Standard rack

// Convert to other length units
double inches = serverHeight.As("in");   // 3.5 inches
double mm = serverHeight.As("mm");       // 88.9 mm
double cm = serverHeight.As("cm");       // 8.89 cm

// Display rack units
Console.WriteLine(serverHeight.AsString("RU"));  // "2 rack-units"

// Arithmetic with rack units
var switch = unitSystem.CreateLength(1, "RU");
var router = unitSystem.CreateLength(2, "RU");
var combined = switch + router;  // 3 RU

// Convert from other units to rack units
var height = unitSystem.CreateLength(3.5, "in");
double rackUnits = height.As("RU");  // 2.0
```

### **Expression Parser Support**
Rack units work seamlessly with the variable declaration parser:

```csharp
// Simple syntax
var parser = VariableDeclarationParser.Parse("serverHeight|RU : 2 RU");
// Creates: serverHeight = 2 RU, displays in RU

// Explicit family syntax
var parser = VariableDeclarationParser.Parse("rackSpace:length(RU) : 3.5 in");
// Creates: rackSpace = 3.5 inches, displays as 2 RU

// Mixed unit calculations
var totalHeight = VariableDeclarationParser.Parse("total|RU : 88.9 mm");
// Creates: total = 88.9 mm, displays as 2 RU
```

### **All Unit Systems Supported**
Rack units are available in all six unit system specifications:

| Unit System | Base Unit | RU Conversion Factor |
|-------------|-----------|---------------------|
| **SI** (International) | meters | 1 RU = 0.04445 m |
| **MKS** (Meter-Kilogram-Second) | meters | 1 RU = 0.04445 m |
| **IPS** (Inch-Pound-Second) | inches | 1 RU = 1.75 in |
| **FPS** (Foot-Pound-Second) | feet | 1 RU = 0.145833 ft |
| **mmNs** (Millimeter-Newton-Second) | millimeters | 1 RU = 44.45 mm |
| **CGS** (Centimeter-Gram-Second) | centimeters | 1 RU = 4.445 cm |

---

## 🏗️ Architecture Highlights

### **Seamless Integration**
- **No Breaking Changes**: RU is added as a standard Length unit
- **Automatic Conversion**: Works with all existing Length unit operations
- **Cross-System Support**: Available in all 6 unit systems (SI, MKS, IPS, FPS, CGS, mmNs)
- **Parser Ready**: Fully supported in expression parser with both simple and explicit syntax

### **Data Center Applications**
Perfect for:
- Server rack space planning and allocation
- Network equipment height specifications
- Data center capacity management
- Telecommunications equipment sizing
- IT infrastructure planning

---

## 📦 Use Cases

### **Data Center Management**
```csharp
var unitSystem = IUnitSystem.MKS();

// Plan server deployment
var available = unitSystem.CreateLength(42, "RU");  // Standard rack
var server2U = unitSystem.CreateLength(2, "RU");
var server4U = unitSystem.CreateLength(4, "RU");
var switch1U = unitSystem.CreateLength(1, "RU");

var used = server2U + server4U + switch1U;  // 7 RU
var remaining = available - used;  // 35 RU

Console.WriteLine($"Rack utilization: {used.As("RU")} of {available.As("RU")} RU");
Console.WriteLine($"Remaining space: {remaining.AsString("RU")}");
```

### **Equipment Specifications**
```csharp
var unitSystem = IUnitSystem.IPS();

// Convert manufacturer specs
var heightInches = unitSystem.CreateLength(3.5, "in");
var heightRU = heightInches.As("RU");  // 2.0 RU

// Verify rack compatibility
var rackHeight = unitSystem.CreateLength(73.5, "in");  // 42 RU standard
var rackUnits = rackHeight.As("RU");  // 42.0

Console.WriteLine($"Equipment height: {heightRU} RU");
Console.WriteLine($"Rack capacity: {rackUnits} RU");
```

### **Mixed Unit Calculations**
```csharp
var unitSystem = IUnitSystem.mmNs();

// Engineering specs in millimeters
var deviceHeight = unitSystem.CreateLength(88.9, "mm");
var rackUnits = deviceHeight.As("RU");  // 2.0

// Combine with other measurements
var clearance = unitSystem.CreateLength(5, "mm");
var totalSpace = deviceHeight + clearance;

Console.WriteLine($"Device: {rackUnits} RU ({deviceHeight.As("mm")} mm)");
Console.WriteLine($"With clearance: {totalSpace.As("mm")} mm");
```

---

## 🚀 Upgrade Instructions

### From 10.9.x:
```xml
<PackageReference Include="ApprenticeFoundryRulesAndUnits" Version="10.10.0" />
```

### Compatibility:
- ✅ **Fully backward compatible** with 10.9.x
- ✅ **No breaking changes** - existing code continues to work
- ✅ **New functionality** - RU unit available immediately
- ✅ **.NET 9.0** target framework

---

## 🎉 Benefits of This Release

### **For Data Center Professionals**
- Native rack unit support eliminates manual conversion errors
- Seamless integration with existing measurement workflows
- Accurate space planning with automatic unit conversions

### **For Developers**
- Type-safe rack unit operations with compile-time checking
- Expression parser support for configuration files
- Consistent API across all length units

### **For System Architects**
- Standardized equipment height specifications
- Cross-system compatibility (metric and imperial)
- Future-proof infrastructure planning tools

---

## 📋 Complete Feature List

### **Length Family Units** (Updated)
- meters (m), centimeters (cm), millimeters (mm), micrometers (μm), nanometers (nm)
- kilometers (km), feet (ft), inches (in), yards (yd), miles (mi)
- **rack-units (RU)** ⭐ NEW!
- pixels (px), mils (mil), angstroms (Å)

### **All Previous Features**
- 27 unit families including Currency, CostPerQuantity, CostPerTime (v10.9.0)
- 6 complete unit systems (SI, MKS, CGS, FPS, IPS, mmNs)
- Mathematical operations with type inference
- ContextWrapper factory methods (v10.7.0)
- StatusBitArray for state management (v10.8.x)
- Two-tier family system for zero ambiguity

---

## 📁 Technical Details

### **Unit Definition Format**
```csharp
// Example from SIUnitSystemSpecification.cs
UnitDefinition.LinearUnit("RU", "rack-units", UnitFamilyName.Length, 0.04445)
// 1 RU = 0.04445 m (SI base unit)

// Example from IPSUnitSystemSpecification.cs  
UnitDefinition.LinearUnit("RU", "rack-units", UnitFamilyName.Length, 1.75)
// 1 RU = 1.75 in (IPS base unit)
```

### **Files Modified**
- `UnitSystem/Specifications/SIUnitSystemSpecification.cs`
- `UnitSystem/Specifications/MKSUnitSystemSpecification.cs`
- `UnitSystem/Specifications/IPSUnitSystemSpecification.cs`
- `UnitSystem/Specifications/FPSUnitSystemSpecification.cs`
- `UnitSystem/Specifications/mmNsUnitSystemSpecification.cs`
- `UnitSystem/Specifications/CGSUnitSystemSpecification.cs`

---

## 📞 Support & Resources

- **GitHub**: [FoundryRulesAndUnits Repository](https://github.com/ApprenticeFoundry/FoundryRulesAndUnits)
- **Documentation**: [Apprentice Foundry](https://apprenticefoundry.github.io/)
- **NuGet**: [ApprenticeFoundryRulesAndUnits](https://www.nuget.org/packages/ApprenticeFoundryRulesAndUnits/)

---

**Version**: 10.10.0 | **Target**: .NET 9.0 | **Updated**: December 28, 2025
