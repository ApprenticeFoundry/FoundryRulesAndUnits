# Unit System Refactoring Plan

## Overview
This document outlines a comprehensive plan to refactor the FoundryRulesAndUnits unit system to properly implement all five unit systems (IPS, FPS, MKS, CGS, mmNs) with enhanced flexibility, extensibility, and **critical base unit accuracy**.

## Current State Assessment

### Problems Identified
- ❌ **CRITICAL**: All unit systems (`IPS()`, `FPS()`, `MKS()`, `CGS()`, `MMNs()`) call the same `EstablishCommonUnit()` method
- ❌ **CRITICAL**: No differentiation between unit systems - all use metric base with mixed imperial units
- ❌ **ACCURACY ISSUE**: Base unit storage not properly implemented - values may be stored with conversion losses
- ❌ Missing core physical quantities (Mass, Force, Pressure, etc.)
- ❌ Limited extensibility for custom units or new unit systems

### Current Implementation
- ✅ Solid foundation with `UnitCategory` and `UnitSpec` classes
- ✅ Functional conversion system between units
- ✅ Good separation of concerns with `UnitCategoryService`
- ✅ Support for complex conversions (temperature, angles)
- ⚠️ **BUT**: Current `EstablishCommonUnit()` essentially implements MKS with some imperial conversions

### Critical Base Unit Accuracy Requirements
**KEY INSIGHT**: Each unit system must store values in its **native base units** without conversion to avoid cumulative errors and precision loss.

#### Example of Current Problem:
```csharp
// Current approach - ALL systems use meters as base (WRONG for IPS/FPS)
length = new UnitCategory("Length", new UnitSpec("m", "meters", UnitFamilyName.Length))
    .Units("in", "inches")
    .Conversion(39.3701, "in", 1, "m")  // Inches derived from meters - accuracy loss!
```

#### Required Solution:
```csharp
// IPS System - inches as TRUE base unit
length = new UnitCategory("Length", new UnitSpec("in", "inches", UnitFamilyName.Length))
    .Units("ft", "feet")
    .Conversion(1, "ft", 12, "in")      // EXACT: 1 ft = 12 in
    .Units("m", "meters")
    .Conversion(0.0254, "m", 1, "in")   // EXACT by international definition

// MKS System - meters as TRUE base unit  
length = new UnitCategory("Length", new UnitSpec("m", "meters", UnitFamilyName.Length))
    .Units("mm", "millimeters")
    .Conversion(1000, "mm", 1, "m")     // EXACT: 1000 mm = 1 m
    .Units("in", "inches")
    .Conversion(39.3700787402, "in", 1, "m")  // HIGH PRECISION conversion
```

## Architecture Decision: Hybrid Strategy with Base Unit Integrity

After analyzing code reuse strategies and base unit accuracy requirements, we've chosen:

### **Primary Strategy: Fluent Extension Methods + Template Method Pattern**

**Rationale:**
1. **Builds on Existing Patterns**: Leverages the successful fluent API (`.Units().Conversion()`) already in use
2. **Minimal Code Changes**: Evolutionary rather than revolutionary approach
3. **Base Unit Accuracy**: Ensures each system stores values in native base units
4. **Maximum Code Reuse**: Extension methods eliminate 70-80% of duplication
5. **Backward Compatible**: Existing `EstablishCommonUnit()` becomes MKS implementation

### **Code Reuse Strategy Details:**

