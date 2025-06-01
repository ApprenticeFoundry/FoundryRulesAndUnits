# FoundryRulesAndUnits Library - Copilot Reference Guide

please create a reference document for this library that a copilot could use to make decisions about calling functions or integrating this library

## Overview
FoundryRulesAndUnits is a comprehensive .NET library providing type-safe unit conversion, measurement handling, file operations, and data transfer utilities for engineering and foundry applications. This reference guide enables AI assistants to make informed decisions about function calls and integration patterns.

## Core Capabilities

### 1. Type-Safe Unit System
- **Purpose**: Provides strongly-typed measurements with automatic unit conversion
- **Benefits**: Eliminates unit conversion errors, maintains precision, supports complex calculations
- **Key Classes**: All inherit from `MeasuredValue` base class

### 2. File & Data Operations  
- **Purpose**: Robust file I/O with error handling and JSON serialization
- **Benefits**: Automatic error handling, type-safe serialization, path management
- **Key Classes**: `FileHelpers`, `StorageHelpers`, `CodingExtensions`

### 3. Data Transfer Objects
- **Purpose**: Structured data exchange with metadata support
- **Benefits**: Consistent API responses, hierarchical data modeling, validation
- **Key Classes**: `ContextWrapper<T>`, `DT_Part`, position/geometry models

## Unit System Reference

### Available Measurement Types

| Class | Internal Unit | Common Units | Use Cases |
|-------|--------------|--------------|-----------|
| `Length` | meters (m) | m, cm, mm, in, ft, km | Linear measurements, distances |
| `Area` | square meters (m²) | m², cm², mm², in² | Surface area calculations |
| `Volume` | cubic meters (m³) | m³, cm³, L, mL | Volume measurements |
| `Angle` | radians (rad) | rad, deg | Angular measurements, rotations |
| `Temperature` | Celsius (C) | C, F, K | Temperature readings |
| `Time`/`Duration` | seconds (s) | s, min, hr, days | Time measurements |
| `Speed` | meters/second (m/s) | m/s, km/h, mph | Velocity calculations |
| `DataStorage` | KB | KB, MB, GB, TB, Bytes | File sizes, memory |
| `DataFlow` | KB/sec | KB/s, MB/s, GB/s | Transfer rates |
| `Distance` | meters (m) | Same as Length | Distance-specific operations |
| `Quantity` | each (ea) | ea, dz (dozen), gr (gross) | Count-based measurements |
| `QuantityFlow` | each/second (ea/s) | ea/s, ea/min, ea/hr | Rate measurements |
| `Voltage` | volts | V, mV, kV | Electrical measurements |
| `Current` | amperes | A, mA | Electrical measurements |
| `Power` | watts | W, kW, MW | Power calculations |
| `Resistance` | ohms | Ω, kΩ, MΩ | Electrical resistance |
| `Percent` | percent | %, ratio | Percentage calculations |
| `Frequency` | hertz | Hz, kHz, MHz | Frequency measurements |

### Unit System Usage Patterns

```csharp
// Creating measurements
var length = new Length(5.0, "m");
var temp = new Temperature(20.0, "C");

// Unit conversion
double inches = length.As("in");
double fahrenheit = temp.As("F");

// Arithmetic operations (type-safe)
var total = length1 + length2;
var area = length1 * length2; // Returns Area object
var volume = area * length3;   // Returns Volume object

// Display formatting
string display = length.ToString(); // "5 m"
string formatted = length.Format("F2"); // "5.00 m"
string custom = length.AsString("in"); // "196.85 in"
```

### When to Use Unit System
- ✅ Any calculation involving physical measurements
- ✅ User input that includes units
- ✅ Data exchange between different unit systems
- ✅ Configuration files with measurement values
- ✅ API responses containing measurements
- ❌ Simple counters or indices without units
- ❌ Time calculations under 1 second (use TimeSpan)

## File Operations Reference

### FileHelpers - Basic File Operations

```csharp
// Reading/Writing text data
string data = FileHelpers.ReadData("config", "settings.txt");
FileHelpers.WriteData("output", "results.txt", data);

// Object serialization (with units)
var config = FileHelpers.ReadObjectFromFile<MyConfig>("config.json", "settings");
var parts = FileHelpers.ReadListFromFile<DT_Part>("parts.json", "data");
FileHelpers.WriteListToFile(partList, "export.json", "output");

// Settings management (automatic naming)
FileHelpers.WriteSetting(mySettings); // Creates MySettings.json in config/

// Path utilities
string fullPath = FileHelpers.FullPath("data", "file.json");
bool exists = FileHelpers.FileExist(fullPath);
```

**When to use FileHelpers:**
- ✅ Simple read/write operations
- ✅ JSON serialization with unit support
- ✅ Configuration file management
- ✅ Basic error handling needed

