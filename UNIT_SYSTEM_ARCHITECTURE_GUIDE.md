# Unit System Architecture Guide

## Overview

The FoundryRulesAndUnits unit system implements a revolutionary **hub-and-spoke architecture** that eliminates the traditional N² complexity of unit conversions while providing type-safe, performant, and extensible unit management across multiple engineering domains.

## 🏗️ Core Architecture Principles

### Hub-and-Spoke Conversion Model
Instead of maintaining N² conversion matrices, each unit knows how to convert to/from its base unit:

```csharp
// Traditional N² approach (BAD):
Dictionary<(string from, string to), double> conversions = {
    ("ft", "in") => 12.0,
    ("in", "ft") => 1/12.0,
    ("ft", "m") => 0.3048,
    ("m", "ft") => 1/0.3048,
    // ... grows exponentially!
};

// Hub-and-spoke approach (GOOD):
UnitDefinition.BaseUnit("m", "meters", UnitFamilyName.Length),
UnitDefinition.LinearUnit("ft", "feet", UnitFamilyName.Length, 0.3048),  // 1 ft = 0.3048 m
UnitDefinition.LinearUnit("in", "inches", UnitFamilyName.Length, 0.0254) // 1 in = 0.0254 m

// Any conversion: Value → Base Unit → Target Unit (max 2 operations)
```

### Performance Through Lazy Caching
All unit systems implement identical caching patterns for O(1) lookups:

```csharp
private Dictionary<string, UnitFamilyName>? _cachedSymbolToFamily = null;

public Dictionary<string, UnitFamilyName> GetSymbolToFamilyMap()
{
    if (_cachedSymbolToFamily == null)  // Lazy evaluation
    {
        _cachedSymbolToFamily = new Dictionary<string, UnitFamilyName>();
        foreach (var unit in UnitDefinitions)
        {
            _cachedSymbolToFamily[unit.Symbol] = unit.Family;
        }
    }
    return _cachedSymbolToFamily;  // O(1) lookups after initialization
}
```

## 📋 Interface Contract

All unit systems implement `IUnitSystemSpecification`:

```csharp
public interface IUnitSystemSpecification
{
    string SystemName { get; }
    string SystemDescription { get; }
    IReadOnlyList<UnitDefinition> UnitDefinitions { get; }

    // Performance-optimized cached methods
    List<string> GetAllUnitSymbols();
    List<UnitDefinition> GetAllBaseUnits();
    Dictionary<UnitFamilyName, UnitDefinition> GetBaseUnitsByFamily();
    Dictionary<UnitFamilyName, List<UnitDefinition>> GetAllUnitsByFamily();
    Dictionary<string, UnitFamilyName> GetSymbolToFamilyMap();
}
```

## 🎯 Available Unit Systems

### 1. **MKS (Meter-Kilogram-Second)**
- **Base Units**: meters, kilograms, seconds, Celsius, radians, newtons
- **Target**: General engineering applications
- **Key Features**: Standard metric base units

### 2. **SI (International System)**
- **Base Units**: meters, kilograms, seconds, Kelvin, amperes, moles, candela
- **Target**: Scientific applications with complete SI coverage
- **Key Features**: All 7 SI base units, electrical, radioactivity, dose units

### 3. **CGS (Centimeter-Gram-Second)**
- **Base Units**: centimeters, grams, seconds, Celsius, degrees, dynes
- **Target**: Laboratory and scientific precision work
- **Key Features**: Uses dynes (force), barye (pressure), ergs (energy)

### 4. **FPS (Foot-Pound-Second)**
- **Base Units**: feet, pounds, seconds, Fahrenheit, degrees
- **Target**: Structural and civil engineering
- **Key Features**: Uses psf (pressure), optimized for construction

### 5. **IPS (Inch-Pound-Second)**
- **Base Units**: inches, pounds, seconds, Fahrenheit, degrees
- **Target**: Imperial engineering applications
- **Key Features**: Inch-based measurements for precision machining

### 6. **mmNs (Millimeter-Newton-Second)**
- **Base Units**: millimeters, newtons, seconds, Celsius, degrees
- **Target**: Precision mechanical engineering and CAD
- **Key Features**: kPa pressure, mJ energy scales for mechanical design

## 🚀 Usage Patterns

### Basic Unit Conversion
```csharp
public class UniversalConverter
{
    public double Convert(double value, string fromUnit, string toUnit, 
                         IUnitSystemSpecification system)
    {
        var fromDef = system.FindUnit(fromUnit);
        var toDef = system.FindUnit(toUnit);
        
        // Hub-and-spoke: from → base → to
        var baseValue = fromDef.ConvertToBase(value);
        return toDef.ConvertFromBase(baseValue);
    }
}

// Usage:
var siSystem = new SIUnitSystemSpecification();
var result = converter.Convert(100, "cm", "m", siSystem); // Result: 1.0
```