#### **Extension Methods for Common Patterns:**
```csharp
public static class UnitCategoryExtensions
{
    // Reusable metric length units (adaptable to any metric base)
    public static UnitCategory AddMetricLengthUnits(this UnitCategory category, string baseUnit = "m")
    {
        if (baseUnit == "m")
        {
            return category
                .Units("mm", "millimeters").Conversion(1000, "mm", 1, "m")
                .Units("cm", "centimeters").Conversion(100, "cm", 1, "m")
                .Units("km", "kilometers").Conversion(1, "km", 1000, "m");
        }
        else if (baseUnit == "mm")
        {
            return category
                .Units("cm", "centimeters").Conversion(1, "cm", 10, "mm")
                .Units("m", "meters").Conversion(0.001, "m", 1, "mm")
                .Units("km", "kilometers").Conversion(0.000001, "km", 1, "mm");
        }
        return category;
    }
    
    // Reusable imperial length units (adaptable to any imperial base)
    public static UnitCategory AddImperialLengthUnits(this UnitCategory category, string baseUnit = "in")
    {
        if (baseUnit == "in")
        {
            return category
                .Units("ft", "feet").Conversion(1, "ft", 12, "in")      // EXACT
                .Units("yd", "yards").Conversion(1, "yd", 36, "in")     // EXACT
                .Units("mi", "miles").Conversion(1, "mi", 63360, "in"); // EXACT
        }
        else if (baseUnit == "ft")
        {
            return category
                .Units("in", "inches").Conversion(12, "in", 1, "ft")    // EXACT
                .Units("yd", "yards").Conversion(1, "yd", 3, "ft")      // EXACT
                .Units("mi", "miles").Conversion(1, "mi", 5280, "ft");  // EXACT
        }
        return category;
    }
    
    // Cross-system conversions with high precision
    public static UnitCategory AddCrossSystemConversions(this UnitCategory category)
    {
        var baseUnit = category.BaseUnit;
        
        if (baseUnit == "m") // Metric base - add imperial conversions
        {
            return category
                .Units("in", "inches").Conversion(39.3700787402, "in", 1, "m")  // HIGH PRECISION
                .Units("ft", "feet").Conversion(3.28083989501, "ft", 1, "m");   // HIGH PRECISION
        }
        else if (baseUnit == "in") // Imperial base - add metric conversions
        {
            return category
                .Units("mm", "millimeters").Conversion(25.4, "mm", 1, "in")     // EXACT by definition
                .Units("cm", "centimeters").Conversion(2.54, "cm", 1, "in")     // EXACT by definition
                .Units("m", "meters").Conversion(0.0254, "m", 1, "in");         // EXACT by definition
        }
        else if (baseUnit == "ft") // Foot base - add metric conversions
        {
            return category
                .Units("m", "meters").Conversion(0.3048, "m", 1, "ft")          // EXACT (12 × 0.0254)
                .Units("cm", "centimeters").Conversion(30.48, "cm", 1, "ft");   // EXACT
        }
        
        return category;
    }
}
```

### **Base Unit Accuracy Principles:**

#### **1. Native Storage Principle**
- **IPS System**: ALL values stored as inches internally (no meter conversion)
- **FPS System**: ALL values stored as feet internally (no meter conversion)
- **MKS System**: ALL values stored as meters internally (no inch conversion)
- **CGS System**: ALL values stored as centimeters internally
- **mmNs System**: ALL values stored as millimeters internally

#### **2. Exact Conversions Within System Family**
```csharp
// Imperial system - these MUST be exact (no floating point errors)
1 foot = 12 inches (exactly)
1 yard = 36 inches (exactly)  
1 mile = 63,360 inches (exactly)
1 mil = 0.001 inches (exactly)

// Metric system - these MUST be exact
1 meter = 1000 millimeters (exactly)
1 meter = 100 centimeters (exactly)
1 kilometer = 1000 meters (exactly)
```

#### **3. High Precision Cross-System Conversions**
```csharp
// International legal definitions (exact by international treaty)
1 inch = 25.4 millimeters (exactly, since 1959)
1 pound = 0.45359237 kilograms (exactly, since 1958)

// Derived high-precision conversions (calculated from exact definitions)
1 meter = 39.3700787402 inches (calculated from 1000/25.4)
1 foot = 0.3048 meters (exactly, 12 × 0.0254)
```

## Implementation Phases

## Phase 1: Base Unit Accuracy & System Differentiation 🎯 **Priority: CRITICAL**

### Goals
- **CRITICAL**: Ensure each unit system stores values in native base units
- Implement distinct unit systems with appropriate base units
- Maintain backward compatibility
- Establish proper unit system differentiation
- Create extension methods for code reuse

### Tasks

#### 1.1 Create Extension Methods for Code Reuse
- [ ] **Create UnitCategoryExtensions.cs**
  - `AddMetricLengthUnits()` - reusable metric length units
  - `AddImperialLengthUnits()` - reusable imperial length units  
  - `AddCrossSystemConversions()` - high-precision cross-system conversions
  - `AddTemperatureConversions()` - temperature conversion methods
  - `AddMassUnits()` - mass/weight unit methods

#### 1.2 Add Missing Unit Categories with Proper Base Units
- [ ] **Mass/Weight Category**
  - IPS/FPS: pounds as base (exact: 1 lb = 16 oz, 1 ton = 2000 lb)
  - MKS: kilograms as base (exact: 1 kg = 1000 g)
  - CGS: grams as base (exact: 1 g = 1000 mg)
  - Cross-conversions: 1 lb = 0.45359237 kg (exact by definition)
  
- [ ] **Force Category**
  - IPS/FPS: pound-force (lbf) as base
  - MKS/mmNs: newtons (N) as base  
  - CGS: dynes as base (1 N = 100,000 dyne exactly)
  - Conversions: 1 lbf = 4.4482216152605 N (exact)

- [ ] **Time Category** (system-independent)
  - Base unit: seconds (all systems)
  - Exact conversions: 1 min = 60 s, 1 hr = 3600 s, 1 day = 86400 s

#### 1.3 Implement System-Specific Methods with Native Base Units

