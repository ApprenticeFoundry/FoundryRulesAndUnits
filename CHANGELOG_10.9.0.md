# FoundryRulesAndUnits 10.9.0 Release Notes

**Release Date**: December 21, 2025  
**Package**: `ApprenticeFoundryRulesAndUnits` version 10.9.0  
**Target Framework**: .NET 9.0

---

## 🎯 What's New in 10.9.0

### **💰 New Feature: Currency and Cost Tracking (v1.0)**

Added comprehensive support for currency and system-independent cost tracking across all six unit systems (SI, FPS, IPS, MKS, CGS, mmNs).

**Three New Unit Families:**

1. **Currency** - Monetary values with 14 international currencies
2. **CostPerQuantity** - Pricing per discrete item/unit/piece
3. **CostPerTime** - Labor and service rates (hourly, daily, yearly)

---

## ✨ Key Features

### **1. Currency Support**
Multi-currency support with automatic conversion:

```csharp
var unitSystem = new UnitSystem(UnitSystemType.SI);
var usd = unitSystem.CreateUnit<Currency>(100, "USD");
var eur = unitSystem.CreateUnit<Currency>(92, "EUR");

// Convert between currencies
var usdValue = eur.As("USD"); // Returns 100.00 (based on exchange rates)

// Format with currency symbols
Console.WriteLine(usd.FormatCurrency()); // "$100.00"
Console.WriteLine(eur.FormatCurrency()); // "€92.00"
```

**Supported Currencies:**
- USD (US Dollar) - Base currency
- EUR (Euro), GBP (British Pound), JPY (Japanese Yen)
- CNY (Chinese Yuan), CAD (Canadian Dollar), AUD (Australian Dollar)
- CHF (Swiss Franc), INR (Indian Rupee), MXN (Mexican Peso)
- BRL (Brazilian Real), KRW (South Korean Won)
- SGD (Singapore Dollar), HKD (Hong Kong Dollar)
- cent (US cents)

### **2. Cost Per Quantity**
Pricing for discrete items with bulk pricing support:

```csharp
var unitSystem = new UnitSystem(UnitSystemType.SI);
var currencyGroup = unitSystem.GetUnitGroup(UnitFamilyName.Currency);

// Component pricing
var screwPrice = unitSystem.CreateUnit<CostPerQuantity>(0.15, "USD/ea");

// Calculate total cost
var totalCost = screwPrice.CalculateCost(250, currencyGroup);
Console.WriteLine($"250 screws: {totalCost.FormatCurrency()}"); // "$37.50"

// Apply volume discount
var bulkPrice = screwPrice.ApplyVolumeDiscount(15); // 15% off
Console.WriteLine($"Bulk price: {bulkPrice.FormatCost()}"); // "$0.13/ea"

// Bulk unit support
var dozenPrice = unitSystem.CreateUnit<CostPerQuantity>(18.00, "USD/dozen");
var hundredPrice = unitSystem.CreateUnit<CostPerQuantity>(120.00, "USD/hundred");
```

**Supported Units:**
- USD/unit, USD/ea, USD/piece, USD/item (aliases)
- USD/dozen (÷12), USD/hundred (÷100), USD/thousand (÷1000)
- Multi-currency: EUR/unit, GBP/unit, JPY/unit, CAD/unit

### **3. Cost Per Time**
Labor and service rate tracking:

```csharp
var unitSystem = new UnitSystem(UnitSystemType.SI);
var currencyGroup = unitSystem.GetUnitGroup(UnitFamilyName.Currency);

// Hourly rate
var hourlyRate = unitSystem.CreateUnit<CostPerTime>(75.00, "USD/hr");

// Calculate labor cost
var laborCost = hourlyRate.CalculateCost(40, currencyGroup); // 40 hours
Console.WriteLine($"Weekly labor: {laborCost.FormatCurrency()}"); // "$3,000.00"

// Annual salary conversion
var salary = unitSystem.CreateUnit<CostPerTime>(120000, "USD/yr");
var hourly = salary.As("USD/hr"); // Convert annual to hourly

// Apply markup/discount
var contractRate = hourlyRate.ApplyAdjustment(25); // 25% markup
Console.WriteLine($"Contract rate: {contractRate.FormatCost()}"); // "$93.75/hr"
```

**Supported Units:**
- USD/hr, USD/h (hour - base unit)
- USD/s (second), USD/min (minute)
- USD/day, USD/wk (week), USD/mo (month), USD/yr (year)
- Multi-currency: EUR/hr, GBP/hr, JPY/hr, CAD/hr

---

## 🏗️ Architecture Highlights

### **System-Independent Design**
Currency and cost types work identically across all six unit systems:
- SI (meters/kg), FPS (feet/lb), IPS (inches/lb)
- MKS, CGS (cm/g), mmNs

**Why?** Currency is universal - $100 USD = $100 USD regardless of whether you measure length in meters or feet.

### **Hub-and-Spoke Conversion**
All currency conversions use USD as the base:
- EUR → USD → JPY (not direct EUR → JPY)
- Simplifies exchange rate management
- Single source of truth for rates

### **Static Exchange Rates**
Rates are embedded in specifications:
- Updated manually every 3-6 months
- Suitable for engineering/cost estimation applications
- Rates as of December 2025

---

## 📦 New API Methods

### Currency Class
```csharp
public class Currency : MeasuredValue
{
    public string FormatCurrency(int decimalPlaces = 2)
    // Arithmetic operators: +, -, *, /
    // Comparison operators: ==, !=, <, >
}
```

### CostPerQuantity Class
```csharp
public class CostPerQuantity : MeasuredValue
{
    public Currency CalculateCost(double quantity, UnitGroup currencyGroup)
    public CostPerQuantity ApplyVolumeDiscount(double discountPercent)
    public string FormatCost(int decimalPlaces = 2)
    // Arithmetic and comparison operators
}
```

