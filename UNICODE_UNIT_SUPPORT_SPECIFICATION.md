# Unit System Extension Guide: Unicode Support & Unit Family Template

**Date:** January 3, 2026  
**Version:** 1.0  
**Status:** Living Template & Specification  
**Purpose:** Define repeatable process for adding unit families to FoundryRulesAndUnits

---

## Executive Summary

This specification establishes a **repeatable pattern** for extending the FoundryRulesAndUnits system with new unit families and Unicode character support. While this document introduces specific new families (Inertia, Inductance, Torque, etc.), the primary goal is to define a **template process** that can be applied whenever new engineering domains require unit support.

**Key Insight**: The `units(value, 'symbol')` function provides **direct access** to any unit family once defined in the system specifications. This means adding new unit families is primarily a **data definition task**, not a parser rewrite.

**Dual Audience**: Enable both AI agents (comfortable with Unicode: `Ω`, `μ`, `⋅`) and human users (preferring ASCII: `ohm`, `u`, `*`) to work seamlessly.

---

## Architecture Foundation: The `units()` Function

### Direct Unit Family Access

The `units(value, 'symbol')` function is the **gateway** to the entire unit system. Once a unit family is defined in the system specifications, it becomes immediately accessible through this function:

```javascript
// As soon as Inertia family is defined, this works:
units(0.05, 'kg⋅m²')    // Returns Inertia measured value
units(0.05, 'kg*m2')    // ASCII equivalent - same result

// No parser changes needed - just add the unit definitions
units(10, 'μH')         // Inductance
units(2.5, 'N⋅m')       // Torque
```

### What This Means for Extensions

**Adding a new unit family requires:**
1. ✅ Add enum value to `UnitFamilyName`
2. ✅ Add unit definitions to system specifications (MKS, CGS, IPS, FPS)
3. ✅ Create `MeasuredValue` subclass (e.g., `Inertia`, `Torque`)
4. ✅ Update `KnParameter` constructor switch statement

**What you DON'T need to do:**
- ❌ Modify the parser tokenizer
- ❌ Add new parsing rules
- ❌ Change the `units()` function signature
- ❌ Update formula evaluation logic

The parser already knows how to look up any unit symbol via the `UnitSystem` dictionary. Adding new units is **data-driven**, not **code-driven**.

### Implication for This Specification

This document should be read as a **template** and **reference guide** for all future unit family additions. The specific families introduced here (Inertia, Inductance, etc.) serve as **examples** of the pattern, not one-time special cases.

---

## Problem Statement

### Current State
1. **Unicode in Existing System**: The system already supports some Unicode symbols (`Ω`, `μ`, `°`, `Å`) with ASCII aliases
2. **Inconsistent Coverage**: Not all Unicode engineering symbols have ASCII alternatives
3. **Missing Unit Families**: Important engineering unit families are absent (e.g., Inertia, Inductance, Torque)
4. **Parser Limitations**: No formal specification for how the parser should handle Unicode normalization

### User Scenarios

**Scenario 1: LLM Agent Creating Formula**
```javascript
// LLM naturally generates Unicode
set_parameter("R1", "Resistance", "units(1000, 'Ω')")
set_parameter("C1", "Capacitance", "units(10, 'μF')")
set_parameter("I", "Inertia", "units(0.05, 'kg⋅m²')")
```

**Scenario 2: Human User Typing Formula**
```javascript
// Human types ASCII aliases (easier on keyboard)
set_parameter("R1", "Resistance", "units(1000, 'ohm')")
set_parameter("C1", "Capacitance", "units(10, 'uF')")
set_parameter("I", "Inertia", "units(0.05, 'kg*m2')")
```

**Scenario 3: Display & Documentation**
```
Display should always use Unicode for professional appearance:
- Resistance: 1.0 kΩ (not "1.0 kohm")
- Capacitance: 10 μF (not "10 uF")
- Inertia: 0.05 kg⋅m² (not "0.05 kg*m2")
```

---

## Design Principles

### 1. Dual Representation Strategy
Every Unicode unit symbol MUST have an ASCII alias for human typing convenience.

**Mapping Convention:**
- Unicode `Ω` ↔ ASCII `ohm`, `Ohm`
- Unicode `μ` ↔ ASCII `u` (as prefix, e.g., `uF`, `um`, `uA`)
- Unicode `°` ↔ ASCII `deg` (for degrees)
- Unicode `⋅` ↔ ASCII `*` (multiplication operator for compound units)
- Unicode `²` ↔ ASCII `2` (superscript for square units)
- Unicode `³` ↔ ASCII `3` (superscript for cubic units)

### 2. Parser Normalization
The parser should accept BOTH Unicode and ASCII representations, normalizing internally to a canonical form.

**Normalization Flow:**
```
Input: "kΩ", "kohm", "kOhm", "kilohm"
  ↓
Parser Tokenization & Normalization
  ↓
Internal Canonical Form: "kΩ"
  ↓
Display Formatter
  ↓
Output: "kΩ" (Unicode for UI), "kohm" (ASCII for code gen)
```

### 3. Display Priority
Always use Unicode for display and presentation, ASCII for code generation and human input.

### 4. Case Insensitivity
ASCII aliases should be case-insensitive where appropriate:
- `ohm`, `Ohm`, `OHM` all map to `Ω`
- SI prefixes maintain case sensitivity: `m` (milli) ≠ `M` (mega)

---

## Repeatable Extension Process

### How to Add Any New Unit Family

This process can be followed for **any engineering domain** requiring new units:

#### Step 1: Identify the Unit Family Need
**Questions to answer:**
- What physical quantity does this measure? (e.g., rotational inertia, magnetic flux, acoustic pressure)
- What engineering domains need this? (mechanical, electrical, acoustics, etc.)
- What are the common units used in practice? (metric, imperial, industry-specific)

#### Step 2: Define Base Units for Each System
**Each unit system needs a base unit:**
- **MKS**: Typically SI-based (meters, kilograms, seconds)
- **CGS**: Centimeter-gram-second based
- **IPS**: Inch-pound-second based
- **FPS**: Foot-pound-second based