##### 1.3.1 Refactor MKS System (Current EstablishCommonUnit)
```csharp
private bool MKS()
{
    return EstablishMKSUnits(); // Rename EstablishCommonUnit -> EstablishMKSUnits
}

private bool EstablishMKSUnits()
{
    // Length: METERS as true base unit
    length = new UnitCategory("Length", new UnitSpec("m", "meters", UnitFamilyName.Length))
        .AddMetricLengthUnits("m")        // mm, cm, km with exact conversions
        .AddCrossSystemConversions();     // in, ft with high precision
    
    // Mass: KILOGRAMS as true base unit
    var mass = new UnitCategory("Mass", new UnitSpec("kg", "kilograms", UnitFamilyName.Mass))
        .AddMetricMassUnits("kg")         // g, mg with exact conversions
        .AddCrossSystemConversions();     // lb, oz with high precision
    
    // Temperature: CELSIUS as base unit
    var temp = new UnitCategory("Temperature", new UnitSpec("C", "Celsius", UnitFamilyName.Temperature))
        .AddTemperatureConversions();     // F, K with exact formulas
    
    EstablishSystemIndependentCategories(); // angle, storage, worktime, quantity
    RegisterAllCategories();
    return true;
}
```

##### 1.3.2 Implement IPS System (Inch-Pound-Second)
```csharp
private bool IPS()
{
    return EstablishIPSUnits();
}

private bool EstablishIPSUnits()
{
    // Length: INCHES as true base unit (native storage)
    length = new UnitCategory("Length", new UnitSpec("in", "inches", UnitFamilyName.Length))
        .AddImperialLengthUnits("in")     // ft, yd, mi with exact conversions
        .AddCrossSystemConversions()      // mm, cm, m with exact definitions
        .Units("mil", "mils")
        .Conversion(1000, "mil", 1, "in") // Exact: 1000 mil = 1 in
        .Units("px", "pixels")
        .Conversion(96, "px", 1, "in");   // Standard 96 DPI
    
    // Mass: POUNDS as true base unit (native storage)
    var mass = new UnitCategory("Mass", new UnitSpec("lb", "pounds", UnitFamilyName.Mass))
        .AddImperialMassUnits("lb")       // oz, ton with exact conversions
        .AddCrossSystemConversions();     // kg, g with exact definitions
    
    // Temperature: FAHRENHEIT as base unit (native storage)
    var temp = new UnitCategory("Temperature", new UnitSpec("F", "Fahrenheit", UnitFamilyName.Temperature))
        .AddTemperatureConversions();     // C, K with exact formulas
    
    EstablishSystemIndependentCategories();
    RegisterAllCategories();
    return true;
}
```

##### 1.3.3 Implement FPS System (Foot-Pound-Second)
```csharp
private bool FPS()
{
    return EstablishFPSUnits();
}

private bool EstablishFPSUnits()
{
    // Length: FEET as true base unit (native storage)
    length = new UnitCategory("Length", new UnitSpec("ft", "feet", UnitFamilyName.Length))
        .AddImperialLengthUnits("ft")     // in, yd, mi with exact conversions
        .AddCrossSystemConversions()      // m, cm with exact definitions
        .Units("px", "pixels")
        .Conversion(96*12, "px", 1, "ft"); // 1152 pixels per foot at 96 DPI
    
    // Mass: POUNDS as true base unit (same as IPS)
    var mass = new UnitCategory("Mass", new UnitSpec("lb", "pounds", UnitFamilyName.Mass))
        .AddImperialMassUnits("lb")
        .AddCrossSystemConversions();
    
    // Temperature: FAHRENHEIT as base unit (same as IPS)
    var temp = new UnitCategory("Temperature", new UnitSpec("F", "Fahrenheit", UnitFamilyName.Temperature))
        .AddTemperatureConversions();
    
    EstablishSystemIndependentCategories();
    RegisterAllCategories();
    return true;
}
```

##### 1.3.4 Implement CGS System (Centimeter-Gram-Second)
```csharp
private bool CGS()
{
    return EstablishCGSUnits();
}

private bool EstablishCGSUnits()
{
    // Length: CENTIMETERS as true base unit (native storage)
    length = new UnitCategory("Length", new UnitSpec("cm", "centimeters", UnitFamilyName.Length))
        .AddMetricLengthUnits("cm")       // mm, m, km with exact conversions
        .AddCrossSystemConversions();     // in, ft with high precision
    
    // Mass: GRAMS as true base unit (native storage)
    var mass = new UnitCategory("Mass", new UnitSpec("g", "grams", UnitFamilyName.Mass))
        .AddMetricMassUnits("g")          // mg, kg with exact conversions
        .AddCrossSystemConversions();     // lb, oz with high precision
    
    // Force: DYNES as true base unit (CGS specific)
    var force = new UnitCategory("Force", new UnitSpec("dyne", "dynes", UnitFamilyName.Force))
        .Units("N", "newtons")
        .Conversion(0.00001, "N", 1, "dyne"); // Exact: 1 N = 100,000 dyne
    
    // Temperature: CELSIUS as base unit
    var temp = new UnitCategory("Temperature", new UnitSpec("C", "Celsius", UnitFamilyName.Temperature))
        .AddTemperatureConversions();
    
    EstablishSystemIndependentCategories();
    RegisterAllCategories();
    return true;
}
```

