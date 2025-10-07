# Unit Types Pattern Documentation - GOLD STANDARD

## Overview
This document outlines the **AUTHORITATIVE GOLD STANDARD** pattern that ALL unit type classes must follow. The definitive examples are `Angle.cs` and `Length.cs` - these represent the complete, correct implementation that all other unit types must match exactly.

## GOLD STANDARD PATTERN (Based on Angle.cs and Length.cs)

### 1. Class Declaration
```csharp
[System.Serializable]
public class UnitTypeName : MeasuredValue
{
    public override UnitFamilyName UnitFamily => UnitFamilyName.UnitTypeName;
    // NO backward compatibility constructors - use UnitFactory.CreateUnitTypeName() instead
```

### 2. Constructor Pattern (UnitGroup Injection ONLY)
```csharp
/// <summary>
/// Constructor with UnitGroup injection - use UnitFactory to create instances
/// </summary>
public UnitTypeName(UnitGroup unitGroup) : base(unitGroup)
{
    if (unitGroup.Family != UnitFamilyName.UnitTypeName)
        throw new ArgumentException($"UnitGroup must be for UnitTypeName family, got {unitGroup.Family}");
}
```

**CRITICAL:** NO backward compatibility constructors allowed

### 3. Required Instance Methods (MANDATORY)
```csharp
public UnitTypeName Assign(double value, string? units)
{
    Init(value, units); // Base class handles everything!
    return this;
}

public UnitTypeName Assign(UnitTypeName source)
{
    Init(source.Value(), source.U); // Base class handles everything!
    return this;
}

public UnitTypeName Copy()
{
    var copy = new UnitTypeName(_unitGroup);
    copy.Init(Value(), Internal());
    return copy;
}
```

### 4. Factory Pattern Comments (REQUIRED)
```csharp
// Static factory methods removed - use UnitFactory.CreateUnitTypeName() instead
// Example: factory.CreateUnitTypeName(90, "deg") or factory.CreateUnitTypeName(value, "unit")
```

### 5. Complete Operator Set (MANDATORY)
```csharp
// Comparison operators (required)
public static bool operator <(UnitTypeName left, UnitTypeName right) => left.Value() < right.Value();
public static bool operator >(UnitTypeName left, UnitTypeName right) => left.Value() > right.Value();

// Arithmetic operators (basic required)
public static UnitTypeName operator +(UnitTypeName left, UnitTypeName right)
{
    var result = new UnitTypeName(left._unitGroup);
    result.Init(left.Value() + right.Value(), left.Internal());
    return result;
}

public static UnitTypeName operator -(UnitTypeName left, UnitTypeName right)
{
    var result = new UnitTypeName(left._unitGroup);
    result.Init(left.Value() - right.Value(), left.Internal());
    return result;
}

// Scalar operations (where applicable - see Length.cs for full set)
public static UnitTypeName operator *(double left, UnitTypeName right)
{
    var result = new UnitTypeName(right._unitGroup);
    result.Init(left * right.Value(), right.Internal());
    return result;
}

public static UnitTypeName operator /(UnitTypeName left, double right)
{
    var result = new UnitTypeName(left._unitGroup);
    result.Init(left.Value() / right, left.Internal());
    return result;
}
```
## PROHIBITED PATTERNS (Must Be Removed)

❌ **Backward compatibility constructors** - Use UnitFactory instead  
❌ **Static factory methods** - Use UnitFactory.CreateUnitTypeName() instead  
❌ **JSON converter classes** - Not part of core unit pattern  
❌ **Manual unit conversion logic** - Delegate to base class  
❌ **Compact/shorthand operators** - Must use full UnitGroup pattern  

## COMPLIANCE REQUIREMENTS

### MANDATORY CHECKLIST for ALL Unit Types:

- [ ] `[System.Serializable]` attribute present
- [ ] Inherits from `MeasuredValue`  
- [ ] `public override UnitFamilyName UnitFamily` format
- [ ] Single UnitGroup constructor with XML docs
- [ ] `Assign(double, string)` method present
- [ ] `Assign(UnitTypeName)` method present  
- [ ] `Copy()` method present
- [ ] Factory pattern comments present
- [ ] `<` and `>` comparison operators
- [ ] `+` and `-` arithmetic operators using UnitGroup pattern
- [ ] Scalar `*` and `/` operators where applicable
- [ ] NO backward compatibility constructors
- [ ] NO static factory methods
- [ ] Uses `left._unitGroup` in operator implementations

## IMPLEMENTATION EXAMPLES

### Example 1: Minimal Gold Standard Implementation
Based on `Angle.cs` (simplified):

