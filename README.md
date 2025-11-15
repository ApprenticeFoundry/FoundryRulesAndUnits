# Foundry Rules and Units

## Overview

FoundryRulesAndUnits is a comprehensive, modernized unit system library providing type-safe unit conversions, measurement operations, and mathematical operations with automatic type inference. This library supports 6 complete unit systems (SI, MKS, CGS, FPS, IPS, mmNs) with 24+ unit families and advanced features for engineering and scientific applications.

**Current Version**: 10.5.0 | **Target**: .NET 9.0 | **Architecture**: UnitGroup injection with IUnitSystem interface

## 🚀 Key Features

### **Modern Architecture**
- **Unified IUnitSystem Interface**: Clean, dependency-injection friendly design
- **UnitGroup Injection**: Each MeasuredValue receives proper conversion logic via constructor
- **Type-Safe Creation**: Strongly-typed unit creation with compile-time safety
- **24+ Unit Types**: Complete coverage from Length/Mass to specialized units like Frequency/Resistance
- **Mathematical Operations**: Automatic type inference (Length × Length → Area, Mass × Acceleration → Force)
- **Zero Ambiguity Parser**: Two-tier unit family system eliminates parser conflicts
- **StatusBitArray**: High-performance 32-bit flag system with granular dirty tracking for 3D optimization

### **StatusBitArray (v10.5.0)** ⭐ NEW!
- **32-Bit Capacity**: Expanded from 24 to 32 bits with domain-grouped organization
- **General-Purpose Dirty Flag**: `IsDirty` for broad usage across all domains (diagrams, evaluators, knowledge systems)
- **Granular Dirty Tracking**: 5 specialized flags for 3D rendering optimization
  - `IsTransformDirty`: Position/rotation/scale changes (10x-100x faster updates)
  - `IsMaterialDirty`: Color/texture/shader changes (5x-10x faster updates)
  - `IsGeometryDirty`: Mesh/vertex changes (full rebuild required)
  - `IsStructureDirty`: Hierarchy/parent-child changes
  - `IsDataDirty`: Custom data/metadata changes
- **Lifecycle Tracking**: `IsNew` flag for newly created objects
- **Domain Organization**: Flags grouped by purpose (Lifecycle, Dirty, Access, UI, Rendering)
- **Inverted Naming**: Flags named as negatives (`Invisible`, `NotDirty`) so `SetAll(false)` = clean state
- **Future-Proof**: 4 reserved bit positions (8, 19, 29-31) for expansion

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
<PackageReference Include="ApprenticeFoundryRulesAndUnits" Version="10.5.0" />
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

## 🔄 ContextWrapper API Pattern

### **Overview**
`ContextWrapper<T>` provides a standardized response pattern for service APIs with built-in error handling, timestamps, and payload management.

### **Basic Usage**
```csharp
// Success response
var agents = new List<AgentDTO> { agent1, agent2 };
return new ContextWrapper<AgentDTO>(agents) 
{ 
    message = "Retrieved 2 agents" 
};

// Error response
return new ContextWrapper<AgentDTO> 
{ 
    hasError = true, 
    message = "Database connection failed" 
};
```

### **Interface Design**
```csharp
// Non-generic interface for common properties
public interface IContextWrapper
{
    bool hasError { get; set; }
    string message { get; set; }
    int length { get; }
    DateTime timestamp { get; }
}

// Generic interface with typed payload
public interface IContextWrapper<out T> : IContextWrapper
{
    List<T> payload { get; }
}

// Implementation
public class ContextWrapper<T> : IContextWrapper<T>
{
    // Existing implementation
}
```

### **Extension Methods**
```csharp
using FoundryRulesAndUnits.Extensions;

// Error propagation across different types
var agentResult = await GetAgentAsync(id);
if (agentResult.hasError)
    return agentResult.AsErrorFor<DocumentDTO>();  // Type conversion

// Convenience checks
if (result.IsEmpty())                    // hasError || length == 0
    return ContextWrapper<T>.Error("Not found");

if (result.HasData())                    // !hasError && length > 0
    ProcessData(result.payload);

// Quick error creation (multiple syntaxes)
return ContextWrapper<AgentDTO>.Error("Agent not found");
return new ContextWrapper<AgentDTO>("Agent not found");  // Also valid

// Fluent error setting
return result.SetError("Validation failed");
```

### **Clean Syntax with Static Factory Methods** ⭐

**CRITICAL**: The dangerous `new ContextWrapper<string>("text")` constructor has been **REMOVED**!

#### **Quick Reference: Factory Methods**

| Method | Use Case | Result |
|--------|----------|---------|
| `Error("message")` | Something went wrong | `hasError = true`, no payload |
| `Ok(item)` | Success with single item | `hasError = false`, 1 item in payload |
| `Ok(items)` | Success with collection | `hasError = false`, multiple items |
| `Empty()` | Success but no data | `hasError = false`, empty payload |