##### 1.3.5 Implement mmNs System (Millimeter-Newton-Second)
```csharp
private bool MMNs()
{
    return EstablishMMNsUnits();
}

private bool EstablishMMNsUnits()
{
    // Length: MILLIMETERS as true base unit (native storage)
    length = new UnitCategory("Length", new UnitSpec("mm", "millimeters", UnitFamilyName.Length))
        .AddMetricLengthUnits("mm")       // cm, m, km with exact conversions
        .AddCrossSystemConversions()      // in, ft with high precision
        .Units("μm", "micrometers")
        .Conversion(1000, "μm", 1, "mm"); // Exact: 1000 μm = 1 mm
    
    // Force: NEWTONS as true base unit (same as MKS)
    var force = new UnitCategory("Force", new UnitSpec("N", "newtons", UnitFamilyName.Force))
        .Units("kN", "kilonewtons")
        .Conversion(1, "kN", 1000, "N")   // Exact: 1 kN = 1000 N
        .Units("dyne", "dynes")
        .Conversion(100000, "dyne", 1, "N"); // Exact: 100,000 dyne = 1 N
    
    // Mass: GRAMS as base unit (derived from force via F=ma)
    var mass = new UnitCategory("Mass", new UnitSpec("g", "grams", UnitFamilyName.Mass))
        .AddMetricMassUnits("g")
        .AddCrossSystemConversions();
    
    // Temperature: CELSIUS as base unit
    var temp = new UnitCategory("Temperature", new UnitSpec("C", "Celsius", UnitFamilyName.Temperature))
        .AddTemperatureConversions();
    
    EstablishSystemIndependentCategories();
    RegisterAllCategories();
    return true;
}
```

#### 1.4 Create System-Independent Categories Method
```csharp
private void EstablishSystemIndependentCategories()
{
    // Angles - same for all systems (radians/degrees)
    angle = new UnitCategory("Angle", new UnitSpec("rad", "radians", UnitFamilyName.Angle))
        .Units("deg", "degrees")
        .Conversion("deg", "rad", v => Math.PI * v / 180.0)
        .Conversion("rad", "deg", v => 180.0 * v / Math.PI);

    // Data Storage - same for all systems (bytes-based)
    storage = new UnitCategory("DataStorage", new UnitSpec("KB", "KiloBytes", UnitFamilyName.DataStorage))
        .Units("GB", "GigaBytes").Conversion(1000, "KB", 1, "GB")
        .Units("TB", "TeraBytes").Conversion(1000000, "KB", 1, "TB")
        .Units("Bytes", "Bytes").Conversion(1000, "Bytes", 1, "KB");

    // Work Time - same for all systems (hours-based)  
    worktime = new UnitCategory("WorkTime", new UnitSpec("Hrs", "Hours", UnitFamilyName.WorkTime))
        .Units("Days", "Days").Conversion(24, "Hrs", 1, "Days")
        .Units("Mins", "Minutes").Conversion(60, "Mins", 1, "Hrs")
        .Units("Wdays", "WorkDays").Conversion(5.0, "Days", 1.0, "Wdays")
        .Units("Wks", "Weeks").Conversion(7.0, "Days", 1.0, "Wks");
        
    // Quantity - same for all systems (each-based)
    var quantity = new UnitCategory("Quantity", new UnitSpec("ea", "each", UnitFamilyName.Quantity))
        .Units("dz", "dozen").Conversion(1, "dz", 12, "ea")
        .Units("gr", "gross").Conversion(1, "gr", 144, "ea");
}
```

#### 1.5 Add Current System Tracking
```csharp
public class UnitSystem : IUnitSystem
{
    // Track active system for base unit accuracy
    public UnitSystemType ActiveSystem { get; private set; } = UnitSystemType.MKS;
    
    public bool Apply(UnitSystemType type)
    {
        var success = type switch
        {
            UnitSystemType.IPS => IPS(),
            UnitSystemType.FPS => FPS(),
            UnitSystemType.MKS => MKS(),
            UnitSystemType.CGS => CGS(),
            UnitSystemType.mmNs => MMNs(),
            _ => throw new NotImplementedException(),
        };
        
        if (success)
        {
            ActiveSystem = type;
        }
        
        return success;
    }
}
```

#### 1.6 Fix Existing Bugs
- [ ] **Volume units bug**: Line 150 has `"km2"` instead of `"km3"` for cubic kilometers
- [ ] **Quantity spelling**: `"quanity"` should be `"quantity"` throughout
- [ ] **QuantityFlow conversion bug**: Line 167 has incorrect `"dz/hr"` conversion

