# FoundryRulesAndUnits Refactoring Specification

**Date**: January 26, 2026  
**Purpose**: Remove redundant, duplicative, confusing, and dead code to focus FoundryRulesAndUnits on its core domain: units and measurements  
**Constraint**: FoundryMicroCore.Library is gospel and cannot be changed

---

## Executive Summary

FoundryRulesAndUnits has accumulated infrastructure code that duplicates or misrepresents functionality from FoundryMicroCore.Library. This specification identifies code to remove, documentation to correct, and patterns to adopt for a cleaner, more focused library.

**Current Version**: 10.11.0 → **Target Version**: 11.0.0  
**Breaking Changes**: YES - removing unused infrastructure classes

---

## 🎯 Core Domain Focus

**What FoundryRulesAndUnits SHOULD be:**
- Unit systems (SI, MKS, CGS, FPS, IPS, mmNs)
- 27+ unit families (Length, Mass, Force, Currency, etc.)
- Unit conversions and mathematical operations
- Expression parsing for unit calculations
- Domain-specific utilities for measurement operations

**What it should NOT be:**
- Generic result/wrapper patterns (use FoundryMicroCore)
- Component tree infrastructure (use MxObject/MxComponent)
- General-purpose status tracking (use StatusBitArray from FoundryMicroCore)
- Data transfer object frameworks (unless unit-specific)

---

## 🗑️ CODE TO REMOVE OR MIGRATE

### 1. ContextWrapper Infrastructure (CRITICAL - HIGH PRIORITY - **MIGRATE TO FOUNDRY MICROCORE**)

**Location**: `Models/ContextWrapper.cs`

**Decision: MOVE to FoundryMicroCore.Library** ✅

**Why Move (Not Remove):**
- **REST API Pattern**: Excellent for uniform processing of REST responses
- **Cross-Cutting Infrastructure**: Multiple projects need this (FoundryWorldsAndDrawings, Blazor apps)
- **Complements MxActionResult**: Different use case (REST vs. internal commands)
- **Collection-First Design**: Natural for API endpoints returning multiple items
- **Timestamp & Metadata**: Valuable for cache invalidation and debugging
- **Factory Methods**: Crystal-clear intent (Error/Ok/Empty/Deprecated)

**Migration Path:**
1. Move file: `FoundryRulesAndUnits/Models/ContextWrapper.cs` → `FoundryMicroCore.Library/Core/ContextWrapper.cs`
2. Update namespace: `FoundryRulesAndUnits.Models` → `FoundryMicroCore.Core`
3. Keep ALL functionality intact (293 lines of valuable code)
4. Update FoundryMicroCore version to v1.8.0
5. Add FoundryMicroCore reference in consuming projects
6. Document as REST API response pattern in FoundryMicroCore README

**Impact:** POSITIVE - Makes REST API pattern available to all projects

**Classes to Migrate:**
```csharp
// All in Models/ContextWrapper.cs → Core/ContextWrapper.cs
- ISuccessOrFailure interface
- Success class
- Failure class
- IContextWrapper interface
- ContextWrapper<T> class (with all factory methods)
```

---

### 2. MockDataGenerator (KEEP - Testing Utility - **VALUABLE**)

**Location**: `Models/MockDataGenerator.cs`

**Decision: KEEP in FoundryRulesAndUnits** ✅

**Why Keep (Not Remove):**
- **Testing utility**: Extremely useful for generating readable test data for models
- **Domain-relevant**: Specifically valuable for REST/gRPC testing workflows with FoundryRulesAndUnits
- **Convenient**: User finds it convenient for flowing test data through system
- **Self-contained**: 145 lines, no external dependencies, lightweight
- **Appropriate names**: Generates readable, understandable names for testing
- **Domain-tuned**: Colors, symbols, names relevant to measurement/engineering testing

**User Feedback:**
> "I found Mock data generator to be extremely useful... when I'm testing Foundry rules and units in the process of it consuming or sending data via REST or GRPC... I find it convenient to use."

**Testing Use Cases:**
```csharp
var generator = new MockDataGenerator();

// Readable test names for units/measurements
var testLocationName = generator.RandomFullName(); // "Thomas North Station"
var testColor = generator.RandomColor();           // "Crimson" 
var testSymbol = generator.RandomSymbol();         // Military-style symbols
var testSentence = generator.RandomSentence();     // Readable descriptions

// Perfect for testing REST/gRPC data flows
var testData = new MeasurementDTO 
{
    Name = generator.RandomFullName(),
    Description = generator.RandomSentence(),
    Color = generator.RandomColor()
};
```

