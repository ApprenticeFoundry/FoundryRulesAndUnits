# Strong Typing Architecture in FoundryRulesAndUnits

## Overview
The FoundryRulesAndUnits system is built around **strongly-typed unit classes** that inherit from `MeasuredValue`. This provides compile-time type safety and prevents common unit conversion errors.

## Architecture Benefits

### 1. Type Safety
```csharp
Temperature temp = new Temperature(25, "C");
Length length = new Length(5, "m");

// This is safe and allowed:
Temperature sum = temp1 + temp2;

// This would cause a compile error:
// var invalid = temp + length; // Cannot add Temperature + Length
```

### 2. Each Physical Quantity Has Its Own Class
- `Temperature : MeasuredValue` - for thermal measurements
- `Length : MeasuredValue` - for distance measurements  
- `Mass : MeasuredValue` - for weight/mass measurements
- `Speed : MeasuredValue` - for velocity measurements
- `Volume : MeasuredValue` - for capacity measurements
- ...and 20+ other specialized types

### 3. Unit Category Association
Each class maintains its own unit category:
```csharp
public static Func<UnitCategory> Category = () =>
{
    return new UnitCategory("Temperature");
};
```

### 4. Type-Specific Operations
```csharp
Temperature temp = new Temperature(25, "C");
double fahrenheit = temp.As("F");  // 77°F
string display = temp.ToString();  // "25°C"
```

### 5. Operator Overloading
```csharp
Temperature temp1 = new Temperature(25, "C");
Temperature temp2 = new Temperature(10, "C");
Temperature sum = temp1 + temp2;    // 35°C
Temperature diff = temp1 - temp2;   // 15°C
```

## Complete Unit Family List

The system includes strongly-typed classes for all physical quantities:

**Core Physical Quantities:**
- Length, Distance, Area, Volume
- Mass, Force, Pressure
- Time, Duration, Speed
- Temperature, Energy, Power

**Electrical:**
- Current, Voltage, Resistance, Capacitance

**Data:**
- DataStorage, DataFlow, Frequency

**Angular:**
- Angle, Heading

**Other:**
- Percent, Quantity, QuantityFlow, WorkTime

## Testing Framework

The `TestRunner.RunAllTests()` method now includes:

1. **Unit System Validation** - Tests all 5 unit systems (IPS, FPS, MKS, CGS, mmNs)
2. **Cross-System Validation** - Verifies conversions work between systems
3. **Base Unit Validation** - Confirms each system has correct base units
4. **Comprehensive Conversion Tables** - Tests all unit families
5. **Strong Typing Demonstrations** - Shows type safety in action

## Usage Example

```csharp
// Create strongly-typed instances
var length = new Length(5.0, "m");
var temperature = new Temperature(25.0, "C");

// Type-safe conversions
double feet = length.As("ft");        // 16.4042 ft
double fahrenheit = temperature.As("F"); // 77.0°F

// Type-safe arithmetic
var length2 = new Length(3.0, "m");
var totalLength = length + length2;   // 8.0 m

// Each type knows its category
string lengthCategory = Length.Category().Title();     // "Length"
string tempCategory = Temperature.Category().Title();   // "Temperature"
```

This architecture ensures that unit operations are both type-safe and physically meaningful, preventing errors like adding temperature to length or confusing different types of measurements.