#### 1.7 Testing Phase 1
- [ ] **Base Unit Accuracy Tests**: Verify values stored in correct native units
- [ ] **Cross-System Precision Tests**: Verify high-precision conversions
- [ ] **Within-System Exact Tests**: Verify exact conversions (12 in = 1 ft exactly)
- [ ] **Unit System Differentiation Tests**: Verify each system uses correct base units
- [ ] **Backward Compatibility Tests**: Ensure existing code continues to work

---

## Phase 2: Enhanced Unit Categories with Derived Units 🎯 **Priority: Medium**

### Goals
- Add missing physical quantity categories with proper base units per system
- Implement derived units (velocity, acceleration, etc.) that automatically adapt to base units
- Enhance existing categories with system-appropriate units

### Tasks

#### 2.1 Add Advanced Physical Quantities with System-Specific Base Units
- [ ] **Velocity/Speed**
  - IPS: in/s as base, with in/min, ft/s, mph
  - FPS: ft/s as base, with in/s, ft/min, mph  
  - MKS: m/s as base, with km/h, cm/s
  - CGS: cm/s as base, with m/s, km/h
  - mmNs: mm/s as base, with m/s, cm/s

- [ ] **Acceleration**
  - IPS: in/s² as base, with ft/s², g-force conversions
  - FPS: ft/s² as base, with in/s², g-force conversions
  - MKS: m/s² as base, with cm/s², g-force (9.80665 m/s²)
  - CGS: cm/s² as base, with gal units
  - mmNs: mm/s² as base, with m/s², cm/s²

- [ ] **Pressure with System-Appropriate Base Units**
  - IPS/FPS: psi as base, with psf, atm, bar conversions
  - MKS: Pa (N/m²) as base, with kPa, MPa, bar, atm
  - CGS: Ba (dyne/cm²) as base, with Pa conversions
  - mmNs: kPa as base (more practical than Pa)

- [ ] **Energy with System-Appropriate Base Units**
  - IPS/FPS: ft⋅lbf as base, with in⋅lbf, BTU, cal
  - MKS: Joules (N⋅m) as base, with kJ, kWh, cal
  - CGS: erg (dyne⋅cm) as base, with J conversions
  - mmNs: mJ as base (more practical scale)

#### 2.2 Implement Smart Derived Category Creation
```csharp
public static class DerivedCategoryExtensions
{
    // Creates area category that adapts to length base unit
    public static UnitCategory CreateAreaCategory(this UnitSystem system)
    {
        var lengthBaseUnit = system.length.BaseUnit;
        var lengthBaseName = system.length.BaseUnitName;
        
        var areaUnit = $"{lengthBaseUnit}2";
        var areaName = $"sq {lengthBaseName}";
        
        var area = new UnitCategory("Area", new UnitSpec(areaUnit, areaName, UnitFamilyName.Area));
        
        // Add appropriate area units based on length system
        if (lengthBaseUnit == "m")
        {
            return area.AddMetricAreaUnits().AddCrossSystemAreaConversions();
        }
        else if (lengthBaseUnit == "in")
        {
            return area.AddImperialAreaUnits("in").AddCrossSystemAreaConversions();
        }
        else if (lengthBaseUnit == "ft")
        {
            return area.AddImperialAreaUnits("ft").AddCrossSystemAreaConversions();
        }
        else if (lengthBaseUnit == "cm")
        {
            return area.AddCGSAreaUnits().AddCrossSystemAreaConversions();
        }
        else if (lengthBaseUnit == "mm")
        {
            return area.AddMmAreaUnits().AddCrossSystemAreaConversions();
        }
        
        return area;
    }
    
    // Creates velocity category that adapts to length and time base units
    public static UnitCategory CreateVelocityCategory(this UnitSystem system)
    {
        var lengthUnit = system.length.BaseUnit;
        var velocityUnit = $"{lengthUnit}/s";
        var velocityName = $"{system.length.BaseUnitName} per second";
        
        var velocity = new UnitCategory("Velocity", new UnitSpec(velocityUnit, velocityName, UnitFamilyName.Speed));
        
        // Add system-appropriate velocity units
        if (lengthUnit == "m")
        {
            return velocity.AddMetricVelocityUnits();
        }
        else if (lengthUnit == "in")
        {
            return velocity.AddIPSVelocityUnits();
        }
        else if (lengthUnit == "ft")
        {
            return velocity.AddFPSVelocityUnits();
        }
        
        return velocity;
    }
}
```

#### 2.3 Electrical Quantities (Same Base Units Across Systems)
- [ ] **Voltage**: V (volts) - same for all systems
- [ ] **Current**: A (amperes) - same for all systems  
- [ ] **Resistance**: Ω (ohms) - same for all systems
- [ ] **Power**: W (watts) for electrical - same for all systems
- [ ] **Capacitance**: F (farads) - same for all systems