### StorageHelpers - Advanced Storage Operations

```csharp
// Directory management
StorageHelpers.EstablishDirectory("output/reports");
bool exists = StorageHelpers.PathExist("data/cache");

// Type registration for dynamic deserialization
StorageHelpers.RegisterLookupType<MyClass>();
Type? foundType = StorageHelpers.LookupType("MyClass");

// Versioned file operations
string version = StorageHelpers.LastSavedVersionNumber("backup", "data.json");
```

**When to use StorageHelpers:**
- ✅ Complex file management scenarios
- ✅ Dynamic type loading requirements
- ✅ Versioned file operations
- ✅ Directory structure management

### CodingExtensions - Serialization & Reflection

```csharp
// JSON operations with unit support
string json = CodingExtensions.Dehydrate(myObject, prettyPrint: true);
MyClass obj = CodingExtensions.Hydrate<MyClass>(json, useUnits: true);
List<MyClass> list = CodingExtensions.HydrateList<MyClass>(json, useUnits: true);

// Object copying and property manipulation
source.CopyProperties(destination);
source.CopyNonNullProperties(destination); // Only copies non-null values

// CSV data encoding/decoding
string csvData = obj.EncodePropertyDataAsCSV();
obj.DecodePropertyDataAsCSV(csvData);

// DTO conversion utilities
var dto = spec.CreateUDTOfromSPEC<SpecClass, DTOClass>();
var spec = dto.CreateSPECfromUDTO<DTOClass, SpecClass>();
```

**When to use CodingExtensions:**
- ✅ Unit-aware JSON serialization
- ✅ Object cloning/copying operations
- ✅ CSV data interchange
- ✅ DTO/domain object conversion

## Data Models Reference

### ContextWrapper<T> - API Response Pattern

```csharp
// Success response
var success = ContextWrapper<MyData>.success("Operation completed");

// Error response
var error = new ContextWrapper<MyData>("Error message occurred");

// Data response
var wrapper = new ContextWrapper<MyData>(myDataObject);
var wrapperList = new ContextWrapper<List<MyData>>(myDataList);

// Accessing data
List<MyData> items = wrapper.PayloadAsList();
bool hasError = wrapper.hasError;
string message = wrapper.message;
DateTime timestamp = wrapper.dateTime;
```

**When to use ContextWrapper:**
- ✅ API response standardization
- ✅ Error handling with data
- ✅ Timestamped data exchange
- ✅ Collection wrapping with metadata

### Position and Geometry Models

#### HighResPosition - Unit-Aware 3D Position
```csharp
// Creating positions with units
var pos = new HighResPosition()
    .Loc(1.0, 2.0, 3.0, "m")
    .Ang(0.1, 0.2, 0.3, "rad");

// Copying and manipulation
var newPos = new HighResPosition(pos);
pos.copyFrom(otherPosition);

// Convert to unit-less DTO for JSON
var dto = pos.AsUDTO();
```

#### BoundingBox - 3D Bounding Box with Units
```csharp
// Creating bounding boxes
var box = new BoundingBox()
    .Box(10.0, 20.0, 30.0, "cm")
    .Pin(1.0, 2.0, 3.0, "cm")
    .Scale(1.5);

// Convert to DTO
var dtoBox = box.AsUDTO();
```

**When to use Position/Geometry models:**
- ✅ 3D coordinate systems
- ✅ CAD/engineering applications
- ✅ Spatial calculations with units
- ✅ Data exchange requiring precision

### DT_Part - Hierarchical Part Management

```csharp
var part = new DT_Part
{
    partNumber = "ABC123",
    referenceDesignation = "U1",
    serialNumber = "SN001",
    version = "Rev A",
    structureReference = "PCB.CPU.U1"
};

// Hierarchy operations
int depth = part.StructureDepth(); // Number of levels deep
bool isChild = part.IsSubStructure(parentPart);
string parentRef = DT_Part.ParentReference(part.structureReference);

// Name generation
string title = part.ComputeTitle(); // "Title ABC123-Rev A (U1) (SN:SN001)"
string partName = part.partName(); // "ABC123-Rev A"

// Matching operations
bool match = part.CompleteMatch(otherPart);
bool nearMatch = part.NearMatch(otherPart);
```

**When to use DT_Part:**
- ✅ Bill of Materials management
- ✅ Hierarchical part structures
- ✅ Part matching and comparison
- ✅ Manufacturing data exchange

## String Extensions Reference

### StringExtensions - Text Processing Utilities