**Example Pattern:**
| Unit Family | MKS Base | CGS Base | IPS Base | FPS Base |
|-------------|----------|----------|----------|----------|
| Inertia | kg⋅m² | g⋅cm² | lb⋅in² | slug⋅ft² |
| Torque | N⋅m | dyne⋅cm | lb⋅in | lb⋅ft |
| Inductance | H | H | H | H |

#### Step 3: Map Unicode and ASCII Representations
**For every unit symbol requiring special characters:**
- Identify Unicode characters needed (Ω, μ, ⋅, ², ³, etc.)
- Define ASCII aliases for human typing
- Ensure consistency with existing conventions

#### Step 4: Add to System
**Four mechanical steps:**

```csharp
// 1. Add enum value (UnitFamilyName.cs)
public enum UnitFamilyName
{
    // ... existing ...
    YourNewFamily,  // Add here
}

// 2. Add unit definitions (MKSUnitSystemSpecification.cs and others)
UnitDefinition.BaseUnit("symbol", "description", UnitFamilyName.YourNewFamily),
UnitDefinition.LinearUnit("prefix+symbol", "description", UnitFamilyName.YourNewFamily, conversionFactor),
// ... add all units and their ASCII aliases ...

// 3. Create MeasuredValue subclass
public class YourNewFamily : MeasuredValue
{
    [UnitFamily(UnitFamilyName.YourNewFamily)]
    public YourNewFamily(double value = 0, string? units = null)
        : base(value, units ?? "default_unit", UnitFamilyName.YourNewFamily) { }
}

// 4. Update KnParameter constructor (KnParameter.cs)
MeasuredValue result = unitFamily switch
{
    // ... existing cases ...
    UnitFamilyName.YourNewFamily => new YourNewFamily(value, units),
    // ...
};
```

#### Step 5: Test the Integration
**Immediately usable via `units()` function:**

```javascript
// No parser changes needed - these work immediately:
set_parameter("param1", "Description", "units(100, 'your_unit')")
set_parameter("param2", "Description", "units(100, 'ascii_alias')")
```

### Why This Works

The architecture separates **concerns**:

1. **Unit Definitions** (data layer): System specifications define what units exist
2. **Parser** (access layer): Looks up units from definitions via dictionary
3. **`units()` Function** (interface layer): Provides access to any defined unit
4. **Type System** (safety layer): `MeasuredValue` subclasses enforce correct family usage

Adding new families only touches #1 and #4 (data and types), not #2 or #3 (parser and interface).

---

## Example: Current Extension (Inertia, Inductance, Torque)

The following sections demonstrate the repeatable process applied to five new unit families. **These serve as templates for future additions.**

### Proposed Unit Families

### 1. Inertia (Moment of Inertia) ⭐ NEW
**Physics Context:** Rotational mass property, resistance to angular acceleration

**Base Unit:** `kg⋅m²` (kilogram-square meters)

**Unit Definitions (Single Definition Per Unit):**
```csharp
// Inertia units - ONE definition per unit with optional Unicode
// Base unit
UnitDefinition.BaseUnit("kg*m2", "kilogram-square meters", UnitFamilyName.Inertia, "kg⋅m²"),

// Metric variations
UnitDefinition.LinearUnit("g*m2", "gram-square meters", UnitFamilyName.Inertia, 0.001, "g⋅m²"),
UnitDefinition.LinearUnit("kg*cm2", "kilogram-square centimeters", UnitFamilyName.Inertia, 0.0001, "kg⋅cm²"),
UnitDefinition.LinearUnit("g*cm2", "gram-square centimeters", UnitFamilyName.Inertia, 0.0000001, "g⋅cm²"),

// Imperial units
UnitDefinition.LinearUnit("slug*ft2", "slug-square feet", UnitFamilyName.Inertia, 1.35582, "slug⋅ft²"),
UnitDefinition.LinearUnit("lb*ft2", "pound-square feet", UnitFamilyName.Inertia, 0.0421401, "lb⋅ft²"),
UnitDefinition.LinearUnit("lb*in2", "pound-square inches", UnitFamilyName.Inertia, 0.000292640, "lb⋅in²"),
UnitDefinition.LinearUnit("oz*in2", "ounce-square inches", UnitFamilyName.Inertia, 0.0000182900, "oz⋅in²"),
```

**Usage Examples:**
```csharp
// Flywheel design
inertia = new Inertia(0.05, "kg⋅m²");  // Unicode
inertia = new Inertia(0.05, "kg*m2");  // ASCII equivalent

// Motor specifications
inertia = new Inertia(180, "oz⋅in²");  // Small motor
inertia = new Inertia(180, "oz*in2");  // ASCII equivalent
```

### 2. Enhanced Resistance Family ⭐ ENHANCED
**Current Issue:** Existing resistance uses Unicode `Ω` but lacks comprehensive ASCII aliases

**Base Unit:** `ohm` (ASCII canonical), `Ω` (Unicode display)

**Enhanced Unit Definitions (Single Definition Per Unit):**
```csharp
// Resistance units - ONE definition per unit with optional Unicode
// Base unit
UnitDefinition.BaseUnit("ohm", "ohms", UnitFamilyName.Resistance, "Ω"),

// Microohms (for low-resistance precision applications)
UnitDefinition.LinearUnit("uohm", "microohms", UnitFamilyName.Resistance, 0.000001, "μΩ"),

// Milliohms
UnitDefinition.LinearUnit("mohm", "milliohms", UnitFamilyName.Resistance, 0.001, "mΩ"),

// Kilohms
UnitDefinition.LinearUnit("kohm", "kilohms", UnitFamilyName.Resistance, 1000.0, "kΩ"),

// Megohms
UnitDefinition.LinearUnit("Mohm", "megohms", UnitFamilyName.Resistance, 1000000.0, "MΩ"),

// Gigohms (for high-impedance applications)
UnitDefinition.LinearUnit("Gohm", "gigohms", UnitFamilyName.Resistance, 1000000000.0, "GΩ"),
```

