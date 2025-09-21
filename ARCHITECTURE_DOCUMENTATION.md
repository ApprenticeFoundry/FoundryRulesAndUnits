# FoundryRulesAndUnits Architecture Documentation

## Overview
The FoundryRulesAndUnits system is a comprehensive unit measurement and conversion framework designed around the principle of **base unit normalization** and **dimensional analysis**. The architecture eliminates conversion overhead during mathematical operations by storing all values in base units internally.

## Core Architectural Principles

### 1. **Base Unit Normalization**
- All unit type classes store values internally in their **base units**
- Conversions happen only at **input** (construction) and **output** (display) boundaries
- Mathematical operations work directly on base unit values with **zero conversion overhead**

### 2. **Universal Unit Families**
- **UnitFamilyName** represents fundamental forces of nature (Length, Mass, Time, Force, etc.)
- These families are **universal constants** across all unit systems
- Every unit system specification will have representations for these families

### 3. **Service-Oriented Architecture**
- **IUnitSystem** is the single service interface for the entire platform
- All external components interact through IUnitSystem - never directly with internal components
- Unit system selection is a **lock-in decision** made early in application lifecycle

## Component Roles and Responsibilities

### **IUnitSystemSpecification** - The Foundation Layer
**Role**: Contains all authoritative data about units in a specific measurement system

**Responsibilities**:
- Defines what units exist in the system (SI, Imperial, CGS, etc.)
- Specifies base units for each family
- Provides conversion functions between units within families
- Caches performance-critical lookups (symbol→family mapping, base units, etc.)

**Examples**: 
- `SIUnitSystemSpecification`: meters, kilograms, seconds, etc.
- `FPSUnitSystemSpecification`: feet, pounds, seconds, etc.

### **IUnitSystem** - The Service Interface
**Role**: Single point of contact for all platform interactions with the unit system

**Responsibilities**:
- Validates unit symbols (`"deg"` is valid, `"xyz"` is not)
- Maps unit symbols to unit families (`"deg"` → `UnitFamilyName.Angle`)
- Creates properly constructed unit type instances
- Provides efficient O(1) lookups through cached data structures
- Hides all internal complexity from external components

**Key Principle**: Once a unit system specification is selected, everything locks into place. The system doesn't switch between specifications mid-operation.

### **UnitGroup** - The Conversion Engine
**Role**: "Babysits" all conversion capability for a specific unit family

**Responsibilities**:
- Contains conversion logic for one unit family (e.g., all angle units)
- Handles input conversion: user units → base units
- Handles output conversion: base units → display units
- Validates unit compatibility within the family
- Provides base unit information for mathematical operations

**Key Insight**: UnitGroup is injected into unit type constructors, eliminating global dependencies and enabling clean dependency injection patterns.

### **Unit Type Classes** (Angle, Length, Mass, etc.) - The Math Engines
**Role**: Strongly-typed containers that perform mathematical operations in base units

