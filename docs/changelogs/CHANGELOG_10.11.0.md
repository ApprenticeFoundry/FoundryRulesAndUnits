# FoundryRulesAndUnits 10.11.0 Release Notes

**Release Date**: January 3, 2026  
**Package**: `ApprenticeFoundryRulesAndUnits` version 10.11.0  
**Target Framework**: .NET 9.0

---

## 🎯 What's New in 10.11.0

### **⚙️ New Mechanical Engineering Unit Families**

Added five new unit families for mechanical engineering applications with full Unicode symbol support:

1. **Torque** - Rotational force measurements
2. **Inertia** - Moment of inertia (rotational mass)
3. **Angular Velocity** - Rotational speed
4. **Angular Acceleration** - Rate of change of angular velocity
5. **Inductance** - Electrical inductance (already existed, enhanced)

### **💱 Unicode Currency Symbols**

Added proper Unicode symbols for all currency units across all unit systems for improved readability in AI agent outputs and user interfaces.

---

## ✨ Key Features

### **Torque Units**
Measure rotational force with proper unit support:

```csharp
var unitSystem = new UnitSystem(UnitSystemType.MKS);

// Create torque measurements
var engineTorque = unitSystem.CreateMeasuredValue(UnitFamilyName.Torque, 250, "N*m");
var displayTorque = engineTorque.AsString();  // "250 N⋅m" (with Unicode)

// Convert between torque units
double lbft = engineTorque.As("lb*ft");  // ~184.4 lb⋅ft

// Supported torque units:
// - MKS/SI: N⋅m (base), mN⋅m, kN⋅m, lb⋅ft, lb⋅in
// - CGS: dyne⋅cm (base)
```

### **Inertia Units**
Moment of inertia for rotational dynamics:

```csharp
// Create inertia measurements
var flywheel = unitSystem.CreateMeasuredValue(UnitFamilyName.Inertia, 5.2, "kg*m2");
var display = flywheel.AsString();  // "5.2 kg⋅m²" (with Unicode)

// Supported inertia units:
// - MKS/SI: kg⋅m² (base), g⋅cm², kg⋅cm², slug⋅ft², lb⋅ft²
// - CGS: g⋅cm² (base)
```

### **Angular Velocity Units**
Rotational speed measurements:

```csharp
// Create angular velocity measurements
var motorSpeed = unitSystem.CreateMeasuredValue(UnitFamilyName.AngularVelocity, 3000, "rpm");
var radPerSec = motorSpeed.As("rad/s");  // ~314.16 rad/s

// Supported angular velocity units:
// - rad/s (base), °/s, rpm, rps, rev/min
```

### **Angular Acceleration Units**
Rate of change of rotational speed:

```csharp
// Create angular acceleration measurements
var spinup = unitSystem.CreateMeasuredValue(UnitFamilyName.AngularAcceleration, 100, "rpm/s");
var display = spinup.AsString();  // "100 rpm/s"

// Supported angular acceleration units:
// - rad/s² (base), °/s², rpm/s, rev/s²
```

### **Inductance Units**
Electrical inductance (enhanced existing family):

```csharp
// Create inductance measurements
var coil = unitSystem.CreateMeasuredValue(UnitFamilyName.Inductance, 47, "uH");
var display = coil.AsString();  // "47 μH" (with Unicode)

// Supported inductance units:
// - H (base), mH, μH, nH
```

### **Unicode Currency Symbols**
All currency units now display with proper Unicode symbols:

```csharp
var price = unitSystem.CreateMeasuredValue(UnitFamilyName.Currency, 99.99, "USD");
Console.WriteLine(price.AsString());  // "99.99 $"

var euroPrice = unitSystem.CreateMeasuredValue(UnitFamilyName.Currency, 85, "EUR");
Console.WriteLine(euroPrice.AsString());  // "85 €"
```

**Supported Currency Symbols:**
- **$** - US Dollar (USD), Mexican Peso (MXN)
- **€** - Euro (EUR)
- **£** - British Pound (GBP)
- **¥** - Japanese Yen (JPY), Chinese Yuan (CNY)
- **₹** - Indian Rupee (INR)
- **₩** - South Korean Won (KRW)
- **₣** - Swiss Franc (CHF)
- **¢** - Cent
- **C$** - Canadian Dollar (CAD)
- **A$** - Australian Dollar (AUD)
- **R$** - Brazilian Real (BRL)
- **S$** - Singapore Dollar (SGD)
- **HK$** - Hong Kong Dollar (HKD)