```csharp
[System.Serializable]
public class Angle : MeasuredValue
{
    public override UnitFamilyName UnitFamily => UnitFamilyName.Angle;
    // NO backward compatibility constructors - use UnitFactory.CreateAngle() instead

    /// <summary>
    /// Constructor with UnitGroup injection - use UnitFactory to create instances
    /// </summary>
    public Angle(UnitGroup unitGroup) : base(unitGroup)
    {
        if (unitGroup.Family != UnitFamilyName.Angle)
            throw new ArgumentException($"UnitGroup must be for Angle family, got {unitGroup.Family}");
    }

    public Angle Assign(double value, string? units)
    {
        Init(value, units); // Base class handles everything!
        return this;
    }

    public Angle Assign(Angle source)
    {
        Init(source.Value(), source.U); // Base class handles everything!
        return this;
    }

    public Angle Copy()
    {
        var copy = new Angle(_unitGroup);
        copy.Init(Value(), Internal());
        return copy;
    }

    // Static factory methods removed - use UnitFactory.CreateAngle() instead

    public static bool operator <(Angle left, Angle right) => left.Value() < right.Value();
    public static bool operator >(Angle left, Angle right) => left.Value() > right.Value();

    public static Angle operator +(Angle left, Angle right)
    {
        var result = new Angle(left._unitGroup);
        result.Init(left.Value() + right.Value(), left.Internal());
        return result;
    }
    
    public static Angle operator -(Angle left, Angle right)
    {
        var result = new Angle(left._unitGroup);
        result.Init(left.Value() - right.Value(), left.Internal());
        return result;
    }
}
```

This is the AUTHORITATIVE pattern that ALL other unit types must follow exactly!

**NOTE:** `Angle.cas` and `Length.cs` are the definitive reference implementations. When in doubt, match their exact structure and implementation details.
```csharp
public static bool operator <({UnitTypeName} left, {UnitTypeName} right) => 
    left.Value() < right.Value();
public static bool operator >({UnitTypeName} left, {UnitTypeName} right) => 
    left.Value() > right.Value();
public static bool operator <=({UnitTypeName} left, {UnitTypeName} right) => 
    left.Value() <= right.Value();
public static bool operator >=({UnitTypeName} left, {UnitTypeName} right) => 
    left.Value() >= right.Value();
```

### 5. Regional Organization Pattern

#### Standard Regions (Optional but Recommended)
```csharp
#region Constructors and Factory Methods
// Constructor goes here
#endregion

#region Unit Conversion
// As() method inherited from MeasuredValue - no override needed
#endregion

#region Operators
// All operator overloads go here
#endregion
```

## Implementation Variants by Complexity

### Level 1: Minimal Implementation (Distance, Speed, Time)
- UnitFamily override
- Single UnitGroup constructor with validation
- Standard 6 arithmetic operators
- Basic structure only

### Level 2: Standard Implementation (Mass, Area, Temperature)
- Level 1 plus:
- Regional organization with `#region` markers
- Optional utility methods (Assign, Copy)
- XML documentation comments

### Level 3: Enhanced Implementation (Current, Voltage, Percent)
- Level 2 plus:
- Comparison operators (`<`, `>`, `<=`, `>=`)
- Advanced operator patterns with unit conversion

### Level 4: Specialized Implementation (Length, Angle)
- Level 3 plus:
- Domain-specific utility methods (e.g., `AsPixels()`, `Degrees()`)
- Legacy compatibility methods
- Cross-unit operation support

## Prohibited Patterns

### ❌ Do NOT Include:
1. **Backward compatibility constructors** - Use UnitFactory instead
2. **Static factory methods** - Use UnitFactory.Create{Type}() instead
3. **Direct unit conversion logic** - Delegate to base class `As()` method
4. **Manual unit validation** - UnitGroup handles this
5. **Global unit system dependencies** - Use injected UnitGroup
6. **JSON converter classes** - Not part of the core unit type pattern

### ❌ Anti-Patterns:
```csharp
// DON'T: Backward compatibility constructor
public Mass(double value, string units) : base() { }

// DON'T: Static factory methods
public static Mass CreateKilograms(double value) { }

// DON'T: Override As() method unnecessarily
public override double As(string units) { }

// DON'T: Manual unit validation
if (!IsValidUnit(units)) throw new Exception();

// DON'T: JSON converter classes in unit types
[JsonConverter(typeof(MassJsonConverter))]
public class Mass : MeasuredValue { }
```

## Special Cases and Exceptions

### Distance vs Length
- `Distance` class returns `UnitFamilyName.Length` for backward compatibility
- `Length` class is the standard implementation
- Both are maintained for legacy support

### Dimensionless
- Returns `UnitFamilyName.None` instead of matching class name
- Supports multiplication and division between Dimensionless objects
- Used as safe fallback for unknown unit types

## Compliance Checklist

For any new unit type class, ensure:

- [ ] `[System.Serializable]` attribute present
- [ ] Inherits from `MeasuredValue`
- [ ] Overrides `UnitFamily` property with correct enum value
- [ ] Has single `UnitGroup` constructor with validation
- [ ] Implements all 6 standard arithmetic operators
- [ ] Uses one of the approved operator implementation patterns
- [ ] Follows consistent naming conventions
- [ ] Includes appropriate XML documentation
- [ ] Does not include prohibited patterns or anti-patterns

## Future Maintenance Guidelines