---

## Phase 3: Unit System Switching 🎯 **Priority: Medium**

### Goals
- Allow dynamic switching between unit systems
- Preserve measured values during system changes
- Provide seamless user experience

### Tasks

#### 3.1 System Switching Infrastructure
- [ ] **Add CurrentSystemType property**
```csharp
public UnitSystemType CurrentSystemType { get; private set; }
```

- [ ] **Implement SwitchUnitSystem method**
```csharp
public bool SwitchUnitSystem(UnitSystemType newSystem, bool preserveValues = true)
{
    if (preserveValues)
    {
        // Convert all current measured values to new system
        ConvertMeasuredValues(CurrentSystemType, newSystem);
    }
    
    var success = Apply(newSystem);
    if (success)
    {
        CurrentSystemType = newSystem;
    }
    return success;
}
```

#### 3.2 Value Preservation
- [ ] **Track active measured values**
- [ ] **Convert values between systems**
- [ ] **Update unit references automatically**

#### 3.3 System Preferences
- [ ] **Add UnitSystemPreferences class**
```csharp
public class UnitSystemPreferences
{
    public Dictionary<UnitFamilyName, string> PreferredUnits { get; set; } = new();
    public UnitSystemType DefaultSystem { get; set; }
    public bool AutoConvertDisplayUnits { get; set; } = true;
}
```

---

## Phase 4: Configuration-Driven Units 🎯 **Priority: Low**

### Goals
- Enable external configuration of unit systems
- Support custom unit definitions
- Provide JSON/XML configuration options

### Tasks

#### 4.1 Configuration Infrastructure
- [ ] **Create UnitSystemConfiguration class**
```csharp
public class UnitSystemConfiguration
{
    public string Name { get; set; }
    public Dictionary<UnitFamilyName, UnitCategoryConfig> Categories { get; set; }
}

public class UnitCategoryConfig
{
    public string BaseUnit { get; set; }
    public string BaseUnitName { get; set; }
    public List<UnitDefinition> Units { get; set; }
    public List<ConversionRule> Conversions { get; set; }
}
```

#### 4.2 Configuration Loading
- [ ] **JSON configuration loader**
- [ ] **XML configuration loader**
- [ ] **Validation and error handling**

#### 4.3 Custom Unit Registration
- [ ] **Runtime unit registration**
```csharp
public void RegisterCustomUnit(UnitFamilyName family, string symbol, string name, double conversionFactor, string baseUnit)
```

---

## Phase 5: Advanced Features 🎯 **Priority: Low**

### Goals
- Add advanced unit system features
- Improve developer experience
- Add localization support

### Tasks

#### 5.1 Unit Validation
- [ ] **Dimensional analysis**
- [ ] **Unit compatibility checking**
- [ ] **Automatic unit inference**

#### 5.2 Localization
- [ ] **Multi-language unit names**
- [ ] **Regional unit preferences**
- [ ] **Cultural formatting**

#### 5.3 Performance Optimization
- [ ] **Caching frequently used conversions**
- [ ] **Lazy loading of unit categories**
- [ ] **Memory optimization**

#### 5.4 Advanced API Features
- [ ] **Unit arithmetic operations**
- [ ] **Unit expression parsing** ("5 ft 2 in" → Length object)
- [ ] **Automatic unit simplification**

---

## Implementation Guidelines

## Implementation Guidelines

### Code Organization
```
UnitSystem/
├── Core/
│   ├── UnitSystemService.cs (main service - MODIFIED)
│   ├── UnitCategory.cs (existing)
│   ├── UnitSpec.cs (existing)
│   ├── UnitConversion.cs (existing)
│   └── UnitCategoryExtensions.cs (NEW - extension methods)
├── Extensions/
│   ├── MetricUnitExtensions.cs (NEW)
│   ├── ImperialUnitExtensions.cs (NEW)
│   ├── TemperatureExtensions.cs (NEW)
│   ├── DerivedCategoryExtensions.cs (NEW)
│   └── ConversionExtensions.cs (existing - enhanced)
├── Validation/
│   ├── UnitSystemValidator.cs (NEW)
│   └── BaseUnitAccuracyTests.cs (NEW)
└── UnitTypes/
    ├── Length.cs (existing - may need modification for base unit awareness)
    ├── Temperature.cs (existing)
    ├── Mass.cs (NEW)
    ├── Force.cs (NEW)
    └── [other unit types...]
```

### Critical Implementation Requirements