### Equation Processing Engine
```csharp
public class EquationParser 
{
    public ParsedEquation Parse(string equation, IUnitSystemSpecification system)
    {
        // "Force = 100 lbf * 2.5 ft / 3.2 s²"
        var units = ExtractUnits(equation); // ["lbf", "ft", "s"]
        
        // Use cached lookup - O(1) performance!
        var families = system.GetSymbolToFamilyMap();
        
        foreach(var unit in units) 
        {
            var family = families[unit];           // O(1) lookup
            var baseUnit = system.GetBaseUnitsByFamily()[family]; // O(1) lookup
            // Convert: unit → baseUnit → target automatically
        }
        
        return new ParsedEquation(/* ... */);
    }
}
```

### Multi-Domain Engineering Calculator
```csharp
public class EngineeringCalculator
{
    private IUnitSystemSpecification currentSystem;
    
    public void SwitchToSystem(string domain)
    {
        currentSystem = domain switch
        {
            "structural" => new FPSUnitSystemSpecification(),    // feet, pounds, psi
            "precision"  => new mmNsUnitSystemSpecification(),   // mm, newtons, kPa  
            "laboratory" => new CGSUnitSystemSpecification(),    // cm, grams, dynes
            "scientific" => new SIUnitSystemSpecification(),     // complete SI
            "imperial"   => new IPSUnitSystemSpecification(),    // inches, pounds
            _ => new MKSUnitSystemSpecification()                // default metric
        };
    }
    
    public StressResult CalculateStress(Force force, Area area)
    {
        // Works with ANY unit system automatically!
        var baseUnits = currentSystem.GetBaseUnitsByFamily();
        var forceBase = baseUnits[UnitFamilyName.Force];
        var areaBase = baseUnits[UnitFamilyName.Area];
        var pressureBase = baseUnits[UnitFamilyName.Pressure];
        
        // Convert inputs to base units
        var forceInBase = ConvertToBase(force.Value, force.Unit);
        var areaInBase = ConvertToBase(area.Value, area.Unit);
        
        // Calculate stress in base pressure units
        var stressInBase = forceInBase / areaInBase;
        
        return new StressResult(stressInBase, pressureBase.Symbol);
    }
}
```

### Type-Safe Unit Family Validation
```csharp
// UnitFamilyName enum prevents runtime errors
public enum UnitFamilyName 
{
    Length, Mass, Force, Temperature, Time, Area, Volume, Speed, 
    Pressure, Energy, Power, Frequency, Voltage, Current, 
    AmountOfSubstance, LuminousIntensity, // ... and more
}

// Compiler catches mistakes at build time:
UnitDefinition.LinearUnit("kg", "kilograms", UnitFamilyName.Mass, 1000.0);     // ✅ Correct
UnitDefinition.LinearUnit("kg", "kilograms", UnitFamilyName.Length, 1000.0);   // ❌ Compile error!
```

## 🔧 Integration Guidelines

### Adding New Unit Systems
1. **Implement IUnitSystemSpecification**
2. **Define all 5 cached methods** (copy pattern from existing systems)
3. **Choose appropriate base units** for your engineering domain
4. **Use UnitFamilyName enum** for type safety
5. **Follow naming conventions**: `[Domain]UnitSystemSpecification`

```csharp
public class CustomUnitSystemSpecification : IUnitSystemSpecification
{
    public string SystemName => "Custom";
    public string SystemDescription => "Custom unit system for [domain]";
    
    // Implement all 5 cached methods (copy from existing systems)
    private List<string>? _cachedUnitSymbols = null;
    // ... etc
    
    public IReadOnlyList<UnitDefinition> UnitDefinitions { get; } = new List<UnitDefinition>
    {
        // Define your base units and conversions
        UnitDefinition.BaseUnit("baseUnit", "description", UnitFamilyName.Length),
        UnitDefinition.LinearUnit("derived", "description", UnitFamilyName.Length, 0.001),
        // ...
    };
}
```

### Extending Unit Families
Add new unit families to the `UnitFamilyName` enum:

```csharp
public enum UnitFamilyName 
{
    // Existing families...
    Length, Mass, Force, Temperature,
    
    // New families for your domain
    Torque, MomentOfInertia, Density, Viscosity
}
```

### Creating Conversion Utilities
```csharp
public static class UnitConversionExtensions
{
    public static double ConvertBetweenSystems(this double value, 
        string unit, IUnitSystemSpecification fromSystem, IUnitSystemSpecification toSystem)
    {
        // Find unit definitions in both systems
        var fromUnit = fromSystem.UnitDefinitions.First(u => u.Symbol == unit);
        var toUnit = toSystem.UnitDefinitions.First(u => u.Symbol == unit && u.Family == fromUnit.Family);
        
        // Convert: value → fromBase → universal → toBase → target
        var fromBase = fromUnit.ConvertToBase(value);
        // Apply system-to-system conversion factors if needed
        var toBase = ConvertBetweenBaseSystems(fromBase, fromUnit.Family, fromSystem, toSystem);
        return toUnit.ConvertFromBase(toBase);
    }
}
```

## 📊 Performance Characteristics