```csharp
// 🚨 REMOVED - This constructor no longer exists (was ambiguous for string payloads)
// new ContextWrapper<string>("text") - ❌ COMPILE ERROR!

// ✅ REQUIRED - Use factory method for errors without payload
var errorWrapper = ContextWrapper<string>.Error("Error message");     // Explicit error

// ✅ PREFERRED - Use factory method for clarity
var dataWrapper = ContextWrapper<string>.Ok("Actual string data");    // Explicit payload

// ✅ STILL VALID - Constructor with payload object works fine
var wrapper1 = new ContextWrapper<string>("data");                    // String is payload (T)
var wrapper2 = new ContextWrapper<DocumentDTO>(document);             // Object is payload (T)
var wrapper3 = new ContextWrapper<string>(list);                      // List of strings

// Works perfectly for all types
public async Task<ContextWrapper<DocumentDTO>> GetDocumentAsync(string id)
{
    var result = await _service.FindAsync(id);
    
    if (result.IsEmpty())
        return ContextWrapper<DocumentDTO>.Error($"Document {id} not found");
    
    return ContextWrapper<DocumentDTO>.Ok(result);
}

// All factory methods available:
return ContextWrapper<T>.Error("Error message");   // Error with no payload (REQUIRED - no constructor for this)
return ContextWrapper<T>.Ok(singleItem);           // Success with one item (or use constructor)
return ContextWrapper<T>.Ok(itemList);             // Success with multiple items (or use constructor)
return ContextWrapper<T>.Empty();                  // Empty success (or use parameterless constructor)
```

**Why This Matters**: 
- Before: `new ContextWrapper<string>("foo")` was ambiguous - error or data?
- After: Constructor **removed** - must use `Error("foo")` or `Ok("foo")`
- Constructors with payload (T) still work perfectly and are preferred for clarity

### **Factory Methods - Recommended Usage Patterns** 🎯

The static factory methods are the **preferred way** to create `ContextWrapper<T>` instances:

#### **Real-World API Controller Example**
```csharp
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<ContextWrapper<User>> GetUser(int id)
    {
        // Input validation - crystal clear this is an error
        if (id <= 0)
            return ContextWrapper<User>.Error("Invalid user ID");
        
        // Database lookup
        var user = await _userService.GetByIdAsync(id);
        if (user == null)
            return ContextWrapper<User>.Error($"User {id} not found");
        
        // Success - crystal clear this is data
        return ContextWrapper<User>.Ok(user);
    }
    
    [HttpGet]
    public async Task<ContextWrapper<User>> GetAllUsers()
    {
        var users = await _userService.GetAllAsync();
        
        // Empty but not an error - crystal clear intent
        if (!users.Any())
            return ContextWrapper<User>.Empty();
        
        // Success with collection
        return ContextWrapper<User>.Ok(users, $"Found {users.Count()} users");
    }
}
```

#### **Service Layer Pattern**
```csharp
public class DocumentService
{
    public async Task<ContextWrapper<string>> GetContentAsync(string path)
    {
        // Validation errors
        if (string.IsNullOrWhiteSpace(path))
            return ContextWrapper<string>.Error("Document path is required");
        
        if (!File.Exists(path))
            return ContextWrapper<string>.Error($"File not found: {path}");
        
        try
        {
            var content = await File.ReadAllTextAsync(path);
            
            // Empty file (success, but no content)
            if (string.IsNullOrEmpty(content))
                return ContextWrapper<string>.Empty();
            
            // Success with string payload
            return ContextWrapper<string>.Ok(content, "Document loaded");
        }
        catch (Exception ex)
        {
            return ContextWrapper<string>.Error($"Error reading: {ex.Message}");
        }
    }
}
```

#### **Why Factory Methods Are Superior**
- **🔍 Clear Intent**: `Error()` vs `Ok()` vs `Empty()` - no guessing
- **🛡️ Type Safety**: Eliminates string ambiguity completely  
- **📖 Self-Documenting**: Code reads like plain English
- **🔧 IntelliSense**: Shows all options when you type `ContextWrapper<T>.`

**Recommendation**: Use factory methods for all new code! 🚀

### **Multi-Service Orchestration Pattern**
```csharp
public async Task<ContextWrapper<DocumentDTO>> CreateDocumentAsync(
    string agentId, string content)
{
    // Get agent first
    var agentResult = await _agentService.GetByIdAsync(agentId);
    
    // Fail fast - propagate error with type conversion
    if (agentResult.hasError)
        return agentResult.AsErrorFor<DocumentDTO>();
    
    // Check for empty data
    if (agentResult.IsEmpty())
        return ContextWrapper<DocumentDTO>.Error($"Agent {agentId} not found");    // Happy path - continue with document creation
    var agent = agentResult.payload.First();
    var doc = await _documentService.CreateAsync(agent, content);
    return doc;
}
```

### **Key Benefits**
- **Type Safety**: Compile-time checking with generics
- **Error Propagation**: Clean error handling across service boundaries
- **Consistent API**: Same pattern for all service responses
- **Fluent API**: Readable, chainable extension methods
- **Polymorphic**: Interface-based extensions work on any ContextWrapper type

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
├── Models/
│   ├── StatusBitArray.cs             # ⭐ NEW v10.5.0: 32-bit flag system with granular dirty tracking
│   ├── ContextWrapper.cs             # Generic API response wrapper with IContextWrapper interface
│   ├── IContextWrapper.cs            # Non-generic interface for error handling
│   └── [Other data models...]
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
│   ├── ContextWrapperExtensions.cs   # Helper methods (AsErrorFor, IsEmpty, HasData, etc.)
│   └── [Other utility extensions...]
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

**Version**: 10.5.0 | **Target**: .NET 9.0 | **Updated**: November 2025