#### **1. Base Unit Accuracy Validation**
```csharp
public static class UnitSystemValidator
{
    public static bool ValidateBaseUnitAccuracy(UnitSystem system)
    {
        // Test round-trip conversions within system family
        var testCases = new[]
        {
            (1.0, system.length.BaseUnit),
            (12.0, GetSecondaryLengthUnit(system)),
            (1.0, system.mass?.BaseUnit)
        };
        
        foreach (var (value, unit) in testCases)
        {
            if (unit == null) continue;
            
            var original = new Length(value, unit);
            var converted = original.ConvertTo(GetAlternateUnit(unit)).ConvertTo(unit);
            
            // Must be exact within floating point precision
            if (Math.Abs(original.Value - converted.Value) > 1e-14)
            {
                return false; // Base unit accuracy compromised
            }
        }
        
        return true;
    }
    
    // Verify exact conversions (no floating point errors)
    public static bool ValidateExactConversions(UnitSystem system)
    {
        if (system.ActiveSystem == UnitSystemType.IPS)
        {
            // 1 foot MUST equal exactly 12 inches (no conversion errors)
            var foot = new Length(1.0, "ft");
            var inches = foot.ConvertTo("in");
            return Math.Abs(inches.Value - 12.0) < 1e-15;
        }
        
        if (system.ActiveSystem == UnitSystemType.MKS)
        {
            // 1 meter MUST equal exactly 1000 millimeters
            var meter = new Length(1.0, "m");
            var millimeters = meter.ConvertTo("mm");
            return Math.Abs(millimeters.Value - 1000.0) < 1e-15;
        }
        
        return true;
    }
}
```

#### **2. MeasuredValue Base Unit Awareness**
```csharp
public abstract class MeasuredValue
{
    // Must be modified to respect active unit system
    protected void Init(UnitCategory category, double value, string units)
    {
        var activeSystem = UnitSystemManager.Current;
        
        if (units == null || units == category.BaseUnit)
        {
            // Already in base units for active system
            V = value;
            I = category.BaseUnit;
        }
        else
        {
            // Convert to base units for active system
            var converted = category.ConvertToBase(value, units);
            V = converted;
            I = category.BaseUnit;
        }
    }
}
```

#### **3. Unit System Manager (NEW)**
```csharp
public static class UnitSystemManager
{
    private static UnitSystem _current = new UnitSystem();
    
    public static UnitSystem Current => _current;
    
    public static bool SwitchSystem(UnitSystemType newSystem)
    {
        var newUnitSystem = new UnitSystem();
        var success = newUnitSystem.Apply(newSystem);
        
        if (success)
        {
            _current = newUnitSystem;
            return true;
        }
        
        return false;
    }
    
    public static void ValidateCurrentSystem()
    {
        if (!UnitSystemValidator.ValidateBaseUnitAccuracy(_current))
        {
            throw new InvalidOperationException("Current unit system has base unit accuracy issues");
        }
    }
}
```

### Best Practices for Implementation

#### **Accuracy-First Principles:**
- ✅ **Exact Conversions**: Within-system conversions must be mathematically exact (12 in = 1 ft exactly)
- ✅ **High Precision Cross-System**: Use internationally defined exact conversion factors
- ✅ **Native Storage**: Each system stores values in its base units without intermediate conversions
- ✅ **Validation Testing**: Every unit system must pass base unit accuracy tests

#### **Code Quality Principles:**
- ✅ **Maintain backward compatibility** throughout all phases
- ✅ **Write comprehensive unit tests** with focus on accuracy validation
- ✅ **Document all public APIs** with XML comments
- ✅ **Use consistent naming conventions** (follow existing patterns)
- ✅ **Validate input parameters** in all public methods
- ✅ **Handle edge cases gracefully** (null values, invalid units)
- ✅ **Optimize for common use cases** (length, temperature conversions)

#### **Extension Method Guidelines:**
- ✅ **Single Responsibility**: Each extension method handles one specific unit family
- ✅ **Fluent Interface**: Maintain existing `.Units().Conversion()` pattern
- ✅ **Base Unit Agnostic**: Extensions adapt to any base unit of their family
- ✅ **High Precision**: Use exact conversion factors where internationally defined

### Testing Strategy

#### **Unit Tests (Critical for Accuracy):**
```csharp
[TestClass]
public class BaseUnitAccuracyTests
{
    [TestMethod]
    public void IPS_System_Stores_Values_In_Inches()
    {
        var system = new UnitSystem();
        system.Apply(UnitSystemType.IPS);
        
        var length = new Length(12.0, "in");
        Assert.AreEqual(12.0, length.Value); // Stored as inches
        Assert.AreEqual("in", length.Units); // Base unit is inches
    }
    
    [TestMethod]
    public void MKS_System_Stores_Values_In_Meters()
    {
        var system = new UnitSystem();
        system.Apply(UnitSystemType.MKS);
        
        var length = new Length(1000.0, "mm");
        Assert.AreEqual(1.0, length.Value); // Stored as meters (1000mm = 1m)
        Assert.AreEqual("m", length.Units); // Base unit is meters
    }
    
    [TestMethod]
    public void Within_System_Conversions_Are_Exact()
    {
        var system = new UnitSystem();
        system.Apply(UnitSystemType.IPS);
        
        var foot = new Length(1.0, "ft");
        var inches = foot.ConvertTo("in");
        
        // Must be exactly 12, not 11.999999 or 12.000001
        Assert.AreEqual(12.0, inches.Value, 0.0);
    }
    
    [TestMethod]
    public void Cross_System_Conversions_Use_Exact_Definitions()
    {
        var system = new UnitSystem();
        system.Apply(UnitSystemType.IPS);
        
        var inch = new Length(1.0, "in");
        var millimeters = inch.ConvertTo("mm");
        
        // Must use exact definition: 1 in = 25.4 mm exactly
        Assert.AreEqual(25.4, millimeters.Value, 1e-15);
    }
}
```