```csharp
// Type conversion
T value = "123".GetValue<T>();

// String validation and comparison
bool isEmpty = myString.IsNullOrEmpty();
bool matches = str1.Matches(str2); // Case-insensitive
bool startsWith = str1.BeginsWith(str2); // Case-insensitive
bool contains = str1.ContainsNoCase(str2);
bool containsAny = myString.ContainsAny(searchTerms);

// Text cleaning and formatting
string cleaned = partNumber.CleanPartNumber(); // Removes ), }, ], ;, ,, .
string safeAddress = address.CleanAddress(); // Removes ', ;, ,
string filename = title.CleanToFilename(); // Safe for file system
string internalName = displayName.CreateInternalName(); // Safe for identifiers

// String manipulation
string noSpaces = text.RemoveSpace(); // Removes all whitespace
string withSerial = description.InsertSerialNumber("SN123");
```

**When to use StringExtensions:**
- ✅ User input sanitization
- ✅ Filename generation from user text
- ✅ Part number normalization
- ✅ Case-insensitive string operations

## BasicMath Extensions Reference

```csharp
// Safe string to number conversion (returns 0 if invalid)
double value = "123.45".toDouble();
int count = "42".toInteger();

// Angle conversions
double radians = degrees.toRad();
double degrees = radians.toDeg();

// Random number generation
var rand = new Random();
double randomValue = rand.DoubleBetween(1.0, 10.0);
```

**When to use BasicMath:**
- ✅ Safe parsing of user input
- ✅ Angle unit conversions
- ✅ Random value generation within ranges

## Decision Tree for Library Usage

### For Measurements and Units:
1. **Does the value have physical units?** → Use appropriate MeasuredValue class
2. **Need unit conversion?** → Use `.As(units)` method
3. **Performing calculations?** → Use arithmetic operators for type safety
4. **Storing/transmitting?** → Use unit-aware serialization

### For File Operations:
1. **Simple text files?** → Use `FileHelpers.ReadData/WriteData`
2. **Object serialization?** → Use `FileHelpers.ReadObjectFromFile/WriteListToFile`
3. **Complex scenarios?** → Use `StorageHelpers` for directory management
4. **Need reflection/copying?** → Use `CodingExtensions`

### For Data Exchange:
1. **API responses?** → Wrap in `ContextWrapper<T>`
2. **Internal objects with units?** → Use domain objects (HighResPosition, BoundingBox)
3. **External JSON/API?** → Use UDTO variants (unit-less DTOs)
4. **Error handling?** → Use ContextWrapper error constructors

### For String Processing:
1. **User input validation?** → Use `StringExtensions` validation methods
2. **Filename generation?** → Use `.CleanToFilename()`
3. **Safe parsing?** → Use `BasicMath` conversion methods
4. **Case-insensitive operations?** → Use `StringExtensions` comparison methods

## Common Integration Patterns

### API Controller Pattern
```csharp
public ContextWrapper<List<PartData>> GetParts()
{
    try 
    {
        var parts = LoadPartsFromDatabase();
        return new ContextWrapper<List<PartData>>(parts);
    }
    catch (Exception ex)
    {
        return new ContextWrapper<List<PartData>>(ex.Message);
    }
}
```

### Configuration Management Pattern
```csharp
// Reading configuration with units
var config = FileHelpers.ReadObjectFromFile<SystemConfig>("config.json", "settings");

// Saving user settings
FileHelpers.WriteSetting(userPreferences); // Auto-generates filename
```

### Unit Conversion in Calculations Pattern
```csharp
// Convert all measurements to common unit for calculation
var totalLength = measurements
    .Select(m => m.As("m")) // Convert all to meters
    .Sum();

var result = new Length(totalLength, "m");
```

### DTO Conversion at Boundaries Pattern
```csharp
// API endpoint receiving unit-less data
public IActionResult UpdatePosition([FromBody] UDTO_HighResPosition dto)
{
    // Convert to unit-rich domain object
    var position = new HighResPosition()
        .Loc(dto.xLoc, dto.yLoc, dto.zLoc, "m")
        .Ang(dto.xAng, dto.yAng, dto.zAng, "rad");
    
    // Use domain object for business logic
    ProcessPosition(position);
    
    // Return unit-less DTO
    return Ok(position.AsUDTO());
}
```

## Error Handling Guidelines

1. **File Operations**: Use try-catch blocks; FileHelpers includes basic error handling
2. **Unit Conversions**: Validate unit strings before conversion
3. **String Processing**: Use safe conversion methods from BasicMath
4. **JSON Serialization**: Handle null values and missing properties
5. **Type Conversions**: Use CodingExtensions for safe type operations

## Performance Considerations

1. **Cache UnitCategory instances** for repeated conversions
2. **Use CopyNonNullProperties** to avoid overwriting valid data
3. **Pre-register types** with StorageHelpers for faster lookup
4. **Avoid unnecessary unit conversions** in tight loops
5. **Use appropriate DTO types** for data serialization boundaries

This reference guide provides comprehensive coverage for AI assistants to make informed decisions about using the FoundryRulesAndUnits library effectively and safely.