### CostPerTime Class
```csharp
public class CostPerTime : MeasuredValue
{
    public Currency CalculateCost(double duration, UnitGroup currencyGroup)
    public Currency CalculateCost(Time duration, UnitGroup currencyGroup)
    public CostPerTime ApplyAdjustment(double adjustmentPercent)
    public string FormatCost(int decimalPlaces = 2)
    // Arithmetic and comparison operators
}
```

---

## 🔧 Changes

### **New Files Created**
- ✅ `UnitSystem/UnitTypes/Currency.cs` - Currency unit type
- ✅ `UnitSystem/UnitTypes/CostPerQuantity.cs` - Cost per item/unit
- ✅ `UnitSystem/UnitTypes/CostPerTime.cs` - Cost per time/labor rates
- ✅ `CURRENCY_AND_COST_UNITS_SPECIFICATION.md` - Comprehensive specification document

### **Modified Files**
- ✅ `UnitSystem/UnitFamilyName.cs` - Added Currency, CostPerQuantity, CostPerTime enums
- ✅ `UnitSystem/MeasuredValue.cs` - Added JsonDerivedType attributes for new types
- ✅ All 6 unit system specifications - Added currency and cost unit definitions
  - SIUnitSystemSpecification.cs
  - FPSUnitSystemSpecification.cs
  - IPSUnitSystemSpecification.cs
  - MKSUnitSystemSpecification.cs
  - CGSUnitSystemSpecification.cs
  - mmNsUnitSystemSpecification.cs

---

## 📋 Complete Change Summary

| Type | Description | Files Modified |
|------|-------------|----------------|
| ✨ Feature | Currency support (14 currencies) | 3 new files, 8 modified |
| ✨ Feature | CostPerQuantity (discrete item pricing) | 1 new file, 8 modified |
| ✨ Feature | CostPerTime (labor/service rates) | 1 new file, 8 modified |
| 📚 Docs | Comprehensive specification document | 1 new file |

**Total**: 3 new unit types, 15 currency units, 13 cost-per-quantity units, 13 cost-per-time units

---

## 📦 Upgrade Instructions

### From 10.8.2

**Update Package Reference**:
```xml
<PackageReference Include="ApprenticeFoundryRulesAndUnits" Version="10.9.0" />
```

**Compatibility**:
- ✅ **Fully backward compatible** - No breaking changes
- ✅ Drop-in replacement for 10.8.2
- ✅ New unit families available immediately

**Migration Steps**:
1. Update package reference to 10.9.0
2. Optional: Add currency/cost tracking to your application
3. Access currency group: `unitSystem.GetUnitGroup(UnitFamilyName.Currency)`

---

## 🎓 Usage Examples

### Example 1: Multi-Currency Bill of Materials
```csharp
var unitSystem = new UnitSystem(UnitSystemType.SI);

var componentA = unitSystem.CreateUnit<Currency>(250, "USD");
var componentB = unitSystem.CreateUnit<Currency>(180, "EUR");
var componentC = unitSystem.CreateUnit<Currency>(150, "GBP");

// Convert all to USD
var total = componentA + componentB.As("USD") + componentC.As("USD");
Console.WriteLine($"Total BOM: ${total:F2}");
```

### Example 2: Project Cost Estimation
```csharp
var unitSystem = new UnitSystem(UnitSystemType.SI);
var currencyGroup = unitSystem.GetUnitGroup(UnitFamilyName.Currency);

// Components
var connectorPrice = unitSystem.CreateUnit<CostPerQuantity>(3.25, "USD/ea");
var connectorCost = connectorPrice.CalculateCost(24, currencyGroup);

// Labor
var laborRate = unitSystem.CreateUnit<CostPerTime>(85.00, "USD/hr");
var laborCost = laborRate.CalculateCost(6.5, currencyGroup);

var projectTotal = connectorCost + laborCost;
Console.WriteLine($"Project cost: {projectTotal.FormatCurrency()}");
```

### Example 3: Volume Discount Pricing
```csharp
var unitSystem = new UnitSystem(UnitSystemType.SI);
var currencyGroup = unitSystem.GetUnitGroup(UnitFamilyName.Currency);

var standardPrice = unitSystem.CreateUnit<CostPerQuantity>(5.00, "USD/unit");
var bulkPrice = standardPrice.ApplyVolumeDiscount(15); // 15% off

var qty = 1000;
var savings = standardPrice.CalculateCost(qty, currencyGroup) - 
              bulkPrice.CalculateCost(qty, currencyGroup);
Console.WriteLine($"Savings: {savings.FormatCurrency()}"); // "$750.00"
```

---

## 🚀 What's Next (v2.0 - Future)

**System-Dependent Cost Types** (deferred):
- CostPerLength (USD/m, USD/ft)
- CostPerMass (USD/kg, USD/lb)
- CostPerVolume (USD/L, USD/gal)
- CostPerArea (USD/m², USD/ft²)

These require complex interactions with physical unit systems and are deferred to v2.0 based on user demand.

---

## 🔗 Resources

- **Package**: https://www.nuget.org/packages/ApprenticeFoundryRulesAndUnits/
- **Repository**: https://github.com/ApprenticeFoundry/FoundryRulesAndUnits
- **Specification**: See `CURRENCY_AND_COST_UNITS_SPECIFICATION.md` for complete details

---

**Previous Version**: 10.8.2  
**Upgrade Recommended**: Yes - Major new feature for cost tracking  
**Breaking Changes**: None  
**Migration Effort**: Zero - Drop-in replacement with new optional features