**Lookup table automatically maps both forms:**
```
"ohm"  → [UnitDefinition]  // Symbol lookup
"Ω"    → [UnitDefinition]  // UnicodeSymbol lookup (same object)
"kohm" → [UnitDefinition]  // Symbol lookup
"kΩ"   → [UnitDefinition]  // UnicodeSymbol lookup (same object)
```

**Usage Examples:**
```csharp
// Voltage divider circuit
resistance = new Resistance(10000, "Ω");     // Unicode
resistance = new Resistance(10000, "ohm");   // ASCII short
resistance = new Resistance(10, "kΩ");       // Unicode with prefix
resistance = new Resistance(10, "kohm");     // ASCII with prefix

// PCB trace resistance
resistance = new Resistance(50, "mΩ");       // Unicode milliohm
resistance = new Resistance(50, "mohm");     // ASCII milliohm

// High impedance input
resistance = new Resistance(10, "MΩ");       // Unicode megohm
resistance = new Resistance(10, "Mohm");     // ASCII megohm
```

### 3. Inductance ⭐ NEW
**Physics Context:** Property of conductor opposing change in current

**Base Unit:** `H` (henry)

**Unit Definitions (Single Definition Per Unit):**
```csharp
// Inductance units - ONE definition per unit with optional Unicode
UnitDefinition.BaseUnit("H", "henrys", UnitFamilyName.Inductance),

// Standard SI prefixes
UnitDefinition.LinearUnit("mH", "millihenrys", UnitFamilyName.Inductance, 0.001),
UnitDefinition.LinearUnit("uH", "microhenrys", UnitFamilyName.Inductance, 0.000001, "μH"),
UnitDefinition.LinearUnit("nH", "nanohenrys", UnitFamilyName.Inductance, 0.000000001),
UnitDefinition.LinearUnit("pH", "picohenrys", UnitFamilyName.Inductance, 0.000000000001),
UnitDefinition.LinearUnit("kH", "kilohenrys", UnitFamilyName.Inductance, 1000.0), // Rare but valid
```

**Usage Examples:**
```csharp
// RF circuit design
inductance = new Inductance(100, "nH");   // PCB trace
inductance = new Inductance(10, "μH");    // Surface mount inductor
inductance = new Inductance(10, "uH");    // ASCII equivalent

// Power supply design
inductance = new Inductance(470, "μH");   // DC-DC converter
inductance = new Inductance(2.2, "mH");   // Filter choke
```

### 4. Torque ⭐ NEW
**Physics Context:** Rotational force, moment of force

**Base Unit:** `N⋅m` (newton-meter)

**Unit Definitions (Single Definition Per Unit):**
```csharp
// Torque units - ONE definition per unit with optional Unicode
// Base unit
UnitDefinition.BaseUnit("N*m", "newton-meters", UnitFamilyName.Torque, "N⋅m"),

// Metric variations
UnitDefinition.LinearUnit("kN*m", "kilonewton-meters", UnitFamilyName.Torque, 1000.0, "kN⋅m"),
UnitDefinition.LinearUnit("mN*m", "millinewton-meters", UnitFamilyName.Torque, 0.001, "mN⋅m"),

// Imperial units
UnitDefinition.LinearUnit("lb*ft", "pound-feet", UnitFamilyName.Torque, 1.35582, "lb⋅ft"),
UnitDefinition.LinearUnit("lbf*ft", "pound-force-feet", UnitFamilyName.Torque, 1.35582, "lbf⋅ft"),
UnitDefinition.LinearUnit("lb*in", "pound-inches", UnitFamilyName.Torque, 0.112985, "lb⋅in"),
UnitDefinition.LinearUnit("lbf*in", "pound-force-inches", UnitFamilyName.Torque, 0.112985, "lbf⋅in"),
UnitDefinition.LinearUnit("oz*in", "ounce-inches", UnitFamilyName.Torque, 0.00706155, "oz⋅in"),
```

**Usage Examples:**
```csharp
// Motor specifications
torque = new Torque(2.5, "N⋅m");    // Unicode
torque = new Torque(2.5, "N*m");    // ASCII equivalent

// Fastener tightening specifications
torque = new Torque(100, "lb⋅ft");  // Automotive wheel lug nuts
torque = new Torque(25, "lb⋅in");   // Electronic enclosure screws
```

### 5. Angular Velocity ⭐ NEW
**Physics Context:** Rate of rotation

**Base Unit:** `rad/s` (radians per second)

**Unit Definitions:**
```csharp
// Angular velocity units (rad/s as base)
UnitDefinition.BaseUnit("rad/s", "radians per second", UnitFamilyName.AngularVelocity),

// Common rotational speed units
UnitDefinition.LinearUnit("deg/s", "degrees per second", UnitFamilyName.AngularVelocity, Math.PI / 180.0),
UnitDefinition.LinearUnit("rpm", "revolutions per minute", UnitFamilyName.AngularVelocity, 2.0 * Math.PI / 60.0),
UnitDefinition.LinearUnit("rps", "revolutions per second", UnitFamilyName.AngularVelocity, 2.0 * Math.PI),
UnitDefinition.LinearUnit("Hz", "hertz", UnitFamilyName.AngularVelocity, 2.0 * Math.PI), // 1 Hz = 2π rad/s
```

**Usage Examples:**
```csharp
// Motor speed specification
angularVel = new AngularVelocity(3000, "rpm");  // DC motor
angularVel = new AngularVelocity(50, "Hz");     // AC synchronous motor at 60Hz

// Robotics
angularVel = new AngularVelocity(1.5, "rad/s"); // Joint velocity limit
```

### 6. Angular Acceleration ⭐ NEW
**Physics Context:** Rate of change of angular velocity

**Base Unit:** `rad/s²` (radians per second squared)

