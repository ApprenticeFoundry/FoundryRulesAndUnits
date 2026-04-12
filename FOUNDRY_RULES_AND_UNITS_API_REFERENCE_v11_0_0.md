# FoundryRulesAndUnits API Reference v11_0_0

**Version:** 11.0.0  
**Date:** February 11, 2026  
**Purpose:** LLM Reference Guide - Prevent API Hallucination & Enforce Correct Patterns

> 🎯 **For LLMs:** This document defines the EXACT APIs available in FoundryRulesAndUnits. Do not assume methods exist that are not listed here. Always use the patterns shown in the examples.

---

## Core Architecture Principles

### 📏 **Unit System Architecture**
- **Factory Pattern** - Use `UnitSystem` to create all `MeasuredValue` instances
- **Type Safety** - Each measurement type (Length, Temperature, etc.) has its own class
- **Unit Families** - Units are organized by family (Length, Temperature, Mass, etc.)
- **Conversion System** - Built-in conversion between compatible units

### 🔄 **Immutability Pattern**
- **MeasuredValue objects are mutable** - can be reassigned with `Assign()` methods
- **Safe assignment patterns** with copy semantics available
- **Factory creation** via UnitSystem for proper initialization

---

## UnitSystem (Factory & Converter)

### Core Properties
```csharp
public IUnitSystemSpecification Current { get; }       // Current active specification  
public UnitSystemType ActiveType { get; }              // Currently active system type
```

### Factory Methods
```csharp
// Create specific measurement types
public T CreateUnit<T>(double value, string units) where T : MeasuredValue, new()

// Create any MeasuredValue from unit symbol
public MeasuredValue CreateMeasuredValueFromParsableUnit(string unit, double value)

// Apply different unit system
public IUnitSystemSpecification Apply(UnitSystemType systemType)
```

### Unit Validation & Information
```csharp
// Validation
public bool IsValidUnit(string unit)                                    // → bool
public bool IsValidUnit(string unit, UnitFamilyName family)            // → bool

// Unit family lookup
public UnitFamilyName GetUnitFamily(string unit)                       // → UnitFamilyName
public UnitFamilyName DetermineUnitFamilyFromUnit(string unit)         // → UnitFamilyName (throws if invalid)

// Unit information
public bool TryGetUnitInfo(string unit, out UnitLookupInfo? unitInfo)  // → bool
public UnitDefinition? GetUnitMetadata(string unitSymbol)              // → UnitDefinition?
public List<UnitDefinition> GetAllKnownUnits()                         // → List<UnitDefinition>
```

### Unit Lists & Conversion
```csharp
// Get units for family
public List<string> GetUnitsForFamily(UnitFamilyName family)           // → List<string>
public string GetBaseUnitForFamily(UnitFamilyName family)              // → string

// Unit conversion
public double Convert(double value, string fromUnit, string toUnit)     // → double
```

### Usage Examples
```csharp
// ✅ CORRECT - Create measurement values
var unitSystem = new UnitSystem();
var length = unitSystem.CreateUnit<Length>(100, "mm");
var temperature = unitSystem.CreateUnit<Temperature>(25, "C");
var angle = unitSystem.CreateUnit<Angle>(45, "deg");

// ✅ CORRECT - Switch unit systems
unitSystem.Apply(UnitSystemType.SI);
unitSystem.Apply(UnitSystemType.FPS);

// ✅ CORRECT - Validate units
if (unitSystem.IsValidUnit("mm"))
{
    var family = unitSystem.GetUnitFamily("mm"); // → UnitFamilyName.Length
}

// ✅ CORRECT - Convert values
var metersValue = unitSystem.Convert(1000, "mm", "m"); // → 1.0

// ✅ CORRECT - Get unit information
if (unitSystem.TryGetUnitInfo("kg", out var unitInfo))
{
    var family = unitInfo.Family;        // → UnitFamilyName.Mass
    var definition = unitInfo.Definition; // → UnitDefinition details
}

// ❌ WRONG - Don't create MeasuredValue directly
var length = new Length(); // Missing UnitGroup injection!
```

---

## MeasuredValue (Base Measurement Class)

### Core Properties
```csharp
public double V { get; set; }                    // Value in internal units
public string I { get; set; }                    // Internal storage units 
public string U { get; set; }                    // Display units
public virtual UnitFamilyName UnitFamily { get; } // Unit family from attribute
```