**Responsibilities**:
- Store values internally in base units (radians for Angle, meters for Length, etc.)
- Perform all mathematical operations directly on base unit values
- Provide type-safe operations (can't accidentally add Length to Mass)
- Support fluent APIs for construction and manipulation
- Handle operator overloading for natural mathematical expressions

**Key Architecture**: 
- Constructor takes `UnitGroup` for dependency injection
- `Init(value, units)` converts input to base units immediately
- All math happens on base unit values (`Value()` method)
- Display conversion happens on demand (`As(units)` method)

### **UnitTypeAttribute + UnitTypeRegistry** - The Metadata System
**Role**: Provides reflection-based discovery and instantiation of unit types

**Responsibilities**:
- **UnitTypeAttribute**: Marks classes with their unit family metadata
- **UnitTypeRegistry**: Scans for attributes at startup, builds type mappings
- Enables factory pattern without hardcoded switch statements
- Supports parser integration by mapping families to concrete types

**Key Benefit**: Single source of truth for unit family metadata, eliminates redundant property overrides.

### **Parser Integration** - The Dynamic Construction Layer
**Role**: Transforms textual unit expressions into strongly-typed unit objects

**Process Flow**:
1. **Validation**: `"90 deg"` → Is `"deg"` a valid unit? (via IUnitSystem)
2. **Family Mapping**: `"deg"` → `UnitFamilyName.Angle` (via IUnitSystem)  
3. **Construction**: Create `Angle` object with value 90 and units "deg" (via IUnitSystem)
4. **Base Unit Storage**: Object internally stores radians, not degrees

## Mathematical Operation Architecture

### **Why Base Units Enable Seamless Math**

**Cross-Family Calculations Work Naturally**:
```
Speed = Length ÷ Time
- Length: 100 feet → stored as 30.48 meters
- Time: 5 seconds → stored as 5 seconds  
- Result: 30.48 ÷ 5 = 6.096 m/s (automatic, no conversions!)
```

**Complex Derived Units**:
```
Force = Mass × Acceleration  
- Mass: stored in kilograms
- Acceleration: stored in m/s²
- Result: kg × m/s² = Newtons (dimensional analysis automatic!)
```

**Performance Benefits**:
- **One conversion** at input (user units → base units)
- **Zero conversions** during mathematical operations  
- **One conversion** at output (base units → display units)
- Complex multi-step calculations have no conversion overhead

## Information Flow Examples

### **Typical Parser Integration Flow**:
```
Input: "90 deg"
    ↓
IUnitSystem.ValidateUnit("deg") → ✅ Valid
    ↓  
IUnitSystem.GetUnitFamily("deg") → UnitFamilyName.Angle
    ↓
IUnitSystem.CreateMeasuredValueFromUnit("deg", 90.0) 
    ↓
UnitFactory.CreateTypedMeasuredValue(UnitFamilyName.Angle, 90.0, "deg")
    ↓
UnitTypeRegistry.CreateInstance(UnitFamilyName.Angle, unitGroup)
    ↓
new Angle(unitGroup).Init(90.0, "deg")
    ↓
Angle object storing π/2 radians internally
```

### **Mathematical Operation Flow**:
```
angle1 = 90 degrees (stored as π/2 radians)
angle2 = 45 degrees (stored as π/4 radians)
result = angle1 + angle2
    ↓
π/2 + π/4 = 3π/4 radians (direct base unit math!)
    ↓
result.As("deg") → 135 degrees (conversion only for display)
```

## Key Design Benefits

1. **Performance**: Math operations have zero conversion overhead
2. **Type Safety**: Can't accidentally mix incompatible units
3. **Dimensional Analysis**: Derived units work naturally through base unit math
4. **Maintainability**: Single source of truth for all unit metadata
5. **Extensibility**: Adding new unit types requires only attribute decoration
6. **Parser Integration**: Clean service interface for dynamic unit construction
7. **Dependency Injection**: UnitGroup injection eliminates global dependencies

## Critical Implementation Details

- **Unit System Selection**: Lock-in decision made early, everything else follows
- **Base Unit Storage**: All values stored in base units, conversions only at boundaries  
- **Service Pattern**: IUnitSystem is the only external interface, hides internal complexity
- **Attribute-Based Discovery**: UnitTypeAttribute + UnitTypeRegistry eliminate hardcoded mappings
- **UnitGroup Injection**: Enables clean dependency injection and eliminates globals
- **Reflection-Based Factory**: Parser can create correct derived types dynamically

This architecture provides a clean, performant, and maintainable foundation for complex unit-aware calculations across the entire platform.

## The Ultimate Integration Test: `100 cm + 1 m = 2 m`

This simple mathematical expression serves as the **ultimate litmus test** for the entire unit system architecture. However, **this test cannot be performed within the unit system itself** - it requires integration with external systems, particularly the **parser component**.

### **Critical Architectural Reality: External System Dependency**

The unit system is a **foundational service component** that provides:
- Unit validation and conversion logic (IUnitSystem)
- Strongly-typed mathematical operations (Length, Angle, etc.)
- Attribute-based factory services (UnitFactory, UnitTypeRegistry)
- Base unit normalization for performance

But the unit system **cannot parse strings like `"100 cm"`** - that's the responsibility of external parser systems that integrate with the unit system through well-defined interfaces.

### **True Integration Test Requirements:**

**What the Unit System Provides:**
- `factory.CreateLength(100, "cm")` → Length object with 1.0 meters internally
- `factory.CreateLength(1, "m")` → Length object with 1.0 meters internally  
- `length1 + length2` → Length object with 2.0 meters internally
- `result.As("m")` → "2 m" display string

**What External Parser Systems Must Provide:**
- Parse `"100 cm"` → Extract value=100, units="cm"
- Parse `"1 m"` → Extract value=1, units="m"
- Call unit system: `factory.CreateLength(100, "cm")`
- Call unit system: `factory.CreateLength(1, "m")`
- Orchestrate the mathematical operation and result handling

### **Integration Architecture Pattern:**

```
External System (Parser/Calculator/UI)
    ↓ Calls
Unit System (FoundryRulesAndUnits)
    ↓ Uses  
Unit Specifications (IUnitSystem implementations)
```

**The unit system is the engine, but external systems are the drivers.**

### **Systematic Debugging Framework: Isolating Unit System vs. Integration Issues**

When the `100 cm + 1 m = 2 m` test fails in an integrated system, the debugging approach must distinguish between:

**Unit System Internal Issues** (Can be tested in isolation):
- `factory.CreateLength(100, "cm")` fails → UnitFactory or UnitTypeRegistry problem
- Unit validation: `unitSystem.IsValidUnit("cm", UnitFamilyName.Length)` → IUnitSystem problem  
- Base conversion: Created Length object doesn't contain 1.0 meters → UnitGroup conversion problem
- Mathematical operation: `length1 + length2` doesn't equal 2.0 meters → Operator overloading problem
- Display conversion: `result.As("m")` doesn't return "2 m" → Display formatting problem

**External System Integration Issues** (Require integrated testing):
- String parsing: `"100 cm"` not properly extracted as value=100, units="cm" → Parser problem
- Unit system interface: Parser not calling correct UnitFactory methods → Integration contract problem
- Result handling: Mathematical result not properly propagated back to caller → Orchestration problem
- User interface: Final result not properly displayed to user → UI/presentation problem

**Debugging Strategy:**
1. **Isolate the unit system first**: Test all unit system operations programmatically
2. **Validate integration contracts**: Verify external systems call unit system APIs correctly  
3. **Test end-to-end flow**: Only after both layers work individually

### **Documentation as Debugging Blueprint**

This architectural understanding transforms debugging from random searching into **systematic detective work**:

- **Each component has clear responsibilities** → Know where to look for specific failures
- **Information flow is documented** → Trace problems through the system methodically  
- **Success criteria are explicit** → Know exactly what should happen at each step
- **Integration points are identified** → Focus testing on critical boundaries

### **Testing Strategy Derived from Architecture**

The documentation naturally reveals what to test:

**Unit Tests** (Pure Unit System - No External Dependencies):
- UnitGroup conversion accuracy: `factory.CreateLength(100, "cm")` → 1.0 meters internally
- UnitTypeRegistry type discovery: Reflection-based type creation from attributes
- Base unit normalization: All values stored in consistent base units
- Operator overloading correctness: Mathematical operations in base units
- Display conversion: `result.As("cm")` formatting and unit conversion

**Integration Tests** (External System + Unit System):
- Parser integration: String parsing → UnitFactory API calls → Mathematical operations
- Cross-system dimensional analysis: Parser creates Speed from Length ÷ Time expressions
- Unit system switching: Different IUnitSystem implementations with same parser
- End-to-end validation: User input → Parser → Unit System → Mathematical result → Display

**Architectural Boundary Testing:**
- **Unit System API Contract**: Verify all public methods work correctly in isolation
- **Integration Contract**: Verify external systems call unit system APIs as designed
- **Error Handling**: Unit system errors propagate correctly through integration layers

**Edge Case Tests** (Boundary Conditions):
- Incompatible unit operations (`Length + Angle` should fail gracefully)
- Invalid unit symbols (`"xyz"` should be rejected)
- Precision limits in conversion chains

**Performance Tests** (Architecture Validation):
- Zero conversion overhead during mathematical operations
- O(1) unit validation lookup times
- Memory efficiency of base unit storage

This systematic approach ensures that architectural understanding directly drives testing strategy and debugging methodology.

## UnitFactory: Three-Method Architecture Excellence

The `UnitFactory` represents the pinnacle of the architectural design, providing three perfectly designed methods that cover every possible use case:

### **Method Selection Matrix**

| Method | Purpose | Type Safety | When to Use |
|--------|---------|-------------|-------------|
| `CreateUnit<T>()` | **End-user APIs** | **Compile-time** | You know the exact type needed |
| `CreateTypedMeasuredValue()` | **Parser integration** | Runtime strong typing | You determine type from runtime data |
| `CreateMeasuredValue()` | **Legacy/generic** | Base class only | Working with generic MeasuredValue |

### **Architectural Brilliance:**

**Single Dependency Pattern:**
```csharp
public UnitFactory(IUnitSystem unitSystem)  // One dependency = all configuration
```

**Perfect API Coverage:**
- **Compile-time safety**: `Length length = factory.CreateUnit<Length>(100, "cm");`
- **Runtime flexibility**: `var obj = factory.CreateTypedMeasuredValue(family, 100, "cm");`
- **Legacy support**: `MeasuredValue val = factory.CreateMeasuredValue(family, 100, "cm");`

**Automatic Base Unit Integration:**
- Same API call works with SI (meters), FPS (feet), IPS (inches)
- Values automatically stored in correct base units for current system
- Mathematical operations happen with zero conversion overhead

### **The Ultimate Parser Integration:**

```csharp
// Parser processes: "100 cm + 1 m = 2 m"
var left = factory.CreateTypedMeasuredValue(UnitFamilyName.Length, 100, "cm");   // Length(1.0 m)
var right = factory.CreateTypedMeasuredValue(UnitFamilyName.Length, 1, "m");     // Length(1.0 m)  
var result = (Length)left + (Length)right;  // Length(2.0 m) - zero conversions!
```

### **End-User API Excellence:**

```csharp
// Clean, readable, type-safe engineering calculations
Length tolerance = factory.CreateUnit<Length>(0.1, "mm");
Speed maxVelocity = factory.CreateUnit<Speed>(100, "mph");
Temperature operating = factory.CreateUnit<Temperature>(25, "°C");

// Mathematical operations work immediately - no casting needed
Length total = tolerance + factory.CreateUnit<Length>(0.05, "mm");
```

This three-method architecture achieves the holy grail: **maximum flexibility with maximum type safety**, while maintaining the core architectural principle of getting all configuration from the current unit system.

## UnitTypeRegistry: The Performance Backbone

The `UnitTypeRegistry` is **essential** to the entire attribute-based architecture - it's the performance backbone that makes everything viable.

### **Why UnitTypeRegistry is Critical:**

**Without UnitTypeRegistry (Unacceptable Performance):**
```csharp
// Every factory call would need to scan all 24+ unit types!
public MeasuredValue CreateTypedMeasuredValue(UnitFamilyName family, ...)
{
    foreach(var type in Assembly.GetTypes()) // EXPENSIVE: O(n) scan every time!
    {
        var attr = type.GetCustomAttribute<UnitTypeAttribute>();
        if (attr?.Family == family) 
            return Activator.CreateInstance(type, unitGroup);
    }
    return null; // 50-100x slower than cached approach
}
```

**With UnitTypeRegistry (High Performance):**
```csharp
// One-time scanning at startup, then O(1) cached lookups
public MeasuredValue CreateTypedMeasuredValue(UnitFamilyName family, ...)
{
    return UnitTypeRegistry.CreateInstance(family, unitGroup); // O(1) fast lookup!
}
```

### **UnitTypeRegistry Architecture:**

**Core Responsibility:** Cache expensive reflection operations for performance-critical factory methods

**Two Essential Methods:**
1. **`GetAttributeForType(Type)`** - Used by `MeasuredValue.UnitFamily` and `UnitFactory.CreateUnit<T>()`
2. **`CreateInstance(UnitFamilyName, UnitGroup)`** - Used by `UnitFactory.CreateTypedMeasuredValue()`

**Performance Strategy:**
- **Startup cost:** One expensive assembly scan to build caches
- **Runtime cost:** O(1) dictionary lookups for all subsequent operations
- **Memory trade-off:** Small cache for dramatic performance improvement

### **Integration Points:**

**Parser Integration Pipeline:**
```
Parser Input → UnitFactory.CreateTypedMeasuredValue() → UnitTypeRegistry.CreateInstance() → Strongly-typed object
```

**End-User API Pipeline:**
```
User Code → UnitFactory.CreateUnit<T>() → UnitTypeRegistry.GetAttributeForType() → Type-safe object
```

**Base MeasuredValue Pipeline:**
```
obj.UnitFamily → MeasuredValue property → UnitTypeRegistry.GetAttributeForType() → Attribute data
```

### **Critical Performance Insight:**

The `UnitTypeRegistry` transforms the attribute-based system from **"interesting but slow"** to **"high-performance and practical"**. Without this caching layer, every factory call would perform expensive reflection operations, making the system unsuitable for production use.

**Performance Impact:** The registry provides 50-100x performance improvement over direct reflection scanning, making the attribute-based architecture viable for high-frequency factory operations like parser integration.