**Unit Definitions (Single Definition Per Unit):**
```csharp
// Angular acceleration units - ONE definition per unit with optional Unicode
UnitDefinition.BaseUnit("rad/s2", "radians per second squared", UnitFamilyName.AngularAcceleration, "rad/s²"),

// Alternative representations
UnitDefinition.LinearUnit("deg/s2", "degrees per second squared", UnitFamilyName.AngularAcceleration, Math.PI / 180.0, "deg/s²"),
UnitDefinition.LinearUnit("rpm/s", "revolutions per minute per second", UnitFamilyName.AngularAcceleration, 2.0 * Math.PI / 60.0),
```

**Usage Examples:**
```csharp
// Servo motor acceleration
angularAccel = new AngularAcceleration(10, "rad/s²");   // Unicode
angularAccel = new AngularAcceleration(10, "rad/s2");   // ASCII

// Robotics motion planning
angularAccel = new AngularAcceleration(500, "deg/s²");  // Joint acceleration limit
```

---

## Unicode Character Reference

### Greek Letters (Engineering Context)
| Unicode | Hex Code | ASCII Alias(es) | Common Usage |
|---------|----------|-----------------|--------------|
| `Ω` (Omega) | U+03A9 | `ohm`, `Ohm` | Resistance (electrical) |
| `μ` (mu) | U+03BC | `u` (as prefix) | Micro prefix (10⁻⁶) |
| `π` (pi) | U+03C0 | `pi`, `PI` | Mathematical constant |
| `Δ` (Delta) | U+0394 | `delta`, `Delta` | Change/difference |
| `α` (alpha) | U+03B1 | `alpha` | Angle, coefficient |
| `β` (beta) | U+03B2 | `beta` | Angle, coefficient |
| `γ` (gamma) | U+03B3 | `gamma` | Angle, specific heat ratio |
| `θ` (theta) | U+03B8 | `theta` | Angle |
| `ω` (omega) | U+03C9 | `omega` | Angular velocity |
| `τ` (tau) | U+03C4 | `tau` | Torque, time constant |
| `ρ` (rho) | U+03C1 | `rho` | Density |
| `σ` (sigma) | U+03C3 | `sigma` | Stress, standard deviation |

### Mathematical Symbols
| Unicode | Hex Code | ASCII Alias(es) | Common Usage |
|---------|----------|-----------------|--------------|
| `°` (degree) | U+00B0 | `deg` | Angle degrees |
| `⋅` (dot operator) | U+22C5 | `*`, `.` | Multiplication (compound units) |
| `×` (multiplication) | U+00D7 | `*`, `x` | Multiplication |
| `²` (superscript 2) | U+00B2 | `2` | Square (area, acceleration) |
| `³` (superscript 3) | U+00B3 | `3` | Cubic (volume) |
| `½` (vulgar fraction) | U+00BD | `1/2` | One-half |
| `¼` (vulgar fraction) | U+00BC | `1/4` | One-quarter |
| `∞` (infinity) | U+221E | `inf`, `infinity` | Infinite value |

### Special Unit Symbols
| Unicode | Hex Code | ASCII Alias(es) | Common Usage |
|---------|----------|-----------------|--------------|
| `Å` (angstrom) | U+00C5 | `angstrom`, `A` | Length (10⁻¹⁰ m) |
| `℃` (Celsius) | U+2103 | `C`, `degC` | Temperature |
| `℉` (Fahrenheit) | U+2109 | `F`, `degF` | Temperature |

---

## Parser Implementation Requirements

### 1. Token Recognition
The parser must recognize both Unicode and ASCII forms of unit symbols.

**Example Token Patterns:**
```regex
// Resistance patterns
Ω|ohm|Ohm|OHM

// Micro prefix patterns (before unit)
μ|u|micro

// Compound unit operators
⋅|\*|\.|×|x

// Superscripts for powers
²|2|³|3
```

### 2. Normalization Rules

**Rule 1: Case-Insensitive ASCII Matching (where appropriate)**
```
Input: "OHM", "Ohm", "ohm", "oHM"
Normalized: "Ω"

Input: "KOHM", "kohm", "kOhm", "Kohm"
Normalized: "kΩ"
```

**Rule 2: Prefix Preservation**
SI prefix case MUST be preserved for correct interpretation:
```
"m" = milli (10⁻³)
"M" = mega (10⁶)
"k" = kilo (10³)
"K" = kilo (10³) [both accepted]
"u" = micro (10⁻⁶)
"n" = nano (10⁻⁹)
"p" = pico (10⁻¹²)
```

**Rule 3: Compound Unit Operator Normalization**
```
Input: "kg*m2", "kg⋅m²", "kg.m2", "kg×m2"
Normalized: "kg⋅m²" (Unicode for display)
Internal: Dictionary key "kg*m2" (ASCII for lookup)
```

**Rule 4: Superscript Normalization**
```
Input: "m2", "m²", "m^2"
Normalized: "m²" (Unicode for display)
Internal: "m2" (ASCII for lookup)

Input: "m3", "m³", "m^3"
Normalized: "m³" (Unicode for display)
Internal: "m3" (ASCII for lookup)
```

### 3. Lookup Strategy

**Single-Phase Dual-Key Lookup:**

The lookup table contains **two keys per unit** (when Unicode is available) pointing to the **same UnitDefinition**:

```csharp
// Building the lookup table from unit definitions:
foreach (var definition in unitDefinitions)
{
    // Always add ASCII Symbol as key
    lookupTable[definition.Symbol] = definition;
    
    // If Unicode available, add as second key to SAME definition
    if (!string.IsNullOrEmpty(definition.UnicodeSymbol))
    {
        lookupTable[definition.UnicodeSymbol] = definition;  // Same object!
    }
}

// Result: Both keys find the same definition
// "ohm"  → [UnitDefinition instance]
// "Ω"    → [UnitDefinition instance] (same object)
```

**Single Lookup (O(1)):**
```csharp
// Direct lookup - works for both ASCII and Unicode
if (unitLookupTable.TryGetValue(inputToken, out var definition))
    return definition;  // Found!
```