---

## 🔧 Technical Implementation

### **Attribute-Based Type Registration**
All new unit types use the `[UnitType]` attribute for automatic registration:

```csharp
[Serializable]
[UnitType(UnitFamilyName.Torque)]
public class Torque : MeasuredValue
{
    public Torque(UnitGroup unitGroup) : base(unitGroup)
    {
        if (unitGroup.Family != UnitFamilyName.Torque)
            throw new ArgumentException($"UnitGroup family must be Torque, not {unitGroup.Family}");
    }
    // Full operator support...
}
```

### **JSON Serialization Support**
All new types registered with `JsonDerivedType` attributes in MeasuredValue:

```csharp
[JsonDerivedType(typeof(Torque))]
[JsonDerivedType(typeof(Inertia))]
[JsonDerivedType(typeof(AngularVelocity))]
[JsonDerivedType(typeof(AngularAcceleration))]
[JsonDerivedType(typeof(Inductance))]
```

### **Unit System Coverage**
New mechanical engineering units added to:
- ✅ MKS (Meter-Kilogram-Second)
- ✅ SI (International System)
- ✅ CGS (Centimeter-Gram-Second)

Currency Unicode symbols added to all 6 unit systems:
- ✅ MKS
- ✅ SI
- ✅ CGS
- ✅ IPS (Inch-Pound-Second)
- ✅ FPS (Foot-Pound-Second)
- ✅ mmNs (Millimeter-Newton-Second)

---

## 📦 Package Information

### **Installation**
```bash
dotnet add package ApprenticeFoundryRulesAndUnits --version 10.11.0
```

### **NuGet Package**
- **Package ID**: `ApprenticeFoundryRulesAndUnits`
- **Version**: 10.11.0
- **License**: MIT
- **Target Framework**: .NET 9.0

---

## 🔄 Migration Guide

### **From 10.10.0 to 10.11.0**

No breaking changes. This release is fully backward compatible.

**New Capabilities:**
```csharp
// New unit families available
var torque = unitSystem.CreateMeasuredValue(UnitFamilyName.Torque, 100, "N*m");
var inertia = unitSystem.CreateMeasuredValue(UnitFamilyName.Inertia, 2.5, "kg*m2");
var angularVel = unitSystem.CreateMeasuredValue(UnitFamilyName.AngularVelocity, 1000, "rpm");
var angularAcc = unitSystem.CreateMeasuredValue(UnitFamilyName.AngularAcceleration, 50, "rad/s2");
var inductance = unitSystem.CreateMeasuredValue(UnitFamilyName.Inductance, 100, "uH");

// Currency symbols automatically display
var money = unitSystem.CreateMeasuredValue(UnitFamilyName.Currency, 50, "EUR");
Console.WriteLine(money.AsString());  // "50 €" (automatic Unicode)
```

---

## 📝 Notes

### **Exchange Rates**
Currency exchange rates are approximate as of January 2026. For production financial calculations:
- Consider implementing dynamic exchange rate updates from a financial API
- The architecture supports runtime modification of conversion factors
- Current rates are suitable for development/testing purposes

### **Unicode Symbol Support**
All unit families now have optional Unicode display symbols:
- Mechanical units: ⋅ (middle dot), ² (squared), ° (degree)
- Currency: Proper currency symbols ($, €, £, ¥, ₹, ₩, etc.)
- Display automatically uses Unicode when available

---

## 🎉 Summary

Version 10.11.0 significantly expands FoundryRulesAndUnits with mechanical engineering capabilities and improved Unicode symbol support across all unit families. The new unit types follow the established gold-standard pattern with full operator support, JSON serialization, and automatic type registration.

**Total Unit Families**: 30+ families covering length, mass, force, energy, temperature, angles, electrical, currency, and now mechanical engineering domains.

---

## 📞 Support

- **GitHub**: [ApprenticeFoundry/FoundryRulesAndUnits](https://github.com/ApprenticeFoundry/FoundryRulesAndUnits)
- **Website**: [https://apprenticefoundry.github.io/](https://apprenticefoundry.github.io/)
- **License**: MIT

---

**Happy Engineering! 🚀**