### Core Methods
```csharp
// Value access
public double Value()                            // → double (current value in display units)
public double BaseValue()                        // → double (value in base units)
public string BaseUnits()                        // → string (base unit symbol)
public string DisplayUnits()                     // → string (current display units)
public string Internal()                         // → string (internal storage units)

// Unit conversion & display
public double As(string units)                   // → double (value converted to specified units)
public string AsString(string units)            // → string (formatted value in specified units)
public void SetDisplayUnits(string units)       // Changes display units
public string Format(string format)             // → string (custom formatted output)

// Assignment & initialization
protected void Init(double value, string? units) // Initialize with value and units
```

### Debugging Methods
```csharp
public string Debug()                            // → string (basic debug info)
public string InternalRepresentation()          // → string (internal state)
public string BaseUnitInfo()                    // → string (base unit details)  
public string DisplayInfo()                     // → string (display format info)
public string ConversionInfo()                  // → string (conversion details)
public string DetailedDebug()                   // → string (comprehensive debug)
```

### Usage Examples
```csharp
// ✅ CORRECT - Working with MeasuredValue
var unitSystem = new UnitSystem();
var length = unitSystem.CreateUnit<Length>(1000, "mm");

// Get values in different units
var inMillimeters = length.Value();              // → 1000.0 (in display units)
var inMeters = length.As("m");                  // → 1.0 (converted)
var formatted = length.AsString("cm");          // → "100 cm"

// Change display units
length.SetDisplayUnits("m");
var newValue = length.Value();                  // → 1.0 (now in meters)

// Debug information  
var debugInfo = length.Debug();                 // Detailed state information
```

---

## Specific Measurement Types

All measurement types inherit from `MeasuredValue` and follow the same pattern:

### Length
```csharp
[UnitType(UnitFamilyName.Length, Description = "Length measurement")]
public class Length : MeasuredValue

// Assignment methods
public Length Assign(double value, string? units)       // → Length
public Length Assign(Length source)                     // → Length  
public Length Copy()                                     // → Length (deep copy)

// Legacy compatibility
public int AsPixels()                                   // → int (pixel conversion)
```

### Temperature  
```csharp
[UnitType(UnitFamilyName.Temperature, Description = "Temperature measurement")]  
public class Temperature : MeasuredValue

// Arithmetic operators
public static Temperature operator +(Temperature left, Temperature right)
public static Temperature operator -(Temperature left, Temperature right)
```

### Complete List of Measurement Types
- `Angle` - Angular measurements (degrees, radians)
- `AngularAcceleration` - Angular acceleration  
- `AngularVelocity` - Angular velocity
- `Area` - Area measurements  
- `Bearing` - Bearing/heading measurements
- `Capacitance` - Electrical capacitance
- `CostPerQuantity` - Cost per unit calculations
- `CostPerTime` - Cost per time calculations  
- `Currency` - Monetary values
- `Current` - Electrical current
- `DataFlow` - Data transfer rates
- `DataStorage` - Data storage amounts
- `Dimensionless` - Unitless values
- `Distance` - Distance measurements
- `Duration` - Time durations
- `Energy` - Energy measurements
- `Force` - Force measurements
- `Frequency` - Frequency measurements
- `Inductance` - Electrical inductance
- `Inertia` - Moment of inertia
- `Length` - Length measurements
- `Mass` - Mass measurements
- `Momentum` - Momentum calculations
- `Percent` - Percentage values
- `Power` - Power measurements
- `Quantity` - Generic quantity measurements
- `QuantityFlow` - Flow rates
- `Resistance` - Electrical resistance
- `Speed` - Speed/velocity measurements
- `Temperature` - Temperature measurements
- `Time` - Time measurements
- `Torque` - Torque measurements
- `Voltage` - Electrical voltage
- `Volume` - Volume measurements

### Usage Examples
```csharp
// ✅ CORRECT - Create and use specific types
var unitSystem = new UnitSystem();
var length1 = unitSystem.CreateUnit<Length>(100, "mm");
var length2 = unitSystem.CreateUnit<Length>(50, "mm");

// Copy and assign
var length3 = length1.Copy();
length3.Assign(200, "cm");

// Type-specific operations
var temp1 = unitSystem.CreateUnit<Temperature>(20, "C");  
var temp2 = unitSystem.CreateUnit<Temperature>(5, "C");
var tempSum = temp1 + temp2; // → Temperature (25°C)
```

---

## Model Classes