**No normalization needed** - both forms are directly in the table.

**Normalization Function Pseudocode:**
```csharp
string NormalizeUnitToken(string input)
{
    // Step 1: Replace Unicode with ASCII for internal lookup
    string normalized = input;
    normalized = normalized.Replace("Ω", "ohm");
    normalized = normalized.Replace("μ", "u");
    normalized = normalized.Replace("°", "deg");
    normalized = normalized.Replace("⋅", "*");
    normalized = normalized.Replace("×", "*");
    normalized = normalized.Replace("²", "2");
    normalized = normalized.Replace("³", "3");
    normalized = normalized.Replace("Å", "angstrom");
    
    // Step 2: Apply case normalization rules
    // Keep SI prefixes case-sensitive (m vs M, k vs K)
    // Make unit base case-insensitive
    
    // Example: "KOHM" -> "kOHM" (preserve k) -> "kohm" (normalize base)
    
    return normalized;
}
```

### 4. Display Formatting

**Unicode preferred for display (when available):**
```csharp
string FormatUnitForDisplay(UnitDefinition unit)
{
    // DisplaySymbol returns UnicodeSymbol ?? Symbol
    return unit.DisplaySymbol; // Returns "kΩ" if Unicode available, "kohm" if not
}
```

**ASCII always for code generation:**
```csharp
string FormatUnitForCode(UnitDefinition unit)
{
    // CodeSymbol always returns ASCII Symbol
    return unit.CodeSymbol; // Returns "kohm" always
}
```

**Built-in properties handle the logic:**
```csharp
public string DisplaySymbol => UnicodeSymbol ?? Symbol;  // Unicode first, fallback to ASCII
public string CodeSymbol => Symbol;                       // Always ASCII
```

### 5. Error Messages & Suggestions

When a unit is not recognized, provide helpful suggestions:

```csharp
// User enters: "kohms" (incorrect plural)
Parser Error:
  "Unit 'kohms' not recognized. Did you mean 'kohm' or 'kΩ'?"

// User enters: "kω" (lowercase omega)
Parser Error:
  "Unit 'kω' not recognized. Did you mean 'kΩ' or 'kohm'?"

// User enters: "kg.m2" (wrong operator for compound unit)
Parser Error:
  "Unit 'kg.m2' not recognized. Did you mean 'kg*m2' or 'kg⋅m²'?"
```

---

## Implementation Roadmap

**Note**: This section describes the mechanical steps for the current extension (5 new families). Future extensions follow the same pattern with different unit families.

### Phase 1: Extend UnitFamilyName Enum (Repeatable)
```csharp
public enum UnitFamilyName
{
    // ... existing families ...
    
    // NEW families (add these)
    Inertia,              // kg⋅m², slug⋅ft²
    Inductance,           // H, mH, μH
    Torque,               // N⋅m, lb⋅ft
    AngularVelocity,      // rad/s, rpm
    AngularAcceleration,  // rad/s², deg/s²
}
```

### Phase 2: Add Unit Definitions to Each System Specification (Repeatable)

**Core Principle**: Every unit system (MKS, CGS, IPS, FPS) must include all unit families with system-appropriate base units.

**This is the primary work**: Defining unit symbols, conversion factors, and ASCII aliases. The parser automatically picks these up via dictionary lookup.

**Example for MKS:**
- Inertia base: `kg⋅m²`
- Torque base: `N⋅m`

**Example for IPS:**
- Inertia base: `lb⋅in²`
- Torque base: `lb⋅in`

### Phase 3: Dual Representation in UnitDefinition ✅ COMPLETED

**Updated `UnitDefinition` record:**

```csharp
public record UnitDefinition(
    string Symbol,                        // ASCII canonical: "kohm"
    string Name,                          // Long form: "kilohms"
    UnitFamilyName Family,
    bool IsBaseUnit,
    Func<double, double>? ToBaseUnit,
    Func<double, double>? FromBaseUnit,
    string? UnicodeSymbol = null          // Optional Unicode: "kΩ"
)
{
    public string DisplaySymbol => UnicodeSymbol ?? Symbol;  // For UI display
    public string CodeSymbol => Symbol;                       // For code generation
}
```

**Factory methods support Unicode parameter:**
```csharp
UnitDefinition.BaseUnit("ohm", "ohms", UnitFamilyName.Resistance, "Ω")
UnitDefinition.LinearUnit("kohm", "kilohms", UnitFamilyName.Resistance, 1000.0, "kΩ")
```

### Phase 4: Lookup Table Enhancement
Update lookup table building in `UnitSystem` to:
1. Add both Symbol and UnicodeSymbol as keys to the lookup dictionary
2. Both keys point to the **same** UnitDefinition instance
3. No normalization needed - direct O(1) lookup works for both forms

**Implementation:**
```csharp
private Dictionary<string, UnitDefinition> BuildLookupTable()
{
    var lookup = new Dictionary<string, UnitDefinition>();
    
    foreach (var def in _currentSystem.UnitDefinitions)
    {
        // Always add ASCII Symbol
        lookup[def.Symbol] = def;
        
        // Add Unicode Symbol if present (points to same object)
        if (!string.IsNullOrEmpty(def.UnicodeSymbol))
            lookup[def.UnicodeSymbol] = def;
    }
    
    return lookup;
}
```

### Phase 5: Create MeasuredValue Subclasses (Repeatable)

