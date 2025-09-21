# Unit Types Gold Standard Documentation

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

### Complete Gold Standard Implementation
Based on `Angle.cs`:

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

**This is the AUTHORITATIVE pattern that ALL other unit types must follow exactly!**

**NOTE:** `Angle.cs` and `Length.cs` are the definitive reference implementations. When in doubt, match their exact structure and implementation details.