### BoundingBox (3D Boundaries)
```csharp
public class BoundingBox

// Dimensions
public Length width { get; set; }
public Length height { get; set; }  
public Length depth { get; set; }

// Pin point (reference position)
public Length pinX { get; set; }
public Length pinY { get; set; }
public Length pinZ { get; set; }

// Scale factors
public double scaleX { get; set; } = 1
public double scaleY { get; set; } = 1  
public double scaleZ { get; set; } = 1

// Constructors  
public BoundingBox()                                                    // Default
public BoundingBox(BoundingBox source)                                 // Copy constructor
public BoundingBox(double width, double height, double depth, string units = "m")

// Methods
public BoundingBox copyFrom(BoundingBox source)                        // → BoundingBox
public BoundingBox Box(double width, double height, double depth, string units)
```

### HighResPosition (High Precision Positioning)
```csharp  
public class HighResPosition
// High-precision position coordinates with Length measurements
// (Specific API details available in source code)
```

### HighResOffset (High Precision Offsets)
```csharp
public class HighResOffset  
// High-precision offset calculations with Length measurements
// (Specific API details available in source code)
```

---

## Extension Methods

### ManufacturingStringExtensions (Domain-Specific)
```csharp
// Manufacturing domain operations (modern .NET 10 syntax)
public static string CleanPartNumber(this string? partNumber)           // → string
public static string CleanAddress(this string? address)                 // → string  
public static string InsertSerialNumber(this string? description, string? serialNumber) // → string
public static string CreateInternalName(this string? name)              // → string
```

### MeasuredValueDisplayExtensions (Unicode Display)
```csharp
// Enhanced display formatting with Unicode support
public static string ToUnicodeString(this MeasuredValue value)          // → string (with Unicode symbols)
public static string ToAsciiString(this MeasuredValue value)            // → string (ASCII only)

// Temperature-specific formatting
public static string ToTemperatureString(this Temperature temp, bool useUnicode = true)

// Angle-specific formatting  
public static string ToAngleString(this Angle angle, bool useUnicode = true)
```

### JsonFileOperations (File I/O)
```csharp
// JSON file operations with type safety
public static List<T> ReadListFromFile<T>(string filename, string directory) where T : class
public static T? ReadFromFile<T>(string filename, string directory) where T : class  
public static List<T> WriteListToFile<T>(List<T> data, string filename, string directory) where T : class
public static T WriteToFile<T>(T value, string filename, string directory) where T : class
```

### TypeAwareJsonStorage (Type Registration)
```csharp
// Type-aware JSON storage with registration
public static void RegisterType<T>() where T : class                    // Register type for serialization
public static List<T> ReadListFromFile<T>(string filename, string directory) where T : class
public static T? ReadFromFile<T>(string filename, string directory) where T : class
```

### MimeTypeMapping (File Extensions)
```csharp
// MIME type mapping for file extensions
public static string GetMimeType(this string fileExtension)             // → string
public static string GetFileExtension(this string mimeType)             // → string
```

---

## Enumeration Types

### UnitSystemType
```csharp
public enum UnitSystemType
{
    MKS,    // Meter-Kilogram-Second (default)
    SI,     // International System of Units  
    CGS,    // Centimeter-Gram-Second
    FPS,    // Foot-Pound-Second
    IPS,    // Inch-Pound-Second
    mmNs    // Millimeter-Newton-Second
}
```

### UnitFamilyName  
```csharp
public enum UnitFamilyName
{
    None, Length, Mass, Time, Temperature, Angle, Area, Volume,
    Speed, Force, Power, Energy, Frequency, Current, Voltage,
    Resistance, Capacitance, Inductance, DataStorage, DataFlow,
    Currency, CostPerTime, CostPerQuantity, Quantity, QuantityFlow,
    Percent, Dimensionless, Duration, Distance, Bearing, Momentum,
    Torque, Inertia, AngularVelocity, AngularAcceleration
}
```

---

## Common Patterns & Best Practices

### ✅ **Correct Unit System Usage**

```csharp
// CREATE unit system factory
var unitSystem = new UnitSystem();

// CREATE measurements via factory
var length = unitSystem.CreateUnit<Length>(100, "mm");
var temp = unitSystem.CreateUnit<Temperature>(25, "C");  
var angle = unitSystem.CreateUnit<Angle>(90, "deg");

// CONVERT between units  
var meters = length.As("m");           // → 0.1
var fahrenheit = temp.As("F");         // → 77.0

// VALIDATE units before use
if (unitSystem.IsValidUnit("mph"))
{
    var speed = unitSystem.CreateUnit<Speed>(60, "mph");
}
```

### ✅ **Correct Assignment & Copying**