**Pattern**: Each new unit family gets a strongly-typed `MeasuredValue` subclass. This provides compile-time type safety and IDE autocomplete.
```csharp
public class Inertia : MeasuredValue
{
    [UnitFamily(UnitFamilyName.Inertia)]
    public Inertia(double value = 0, string? units = null)
        : base(value, units ?? "kg⋅m²", UnitFamilyName.Inertia) { }
}

public class Inductance : MeasuredValue
{
    [UnitFamily(UnitFamilyName.Inductance)]
    public Inductance(double value = 0, string? units = null)
        : base(value, units ?? "H", UnitFamilyName.Inductance) { }
}

public class Torque : MeasuredValue
{
    [UnitFamily(UnitFamilyName.Torque)]
    public Torque(double value = 0, string? units = null)
        : base(value, units ?? "N⋅m", UnitFamilyName.Torque) { }
}

public class AngularVelocity : MeasuredValue
{
    [UnitFamily(UnitFamilyName.AngularVelocity)]
    public AngularVelocity(double value = 0, string? units = null)
        : base(value, units ?? "rad/s", UnitFamilyName.AngularVelocity) { }
}

public class AngularAcceleration : MeasuredValue
{
    [UnitFamily(UnitFamilyName.AngularAcceleration)]
    public AngularAcceleration(double value = 0, string? units = null)
        : base(value, units ?? "rad/s²", UnitFamilyName.AngularAcceleration) { }
}
```

### Phase 6: Update KnParameter Constructor (Repeatable)

**One-line addition per family**: Add new families to the switch statement in `KnParameter.cs`.

**This is the only code change outside unit definitions**:

```csharp
MeasuredValue result = unitFamily switch
{
    // ... existing cases ...
    UnitFamilyName.Inertia => new Inertia(value, units),
    UnitFamilyName.Inductance => new Inductance(value, units),
    UnitFamilyName.Torque => new Torque(value, units),
    UnitFamilyName.AngularVelocity => new AngularVelocity(value, units),
    UnitFamilyName.AngularAcceleration => new AngularAcceleration(value, units),
    // ... rest of cases ...
};
```

### Summary: What Changed

**For the current 5 new families:**
- ✅ 5 lines added to `UnitFamilyName` enum
- ✅ ~80 unit definitions added across 4 system specifications (20 per system avg)
- ✅ 5 new `MeasuredValue` subclasses created
- ✅ 5 lines added to `KnParameter` constructor switch
- ✅ 0 changes to parser logic
- ✅ 0 changes to `units()` function
- ✅ 0 changes to formula evaluator

**Total code impact**: ~100 lines of new code, mostly data definitions.

**Immediate result**: All new units are accessible via `units(value, 'symbol')` function in formulas.

---

## Architecture Summary: No Definition Explosion

**Before (Problematic):**
```csharp
// ❌ Multiple definitions = lookup explosion + conversion confusion
UnitDefinition.BaseUnit("Ω", "ohms", UnitFamilyName.Resistance),
UnitDefinition.BaseUnit("ohm", "ohms", UnitFamilyName.Resistance),
UnitDefinition.BaseUnit("Ohm", "ohms", UnitFamilyName.Resistance),
// Result: 3 separate objects, 3 separate conversion functions
```

**After (Clean Architecture):**
```csharp
// ✅ Single definition with dual lookup
UnitDefinition.BaseUnit("ohm", "ohms", UnitFamilyName.Resistance, "Ω"),
// Result: 1 object, 2 lookup keys, 1 conversion function

// Lookup table:
// "ohm" → [definition]
// "Ω"   → [definition]  (same object)
```

**Benefits:**
- ✅ One definition = one source of truth for conversions
- ✅ Both ASCII and Unicode input accepted
- ✅ No lookup table explosion
- ✅ Optional Unicode - only add where it helps
- ✅ Display uses Unicode when available
- ✅ Code generation always uses ASCII

---

## Testing Requirements

### 1. Unit Recognition Tests
Verify parser accepts all forms:
```csharp
[Test]
public void Parser_Should_Accept_Unicode_And_ASCII_Resistance()
{
    Assert.True(unitSystem.IsValidUnit("Ω"));
    Assert.True(unitSystem.IsValidUnit("ohm"));
    Assert.True(unitSystem.IsValidUnit("Ohm"));
    Assert.True(unitSystem.IsValidUnit("kΩ"));
    Assert.True(unitSystem.IsValidUnit("kohm"));
    Assert.True(unitSystem.IsValidUnit("kilohm"));
}

[Test]
public void Parser_Should_Accept_Unicode_And_ASCII_Inertia()
{
    Assert.True(unitSystem.IsValidUnit("kg⋅m²"));
    Assert.True(unitSystem.IsValidUnit("kg*m2"));
    Assert.True(unitSystem.IsValidUnit("slug⋅ft²"));
    Assert.True(unitSystem.IsValidUnit("slug*ft2"));
}
```

### 2. Conversion Tests
Verify conversions work correctly:
```csharp
[Test]
public void Inertia_Should_Convert_Between_Metric_And_Imperial()
{
    var metric = new Inertia(1.0, "kg⋅m²");
    var imperial = metric.ConvertTo("slug⋅ft²");
    Assert.AreEqual(0.7375, imperial.Value, 0.001);
}

[Test]
public void Torque_Should_Convert_Between_Nm_And_LbFt()
{
    var metric = new Torque(100, "N⋅m");
    var imperial = metric.ConvertTo("lb⋅ft");
    Assert.AreEqual(73.756, imperial.Value, 0.001);
}
```

### 3. Display Format Tests
```csharp
[Test]
public void Display_Should_Use_Unicode_By_Default()
{
    var resistance = new Resistance(1000, "ohm");
    Assert.AreEqual("1.0 kΩ", resistance.ToString());
    
    var inertia = new Inertia(0.05, "kg*m2");
    Assert.AreEqual("0.05 kg⋅m²", inertia.ToString());
}

[Test]
public void CodeGen_Should_Use_ASCII_Aliases()
{
    var resistance = new Resistance(1000, "Ω");
    Assert.AreEqual("units(1000, 'ohm')", resistance.ToCodeString());
    
    var inertia = new Inertia(0.05, "kg⋅m²");
    Assert.AreEqual("units(0.05, 'kg*m2')", inertia.ToCodeString());
}
```

