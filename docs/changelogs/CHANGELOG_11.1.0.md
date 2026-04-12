# FoundryRulesAndUnits 11.1.0 Release Notes

**Release Date**: April 12, 2026  
**Package**: `ApprenticeFoundryRulesAndUnits` version 11.1.0  
**Target Framework**: .NET 10.0  
**Type**: Additive — no breaking changes

---

## Overview

Three additive improvements to the unit system: a new `Acceleration` unit type with cross-family arithmetic, display-unit preservation in `Length` arithmetic, and cost-related unit families in the MKS specification.

---

## Added

### Acceleration Unit Type (`UnitSystem/UnitTypes/Acceleration.cs`)

New `Acceleration` class following the same pattern as all typed unit classes:

- Full arithmetic operators (`+`, `-`, `*`, `/`)
- Comparison operators (`<`, `>`, `==`, `!=`)
- `Assign`, `Copy` helpers
- **Cross-family operator:** `Acceleration × Time → Speed` (v = a × t)
  - Computes in base units (m/s² × s = m/s) and returns a properly typed `Speed`

```csharp
var a = unitSystem.CreateTypedMeasuredValue(UnitFamilyName.Acceleration, 9.81, "m/s²");
var t = unitSystem.CreateTypedMeasuredValue(UnitFamilyName.Time, 3.0, "s");
Speed v = (Acceleration)a * (Time)t;  // 29.43 m/s
```

`UnitFamilyName.Acceleration` added to the enum.

---

### Cost-Related Unit Families (MKS Specification)

Seven new `UnitFamilyName` entries for cost and currency tracking:

| Family | Description |
|---|---|
| `Currency` | Monetary values |
| `CostPerQuantity` | Cost per item/count |
| `CostPerTime` | Cost per unit time |
| `CostPerMass` | Cost per unit mass |
| `CostPerLength` | Cost per unit length |
| `CostPerArea` | Cost per unit area |
| `CostPerVolume` | Cost per unit volume |
| `CostPerEnergy` | Cost per unit energy |

These families are registered in `MKSUnitSystemSpecification`.

---

## Fixed

### Length Arithmetic Preserves Display Unit

`Length` arithmetic operations now carry the display unit of the left operand through to the result. Previously, results defaulted to the internal base unit for display.

```csharp
var a = length.Assign(5.0, "ft");
var b = length.Assign(3.0, "ft");
var c = a + b;   // c.U is now "ft", not "m"
```

---

## No Breaking Changes

All changes are additive. No existing APIs were removed or modified.