### Scalability Comparison
| Approach | Adding 1 Unit | Adding N Units | Lookup Performance |
|----------|---------------|----------------|-------------------|
| Traditional Matrix | O(N) conversions | O(N²) conversions | O(1) |
| Hub-and-Spoke | O(1) definition | O(N) definitions | O(1) with caching |

### Memory Usage
- **Traditional**: N² conversion factors stored
- **Hub-and-Spoke**: 2N conversion functions (to/from base)
- **Caching**: 5 lookup dictionaries per system (lazy initialized)

## 🎯 Best Practices

### 1. **Always Use Interface**
```csharp
// Good - flexible and testable
public void ProcessUnits(IUnitSystemSpecification system) { }

// Bad - tightly coupled
public void ProcessUnits(SIUnitSystemSpecification system) { }
```

### 2. **Leverage Cached Methods**
```csharp
// Good - O(1) after first call
var symbolMap = system.GetSymbolToFamilyMap();
foreach(var symbol in userInput)
{
    var family = symbolMap[symbol]; // O(1)
}

// Bad - O(N) every time
foreach(var symbol in userInput)
{
    var family = system.UnitDefinitions.First(u => u.Symbol == symbol).Family; // O(N)
}
```

### 3. **Choose Appropriate Base Units**
- **Scientific**: Use SI base units (meters, kilograms, seconds, Kelvin)
- **Engineering**: Use practical base units (appropriate scale for calculations)
- **Domain-Specific**: Use base units that minimize conversion factors

### 4. **Handle Temperature Conversions Carefully**
```csharp
// Temperature requires special handling (not linear)
UnitDefinition.DerivedUnit("F", "Fahrenheit", UnitFamilyName.Temperature,
    f => (f - 32.0) * 5.0/9.0,       // F to base (C): (F-32)*5/9
    c => c * 9.0/5.0 + 32.0),        // base (C) to F: C*9/5 + 32
```

## 🔄 Integration with Existing Modules

### Parser Integration
```csharp
// Equation parser can now handle units automatically
public class FormulaParser
{
    private IUnitSystemSpecification unitSystem;
    
    public ParsedFormula Parse(string formula)
    {
        var tokens = Tokenize(formula);
        var unitTokens = tokens.Where(IsUnitSymbol);
        
        // Validate all units exist in current system
        var symbolMap = unitSystem.GetSymbolToFamilyMap();
        foreach(var unit in unitTokens)
        {
            if (!symbolMap.ContainsKey(unit.Value))
                throw new UnknownUnitException(unit.Value);
        }
        
        return new ParsedFormula(tokens, unitSystem);
    }
}
```

### Calculation Engine Integration
```csharp
// Calculation engine with automatic unit handling
public class CalculationEngine
{
    public CalculationResult Evaluate(ParsedFormula formula, 
                                    Dictionary<string, ValueWithUnit> variables)
    {
        // All calculations automatically handle unit conversions
        foreach(var variable in variables)
        {
            // Convert to base units for calculation
            var baseValue = ConvertToBaseUnit(variable.Value, variable.Unit);
            // Perform calculation in consistent base units
            // Convert result back to appropriate target units
        }
    }
}
```

### Validation and Error Handling
```csharp
public class UnitValidator
{
    public ValidationResult ValidateFormula(string formula, IUnitSystemSpecification system)
    {
        var errors = new List<string>();
        var units = ExtractUnits(formula);
        var symbolMap = system.GetSymbolToFamilyMap();
        
        foreach(var unit in units)
        {
            if (!symbolMap.ContainsKey(unit))
                errors.Add($"Unknown unit: {unit}");
        }
        
        return new ValidationResult(errors.Count == 0, errors);
    }
}
```

## 🚀 Future Enhancements

### 1. **Dynamic Unit System Loading**
```csharp
public class UnitSystemFactory
{
    public IUnitSystemSpecification LoadFromConfiguration(string configPath)
    {
        // Load unit definitions from JSON/XML/database
        // Generate unit system at runtime
    }
}
```

### 2. **Unit Dimension Analysis**
```csharp
public class DimensionalAnalyzer
{
    public bool ValidateDimensionalConsistency(ParsedFormula formula)
    {
        // Verify that equation dimensions balance
        // E.g., Force = Mass × Acceleration
        //       [MLT⁻²] = [M] × [LT⁻²] ✓
    }
}
```

### 3. **Cross-System Conversion Tables**
```csharp
public class CrossSystemConverter
{
    public double Convert(double value, string unit, 
                         IUnitSystemSpecification fromSystem,
                         IUnitSystemSpecification toSystem)
    {
        // Handle conversions between different unit systems
        // E.g., SI meters to FPS feet
    }
}
```

## 📝 Summary

This unit system architecture provides:

- **Scalability**: O(N) instead of O(N²) complexity
- **Performance**: O(1) lookups with lazy caching
- **Type Safety**: Enum-based unit family validation
- **Flexibility**: Drop-in unit system replacement
- **Maintainability**: Single source of truth per unit
- **Extensibility**: Easy addition of new units and systems

The hub-and-spoke model with interface-based design creates a robust foundation for building sophisticated engineering calculation tools while maintaining simplicity and performance.