### 4. Case Sensitivity Tests
```csharp
[Test]
public void Parser_Should_Handle_Case_Variants_Of_Ohm()
{
    Assert.True(unitSystem.IsValidUnit("OHM"));
    Assert.True(unitSystem.IsValidUnit("Ohm"));
    Assert.True(unitSystem.IsValidUnit("ohm"));
    Assert.True(unitSystem.IsValidUnit("oHm")); // weird but valid
}

[Test]
public void Parser_Should_Preserve_SI_Prefix_Case()
{
    // These should be different
    var milli = unitSystem.GetUnitMetadata("mV"); // millivolts
    var mega = unitSystem.GetUnitMetadata("MV");  // megavolts
    Assert.AreNotEqual(milli.ConversionFactor, mega.ConversionFactor);
}
```

---

## Documentation Requirements

### 1. User-Facing Documentation
- **Unit Reference Guide**: Complete list of supported units with Unicode and ASCII forms
- **Quick Reference Card**: Common unit symbols for engineers
- **Formula Syntax Guide**: How to write formulas with units

### 2. LLM Agent Guidelines
Provide clear guidance in tool descriptions:

```markdown
## Units Syntax for LLM Agents

When generating formulas with units, use the `units(value, 'symbol')` function.

### Unicode Symbols (Preferred for LLMs):
- Resistance: units(1000, 'Ω'), units(10, 'kΩ')
- Capacitance: units(10, 'μF'), units(100, 'nF')
- Inertia: units(0.05, 'kg⋅m²')
- Torque: units(2.5, 'N⋅m')

### ASCII Aliases (Human-Friendly):
- Resistance: units(1000, 'ohm'), units(10, 'kohm')
- Capacitance: units(10, 'uF'), units(100, 'nF')
- Inertia: units(0.05, 'kg*m2')
- Torque: units(2.5, 'N*m')

Both forms are equivalent and produce identical results.
```

### 3. Developer Documentation
- **Parser Architecture**: How normalization and lookup work
- **Adding New Units**: Step-by-step guide for extending the system
- **Unicode Handling**: Best practices for text encoding

---

## Migration Strategy

### Backward Compatibility
All existing unit symbols remain valid. This is purely additive:
- Existing resistance units (`Ω`, `kΩ`, etc.) continue to work
- New ASCII aliases added without breaking changes
- New unit families added to enum (no impact on existing code)

### Deprecation Timeline
N/A - No deprecations required. This is a non-breaking enhancement.

---

## Performance Considerations

### Lookup Performance
- **Direct Unicode lookup**: O(1) dictionary lookup
- **ASCII alias lookup**: O(1) dictionary lookup after normalization
- **Normalization cost**: O(n) where n is length of unit string (typically < 10 characters)

### Memory Footprint
Each unit definition requires:
- Primary symbol string (Unicode)
- ASCII alias string
- Description string
- Conversion factor (double)

**Estimated impact**: ~50 new unit definitions × ~100 bytes = ~5 KB additional memory

### Caching Strategy
The system already caches unit lookups in `UnitSystemSpecificationBase`. No additional caching needed.

---

## Security Considerations

### Unicode Normalization Attacks
Be aware of Unicode look-alike characters (homoglyphs):
- Greek Ω (U+03A9) vs Latin O with stroke (U+00D8)
- Micro μ (U+03BC) vs Latin u (U+0075)

**Mitigation**: Use strict character code point matching, not visual similarity.

### Input Validation
Always validate unit strings before normalization:
```csharp
public bool IsValidUnitString(string input)
{
    // Reject strings with unexpected control characters
    if (input.Any(c => char.IsControl(c)))
        return false;
    
    // Reject excessively long strings (max 20 chars for unit symbol)
    if (input.Length > 20)
        return false;
    
    // Whitelist allowed characters
    return Regex.IsMatch(input, @"^[a-zA-Z0-9⋅×°ΩμÅ²³/*.-]+$");
}
```

---

## Quick Reference: Adding Any Unit Family

**Checklist for future extensions:**

- [ ] **Define the need**: What physical quantity? What domain?
- [ ] **Choose base units**: For MKS, CGS, IPS, FPS systems
- [ ] **List all unit symbols**: Metric, imperial, industry-specific
- [ ] **Map Unicode to ASCII**: For any special characters
- [ ] **Add to `UnitFamilyName` enum**: One line
- [ ] **Add unit definitions**: To all 4 system specifications
- [ ] **Create `MeasuredValue` subclass**: Copy existing pattern
- [ ] **Update `KnParameter` constructor**: Add switch case
- [ ] **Write tests**: Recognition, conversion, display format
- [ ] **Update documentation**: User guide, API reference
- [ ] **Verify via `units()` function**: Test in actual formulas

**Time estimate**: 2-4 hours for a new family with 10-15 unit variations.

**Critical insight**: You're adding **data**, not **logic**. The architecture already supports infinite unit families.

---

## Future Enhancements

### 1. Localization Support
Allow unit symbols to be displayed in different languages:
- English: "ohm", "kilohm"
- French: "ohm", "kilohm"
- German: "Ohm", "Kiloohm"
- Japanese: "オーム" (ōmu)

### 2. LaTeX Output
Generate proper LaTeX notation for scientific documents:
```latex
1.0~k\Omega
0.05~\text{kg}\cdot\text{m}^2
```

### 3. MathML Output
Generate MathML for web-based formula display:
```xml
<mn>1.0</mn><mo>&#x2009;</mo><mi>k</mi><mo>&#x03A9;</mo>
```

### 4. Voice Input Support
Map spoken unit names to symbols:
- "one thousand ohms" → `units(1000, 'Ω')`
- "ten micro farads" → `units(10, 'μF')`

---

## Appendix A: Complete Unicode-ASCII Mapping Table