**Enhancement Ideas** (Based on user's vision):
1. **Measured Value Generation**:
   - `RandomLength(min, max, units)` → Returns `Length` type with realistic values
   - `RandomTemperature(range, units)` → Returns `Temperature` with scientific distributions
   - `RandomPressure(waveform, frequency)` → Returns `Pressure` following waveform patterns

2. **Scientific Distributions**:
   - `GaussianDistribution(mean, stdDev)` → Normal distribution for measurements
   - `WeibullDistribution(shape, scale)` → Reliability/survival data
   - `ExponentialDecay(halfLife)` → Radioactive decay, cooling curves
   - `PoissonDistribution(lambda)` → Count-based measurements

3. **Waveform Fitting**:
   - `SinusoidalPattern(frequency, amplitude, phase)` → Periodic measurements
   - `SawtoothWaveform(period, amplitude)` → Ramp-like data
   - `SquareWave(dutyCycle, amplitude)` → Digital signal patterns
   - `WhiteNoise(amplitude)` → Random measurement noise

4. **Domain-Specific Test Data**:
   - `RealisticBoundingBox(objectType)` → Size-appropriate boxes for different objects
   - `EngineeringTolerances(nominalValue, tolerance)` → Manufacturing-realistic measurements
   - `EnvironmentalConditions()` → Temperature, pressure, humidity combinations that make sense

This evolution would make MockDataGenerator a **cornerstone testing utility** that no generic library could replace - it would be deeply integrated with FoundryRulesAndUnits' measurement types and scientific domain knowledge.

**Impact:** POSITIVE - Restoring valuable testing utility

---

### 3. ControlParameterCSV (LOW PRIORITY - DEAD CODE)

**Location**: `Models/MockDataGenerator.cs`

**Why Remove:**
- **145 lines** of test data generation unrelated to units
- **Not used** in production code (0 references found)
- **Belongs in tests**, not production library
- Contains hardcoded names, symbols, colors - not unit domain

**Classes to Remove:**
```csharp
- MockDataGenerator class
- NameList internal class
```

**Migration Path:**
- Move to test project if actually needed
- Replace with modern libraries like Bogus or Faker.NET if required for testing

**Impact:** NONE - appears to be legacy testing code

---

### 3. Models Folder Miscellaneous Classes (EVALUATE INDIVIDUALLY)

**Location**: `Models/` directory

#### 3a. ControlParameterCSV.cs - **REMOVE**
- **12 lines** - minimal CSV DTO
- **No references found** in codebase
- **Generic** data transfer, not unit-specific
- FoundryMicroCore has `ControlParameters` - this appears to be dead alternative

#### 3b. DT_Error.cs, DT_Info.cs, DT_Success.cs, DT_Warning.cs - **EVALUATE**
- **No references found** in search
- If unused: DELETE
- If used in DataModels: Evaluate if they're unit-domain specific
- **Smell**: Multiple result/status classes suggest confused architecture

**Action Required:**
- Audit DataModels folder to see what actually uses these
- If only used in legacy serialization: Mark as obsolete, plan removal
- If active: Document purpose clearly

---

### 4. DT_Base Status Wrapper (MEDIUM PRIORITY - ARCHITECTURAL)

**Location**: `Models/DT_Base.cs` and `DataModels/DT_*.cs`

**Issue**: 
DT_Base wraps `StatusBitArray` and `ControlParameters` from FoundryMicroCore but adds thin wrapper methods:

```csharp
protected ControlParameters? metadata;
protected StatusBitArray statusBits = new();

public bool IsSet(int bitIndex) => statusBits.GetBitByIndex(bitIndex);
public void Set(int bitIndex) => statusBits.SetBitByIndex(bitIndex, true);
```

**Problem:**
- **Unnecessary abstraction** - just use StatusBitArray directly
- **Confusing ownership** - DT_Base appears to own status bits but doesn't
- **Not unit-specific** - these are generic DTOs, not measurement types

**Recommendation:**
- **Option A (Aggressive)**: Remove entire DT_* hierarchy if not actively used
- **Option B (Conservative)**: Keep but remove wrapper methods, expose StatusBitArray directly
- **Option C (Minimal)**: Document that StatusBitArray comes from FoundryMicroCore

**If keeping DT_Base:**
```csharp
// REMOVE thin wrapper methods
// public bool IsSet(int bitIndex) { ... }
// public void Set(int bitIndex) { ... }

// EXPOSE directly instead
public StatusBitArray StatusBits { get; protected set; } = new();

// Let consumers use: obj.StatusBits.IsDirty instead of obj.IsSet(2)
```

---

## � CROSS-LIBRARY ANALYSIS: Already Exists or Should Migrate?

This section analyzes each component being removed to determine:
1. **Does FoundryMicroCore already provide this?**
2. **Should it be moved to FoundryMicroCore?**
3. **Is it truly dead code with no home?**

### 1. ContextWrapper<T> - VALUABLE (REST API Pattern - Should Move to FoundryMicroCore)

**Current Implementation**: `FoundryRulesAndUnits.Models.ContextWrapper<T>`

**FoundryMicroCore Equivalent**: ⚠️ **SIMILAR BUT DIFFERENT** - `MxActionResult<T>` exists but serves different purpose

```csharp
// FoundryMicroCore.Core.MxActionResult<T> - Command/Action focused
public class MxActionResult<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T? Data { get; set; }          // Single item
    public List<string> Errors { get; }
}

// FoundryRulesAndUnits.Models.ContextWrapper<T> - REST API focused
public class ContextWrapper<T>
{
    public bool hasError { get; set; }
    public string message { get; set; }
    public ICollection<T> payload { get; set; }  // Collection of items
    public DateTime dateTime { get; set; }        // Timestamp tracking
    public string payloadType { get; set; }       // Type metadata
    public int length => payload?.Count ?? 0;     // Payload size
    public bool isDeprecated { get; set; }        // Migration tracking
    
    // Factory methods for clarity
    public static ContextWrapper<T> Error(string message) { ... }
    public static ContextWrapper<T> Ok(T item) { ... }
    public static ContextWrapper<T> Ok(IEnumerable<T> items) { ... }
}
```

**Key Differences**:

| Feature | MxActionResult<T> | ContextWrapper<T> |
|---------|-------------------|-------------------|
| **Primary Use** | Internal commands/actions | REST API responses |
| **Payload Type** | Single `T? Data` | Collection `ICollection<T>` |
| **Timestamp** | ❌ No | ✅ Yes (dateTime) |
| **Type Tracking** | ❌ No | ✅ Yes (payloadType) |
| **JSON Optimized** | ❌ No | ✅ Yes ([JsonInclude]) |
| **Deprecation** | ❌ No | ✅ Yes (isDeprecated flag) |
| **Factory Pattern** | ❌ No | ✅ Yes (Error/Ok/Empty) |
| **Errors Collection** | ✅ Yes (List<string>) | ❌ No (single message) |

**Analysis - Why ContextWrapper is Valuable**:
1. **REST API Serialization Pattern**: Designed for JSON responses with metadata
2. **Collection-First**: Naturally handles multiple results (common in REST APIs)
3. **Timestamp Tracking**: Records when response was created
4. **Type Metadata**: `payloadType` helps with deserialization and debugging
5. **Uniform Processing**: Consistent shape for all API responses
6. **Deprecation Support**: Tracks legacy endpoints needing migration
7. **Factory Methods**: Crystal-clear intent (Error/Ok/Empty)

**Real-World REST API Use Case**:
```csharp
// Controller returning multiple items
[HttpGet("users")]
public async Task<ContextWrapper<UserDTO>> GetUsers()
{
    var users = await _service.GetAllAsync();
    return ContextWrapper<UserDTO>.Ok(users); // Collection payload
}

// Client processing
var response = await httpClient.GetFromJsonAsync<ContextWrapper<UserDTO>>("/users");
if (response.IsError()) 
    ShowError(response.message);
else
    DisplayUsers(response.payload);  // ICollection<UserDTO>

// Timestamp for cache invalidation
if (response.dateTime < lastSync)
    RefreshCache();
```

**Recommendation**: ✅ **MOVE TO FOUNDRY MICROCORE**

**Why it belongs in FoundryMicroCore:**
1. **Cross-Cutting Infrastructure**: REST API patterns are foundational, not domain-specific
2. **Multiple Projects Need It**: FoundryWorldsAndDrawings, future Blazor projects, etc.
3. **Complements MxActionResult**: Different use cases don't conflict
   - Use `MxActionResult<T>` for internal operations/commands
   - Use `ContextWrapper<T>` for REST API responses/serialization
4. **Zero Dependencies**: Only needs System.Text.Json, already available in .NET 10
5. **Standalone Pattern**: Doesn't depend on units or measurements

**Migration Plan:**
1. Move `Models/ContextWrapper.cs` → `FoundryMicroCore.Library/Core/ContextWrapper.cs`
2. Update namespace: `FoundryRulesAndUnits.Models` → `FoundryMicroCore.Core`
3. Keep all factory methods and functionality intact
4. Update FoundryMicroCore to v1.8.0
5. Update FoundryRulesAndUnits to reference new location
6. Document in FoundryMicroCore README as REST API result pattern

**Should move to FoundryMicroCore?**: ✅ **YES - REST API infrastructure pattern**

**Coexistence with MxActionResult**:
```csharp
// Internal command execution
var commandResult = await ExecuteCommand<UpdateUser>(command);
if (!commandResult.Success)
    return ContextWrapper<UserDTO>.Error(commandResult.Message);

// Convert to REST response
var user = MapToDTO(commandResult.Data);
return ContextWrapper<UserDTO>.Ok(user);
```

They work together: MxActionResult for internal flow, ContextWrapper for API contracts.

---

### 2. StatusBitArray - ALREADY IN FOUNDRY MICROCORE

**Current Implementation**: NONE - documented but not implemented in FoundryRulesAndUnits

**FoundryMicroCore Location**: ✅ `FoundryMicroCore.Core.StatusBitArray`

```csharp
// FoundryMicroCore.Library/Core/StatusBitArray.cs
[InlineArray(4)]  // 4 bytes = 32 bits
public struct StatusBitArray
{
    private byte _element0;
    
    // Properties: IsDirty, IsNew, IsTransformStale, etc.
    // Methods: SetDirty(), ClearAllStaleFlags(), etc.
}
```

**Analysis**:
- StatusBitArray is **already implemented** in FoundryMicroCore.Library v1.6.0+
- Uses modern C# 14 inline arrays for maximum performance
- 32-bit flag system with domain-organized properties
- DT_Base in FoundryRulesAndUnits **already uses it** via dependency

**Recommendation**: ✅ **CORRECT DOCUMENTATION ONLY**
- Remove from FoundryRulesAndUnits README (not our code)
- Keep using it via FoundryMicroCore dependency
- Add attribution in "Dependencies" section

**Should move to FoundryMicroCore?**: ✅ **ALREADY THERE** - no action needed

---

### 3. ControlParameters - ALREADY IN FOUNDRY MICROCORE

**Current Status**: Used but not defined in FoundryRulesAndUnits

**FoundryMicroCore Location**: ✅ `FoundryMicroCore.Core.ControlParameters`

```csharp
// FoundryMicroCore.Library/Core/ControlParameters.cs
public class ControlParameters
{
    private Dictionary<string, object> _parameters = new();
    
    public T? Get<T>(string key) { ... }
    public void Set<T>(string key, T value) { ... }
    public bool TryGet<T>(string key, out T? value) { ... }
    // ... extensible metadata container
}
```

**Analysis**:
- DT_Base uses `protected ControlParameters? metadata;` from FoundryMicroCore
- Provides type-safe extensible metadata storage
- No need to duplicate or reimagine

**Recommendation**: ✅ **KEEP USING FROM FOUNDRY MICROCORE**
- Already correctly using via dependency
- Document in "Dependencies" section

**Should move to FoundryMicroCore?**: ✅ **ALREADY THERE** - no action needed

---

### 4. MockDataGenerator - KEEP (Testing Utility - VALUABLE)

**Current Implementation**: `FoundryRulesAndUnits.Models.MockDataGenerator`

**FoundryMicroCore Equivalent**: ❌ **NO - And shouldn't be there**

**Analysis**:
- **145 lines** of test data generation for readable names, colors, symbols
- **Actively used** by user for testing FoundryRulesAndUnits models
- **Domain-relevant testing**: Specifically useful for REST/gRPC testing workflows
- **Convenient utility**: Generates understandable test data for flowing through system
- **Testing-specific**: Provides readable names and data for model testing

**User Feedback**:
> "I found Mock data generator to be extremely useful when it comes to testing models, specifying names that are readable and understandable... I find it convenient to use when I'm testing Foundry rules and units in the process of it consuming or sending data via REST or GRPC"
> 
> "Actually, I'm more likely to extend it so that it can generate measured values for me with certain scientific process properties. Like fitting different waveforms. Or data distributions."

**Future Evolution Potential**:
MockDataGenerator could become a sophisticated **scientific test data generator** for FoundryRulesAndUnits:

```csharp
// Current capabilities
var generator = new MockDataGenerator();
var testName = generator.RandomFullName();
var testColor = generator.RandomColor();

// Potential enhancements for measured values
var temperatureReading = generator.RandomTemperature(20.0, 25.0, "°C"); // Temperature type
var pressureWaveform = generator.SinusoidalPressure(frequency: 60.0, amplitude: 10.0, "Pa");
var normalDistributionMass = generator.GaussianMass(mean: 50.0, stdDev: 5.0, "kg");
var exponentialDecay = generator.ExponentialDecayForce(halfLife: 30.0, "N");

// Scientific distributions for realistic test data
var samples = generator.WeibullDistribution(shape: 2.0, scale: 100.0, count: 1000);
var measurements = generator.FitToWaveform(WaveformType.Sawtooth, period: 2.0);
```

This evolution would make MockDataGenerator a **uniquely valuable testing utility** that combines:
- Human-readable names and descriptions
- **Scientifically realistic measurement data**
- **Unit-aware test generation** using FoundryRulesAndUnits types
- **Statistical distributions** for modeling real-world data
- **Waveform fitting** for signal processing and sensor data tests

**Modern Alternatives Available But Not Better for This Use Case**:
```csharp
// MockDataGenerator provides domain-friendly test data
var generator = new MockDataGenerator();
var testName = generator.RandomFullName(); // "Thomas North"
var testColor = generator.RandomColor();   // "Crimson"
var testSymbol = generator.RandomSymbol(); // "SFG-UCI----"

// vs. Generic libraries like Bogus (more complex setup for simple needs)
var faker = new Faker();
var name = faker.Name.FullName(); // Requires NuGet package, more complex
```

**Recommendation**: ✅ **KEEP - Valuable testing utility**
- Provides domain-appropriate test data generation
- Lightweight and self-contained
- Specifically tuned for FoundryRulesAndUnits testing needs
- Convenient for REST/gRPC testing workflows
- No external dependencies

**Should move to FoundryMicroCore?**: ❌ **NO**
- Domain-specific test data (symbols, colors, names relevant to this library)
- Testing utility, not foundational infrastructure
- Specifically designed for FoundryRulesAndUnits testing scenarios

**Enhancement Roadmap** (Based on user's scientific vision):

1. **Measured Value Generation**:
   - `RandomLength(min, max, units)` → Returns `Length` type with realistic values
   - `RandomTemperature(range, units)` → Returns `Temperature` with scientific distributions
   - `RandomPressure(waveform, frequency)` → Returns `Pressure` following waveform patterns
   - `RandomForce(distribution, parameters)` → Returns `Force` with statistical properties

2. **Scientific Distributions**:
   - `GaussianDistribution(mean, stdDev)` → Normal distribution for measurements
   - `WeibullDistribution(shape, scale)` → Reliability/survival data (material testing)
   - `ExponentialDecay(halfLife)` → Radioactive decay, cooling curves, chemical reactions
   - `PoissonDistribution(lambda)` → Count-based measurements (particles, events)
   - `LogNormalDistribution()` → Multiplicative processes (particle sizes, concentrations)

3. **Waveform Fitting**:
   - `SinusoidalPattern(frequency, amplitude, phase)` → Periodic measurements (vibration, AC signals)
   - `SawtoothWaveform(period, amplitude)` → Ramp-like data (motor speeds, temperature cycles)
   - `SquareWave(dutyCycle, amplitude)` → Digital signal patterns (on/off processes)
   - `WhiteNoise(amplitude)` → Random measurement noise overlay
   - `TriangularWave()` → Symmetric periodic patterns

4. **Domain-Specific Scientific Test Data**:
   - `RealisticBoundingBox(objectType)` → Size-appropriate boxes for different engineering objects
   - `EngineeringTolerances(nominalValue, tolerance)` → Manufacturing-realistic measurements
   - `EnvironmentalConditions()` → Temperature, pressure, humidity combinations that correlate realistically
   - `MaterialProperties()` → Density, modulus, strength values that make physical sense
   - `SensorReadings(sensorType, environment)` → Realistic sensor data with appropriate noise and drift

This evolution would make MockDataGenerator a **cornerstone scientific testing utility** that no generic library could replace - deeply integrated with FoundryRulesAndUnits' measurement types and scientific domain knowledge.

---

### 5. ControlParameterCSV - TRULY DEAD CODE

**Current Implementation**: `FoundryRulesAndUnits.Models.ControlParameterCSV`

**FoundryMicroCore Equivalent**: ❌ **NO - And shouldn't be there**

```csharp
// 12 lines - minimal DTO
public class ControlParameterCSV
{
    public string? guid { get; set; }
    public string? name { get; set; }
    public string? type { get; set; }
    public string? field { get; set; }
    public string? value { get; set; }
}
```

**Analysis**:
- Appears to be legacy CSV serialization format
- **Zero references** in codebase
- Redundant with `ControlParameters` from FoundryMicroCore
- Generic DTO, not measurement-specific

**Recommendation**: ✅ **REMOVE - True dead code**
- Unused alternative to ControlParameters
- No production references
- Legacy artifact

**Should move to FoundryMicroCore?**: ❌ **NO**
- ControlParameters already provides better solution
- CSV serialization should use standard approaches
- Not foundational pattern

---

### 6. DT_* Classes (Base, Component, Hero, etc.) - DOMAIN-SPECIFIC

**Current Implementation**: `FoundryRulesAndUnits.Models.DT_Base` and `DataModels/DT_*.cs`

**FoundryMicroCore Equivalent**: ❌ **NO - Domain-specific data models**

**Analysis**:
```csharp
// DT_Base uses FoundryMicroCore infrastructure
public class DT_Base
{
    protected ControlParameters? metadata;      // From FoundryMicroCore ✅
    protected StatusBitArray statusBits = new(); // From FoundryMicroCore ✅
    
    public string? Guid { get; set; }
    public string? Name { get; set; }
    public List<string> Tags { get; set; }
    // ... domain-specific properties
}
```

**Key Finding**:
- DT_* classes are **data transfer objects** for serialization
- Use FoundryMicroCore infrastructure (StatusBitArray, ControlParameters)
- Contain domain-specific properties (Position, BoundingBox, measurement-aware)
- Thin wrapper methods around StatusBitArray should be removed

**Recommendation**: 🔍 **EVALUATE FURTHER** (Phase 3 of implementation)
- If actively used for API/serialization: **KEEP** but clean up
  - Remove thin wrapper methods on StatusBitArray
  - Expose StatusBitArray directly
  - Document that infrastructure comes from FoundryMicroCore
- If unused: **REMOVE** entire DataModels folder

**Should move to FoundryMicroCore?**: ❌ **NO**
- Domain-specific to measurements/units/3D positioning
- FoundryMicroCore is domain-agnostic infrastructure
- These are application DTOs, not foundational patterns

**Example cleanup** (if keeping):
```csharp
// BEFORE - thin wrapper
protected StatusBitArray statusBits = new();
public bool IsSet(int bitIndex) => statusBits.GetBitByIndex(bitIndex);
public void Set(int bitIndex) => statusBits.SetBitByIndex(bitIndex, true);

// AFTER - direct exposure
public StatusBitArray StatusBits { get; protected set; } = new();
// Use: obj.StatusBits.IsDirty instead of wrapper methods
```

---

### 7. BoundingBox, HighResPosition - DOMAIN-SPECIFIC (KEEP)

**Current Implementation**: 
- `FoundryRulesAndUnits.Models.BoundingBox`
- `FoundryRulesAndUnits.Models.HighResPosition`

**FoundryMicroCore Equivalent**: ❌ **NO - Unit-aware domain models**

```csharp
public class BoundingBox
{
    public Length width;   // Unit-aware!
    public Length height;  // Unit-aware!
    public Length depth;   // Unit-aware!
    
    public Length pinX;    // Measurement types
    public Length pinY;
    public Length pinZ;
}
```

**Analysis**:
- **Measurement-aware** - uses Length, not raw doubles
- Specific to 3D spatial calculations with units
- Domain expertise of FoundryRulesAndUnits

**Recommendation**: ✅ **KEEP IN FOUNDRY RULES AND UNITS**
- Core domain models for unit-aware 3D positioning
- Not generic infrastructure
- Exactly what this library should provide

**Should move to FoundryMicroCore?**: ❌ **NO**
- Requires unit system knowledge
- Domain-specific to measurements
- Would create circular dependency

---

## 📊 Summary Matrix

| Component | Exists in FoundryMicroCore? | Should Move? | Action |
|-----------|---------------------------|--------------|--------|
| **ContextWrapper<T>** | ⚠️ SIMILAR (MxActionResult different use) | ✅ YES | MOVE to FoundryMicroCore |
| **StatusBitArray** | ✅ YES (already there) | ✅ ALREADY THERE | Document only |
| **ControlParameters** | ✅ YES (already there) | ✅ ALREADY THERE | Keep using |
| **MockDataGenerator** | ❌ NO | ❌ NO | KEEP (testing utility) |
| **ControlParameterCSV** | ❌ NO | ❌ NO | REMOVE (dead code) |
| **DT_* Classes** | ❌ NO (domain DTOs) | ❌ NO | EVALUATE (Phase 3) |
| **BoundingBox** | ❌ NO (unit-aware) | ❌ NO | KEEP (core domain) |
| **HighResPosition** | ❌ NO (unit-aware) | ❌ NO | KEEP (core domain) |

### Key Insights

1. **✅ FoundryMicroCore needs ContextWrapper** - REST API pattern should move upstream
2. **✅ MxActionResult and ContextWrapper are complementary** - Different use cases
3. **✅ Infrastructure properly reused** - DT_Base correctly uses StatusBitArray/ControlParameters
4. **❌ Documentation misleading** - Claims ownership of FoundryMicroCore features
5. **✅ Domain models appropriate** - BoundingBox, HighResPosition belong in FoundryRulesAndUnits
6. **✅ True dead code identified** - MockDataGenerator, ControlParameterCSV have no purpose

### Architectural Boundary

```
┌─────────────────────────────────────────────────────────────┐
│ FoundryMicroCore.Library (Gospel - Infrastructure)          │
│                                                              │
│  • StatusBitArray          (32-bit state tracking)          │
│  • ControlParameters       (extensible metadata)            │
│  • MxObject/MxComponent    (component hierarchy)            │
│  • MxActionResult<T>       (internal command results)       │
│  • ContextWrapper<T>       (REST API responses) ⭐ MOVE     │
│  • MessageBus              (pub/sub)                         │
│  • Walker pattern          (tree traversal)                 │
└─────────────────────────────────────────────────────────────┘
                              ▲
                              │ depends on
                              │
┌─────────────────────────────────────────────────────────────┐
│ FoundryRulesAndUnits (Domain - Units & Measurements)        │
│                                                              │
│  • UnitSystem              (SI, MKS, CGS, etc.)             │
│  • MeasuredValue           (Length, Mass, Force, etc.)      │
│  • BoundingBox             (unit-aware spatial)             │
│  • HighResPosition         (precision coordinates)          │
│  • DT_* models             (serialization DTOs)             │
│  • Expression parser       (unit calculations)              │
│                                                              │
│  Uses ContextWrapper<T> for REST API endpoints              │
└─────────────────────────────────────────────────────────────┘
```

**Clear separation**: Infrastructure vs. Domain
**ContextWrapper migration**: Promotes REST API pattern to infrastructure level

---

## �📝 DOCUMENTATION TO CORRECT

### 1. README.md - StatusBitArray Claims

**Current (INCORRECT):**

Lines 20-46 extensively document StatusBitArray as if it's part of FoundryRulesAndUnits:

```markdown
### **StatusBitArray (v10.8.1)** ⭐ BUG FIX!
- **32-Bit Serialization**: All 32 bits sent to JavaScript...
- **Clean, Simple API**: Removed over-engineered methods...
[... 20+ lines of StatusBitArray features ...]
```

**Problem:**
- **StatusBitArray is from FoundryMicroCore.Library**, not FoundryRulesAndUnits
- Misrepresents what code actually ships in this package
- Creates confusion about dependencies

**Correction:**

```markdown
### **Integration with FoundryMicroCore**
- Uses `StatusBitArray` from FoundryMicroCore.Library for efficient state tracking
- Uses `ControlParameters` from FoundryMicroCore.Library for extensible metadata
- Compatible with MxObject component hierarchy for measurement objects

See [FoundryMicroCore.Library](../FoundryMicroCore/FoundryMicroCore.Library/README.md) 
for details on StatusBitArray and component infrastructure.
```

**Move StatusBitArray docs to:**
- Remove from FoundryRulesAndUnits README entirely
- Or create brief "Dependencies" section explaining it comes from FoundryMicroCore

---

### 2. README.md - ContextWrapper Claims

**Current (INCORRECT):**

Lines 20-28 document ContextWrapper factory methods:

```markdown
### **ContextWrapper Factory Methods (v10.7.0)** ⭐ NEW!
- **Error()**: Create error responses...
- **Ok()**: Create success responses...
[... 8 lines of ContextWrapper features ...]
```

**Problem:**
- **ContextWrapper.cs exists** in Models/ folder but **is not used** anywhere
- Documents feature that doesn't functionally exist in the library
- Misleading to users

**Correction:**
- **REMOVE entirely** from README when ContextWrapper.cs is removed
- Don't advertise unused code as a feature

---

### 3. CHANGELOG_10.7.0.md, CHANGELOG_10.8.0.md - Historical Claims

**Issue:**
- CHANGELOG_10.7.0.md extensively documents ContextWrapper changes
- CHANGELOG_10.8.0.md extensively documents StatusBitArray changes
- **But ContextWrapper was never used**, and **StatusBitArray is from FoundryMicroCore**

**Recommendation:**
- **Keep changelogs as historical record** (never rewrite history)
- **Add correction note** to each file explaining the situation

**Add to top of CHANGELOG_10.7.0.md:**
```markdown
> **⚠️ HISTORICAL NOTE (Jan 2026):** 
> ContextWrapper<T> documented in this changelog was never actually used in the 
> codebase and has been removed in v11.0.0. For result patterns, use standard 
> .NET approaches or FoundryMicroCore.Core.MxActionResult.
```

**Add to top of CHANGELOG_10.8.0.md:**
```markdown
> **⚠️ HISTORICAL NOTE (Jan 2026):** 
> StatusBitArray documented in this changelog is actually provided by 
> FoundryMicroCore.Library, not FoundryRulesAndUnits. DT_Base classes in 
> this library use StatusBitArray but do not define it.
```

---

### 4. Version-Specific Changelogs Referencing Removed Features

**Files to Update:**
- CHANGELOG_10.8.1.md (StatusBitArray bug fix)
- CHANGELOG_10.8.2.md (if it references StatusBitArray)

**Action:**
- Add same historical correction notes
- Keep the content (accurate description of what changed at that time)

---

## 📋 RECOMMENDED CHANGES

### 1. Update FoundryRulesAndUnits.csproj

**Current:**
```xml
<Version>10.11.0</Version>
<TargetFrameworks>net10.0</TargetFrameworks>
```

**Change to:**
```xml
<Version>11.0.0</Version>
<TargetFrameworks>net10.0</TargetFrameworks>
<PackageReleaseNotes>
v11.0.0: BREAKING - Removed unused infrastructure code (ContextWrapper, MockDataGenerator)
to focus library on units and measurements domain. Clarified that StatusBitArray and 
ControlParameters come from FoundryMicroCore.Library dependency.
</PackageReleaseNotes>
```

---

### 2. Create CHANGELOG_11.0.0.md

Document the cleanup:

```markdown
# FoundryRulesAndUnits 11.0.0 Release Notes

**Release Date**: January 26, 2026  
**Package**: `ApprenticeFoundryRulesAndUnits` version 11.0.0  
**Target Framework**: .NET 10.0  
**Type**: BREAKING CHANGE - Code Cleanup

---

## 🎯 Overview

Major cleanup release removing unused infrastructure code to refocus the library 
on its core domain: units and measurements. Clarifies relationship with 
FoundryMicroCore.Library.

---

## 💥 Breaking Changes

### Removed Unused Infrastructure Classes

#### 1. ContextWrapper<T> (Models/ContextWrapper.cs)
- **Removed**: Entire file and all related classes
- **Reason**: Never actually used in codebase, duplicated result patterns
- **Migration**: Use standard .NET patterns or FoundryMicroCore.Core.MxActionResult
- **Impact**: NONE - no code used these classes

Classes removed:
- `ISuccessOrFailure`, `Success`, `Failure`
- `IContextWrapper`
- `ContextWrapper<T>`

#### 2. MockDataGenerator (Models/MockDataGenerator.cs)
- **Removed**: Test data generation code
- **Reason**: Not production code, unused
- **Migration**: Use Bogus or Faker.NET in test projects if needed
- **Impact**: NONE - no production code referenced this

#### 3. ControlParameterCSV (Models/ControlParameterCSV.cs)
- **Removed**: Dead CSV DTO
- **Reason**: Unused, redundant with ControlParameters from FoundryMicroCore
- **Impact**: NONE - no references found

---

## 📖 Documentation Corrections

### Clarified Dependency Attribution

- **README.md**: Removed StatusBitArray documentation (it's from FoundryMicroCore)
- **README.md**: Removed ContextWrapper documentation (was unused)
- **README.md**: Added "Dependencies" section explaining FoundryMicroCore integration
- **CHANGELOGs**: Added historical notes to v10.7.0 and v10.8.0 changelogs

### What This Library Actually Provides

FoundryRulesAndUnits focuses exclusively on:
- ✅ 6 unit systems (SI, MKS, CGS, FPS, IPS, mmNs)
- ✅ 27 unit families (Length, Mass, Force, Currency, etc.)
- ✅ Type-safe unit conversions
- ✅ Mathematical operations with automatic type inference
- ✅ Expression parsing for unit calculations
- ✅ Domain models for 3D positioning and bounding boxes (unit-specific)

Infrastructure from FoundryMicroCore.Library:
- StatusBitArray (efficient state tracking)
- ControlParameters (extensible metadata)
- MxObject compatibility (component hierarchy)

---

## 🎯 Future Direction

This release establishes clear boundaries:
- **FoundryMicroCore.Library**: Component framework, infrastructure
- **FoundryRulesAndUnits**: Units, measurements, conversions

Future development will focus on expanding unit families, improving 
conversion accuracy, and enhancing expression parsing - not on 
building infrastructure.
```

---

### 3. Update README.md Structure

**New structure:**

```markdown
# Foundry Rules and Units

## Overview

FoundryRulesAndUnits is a comprehensive unit system library providing type-safe 
unit conversions, measurement operations, and mathematical operations with 
automatic type inference. Built on FoundryMicroCore.Library component framework.

**Current Version**: 11.0.0 | **Target**: .NET 10.0

---

## 🚀 Key Features

### **Unit Systems & Measurements**
- **6 Complete Unit Systems**: SI, MKS, CGS, FPS, IPS, mmNs
- **27 Unit Families**: Length, Mass, Force, Currency, and more
- **Type-Safe Conversions**: Compile-time safety for unit operations
- **Mathematical Operations**: Automatic type inference (Length × Length → Area)
- **Expression Parser**: Natural language unit expressions

### **Dependencies**
- **FoundryMicroCore.Library**: Provides component infrastructure
  - `StatusBitArray`: Efficient 32-bit state tracking
  - `ControlParameters`: Extensible metadata container
  - `MxObject`: Component hierarchy support

### **Domain-Specific Features**
- **Rack Units (v10.10.0)**: Native support for server rack measurements
- **Currency & Cost (v10.9.0)**: 14 international currencies
- **3D Positioning**: HighResPosition for precision coordinates
- **Bounding Boxes**: Measurement-aware spatial calculations

[... rest of existing README content about unit systems ...]
```

---

### 4. Evaluate DataModels Folder

**Action Required**: Determine if DT_* classes are actively used

**If ACTIVELY USED for serialization:**
- Keep them
- Document their purpose clearly
- Remove thin wrapper methods on StatusBitArray
- Add namespace documentation explaining what DT_* means

**If NOT USED or LEGACY:**
- Remove entire DataModels folder
- Document removal in CHANGELOG_11.0.0.md
- Provide migration path if any external code depends on them

**Audit command:**
```bash
# Check if any DT_* classes are used outside their own folder
rg "DT_Component|DT_Hero|DT_Ingredient" --type cs --glob '!DataModels/**' --glob '!Models/**'
```

---

### 5. Clean Models Folder

**Keep (unit-domain specific):**
- `BoundingBox.cs` - measurement-aware spatial calculations
- `HighResPosition.cs` - precision 3D coordinates
- `HighResOffset.cs` - precision offsets
- `UDTO_BoundingBox.cs` - serialization DTO for BoundingBox
- `UDTO_HighResPosition.cs` - serialization DTO for HighResPosition

**Remove (infrastructure or dead code):**
- `ContextWrapper.cs` ❌
- `MockDataGenerator.cs` ❌
- `ControlParameterCSV.cs` ❌
- `DT_Error.cs` ❌ (if exists and unused)
- `DT_Info.cs` ❌ (if exists and unused)
- `DT_Success.cs` ❌ (if exists and unused)
- `DT_Warning.cs` ❌ (if exists and unused)

**Evaluate:**
- `DT_Base.cs` - if keeping, remove wrapper methods
- `DT_Part.cs` - determine if unit-specific or generic
- `DT_StatusText.cs` - determine usage

---

## 🚀 Implementation Plan

### Phase 0: Migrate ContextWrapper to FoundryMicroCore (NEW - HIGH PRIORITY)
1. ✅ Create `FoundryMicroCore.Library/Core/ContextWrapper.cs`
2. ✅ Copy content from `FoundryRulesAndUnits/Models/ContextWrapper.cs`
3. ✅ Update namespace: `FoundryRulesAndUnits.Models` → `FoundryMicroCore.Core`
4. ✅ Build FoundryMicroCore and verify no errors
5. ✅ Update FoundryMicroCore version to 1.8.0 in .csproj
6. ✅ Add documentation to FoundryMicroCore README about REST API pattern
7. ✅ Create CHANGELOG_v1.8.0.md documenting the addition
8. ✅ Update FoundryRulesAndUnits to use `FoundryMicroCore.Core.ContextWrapper<T>`
9. ✅ Remove `FoundryRulesAndUnits/Models/ContextWrapper.cs`
10. ✅ Test both libraries build successfully

### Phase 1: Safe Removals (After Phase 0 - Zero Risk)
1. ✅ Delete `Models/ControlParameterCSV.cs` (only remaining dead code)
2. ✅ Build and verify no compilation errors

### Phase 2: Documentation Updates (After Phase 0)
1. ✅ Update README.md to remove StatusBitArray sections
2. ✅ Update README.md to note ContextWrapper moved to FoundryMicroCore
3. ✅ Update README.md to add Dependencies section
4. ✅ Add historical notes to CHANGELOG_10.7.0.md (ContextWrapper)
5. ✅ Add historical notes to CHANGELOG_10.8.0.md (StatusBitArray)
6. ✅ Add historical notes to CHANGELOG_10.8.1.md (StatusBitArray)
7. ✅ Create CHANGELOG_11.0.0.md documenting migration and cleanup

### Phase 3: DataModels Evaluation (Requires Analysis)
1. 🔍 Run usage audit on DT_* classes
2. 🔍 Check if any consuming projects use these
3. 📋 Document findings
4. 🎯 Decide: keep/simplify/remove
5. 🎯 If keeping: remove StatusBitArray wrapper methods from DT_Base

### Phase 4: Version Bump (After testing)
1. ✅ Update FoundryMicroCore to 1.8.0 (includes ContextWrapper)
2. ✅ Update FoundryRulesAndUnits to 11.0.0 in .csproj
3. ✅ Add package release notes
4. ✅ Test build
5. ✅ Test NuGet package creation
6. 🚀 Publish both packages

### Phase 5: Downstream Updates
1. Update consuming projects (FoundryWorldsAndDrawings, etc.)
2. Update using statements: `FoundryRulesAndUnits.Models` → `FoundryMicroCore.Core`
3. Verify no breaking changes in actual usage
4. Document any required migrations

---

## 📊 Impact Analysis

### Lines of Code Changed (Estimated)
- ContextWrapper.cs: ~293 lines **MOVED** (not removed)
- MockDataGenerator.cs: ~145 lines **KEPT** (testing utility)
- ControlParameterCSV.cs: ~12 lines **REMOVED**
- **Net change**: -12 lines in FoundryRulesAndUnits, +293 lines in FoundryMicroCore

### Files Changed
- FoundryMicroCore: +1 file (ContextWrapper.cs added)
- FoundryRulesAndUnits: -1 file (ContextWrapper moved, MockDataGenerator kept, ControlParameterCSV removed)
- Potential: Up to 10+ files if DT_Error/Info/Success/Warning are dead code

### Documentation Updated
- README.md: Major restructure (FoundryRulesAndUnits)
- README.md: Add ContextWrapper section (FoundryMicroCore)
- 3-4 CHANGELOG files: Historical notes added (FoundryRulesAndUnits)
- 1 new CHANGELOG_11.0.0.md (FoundryRulesAndUnits)
- 1 new CHANGELOG_v1.8.0.md (FoundryMicroCore)

### Risk Assessment
- **Compilation Risk**: NONE - moved/removed code has proper migration path
- **Runtime Risk**: LOW - ContextWrapper moves but keeps same functionality
- **Downstream Risk**: LOW - namespace change documented, functionality preserved
- **User Confusion Risk**: REDUCED - clearer documentation about what's included
- **API Breaking**: YES - namespace change for ContextWrapper users

---

## ✅ Success Criteria

After refactoring, the architecture will have:

### FoundryMicroCore.Library v1.8.0
1. ✅ **Gains ContextWrapper<T>** - REST API response pattern available to all projects
2. ✅ **No other changes** - remains stable foundational framework
3. ✅ **Updated documentation** - includes REST API pattern guidance
4. ✅ **Clear responsibility** - infrastructure and foundational patterns

### FoundryRulesAndUnits v11.0.0
1. ✅ **Build cleanly** with no unused code warnings
2. ✅ **Have accurate documentation** reflecting actual code
3. ✅ **Clearly depend on FoundryMicroCore** without duplicating it
4. ✅ **Focus on unit domain** - measurements, conversions, calculations
5. ✅ **Be smaller** - ~157 fewer lines of non-domain code
6. ✅ **Be clearer** - no confusion about StatusBitArray ownership
7. ✅ **Be maintainable** - one responsibility, clear boundaries
8. ✅ **Use ContextWrapper from FoundryMicroCore** - for consistent REST API patterns

### Architectural Benefits
1. ✅ **Single Source of Truth** - ContextWrapper in FoundryMicroCore for all projects
2. ✅ **Proper Separation** - Infrastructure vs. Domain concerns
3. ✅ **Reusability** - FoundryWorldsAndDrawings can use ContextWrapper directly
4. ✅ **Consistency** - All projects use same REST API pattern
5. ✅ **Maintainability** - One place to evolve REST response patterns

---

## 🔄 Migration Examples

### Before (v10.11.0)
```csharp
// In FoundryRulesAndUnits controller
using FoundryRulesAndUnits.Models;

[ApiController]
public class UnitsController : ControllerBase
{
    [HttpGet("conversions")]
    public ContextWrapper<ConversionResult> GetConversions()
    {
        var results = _service.GetConversions();
        return ContextWrapper<ConversionResult>.Ok(results);
    }
}
```

### After (v11.0.0)
```csharp
// In any project using FoundryMicroCore
using FoundryMicroCore.Core;

[ApiController]
public class UnitsController : ControllerBase
{
    [HttpGet("conversions")]
    public ContextWrapper<ConversionResult> GetConversions()
    {
        var results = _service.GetConversions();
        return ContextWrapper<ConversionResult>.Ok(results); // Same API!
    }
}
```

### For FoundryWorldsAndDrawings
```csharp
// Now can use ContextWrapper directly from FoundryMicroCore
using FoundryMicroCore.Core;

[ApiController]
public class WorldsController : ControllerBase
{
    [HttpGet("worlds")]
    public ContextWrapper<WorldDTO> GetWorlds()
    {
        var worlds = _service.GetWorlds();
        return ContextWrapper<WorldDTO>.Ok(worlds);
    }
}
```

---

## 📚 References

- **FoundryMicroCore.Library**: v1.7.0 → v1.8.0 - Component framework + REST API patterns
- **FoundryRulesAndUnits**: v10.11.0 → v11.0.0 - This refactoring
- **Project Structure**: Multi-workspace with clear dependency hierarchy
- **Breaking Change**: Namespace migration only, functionality preserved

---

## 🎯 Conclusion

This refactoring achieves **architectural alignment**:

### What We Learned
- **ContextWrapper IS valuable** - excellent REST API pattern for uniform processing
- **Location was wrong** - infrastructure should be in FoundryMicroCore
- **Documentation was misleading** - claimed ownership of borrowed features
- **Domain focus needed** - FoundryRulesAndUnits should concentrate on units/measurements

### What We Accomplished
1. **✅ Promoted ContextWrapper** to infrastructure level (FoundryMicroCore)
2. **✅ Removed dead code** (MockDataGenerator, ControlParameterCSV)
3. **✅ Corrected documentation** (accurate attribution)
4. **✅ Preserved functionality** (no features lost)
5. **✅ Improved reusability** (all projects can use ContextWrapper)
6. **✅ Clarified boundaries** (infrastructure vs. domain)

### The Result
- **FoundryMicroCore.Library**: More complete infrastructure foundation
- **FoundryRulesAndUnits**: Focused, honest, maintainable units library
- **All Projects**: Access to consistent REST API patterns

**Architectural honesty achieved**: Each library owns what it actually provides.

### Lines of Code Removed (Estimated)
- ContextWrapper.cs: ~293 lines
- MockDataGenerator.cs: ~145 lines
- ControlParameterCSV.cs: ~12 lines
- **Total: ~450 lines** of infrastructure code removed

### Files Removed
- Minimum: 3 files (ContextWrapper, MockDataGenerator, ControlParameterCSV)
- Potential: Up to 10+ files if DT_Error/Info/Success/Warning are dead code

### Documentation Updated
- README.md: Major restructure
- 3-4 CHANGELOG files: Historical notes added
- 1 new CHANGELOG_11.0.0.md

### Risk Assessment
- **Compilation Risk**: NONE - removed code has no references
- **Runtime Risk**: NONE - removed code never executed
- **Downstream Risk**: LOW - documented as breaking change but nothing actually breaks
- **User Confusion Risk**: REDUCED - clearer documentation about what's included

---

## ✅ Success Criteria

After refactoring, FoundryRulesAndUnits will:

1. ✅ **Build cleanly** with no unused code warnings
2. ✅ **Have accurate documentation** reflecting actual code
3. ✅ **Clearly depend on FoundryMicroCore** without duplicating it
4. ✅ **Focus on unit domain** - measurements, conversions, calculations
5. ✅ **Be smaller** - ~450 fewer lines of infrastructure code
6. ✅ **Be clearer** - no confusion about StatusBitArray ownership
7. ✅ **Be maintainable** - one responsibility, clear boundaries

---

## 📚 References

- **FoundryMicroCore.Library**: v1.7.0 - Component framework (gospel)
- **FoundryRulesAndUnits**: v10.11.0 → v11.0.0 - This refactoring
- **Project Structure**: Multi-workspace with clear dependency hierarchy

---

## 🎯 Conclusion

This refactoring is about **architectural honesty**:
- Don't document features that don't exist or aren't yours
- Don't duplicate infrastructure from dependencies
- Focus on what you do best: units and measurements

FoundryRulesAndUnits will be a better, clearer library after this cleanup.