```csharp
// COPY measurements
var original = unitSystem.CreateUnit<Length>(100, "mm");
var copy = original.Copy();            // Deep copy

// ASSIGN new values  
copy.Assign(200, "cm");               // Reassign value and units
copy.Assign(original);                // Copy from another measurement

// CHANGE display units
original.SetDisplayUnits("cm");        // Now displays in centimeters
var value = original.Value();         // → 10.0 (same value, different units)
```

### ✅ **Correct Validation & Information**

```csharp
// VALIDATE units
if (unitSystem.IsValidUnit("kg", UnitFamilyName.Mass))
{
    // Safe to use
}

// GET unit information
var family = unitSystem.GetUnitFamily("mm");  // → UnitFamilyName.Length
var baseUnit = unitSystem.GetBaseUnitForFamily(UnitFamilyName.Length); // → "m"

// SAFE unit info lookup
if (unitSystem.TryGetUnitInfo("ft", out var info))
{
    var definition = info.Definition;
    var createFunc = info.CreateMeasuredValue;  
}
```

### ✅ **Correct Model Usage**

```csharp
// CREATE bounding boxes
var box = new BoundingBox(10, 20, 30, "cm");
var copyBox = new BoundingBox(box);    // Copy constructor

// WORK with measurements in models
var width = box.width.As("mm");        // → 100.0  
box.height.SetDisplayUnits("in");      // Change display units
```

### ❌ **Common Mistakes to Avoid**

```csharp
// ❌ WRONG - Don't create MeasuredValue directly
var length = new Length();                    // Missing UnitGroup injection!

// ❌ WRONG - Don't assume methods that don't exist  
length.ConvertTo("m");                        // No such method! Use As("m")
unitSystem.CreateLength(100, "mm");           // No such method! Use CreateUnit<Length>()

// ❌ WRONG - Don't assume unit symbols without validation
var speed = unitSystem.CreateUnit<Speed>(60, "mph"); // Validate "mph" first!

// ❌ WRONG - Don't ignore validation results
unitSystem.CreateMeasuredValueFromParsableUnit("badunit", 100); // Throws if invalid!

// ❌ WRONG - Don't assume arithmetic on all types
var length1 = unitSystem.CreateUnit<Length>(100, "mm");
var length2 = unitSystem.CreateUnit<Length>(50, "mm");  
var sum = length1 + length2;                  // Not all types have operators!
```

---

## Error Handling Patterns

### Unit Validation
```csharp
// ✅ CORRECT - Always validate units
if (unitSystem.IsValidUnit("mph"))
{
    var speed = unitSystem.CreateUnit<Speed>(60, "mph");
}
else
{
    // Handle invalid unit
}

// ✅ CORRECT - Safe unit info retrieval  
if (unitSystem.TryGetUnitInfo("kg", out var unitInfo))
{
    // Use unitInfo safely
}
```

### Exception Handling
```csharp
// ✅ CORRECT - Handle conversion exceptions
try 
{
    var family = unitSystem.DetermineUnitFamilyFromUnit("badunit");
}
catch (ArgumentException ex)
{
    // Handle invalid unit symbol
}

// ✅ CORRECT - Safe measurement creation
try
{
    var measurement = unitSystem.CreateMeasuredValueFromParsableUnit(unitSymbol, value);
}
catch (ArgumentException ex)
{
    // Handle unsupported unit for parsing
}
```

---

## Architecture Guidelines

### When to Use Which Pattern

**Use `UnitSystem` factory when:**
- Creating new measurements
- Validating units
- Converting between unit systems
- Getting unit information

**Use specific measurement types when:**  
- Type safety is important
- Need measurement-specific operations
- Working with domain models

**Use `MeasuredValue` base class when:**
- Generic measurement handling
- Polymorphic operations
- Storage/serialization scenarios

**Use extension methods when:**
- Domain-specific string operations (manufacturing)
- Enhanced display formatting (Unicode)
- File I/O operations (JSON)

### Performance Considerations

- `UnitSystem` caches unit lookup information - reuse instances
- `Copy()` creates deep copies - use sparingly for performance
- `As()` performs conversions - cache results if called repeatedly
- Unit validation is O(1) - safe to call frequently

---

## Version History

- **v11_0_0** (Feb 11, 2026) - Current version - Major API stabilization, comprehensive documentation
- **v10_11_0** - Enhanced unit system specifications
- **v10_10_0** - Unicode display support, improved parsing

---

**🎯 Remember:** If an API is not documented here, it probably doesn't exist. Always refer to this document before assuming method availability. Use the UnitSystem factory pattern for all MeasuredValue creation.