| Unit Family | Unicode Symbol | ASCII Alias(es) | Notes |
|-------------|---------------|-----------------|-------|
| **Resistance** |
| Microohm | μΩ | uohm, uOhm, microhm | Very low resistance |
| Milliohm | mΩ | mohm, mOhm, milliohm | PCB traces, contacts |
| Ohm | Ω | ohm, Ohm, OHM | Base unit |
| Kilohm | kΩ | kohm, kOhm, kilohm | Typical resistors |
| Megohm | MΩ | Mohm, MOhm, megohm | High impedance |
| Gigohm | GΩ | Gohm, GOhm, gigohm | Very high impedance |
| **Capacitance** |
| Picofarad | pF | pF | Tiny capacitors |
| Nanofarad | nF | nF | Ceramic capacitors |
| Microfarad | μF | uF | Electrolytic capacitors |
| Millifarad | mF | mF | Supercapacitors |
| Farad | F | F | Base unit |
| **Inductance** |
| Picohenry | pH | pH | PCB trace |
| Nanohenry | nH | nH | RF inductors |
| Microhenry | μH | uH | Power inductors |
| Millihenry | mH | mH | Filter chokes |
| Henry | H | H | Base unit |
| **Inertia** |
| gram-cm² | g⋅cm² | g*cm2 | Small rotors |
| kg-cm² | kg⋅cm² | kg*cm2 | Medium rotors |
| kg-m² | kg⋅m² | kg*m2 | Base unit (MKS) |
| oz-in² | oz⋅in² | oz*in2 | Small motors |
| lb-in² | lb⋅in² | lb*in2 | Motors |
| lb-ft² | lb⋅ft² | lb*ft2 | Large rotors |
| slug-ft² | slug⋅ft² | slug*ft2 | Base unit (FPS) |
| **Torque** |
| mN⋅m | mN⋅m | mN*m | Small motors |
| N⋅m | N⋅m | N*m | Base unit (MKS) |
| kN⋅m | kN⋅m | kN*m | Large machinery |
| oz⋅in | oz⋅in | oz*in | Servo motors |
| lb⋅in | lb⋅in | lb*in | Hand tools |
| lb⋅ft | lb⋅ft | lb*ft | Automotive |
| lbf⋅ft | lbf⋅ft | lbf*ft | Explicit force notation |

---

## Appendix B: Compound Unit Syntax Rules

### Multiplication Operators
For compound units (e.g., torque, inertia), multiple operators are accepted:

**Unicode Operators:**
- `⋅` (U+22C5) - DOT OPERATOR - **Preferred for display**
- `×` (U+00D7) - MULTIPLICATION SIGN - Valid alternative

**ASCII Operators:**
- `*` (U+002A) - ASTERISK - **Preferred for code**
- `.` (U+002E) - FULL STOP - Discouraged (ambiguous with decimal point)

### Power Notation
For squared and cubed units:

**Unicode Superscripts:**
- `²` (U+00B2) - **Preferred for display**
- `³` (U+00B3) - **Preferred for display**

**ASCII Power Notation:**
- `2` - **Preferred for code**
- `3` - **Preferred for code**
- `^2` - Alternative (caret notation)
- `^3` - Alternative (caret notation)

**Examples:**
```
Area (Unicode):  m², cm², ft²
Area (ASCII):    m2, cm2, ft2

Volume (Unicode): m³, cm³, ft³
Volume (ASCII):   m3, cm3, ft3

Inertia (Unicode):  kg⋅m², slug⋅ft²
Inertia (ASCII):    kg*m2, slug*ft2

Torque (Unicode):   N⋅m, lb⋅ft
Torque (ASCII):     N*m, lb*ft
```

---

## Appendix C: SI Prefix Reference

### Standard SI Prefixes

| Prefix | Symbol | Factor | Example Units |
|--------|--------|--------|---------------|
| yotta | Y | 10²⁴ | YB (yottabyte) |
| zetta | Z | 10²¹ | ZB (zettabyte) |
| exa | E | 10¹⁸ | EB (exabyte) |
| peta | P | 10¹⁵ | PB (petabyte) |
| tera | T | 10¹² | TB (terabyte), THz |
| giga | G | 10⁹ | GB, GHz, GΩ |
| mega | M | 10⁶ | MB, MHz, MΩ |
| kilo | k (or K) | 10³ | kB, kHz, kΩ, km |
| **(base)** | - | 10⁰ | m, Ω, F, H |
| milli | m | 10⁻³ | mm, ms, mA, mΩ |
| micro | μ (or u) | 10⁻⁶ | μm, μs, μA, μF |
| nano | n | 10⁻⁹ | nm, ns, nF, nH |
| pico | p | 10⁻¹² | pm, pF, pH |
| femto | f | 10⁻¹⁵ | fm, fF |
| atto | a | 10⁻¹⁸ | am |
| zepto | z | 10⁻²¹ | zm |
| yocto | y | 10⁻²⁴ | ym |

**Case Sensitivity Rules:**
- Lowercase: `m` (milli), `μ` or `u` (micro), `n` (nano), `p` (pico)
- Uppercase: `M` (mega), `G` (giga), `T` (tera)
- Either case: `k` or `K` (kilo) - both accepted

---

## Appendix D: Engineering Domain Unit Families Summary

| Domain | Unit Families Needed | Status |
|--------|---------------------|--------|
| **Mechanical** | Inertia, Torque, AngularVelocity, AngularAcceleration | ✅ Specified |
| **Electrical** | Resistance (enhanced), Inductance | ✅ Specified |
| **Thermal** | Temperature, ThermalConductivity | ⚠️ Partial (Temperature exists) |
| **Fluid** | Pressure, FlowRate, Viscosity | ⚠️ Partial (Pressure exists) |
| **Optical** | Illuminance, LuminousFlux | ⚠️ Partial (Exists but review needed) |
| **Acoustic** | SoundPressure, Loudness | ❌ Not specified |
| **Chemical** | Concentration, MolarMass | ❌ Not specified |

---

## Revision History

| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2026-01-03 | AI Architecture Team | Initial specification |

---

## Approval

- [ ] Technical Review (Engineering Lead)
- [ ] UX Review (User Experience Team)
- [ ] Security Review (Security Team)
- [ ] Stakeholder Approval (Product Owner)

---

**END OF SPECIFICATION**
