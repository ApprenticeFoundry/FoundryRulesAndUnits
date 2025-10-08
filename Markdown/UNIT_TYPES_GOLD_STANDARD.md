# Unit Types Gold Standard Documentation

## Overview
This document outlines the **AUTHORITATIVE GOLD STANDARD** pattern that ALL unit type classes must follow. The definitive examples are `Angle.cs` and `Length.cs` - these represent the complete, correct implementation that all other unit types must match exactly.

**Current Version**: v9.1.0 targeting .NET 9.0  
**Architecture Pattern**: UnitGroup injection with UnitTypeAttribute registration  
**Creation Method**: Use `IUnitSystem.CreateXxx()` instead of constructors

## GOLD STANDARD PATTERN (Based on Angle.cs and Length.cs)

### 1. Class Declaration
```csharp
[System.Serializable]
[UnitType(UnitFamilyName.UnitTypeName, Description = "Unit type description")]
public class UnitTypeName : MeasuredValue
{
    // UnitFamily comes from UnitTypeAttribute - no need for redundant property override
    // NO backward compatibility constructors - use IUnitSystem.CreateUnitTypeName() instead
```

### 2. Constructor Pattern (UnitGroup Injection ONLY)
```csharp
/// <summary>
/// Constructor with UnitGroup injection - preferred for factory pattern
/// Use IUnitSystem.CreateUnitTypeName() to create instances
/// </summary>
public UnitTypeName(UnitGroup unitGroup) : base(unitGroup)
{
    if (unitGroup.Family != UnitFamilyName.UnitTypeName)
        throw new ArgumentException($"UnitGroup family must be {UnitFamilyName.UnitTypeName}", nameof(unitGroup));
}
```

**CRITICAL:** NO backward compatibility constructors allowed. Use `IUnitSystem.CreateXxx()` methods instead.

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
// Static factory methods removed - use IUnitSystem.CreateUnitTypeName() instead
// Example: unitSystem.CreateUnitTypeName(90, "deg") or unitSystem.CreateUnitTypeName(value, "unit")
```

### 5. Complete Operator Set (MANDATORY)
```csharp
// Comparison operators (required)
public static bool operator <(UnitTypeName left, UnitTypeName right) => left.Value() < right.Value();
public static bool operator >(UnitTypeName left, UnitTypeName right) => left.Value() > right.Value();
public static bool operator ==(UnitTypeName left, UnitTypeName right) => Math.Abs(left.Value() - right.Value()) < 1e-10;
public static bool operator !=(UnitTypeName left, UnitTypeName right) => !(left == right);

// Override Equals and GetHashCode to be consistent with == operator
public override bool Equals(object? obj) => obj is UnitTypeName other && this == other;
public override int GetHashCode() => Value().GetHashCode();

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

public static UnitTypeName operator *(UnitTypeName left, double right)
{
    var result = new UnitTypeName(left._unitGroup);
    result.Init(left.Value() * right, left.Internal());
    return result;
}

public static UnitTypeName operator /(UnitTypeName left, double right)
{
    var result = new UnitTypeName(left._unitGroup);
    result.Init(left.Value() / right, left.Internal());
    return result;
}

public static double operator /(UnitTypeName left, UnitTypeName right) => left.Value() / right.Value();
```

## PROHIBITED PATTERNS (Must Be Removed)

❌ **Backward compatibility constructors** - Use IUnitSystem.CreateXxx() instead  
❌ **Static factory methods** - Use IUnitSystem.CreateUnitTypeName() instead  
❌ **JSON converter classes** - Not part of core unit pattern  
❌ **Manual unit conversion logic** - Delegate to base class and UnitGroup  
❌ **Compact/shorthand operators** - Must use full UnitGroup pattern  
❌ **Missing UnitTypeAttribute** - Required for UnitTypeRegistry registration  

## COMPLIANCE REQUIREMENTS

### MANDATORY CHECKLIST for ALL Unit Types:

- [ ] `[System.Serializable]` attribute present
- [ ] `[UnitType(UnitFamilyName.Xxx, Description = "...")]` attribute present
- [ ] Inherits from `MeasuredValue`  
- [ ] UnitFamily property comes from UnitTypeAttribute (no redundant override)
- [ ] Single UnitGroup constructor with XML docs and family validation
- [ ] `Assign(double, string)` method present
- [ ] `Assign(UnitTypeName)` method present  
- [ ] `Copy()` method present
- [ ] Factory pattern comments present
- [ ] `<`, `>`, `==`, `!=` comparison operators
- [ ] `Equals()` and `GetHashCode()` overrides consistent with `==`
- [ ] `+` and `-` arithmetic operators using UnitGroup pattern
- [ ] Scalar `*` and `/` operators where applicable
- [ ] NO backward compatibility constructors
- [ ] NO static factory methods
- [ ] Uses `left._unitGroup` in operator implementations

## IMPLEMENTATION EXAMPLES

### Complete Gold Standard Implementation
Based on `Angle.cs` and `Length.cs` (v9.1.0):

```csharp
[System.Serializable]
[UnitType(UnitFamilyName.Angle, Description = "Angle measurement")]
public class Angle : MeasuredValue
{
    // UnitFamily comes from UnitTypeAttribute - no need for redundant property override
    // NO backward compatibility constructors - use IUnitSystem.CreateAngle() instead

    /// <summary>
    /// Constructor with UnitGroup injection - preferred for factory pattern
    /// Use IUnitSystem.CreateAngle() to create instances
    /// </summary>
    public Angle(UnitGroup unitGroup) : base(unitGroup)
    {
        if (unitGroup.Family != UnitFamilyName.Angle)
            throw new ArgumentException($"UnitGroup family must be {UnitFamilyName.Angle}", nameof(unitGroup));
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

    // Static factory methods removed - use IUnitSystem.CreateAngle() instead
    // Example: unitSystem.CreateAngle(90, "deg") or unitSystem.CreateAngle(value, "unit")

    // As() method inherited from MeasuredValue - no override needed!

    public static bool operator <(Angle left, Angle right) => left.Value() < right.Value();
    public static bool operator >(Angle left, Angle right) => left.Value() > right.Value();
    public static bool operator ==(Angle left, Angle right) => Math.Abs(left.Value() - right.Value()) < 1e-10;
    public static bool operator !=(Angle left, Angle right) => !(left == right);
    
    // Override Equals and GetHashCode to be consistent with == operator
    public override bool Equals(object? obj) => obj is Angle other && this == other;
    public override int GetHashCode() => Value().GetHashCode();

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

    public static Angle operator *(double left, Angle right)
    {
        var result = new Angle(right._unitGroup);
        result.Init(left * right.Value(), right.Internal());
        return result;
    }

    public static Angle operator *(Angle left, double right)
    {
        var result = new Angle(left._unitGroup);
        result.Init(left.Value() * right, left.Internal());
        return result;
    }

    public static Angle operator /(Angle left, double right)
    {
        var result = new Angle(left._unitGroup);
        result.Init(left.Value() / right, left.Internal());
        return result;
    }

    public static double operator /(Angle left, Angle right) => left.Value() / right.Value();
}
```

**This is the AUTHORITATIVE pattern that ALL other unit types must follow exactly!**

**NOTE:** `Angle.cs` and `Length.cs` are the definitive reference implementations. When in doubt, match their exact structure and implementation details.