1. **Consistency First**: Any changes to the pattern must be applied uniformly across all unit types
2. **UnitFactory Integration**: All creation should go through UnitFactory, not direct constructors
3. **Base Class Delegation**: Leverage MeasuredValue base class methods instead of reimplementation
4. **Unit Group Injection**: Always use injected UnitGroup for conversions and validation
5. **Pattern Evolution**: If the pattern needs to evolve, update this documentation and all implementations simultaneously

This pattern ensures that all unit types behave consistently, are easily maintainable, and integrate seamlessly with the broader unit system architecture.

## Dynamic Type Creation Analysis

### ✅ **Pattern Enables Smart Factory Creation**

The uniform pattern across all unit types is **essential** for enabling the factory and unit system to create the correct variable types dynamically during parsing. Here's how:

### 1. **UnitFactory Dynamic Creation Methods**

The `UnitFactory` class leverages the consistent pattern through multiple approaches:

#### A. **Specific Factory Methods** (Type-Safe)
```csharp
public Length CreateLength(double value = 0, string? units = null)
{
    var unitGroup = _unitGroups[UnitFamilyName.Length];
    var length = new Length(unitGroup);  // ✅ Pattern's UnitGroup constructor
    var defaultUnit = units ?? unitGroup.BaseUnit.Symbol;
    length.Init(value, defaultUnit);     // ✅ Pattern's Init method
    return length;
}
```

#### B. **Generic Factory Method** (Dynamic)
```csharp
public MeasuredValue CreateMeasuredValue(UnitFamilyName family, double value = 0, string? units = null)
{
    if (!_unitGroups.TryGetValue(family, out var unitGroup))
        throw new ArgumentException($"Unit family {family} not available");

    var measuredValue = new MeasuredValue(unitGroup);  // Uses base class
    var defaultUnit = units ?? unitGroup.BaseUnit.Symbol;
    measuredValue.Init(value, defaultUnit);
    return measuredValue;
}
```

### 2. **UnitSystem Smart Unit Lookup**

The `UnitSystem` creates a lookup table that maps unit strings directly to creation functions:

```csharp
private Dictionary<string, UnitLookupInfo> BuildUnitLookupCache()
{
    var lookup = new Dictionary<string, UnitLookupInfo>();
    var factory = GetFactory();
    
    foreach (var unitDef in _currentSystem.UnitDefinitions)
    {
        // Create factory function for this specific unit and family
        Func<double, MeasuredValue> createFunc = (value) => 
            factory.CreateMeasuredValue(unitDef.Family, value, unitDef.Symbol);
        
        lookup[unitDef.Symbol] = new UnitLookupInfo(
            unitDef.Family,
            unitDef,
            createFunc  // ✅ Dynamic creation function
        );
    }
    return lookup;
}
```

### 3. **Parser Integration via CreateMeasuredValueFromParsableUnit**

During parsing, the system can instantly create the correct type:

```csharp
public MeasuredValue CreateMeasuredValueFromParsableUnit(string unit, double value)
{
    var lookup = GetUnitLookup();
    if (lookup.TryGetValue(unit, out var unitInfo))
    {
        return unitInfo.CreateMeasuredValue(value);  // ✅ Creates correct type!
    }
    throw new ArgumentException($"Invalid unit symbol: {unit}");
}
```

### 4. **KnParameter Dynamic Creation Pattern**

The `KnParameter.CreateMeasuredValueFromParsableUnits` method demonstrates how the pattern enables parser integration:

```csharp
public static MeasuredValue CreateMeasuredValueFromParsableUnits(double value, string units)
{
    var unitFamily = DetermineUnitFamily(units);  // O(1) lookup
    
    return unitFamily switch
    {
        UnitFamilyName.Angle => new Angle(value, units),      // ✅ Pattern's constructor
        UnitFamilyName.Length => new Length(value, units),    // ✅ Pattern's constructor  
        UnitFamilyName.Mass => new Mass(value, units),        // ✅ Pattern's constructor
        // ... all other types follow same pattern
        _ => new Dimensionless(value, units)  // Safe fallback
    };
}
```

### **Why the Pattern is Critical**

1. **Consistent Constructor Signature**: All unit types have `(UnitGroup unitGroup)` constructor, enabling uniform factory creation
2. **Standardized Initialization**: All use `Init(value, units)` method, enabling consistent setup
3. **Family Validation**: All validate their `UnitFamilyName` in constructor, preventing mismatched creations
4. **Type Safety**: Factory methods return specific types, not just `MeasuredValue`

### **Parser Flow Example**
```
Parser encounters: "45.5 kg"
   ↓
UnitSystem.CreateMeasuredValueFromParsableUnit("kg", 45.5)
   ↓ 
UnitLookupInfo["kg"].CreateMeasuredValue(45.5)
   ↓
Factory.CreateMeasuredValue(UnitFamilyName.Mass, 45.5, "kg")
   ↓
new Mass(unitGroup).Init(45.5, "kg")  // ✅ Pattern enables this!
   ↓
Returns: Mass instance with correct type and value
```

**Conclusion**: The uniform pattern is **absolutely essential** for dynamic type creation. Without the consistent constructor signatures, initialization methods, and family validation, the factory system could not reliably create the correct `MeasuredValue` types during parsing.