#### **Integration Tests:**
- Test system switching with value preservation
- Test derived unit calculations (area from length base units)
- Test cross-system precision maintenance

#### **Performance Tests:**
- Benchmark conversion operations in each system
- Test memory usage with multiple unit systems
- Validate extension method call overhead

---

## Success Criteria

### Phase 1 Success
- [ ] All 5 unit systems have distinct implementations
- [ ] Each system uses appropriate base units
- [ ] Basic physical quantities (length, mass, force, temperature) work correctly
- [ ] Existing code continues to function without changes

### Phase 2 Success
- [ ] Complete set of physical quantity categories
- [ ] Derived units work correctly in all systems
- [ ] Comprehensive conversion coverage

### Phase 3 Success
- [ ] Users can switch between unit systems seamlessly
- [ ] Measured values are preserved during system changes
- [ ] Performance impact is minimal

### Phase 4 Success
- [ ] Unit systems can be defined via external configuration
- [ ] Custom units can be registered at runtime
- [ ] Configuration validation prevents invalid setups

### Phase 5 Success
- [ ] Advanced features enhance developer productivity
- [ ] System performance is optimized
- [ ] Localization supports international users

---

## Risk Mitigation

### Breaking Changes
- **Risk**: Refactoring might break existing code
- **Mitigation**: Maintain backward compatibility, provide migration path

### Performance Impact
- **Risk**: Additional complexity might slow down conversions
- **Mitigation**: Profile performance, optimize hot paths, implement caching

### Configuration Complexity
- **Risk**: External configuration might be error-prone
- **Mitigation**: Comprehensive validation, clear error messages, good defaults

### Maintenance Burden
- **Risk**: More complex system might be harder to maintain
- **Mitigation**: Clear documentation, good test coverage, consistent patterns

---

## Timeline Estimate

| Phase | Estimated Duration | Dependencies | Critical Success Factors |
|-------|-------------------|--------------|--------------------------|
| **Phase 1** | **2-3 weeks** | None | ✅ Base unit accuracy validation<br>✅ All 5 systems differentiated<br>✅ Extension methods working<br>✅ Backward compatibility maintained |
| Phase 2 | 1-2 weeks | Phase 1 complete | ✅ Derived units adapt to base units<br>✅ Smart category creation |
| Phase 3 | 2-3 weeks | Phase 1 complete | ✅ Seamless system switching<br>✅ Value preservation |
| Phase 4 | 2-4 weeks | Phase 1 complete | ✅ External configuration working<br>✅ Custom unit registration |
| Phase 5 | 3-5 weeks | Previous phases complete | ✅ Performance optimized<br>✅ Advanced features stable |

**Total Estimated Duration**: 10-17 weeks

**Critical Path**: Phase 1 is the foundation - all other phases depend on it being implemented correctly with base unit accuracy.

---

## Ready for Implementation ✅

This plan now includes:

### ✅ **Comprehensive Analysis**
- Current system assessment with accuracy issues identified
- Base unit storage requirements clearly defined
- Code reuse strategy with extension methods
- Complete implementation details for all 5 unit systems

### ✅ **Base Unit Accuracy Focus**
- Native storage principle defined
- Exact conversion requirements specified
- High precision cross-system conversion factors
- Validation methods for accuracy testing

### ✅ **Practical Implementation Approach**
- Hybrid strategy leveraging existing fluent API
- Extension methods for maximum code reuse
- Minimal changes to existing codebase
- Clear migration path from current implementation

### ✅ **Quality Assurance Framework**
- Comprehensive testing strategy focused on accuracy
- Validation methods for base unit integrity
- Performance testing considerations
- Backward compatibility requirements

### 🎯 **Next Steps**
1. **Create UnitCategoryExtensions.cs** with reusable extension methods
2. **Rename EstablishCommonUnit() → EstablishMKSUnits()** 
3. **Implement EstablishIPSUnits()** as the first new system
4. **Add base unit accuracy validation tests**
5. **Progressively add remaining systems (FPS, CGS, mmNs)**

The plan is now ready for execution with clear priorities, detailed specifications, and a focus on the critical requirement of base unit accuracy.
