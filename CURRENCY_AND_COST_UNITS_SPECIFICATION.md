# Currency and Cost-Per-Unit Specification

**Version:** 1.0  
**Date:** December 21, 2025  
**Status:** Proposed

## Executive Summary

This specification introduces currency units and derived cost-per-unit measurements to the FoundryRulesAndUnits system. This enables financial calculations, cost estimations, and economic modeling within the existing unit conversion framework.

### Core Scope (System-Independent Types)

1. **Currency** (`USD`, `EUR`, `GBP`, etc.) - Base monetary values
2. **Cost Per Item** (`$/unit`, `$/ea`, `$/piece`) - Component pricing, inventory costing, discrete parts
3. **Cost Per Hour** (`$/hr`, `€/hr`) - Labor rates, equipment rental, contractor billing, time-based services

**Key Decision:** Focus on system-independent cost types that work identically across all unit systems (SI, IPS, FPS, etc.)

### Future Scope (System-Dependent Types - Version 2.0)

4. Cost per length - Wire, cable, lumber pricing (requires USD/m vs USD/in vs USD/ft per system)
5. Cost per mass - Raw materials, bulk commodities (requires USD/kg vs USD/lb per system)
6. Cost per volume - Liquids, gases (requires USD/m³ vs USD/in³ per system)
7. Cost per area - Real estate, sheet materials (requires USD/m² vs USD/in² per system)

**Rationale:** System-dependent types require different base units per specification (SI uses meters, IPS uses inches). By focusing on system-independent types first, we achieve 80% of functionality with 20% of complexity.

## Quick Start: Essential Use Cases

If you only need the basics, implement these two families first:

### 1. Cost Per Item (CostPerQuantity)
```csharp
// Component pricing - THE most common use case
var screwPrice = unitSystem.CreateUnit<CostPerQuantity>(0.15, "USD/ea");
var quantity = 1000;
var totalCost = screwPrice.CalculateCost(quantity, currencyGroup);
// Result: $150.00

// Bulk pricing with volume discount
var bulkPrice = screwPrice.ApplyVolumeDiscount(20.0); // 20% off
var bulkTotal = bulkPrice.CalculateCost(quantity, currencyGroup);
// Result: $120.00 (saved $30.00)
```

### 2. Cost Per Hour (CostPerTime)
```csharp
// Labor rates - THE most common time-based cost
var laborRate = unitSystem.CreateUnit<CostPerTime>(85.00, "USD/hr");
var workTime = unitSystem.CreateUnit<Time>(6.5, "h");
var laborCost = laborRate.CalculateCost(workTime, currencyGroup);
// Result: $552.50

// Convert to different time units
var dailyRate = laborRate.As("USD/day"); // Assumes 8-hour day
var annualSalary = laborRate.As("USD/yr"); // Assumes 2080 hours/year
```

**These two cover 80%+ of real-world engineering cost calculations.**

**Important:** This version focuses ONLY on system-independent types (Currency, CostPerQuantity, CostPerTime). System-dependent types (per-length, per-mass, per-volume, per-area) are deferred to version 2.0 to keep implementation simple.

## Background

The current unit system supports physical measurements (length, mass, time, etc.) but lacks financial dimensions. Many engineering and manufacturing applications require:

1. **Currency representation** - Different monetary units (USD, EUR, GBP, etc.)
2. **Cost calculations** - Price per unit ($/kg, €/meter, £/hour)
3. **Economic modeling** - Material costs, labor rates, operational expenses
4. **Multi-currency support** - Conversion between currencies with exchange rates

## Architecture Overview

### 1. Currency as a Base Unit Family

Currency will be introduced as a new base unit family, similar to Length or Mass, with special characteristics:

- **Base Unit:** USD (United States Dollar) - industry standard
- **Exchange Rates:** Dynamic conversion factors updated via exchange rate provider
- **Precision:** Support for fractional cents (4 decimal places minimum)
- **Temporal Aspect:** Exchange rates change over time (requires versioning strategy)

### 2. System-Independent Cost Units (Version 1.0 Scope)

Cost units that work identically across all unit systems:

- **Cost per Quantity:** `$/unit`, `€/piece`, `$/ea`, `$/dozen` - discrete items, parts, components, inventory
- **Cost per Time:** `$/hr`, `$/day`, `$/year`, `€/hr` - labor rates, equipment rentals, subscriptions, time-based services

### 3. System-Dependent Cost Units (Deferred to Version 2.0)

Cost units that require different base units per specification:

- **Cost per Length:** `$/m` (SI), `$/in` (IPS), `$/ft` (FPS) - wire, cable, pipe, lumber pricing
- **Cost per Mass:** `$/kg` (SI), `$/lb` (IPS/FPS) - raw materials, commodity pricing  
- **Cost per Area:** `$/m²` (SI), `$/in²` (IPS), `$/ft²` (FPS) - real estate, fabric, sheet materials
- **Cost per Volume:** `$/L` (SI), `$/gal` (IPS), `$/ft³` (FPS) - liquids, gases, bulk materials
- **Cost per Energy:** `$/kWh`, `€/MJ` - electricity, utilities

**Why deferred?** System-dependent types add significant complexity with minimal additional value for initial release.

## Implementation Details

### Phase 1: Currency Unit Family

#### 1.1 Enum Extensions

**File:** `UnitSystem/UnitFamilyName.cs`

Add to `UnitFamilyName` enum (Version 1.0 - System-Independent Only):
```csharp
Currency,
CostPerQuantity,
CostPerTime,
```

Deferred to Version 2.0 (System-Dependent):
```csharp
// CostPerLength,
// CostPerMass,
// CostPerArea,
// CostPerVolume,
// CostPerEnergy,
```

#### 1.2 Currency Class

**File:** `UnitSystem/UnitTypes/Currency.cs`

```csharp
using FoundryRulesAndUnits.Extensions;
using System;

namespace FoundryRulesAndUnits.Units;

[System.Serializable]
[UnitType(UnitFamilyName.Currency, Description = "Monetary value")]
public class Currency : MeasuredValue
{
    /// <summary>
    /// Constructor with UnitGroup injection
    /// </summary>
    public Currency(UnitGroup unitGroup) : base(unitGroup)
    {
        if (unitGroup.Family != UnitFamilyName.Currency)
            throw new ArgumentException($"UnitGroup family must be {UnitFamilyName.Currency}", nameof(unitGroup));
    }

    public Currency Assign(double value, string? units)
    {
        Init(value, units);
        return this;
    }

    public Currency Assign(Currency source)
    {
        Init(source.Value(), source.U);
        return this;
    }

    public Currency Copy()
    {
        var copy = new Currency(_unitGroup);
        copy.Init(Value(), Internal());
        return copy;
    }

    // Formatting for currency display
    public string FormatCurrency(int decimalPlaces = 2)
    {
        var symbol = GetCurrencySymbol(U);
        return $"{symbol}{Value().ToString($"F{decimalPlaces}")}";
    }

    private string GetCurrencySymbol(string currencyCode)
    {
        return currencyCode switch
        {
            "USD" => "$",
            "EUR" => "€",
            "GBP" => "£",
            "JPY" => "¥",
            "CNY" => "¥",
            "CAD" => "C$",
            "AUD" => "A$",
            _ => currencyCode + " "
        };
    }

    public static Currency operator +(Currency left, Currency right)
    {
        var result = new Currency(left._unitGroup);
        result.Init(left.Value() + right.Value(), left.Internal());
        return result;
    }

    public static Currency operator -(Currency left, Currency right)
    {
        var result = new Currency(left._unitGroup);
        result.Init(left.Value() - right.Value(), left.Internal());
        return result;
    }

    public static Currency operator *(double scalar, Currency right)
    {
        var result = new Currency(right._unitGroup);
        result.Init(scalar * right.Value(), right.Internal());
        return result;
    }

    public static Currency operator *(Currency left, double scalar)
    {
        var result = new Currency(left._unitGroup);
        result.Init(left.Value() * scalar, left.Internal());
        return result;
    }

    public static Currency operator /(Currency left, double scalar)
    {
        var result = new Currency(left._unitGroup);
        result.Init(left.Value() / scalar, left.Internal());
        return result;
    }

    public static bool operator <(Currency left, Currency right) => left.Value() < right.Value();
    public static bool operator >(Currency left, Currency right) => left.Value() > right.Value();
    public static bool operator ==(Currency left, Currency right) => Math.Abs(left.Value() - right.Value()) < 1e-4; // 0.01 cent precision
    public static bool operator !=(Currency left, Currency right) => !(left == right);
    
    public override bool Equals(object? obj) => obj is Currency other && this == other;
    public override int GetHashCode() => Value().GetHashCode();
}
```

#### 1.3 Unit System Specifications

Add currency definitions to each unit system specification:

**Example for SIUnitSystemSpecification.cs:**

```csharp
// Currency units (USD as base)
UnitDefinition.BaseUnit("USD", "US Dollars", UnitFamilyName.Currency),
UnitDefinition.LinearUnit("EUR", "Euros", UnitFamilyName.Currency, 1.09),        // Example rate: 1 EUR = 1.09 USD
UnitDefinition.LinearUnit("GBP", "British Pounds", UnitFamilyName.Currency, 1.27), // Example rate: 1 GBP = 1.27 USD
UnitDefinition.LinearUnit("JPY", "Japanese Yen", UnitFamilyName.Currency, 0.0069), // Example rate: 1 JPY = 0.0069 USD
UnitDefinition.LinearUnit("CNY", "Chinese Yuan", UnitFamilyName.Currency, 0.14),   // Example rate: 1 CNY = 0.14 USD
UnitDefinition.LinearUnit("CAD", "Canadian Dollars", UnitFamilyName.Currency, 0.71), // Example rate: 1 CAD = 0.71 USD
UnitDefinition.LinearUnit("AUD", "Australian Dollars", UnitFamilyName.Currency, 0.64), // Example rate: 1 AUD = 0.64 USD
UnitDefinition.LinearUnit("CHF", "Swiss Francs", UnitFamilyName.Currency, 1.13),   // Example rate: 1 CHF = 1.13 USD
UnitDefinition.LinearUnit("INR", "Indian Rupees", UnitFamilyName.Currency, 0.012), // Example rate: 1 INR = 0.012 USD
UnitDefinition.LinearUnit("cent", "cents", UnitFamilyName.Currency, 0.01),        // 1 cent = 0.01 USD
```

**Note:** Exchange rates will need to be updated regularly via an external service or configuration.

### Phase 2: Cost-Per-Unit Families

#### 2.1 CostPerLength Class

**File:** `UnitSystem/UnitTypes/CostPerLength.cs`

```csharp
using FoundryRulesAndUnits.Extensions;
using System;

namespace FoundryRulesAndUnits.Units;

[System.Serializable]
[UnitType(UnitFamilyName.CostPerLength, Description = "Cost per unit length")]
public class CostPerLength : MeasuredValue
{
    public CostPerLength(UnitGroup unitGroup) : base(unitGroup)
    {
        if (unitGroup.Family != UnitFamilyName.CostPerLength)
            throw new ArgumentException($"UnitGroup family must be {UnitFamilyName.CostPerLength}", nameof(unitGroup));
    }

    public CostPerLength Assign(double value, string? units)
    {
        Init(value, units);
        return this;
    }

    public CostPerLength Copy()
    {
        var copy = new CostPerLength(_unitGroup);
        copy.Init(Value(), Internal());
        return copy;
    }

    // Calculate total cost from length
    public Currency CalculateCost(Length length, UnitGroup currencyGroup)
    {
        // Convert to base units: USD/m * meters = USD
        var costPerMeter = Value(); // Assuming base unit is USD/m
        var meters = length.Value(); // Assuming base unit is meters
        var totalCost = costPerMeter * meters;
        
        var result = new Currency(currencyGroup);
        result.Assign(totalCost, "USD");
        return result;
    }

    public static CostPerLength operator *(double scalar, CostPerLength right)
    {
        var result = new CostPerLength(right._unitGroup);
        result.Init(scalar * right.Value(), right.Internal());
        return result;
    }
}
```

#### 2.2 CostPerMass Class

**File:** `UnitSystem/UnitTypes/CostPerMass.cs`

```csharp
using FoundryRulesAndUnits.Extensions;
using System;

namespace FoundryRulesAndUnits.Units;

[System.Serializable]
[UnitType(UnitFamilyName.CostPerMass, Description = "Cost per unit mass")]
public class CostPerMass : MeasuredValue
{
    public CostPerMass(UnitGroup unitGroup) : base(unitGroup)
    {
        if (unitGroup.Family != UnitFamilyName.CostPerMass)
            throw new ArgumentException($"UnitGroup family must be {UnitFamilyName.CostPerMass}", nameof(unitGroup));
    }

    public CostPerMass Assign(double value, string? units)
    {
        Init(value, units);
        return this;
    }

    public CostPerMass Copy()
    {
        var copy = new CostPerMass(_unitGroup);
        copy.Init(Value(), Internal());
        return copy;
    }

    // Calculate total cost from mass
    public Currency CalculateCost(Mass mass, UnitGroup currencyGroup)
    {
        var costPerKg = Value(); // Assuming base unit is USD/kg
        var kilograms = mass.Value(); // Assuming base unit is kg
        var totalCost = costPerKg * kilograms;
        
        var result = new Currency(currencyGroup);
        result.Assign(totalCost, "USD");
        return result;
    }
}
```

#### 2.3 CostPerQuantity Class

**File:** `UnitSystem/UnitTypes/CostPerQuantity.cs`

```csharp
using FoundryRulesAndUnits.Extensions;
using System;

namespace FoundryRulesAndUnits.Units;

[System.Serializable]
[UnitType(UnitFamilyName.CostPerQuantity, Description = "Cost per discrete unit/item")]
public class CostPerQuantity : MeasuredValue
{
    public CostPerQuantity(UnitGroup unitGroup) : base(unitGroup)
    {
        if (unitGroup.Family != UnitFamilyName.CostPerQuantity)
            throw new ArgumentException($"UnitGroup family must be {UnitFamilyName.CostPerQuantity}", nameof(unitGroup));
    }

    public CostPerQuantity Assign(double value, string? units)
    {
        Init(value, units);
        return this;
    }

    public CostPerQuantity Copy()
    {
        var copy = new CostPerQuantity(_unitGroup);
        copy.Init(Value(), Internal());
        return copy;
    }

    // Calculate total cost from quantity
    public Currency CalculateCost(Quantity quantity, UnitGroup currencyGroup)
    {
        var costPerUnit = Value(); // Assuming base unit is USD/unit
        var units = quantity.Value(); // Quantity count
        var totalCost = costPerUnit * units;
        
        var result = new Currency(currencyGroup);
        result.Assign(totalCost, "USD");
        return result;
    }

    // Calculate total cost from item count (simpler overload)
    public Currency CalculateCost(double itemCount, UnitGroup currencyGroup)
    {
        var costPerUnit = Value();
        var totalCost = costPerUnit * itemCount;
        
        var result = new Currency(currencyGroup);
        result.Assign(totalCost, "USD");
        return result;
    }

    // Apply volume discount based on quantity
    public CostPerQuantity ApplyVolumeDiscount(double discountPercent)
    {
        var discountedCost = Value() * (1.0 - discountPercent / 100.0);
        var result = new CostPerQuantity(_unitGroup);
        result.Init(discountedCost, Internal());
        return result;
    }

    // Calculate break-even quantity for bulk pricing
    public double BreakEvenQuantity(CostPerQuantity bulkPrice)
    {
        // Returns the quantity where bulk pricing becomes advantageous
        var standardCost = Value();
        var bulkCost = bulkPrice.Value();
        
        if (bulkCost >= standardCost)
            return double.PositiveInfinity; // Bulk is never cheaper
            
        return 0; // Any quantity benefits from bulk (this is simplified logic)
    }

    public static CostPerQuantity operator *(double scalar, CostPerQuantity right)
    {
        var result = new CostPerQuantity(right._unitGroup);
        result.Init(scalar * right.Value(), right.Internal());
        return result;
    }
}
```

#### 2.4 Unit Specifications for Cost Units

Add to unit system specifications:

```csharp
// Cost per Length units (USD/m as base)
UnitDefinition.BaseUnit("USD/m", "US Dollars per meter", UnitFamilyName.CostPerLength),
UnitDefinition.LinearUnit("USD/ft", "US Dollars per foot", UnitFamilyName.CostPerLength, 3.28084), // 1 USD/ft = 3.28084 USD/m
UnitDefinition.LinearUnit("USD/in", "US Dollars per inch", UnitFamilyName.CostPerLength, 39.3701), // 1 USD/in = 39.3701 USD/m
UnitDefinition.LinearUnit("USD/km", "US Dollars per kilometer", UnitFamilyName.CostPerLength, 0.001), // 1 USD/km = 0.001 USD/m
UnitDefinition.LinearUnit("USD/mi", "US Dollars per mile", UnitFamilyName.CostPerLength, 0.000621371), // 1 USD/mi = 0.000621371 USD/m
UnitDefinition.LinearUnit("EUR/m", "Euros per meter", UnitFamilyName.CostPerLength, 0.917), // Using EUR exchange rate
UnitDefinition.LinearUnit("GBP/ft", "British Pounds per foot", UnitFamilyName.CostPerLength, 2.58), // Using GBP exchange rate

// Cost per Mass units (USD/kg as base)
UnitDefinition.BaseUnit("USD/kg", "US Dollars per kilogram", UnitFamilyName.CostPerMass),
UnitDefinition.LinearUnit("USD/g", "US Dollars per gram", UnitFamilyName.CostPerMass, 1000.0), // 1 USD/g = 1000 USD/kg
UnitDefinition.LinearUnit("USD/lb", "US Dollars per pound", UnitFamilyName.CostPerMass, 2.20462), // 1 USD/lb = 2.20462 USD/kg
UnitDefinition.LinearUnit("USD/oz", "US Dollars per ounce", UnitFamilyName.CostPerMass, 35.274), // 1 USD/oz = 35.274 USD/kg
UnitDefinition.LinearUnit("USD/ton", "US Dollars per metric ton", UnitFamilyName.CostPerMass, 0.001), // 1 USD/ton = 0.001 USD/kg
UnitDefinition.LinearUnit("EUR/kg", "Euros per kilogram", UnitFamilyName.CostPerMass, 0.917), // Using EUR exchange rate

// Cost per Volume units (USD/m³ as base)
UnitDefinition.BaseUnit("USD/m3", "US Dollars per cubic meter", UnitFamilyName.CostPerVolume),
UnitDefinition.LinearUnit("USD/L", "US Dollars per liter", UnitFamilyName.CostPerVolume, 1000.0), // 1 USD/L = 1000 USD/m³
UnitDefinition.LinearUnit("USD/gal", "US Dollars per gallon", UnitFamilyName.CostPerVolume, 264.172), // 1 USD/gal = 264.172 USD/m³
UnitDefinition.LinearUnit("USD/ft3", "US Dollars per cubic foot", UnitFamilyName.CostPerVolume, 35.3147), // 1 USD/ft³ = 35.3147 USD/m³

// Cost per Time units (USD/s as base)
UnitDefinition.BaseUnit("USD/s", "US Dollars per second", UnitFamilyName.CostPerTime),
UnitDefinition.LinearUnit("USD/hr", "US Dollars per hour", UnitFamilyName.CostPerTime, 0.000277778), // 1 USD/hr = 1/3600 USD/s
UnitDefinition.LinearUnit("USD/day", "US Dollars per day", UnitFamilyName.CostPerTime, 0.0000115741), // 1 USD/day = 1/86400 USD/s
UnitDefinition.LinearUnit("USD/wk", "US Dollars per week", UnitFamilyName.CostPerTime, 0.00000165344), // 1 USD/wk USD/s
UnitDefinition.LinearUnit("USD/yr", "US Dollars per year", UnitFamilyName.CostPerTime, 3.17098e-8), // 1 USD/yr USD/s

// Cost per Quantity units (USD/unit as base)
UnitDefinition.BaseUnit("USD/unit", "US Dollars per unit", UnitFamilyName.CostPerQuantity),
UnitDefinition.LinearUnit("USD/ea", "US Dollars each", UnitFamilyName.CostPerQuantity, 1.0), // 1 USD/ea = 1 USD/unit
UnitDefinition.LinearUnit("USD/piece", "US Dollars per piece", UnitFamilyName.CostPerQuantity, 1.0), // 1 USD/piece = 1 USD/unit
UnitDefinition.LinearUnit("USD/item", "US Dollars per item", UnitFamilyName.CostPerQuantity, 1.0), // 1 USD/item = 1 USD/unit
UnitDefinition.LinearUnit("USD/dozen", "US Dollars per dozen", UnitFamilyName.CostPerQuantity, 0.0833333), // 1 USD/dozen = 1/12 USD/unit
UnitDefinition.LinearUnit("USD/hundred", "US Dollars per hundred", UnitFamilyName.CostPerQuantity, 0.01), // 1 USD/hundred = 0.01 USD/unit
UnitDefinition.LinearUnit("USD/thousand", "US Dollars per thousand", UnitFamilyName.CostPerQuantity, 0.001), // 1 USD/thousand = 0.001 USD/unit
UnitDefinition.LinearUnit("EUR/unit", "Euros per unit", UnitFamilyName.CostPerQuantity, 0.917), // Using EUR exchange rate
UnitDefinition.LinearUnit("GBP/unit", "British Pounds per unit", UnitFamilyName.CostPerQuantity, 0.787), // Using GBP exchange rate
```

### Phase 3: Exchange Rate Management

> **⚠️ COMPLEXITY WARNING:** Currency conversion introduces significant complexity due to:
> - **Time-varying rates** - Exchange rates change constantly (second-by-second in forex markets)
> - **Bid/Ask spreads** - Different rates for buying vs selling
> - **Transaction fees** - Banks and exchanges add markup to mid-market rates
> - **Historical accuracy** - Need for point-in-time conversions for financial records
> - **Rate source reliability** - Different sources provide different rates
> - **Cross-rate calculations** - Converting through intermediary currencies
> - **Regional variations** - Rates vary by country, bank, and transaction type

#### 3.1 Exchange Rate Complexity Mitigation Strategies

**Strategy 1: Snapshot-based Rates**
- Lock rates at specific timestamps for quotations
- Store rate source and timestamp with each conversion
- Allow "as-of-date" conversions for historical accuracy

**Strategy 2: Rate Versioning**
- Version all exchange rate data
- Support multiple concurrent rate sets (spot, forward, historical)
- Track rate provenance (source, timestamp, type)

**Strategy 3: Configurable Precision**
- Allow applications to specify acceptable rate staleness
- Support different rate types (spot, daily average, monthly average)
- Provide warning mechanisms when rates are outdated

**Strategy 4: Fallback Mechanisms**
- Maintain cached rates when external services fail
- Support multiple rate sources with priority ordering
- Graceful degradation to last-known-good rates

#### 3.2 Exchange Rate Provider Interface

**File:** `UnitSystem/Currency/IExchangeRateProvider.cs`

```csharp
using System;
using System.Threading.Tasks;

namespace FoundryRulesAndUnits.Units.Currency;

/// <summary>
/// Exchange rate provider with support for historical rates and metadata
/// </summary>
public interface IExchangeRateProvider
{
    /// <summary>
    /// Get the current exchange rate from one currency to another
    /// </summary>
    /// <param name="fromCurrency">Source currency code (e.g., "USD")</param>
    /// <param name="toCurrency">Target currency code (e.g., "EUR")</param>
    /// <returns>Exchange rate and metadata</returns>
    Task<ExchangeRate> GetExchangeRateAsync(string fromCurrency, string toCurrency);
    
    /// <summary>
    /// Get historical exchange rate for a specific date
    /// </summary>
    /// <param name="fromCurrency">Source currency code</param>
    /// <param name="toCurrency">Target currency code</param>
    /// <param name="asOfDate">The date for which to retrieve the rate</param>
    /// <returns>Historical exchange rate and metadata</returns>
    Task<ExchangeRate> GetHistoricalRateAsync(string fromCurrency, string toCurrency, DateTime asOfDate);
    
    /// <summary>
    /// Get the last update timestamp for exchange rates
    /// </summary>
    DateTime GetLastUpdateTime();
    
    /// <summary>
    /// Force refresh of exchange rates from source
    /// </summary>
    Task RefreshRatesAsync();
    
    /// <summary>
    /// Check if rates are stale based on configured threshold
    /// </summary>
    bool AreRatesStale(TimeSpan threshold);
}

/// <summary>
/// Exchange rate with metadata for traceability
/// </summary>
public class ExchangeRate
{
    /// <summary>
    /// The exchange rate value (e.g., 1 USD = 0.92 EUR means Rate = 0.92)
    /// </summary>
    public double Rate { get; set; }
    
    /// <summary>
    /// Source currency code
    /// </summary>
    public string FromCurrency { get; set; } = string.Empty;
    
    /// <summary>
    /// Target currency code
    /// </summary>
    public string ToCurrency { get; set; } = string.Empty;
    
    /// <summary>
    /// Timestamp when this rate was valid
    /// </summary>
    public DateTime Timestamp { get; set; }
    
    /// <summary>
    /// Source of the exchange rate (e.g., "ECB", "FederalReserve", "XE.com")
    /// </summary>
    public string Source { get; set; } = "Unknown";
    
    /// <summary>
    /// Type of rate (Spot, Daily, Monthly, etc.)
    /// </summary>
    public RateType Type { get; set; } = RateType.Spot;
    
    /// <summary>
    /// Confidence level or quality indicator (0-1)
    /// </summary>
    public double Confidence { get; set; } = 1.0;
    
    /// <summary>
    /// Whether this rate came from cache vs live source
    /// </summary>
    public bool IsCached { get; set; }
}

public enum RateType
{
    Spot,           // Real-time market rate
    DailyAverage,   // Average rate for a day
    WeeklyAverage,  // Average rate for a week
    MonthlyAverage, // Average rate for a month
    FixedQuote,     // Locked rate for a quotation
    Historical      // Historical point-in-time rate
}
```

#### 3.3 Static Exchange Rate Provider (Default)

**File:** `UnitSystem/Currency/StaticExchangeRateProvider.cs`

```csharp
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoundryRulesAndUnits.Units.Currency;

/// <summary>
/// Static exchange rate provider with manually configured rates
/// Suitable for offline use or when dynamic rates are not required
/// WARNING: Rates must be manually updated and will become stale over time
/// </summary>
public class StaticExchangeRateProvider : IExchangeRateProvider
{
    private readonly Dictionary<string, double> _ratesFromUSD;
    private readonly DateTime _lastUpdate;
    private readonly string _source;

    public StaticExchangeRateProvider(DateTime? asOfDate = null, string source = "Manual Configuration")
    {
        _lastUpdate = asOfDate ?? DateTime.UtcNow;
        _source = source;
        
        // Rates as of December 2025 (example values - update with real data)
        // WARNING: These rates are EXAMPLES and will become stale quickly
        _ratesFromUSD = new Dictionary<string, double>
        {
            { "USD", 1.0 },
            { "EUR", 0.92 },    // 1 USD = 0.92 EUR
            { "GBP", 0.79 },    // 1 USD = 0.79 GBP
            { "JPY", 145.0 },   // 1 USD = 145 JPY
            { "CNY", 7.2 },     // 1 USD = 7.2 CNY
            { "CAD", 1.36 },    // 1 USD = 1.36 CAD
            { "AUD", 1.55 },    // 1 USD = 1.55 AUD
            { "CHF", 0.88 },    // 1 USD = 0.88 CHF
            { "INR", 83.0 },    // 1 USD = 83 INR
            { "MXN", 17.0 },    // 1 USD = 17 MXN
            { "BRL", 5.0 },     // 1 USD = 5 BRL
            { "KRW", 1320.0 },  // 1 USD = 1320 KRW
            { "SGD", 1.34 },    // 1 USD = 1.34 SGD
            { "HKD", 7.8 },     // 1 USD = 7.8 HKD
        };
    }

    public Task<ExchangeRate> GetExchangeRateAsync(string fromCurrency, string toCurrency)
    {
        return Task.FromResult(CalculateRate(fromCurrency, toCurrency, _lastUpdate, RateType.DailyAverage));
    }

    public Task<ExchangeRate> GetHistoricalRateAsync(string fromCurrency, string toCurrency, DateTime asOfDate)
    {
        // Static provider doesn't have historical data, return configured rate with historical flag
        var rate = CalculateRate(fromCurrency, toCurrency, asOfDate, RateType.Historical);
        rate.Confidence = 0.5; // Lower confidence for historical queries with static data
        return Task.FromResult(rate);
    }

    private ExchangeRate CalculateRate(string fromCurrency, string toCurrency, DateTime timestamp, RateType rateType)
    {
        if (fromCurrency == toCurrency)
        {
            return new ExchangeRate
            {
                Rate = 1.0,
                FromCurrency = fromCurrency,
                ToCurrency = toCurrency,
                Timestamp = timestamp,
                Source = _source,
                Type = rateType,
                Confidence = 1.0,
                IsCached = true
            };
        }

        if (!_ratesFromUSD.ContainsKey(fromCurrency))
            throw new ArgumentException($"Unknown currency: {fromCurrency}. Supported currencies: {string.Join(", ", _ratesFromUSD.Keys)}");
            
        if (!_ratesFromUSD.ContainsKey(toCurrency))
            throw new ArgumentException($"Unknown currency: {toCurrency}. Supported currencies: {string.Join(", ", _ratesFromUSD.Keys)}");

        // Convert through USD: from -> USD -> to
        // Example: EUR to GBP
        // 1 EUR = 1/0.92 USD = 1.087 USD
        // 1.087 USD = 1.087 * 0.79 GBP = 0.859 GBP
        var fromToUSD = 1.0 / _ratesFromUSD[fromCurrency];
        var usdToTarget = _ratesFromUSD[toCurrency];
        
        return new ExchangeRate
        {
            Rate = fromToUSD * usdToTarget,
            FromCurrency = fromCurrency,
            ToCurrency = toCurrency,
            Timestamp = timestamp,
            Source = _source,
            Type = rateType,
            Confidence = 1.0,
            IsCached = true
        };
    }

    public DateTime GetLastUpdateTime() => _lastUpdate;

    public Task RefreshRatesAsync() => Task.CompletedTask; // Static rates don't refresh

    public bool AreRatesStale(TimeSpan threshold)
    {
        return DateTime.UtcNow - _lastUpdate > threshold;
    }
}
```

#### 3.4 Cached Exchange Rate Provider with Fallback

**File:** `UnitSystem/Currency/CachedExchangeRateProvider.cs`

```csharp
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoundryRulesAndUnits.Units.Currency;

/// <summary>
/// Exchange rate provider with intelligent caching and fallback to last-known-good rates
/// Wraps another provider to add caching, staleness detection, and resilience
/// </summary>
public class CachedExchangeRateProvider : IExchangeRateProvider
{
    private readonly IExchangeRateProvider _innerProvider;
    private readonly IExchangeRateProvider? _fallbackProvider;
    private readonly ConcurrentDictionary<string, CachedRate> _cache;
    private readonly TimeSpan _cacheExpiration;
    private readonly TimeSpan _maxStaleness;
    private DateTime _lastSuccessfulUpdate;

    public CachedExchangeRateProvider(
        IExchangeRateProvider innerProvider,
        IExchangeRateProvider? fallbackProvider = null,
        TimeSpan? cacheExpiration = null,
        TimeSpan? maxStaleness = null)
    {
        _innerProvider = innerProvider;
        _fallbackProvider = fallbackProvider;
        _cache = new ConcurrentDictionary<string, CachedRate>();
        _cacheExpiration = cacheExpiration ?? TimeSpan.FromHours(1);
        _maxStaleness = maxStaleness ?? TimeSpan.FromDays(7);
        _lastSuccessfulUpdate = DateTime.UtcNow;
    }

    public async Task<ExchangeRate> GetExchangeRateAsync(string fromCurrency, string toCurrency)
    {
        var cacheKey = $"{fromCurrency}:{toCurrency}";
        
        // Check cache first
        if (_cache.TryGetValue(cacheKey, out var cached))
        {
            if (DateTime.UtcNow - cached.Timestamp < _cacheExpiration)
            {
                cached.Rate.IsCached = true;
                return cached.Rate;
            }
        }

        // Try to fetch fresh rate
        try
        {
            var rate = await _innerProvider.GetExchangeRateAsync(fromCurrency, toCurrency);
            
            // Update cache
            _cache[cacheKey] = new CachedRate
            {
                Rate = rate,
                Timestamp = DateTime.UtcNow
            };
            
            _lastSuccessfulUpdate = DateTime.UtcNow;
            rate.IsCached = false;
            return rate;
        }
        catch (Exception ex)
        {
            // If fetch fails, check if we have acceptable cached data
            if (cached != null && DateTime.UtcNow - cached.Timestamp < _maxStaleness)
            {
                cached.Rate.IsCached = true;
                cached.Rate.Confidence *= 0.8; // Reduce confidence for stale data
                return cached.Rate;
            }

            // Try fallback provider
            if (_fallbackProvider != null)
            {
                try
                {
                    var fallbackRate = await _fallbackProvider.GetExchangeRateAsync(fromCurrency, toCurrency);
                    fallbackRate.Source = $"Fallback: {fallbackRate.Source}";
                    fallbackRate.Confidence *= 0.7; // Reduce confidence for fallback
                    return fallbackRate;
                }
                catch
                {
                    // Fallback also failed, rethrow original exception
                }
            }

            throw new InvalidOperationException(
                $"Unable to retrieve exchange rate for {fromCurrency} to {toCurrency}. " +
                $"Inner provider failed and no acceptable cached or fallback rate available.", ex);
        }
    }

    public async Task<ExchangeRate> GetHistoricalRateAsync(string fromCurrency, string toCurrency, DateTime asOfDate)
    {
        // Historical rates typically don't need caching (they don't change)
        try
        {
            return await _innerProvider.GetHistoricalRateAsync(fromCurrency, toCurrency, asOfDate);
        }
        catch when (_fallbackProvider != null)
        {
            var fallbackRate = await _fallbackProvider.GetHistoricalRateAsync(fromCurrency, toCurrency, asOfDate);
            fallbackRate.Source = $"Fallback: {fallbackRate.Source}";
            fallbackRate.Confidence *= 0.6;
            return fallbackRate;
        }
    }

    public DateTime GetLastUpdateTime() => _lastSuccessfulUpdate;

    public async Task RefreshRatesAsync()
    {
        // Clear cache to force refresh on next request
        _cache.Clear();
        await _innerProvider.RefreshRatesAsync();
    }

    public bool AreRatesStale(TimeSpan threshold)
    {
        return DateTime.UtcNow - _lastSuccessfulUpdate > threshold;
    }

    private class CachedRate
    {
        public ExchangeRate Rate { get; set; } = null!;
        public DateTime Timestamp { get; set; }
    }
}
```
    private readonly DateTime _lastUpdate;

    public StaticExchangeRateProvider(DateTime? asOfDate = null)
    {
        _lastUpdate = asOfDate ?? DateTime.UtcNow;
        
        // Rates as of December 2025 (example values - update with real data)
        _ratesFromUSD = new Dictionary<string, double>
        {
            { "USD", 1.0 },
            { "EUR", 0.92 },    // 1 USD = 0.92 EUR
            { "GBP", 0.79 },    // 1 USD = 0.79 GBP
            { "JPY", 145.0 },   // 1 USD = 145 JPY
            { "CNY", 7.2 },     // 1 USD = 7.2 CNY
            { "CAD", 1.36 },    // 1 USD = 1.36 CAD
            { "AUD", 1.55 },    // 1 USD = 1.55 AUD
            { "CHF", 0.88 },    // 1 USD = 0.88 CHF
            { "INR", 83.0 },    // 1 USD = 83 INR
        };
    }

    public Task<double> GetExchangeRateAsync(string fromCurrency, string toCurrency)
    {
        if (fromCurrency == toCurrency)
            return Task.FromResult(1.0);

        if (!_ratesFromUSD.ContainsKey(fromCurrency))
            throw new ArgumentException($"Unknown currency: {fromCurrency}");
            
        if (!_ratesFromUSD.ContainsKey(toCurrency))
            throw new ArgumentException($"Unknown currency: {toCurrency}");

        // Convert through USD: from -> USD -> to
        var fromToUSD = 1.0 / _ratesFromUSD[fromCurrency];
        var usdToTarget = _ratesFromUSD[toCurrency];
        
        return Task.FromResult(fromToUSD * usdToTarget);
    }

    public DateTime GetLastUpdateTime() => _lastUpdate;

    public Task RefreshRatesAsync() => Task.CompletedTask; // Static rates don't refresh
}
```

#### 3.5 Dynamic Exchange Rate Provider (External API Integration)

**File:** `UnitSystem/Currency/DynamicExchangeRateProvider.cs`

```csharp
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace FoundryRulesAndUnits.Units.Currency;

/// <summary>
/// Dynamic exchange rate provider that fetches rates from external API
/// Supports: exchangerate-api.com, currencyapi.com, ECB, Federal Reserve, etc.
/// IMPORTANT: Requires API key and network connectivity
/// RECOMMENDATION: Always wrap in CachedExchangeRateProvider with fallback
/// </summary>
public class DynamicExchangeRateProvider : IExchangeRateProvider
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _apiUrl;
    private readonly string _source;
    private DateTime _lastUpdate;

    public DynamicExchangeRateProvider(string apiKey, string apiUrl, string source = "External API")
    {
        _httpClient = new HttpClient();
        _httpClient.Timeout = TimeSpan.FromSeconds(10);
        _apiKey = apiKey;
        _apiUrl = apiUrl;
        _source = source;
        _lastUpdate = DateTime.MinValue;
    }

    public async Task<ExchangeRate> GetExchangeRateAsync(string fromCurrency, string toCurrency)
    {
        try
        {
            // Example API call structure (varies by provider)
            var url = $"{_apiUrl}/latest?base={fromCurrency}&symbols={toCurrency}&apikey={_apiKey}";
            var response = await _httpClient.GetStringAsync(url);
            var data = JsonSerializer.Deserialize<ExchangeRateApiResponse>(response);

            if (data?.Rates != null && data.Rates.ContainsKey(toCurrency))
            {
                _lastUpdate = DateTime.UtcNow;
                
                return new ExchangeRate
                {
                    Rate = data.Rates[toCurrency],
                    FromCurrency = fromCurrency,
                    ToCurrency = toCurrency,
                    Timestamp = DateTime.UtcNow,
                    Source = _source,
                    Type = RateType.Spot,
                    Confidence = 0.95,
                    IsCached = false
                };
            }

            throw new InvalidOperationException($"Rate not found in API response for {fromCurrency} to {toCurrency}");
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Failed to retrieve exchange rate from {_source}. " +
                $"Check network connectivity, API key, and rate limits.", ex);
        }
    }

    public async Task<ExchangeRate> GetHistoricalRateAsync(string fromCurrency, string toCurrency, DateTime asOfDate)
    {
        try
        {
            var dateStr = asOfDate.ToString("yyyy-MM-dd");
            var url = $"{_apiUrl}/{dateStr}?base={fromCurrency}&symbols={toCurrency}&apikey={_apiKey}";
            var response = await _httpClient.GetStringAsync(url);
            var data = JsonSerializer.Deserialize<ExchangeRateApiResponse>(response);

            if (data?.Rates != null && data.Rates.ContainsKey(toCurrency))
            {
                return new ExchangeRate
                {
                    Rate = data.Rates[toCurrency],
                    FromCurrency = fromCurrency,
                    ToCurrency = toCurrency,
                    Timestamp = asOfDate,
                    Source = _source,
                    Type = RateType.Historical,
                    Confidence = 0.98, // Historical rates are typically more reliable
                    IsCached = false
                };
            }

            throw new InvalidOperationException($"Historical rate not found for {asOfDate:yyyy-MM-dd}");
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Failed to retrieve historical exchange rate from {_source}.", ex);
        }
    }

    public DateTime GetLastUpdateTime() => _lastUpdate;

    public Task RefreshRatesAsync()
    {
        _lastUpdate = DateTime.MinValue; // Force refresh on next request
        return Task.CompletedTask;
    }

    public bool AreRatesStale(TimeSpan threshold)
    {
        return DateTime.UtcNow - _lastUpdate > threshold;
    }

    // API response structure (simplified - adjust based on actual API)
    private class ExchangeRateApiResponse
    {
        public Dictionary<string, double>? Rates { get; set; }
        public string? Base { get; set; }
        public DateTime Date { get; set; }
    }
}
```

#### 3.6 Recommended Provider Configuration Strategy

```csharp
// Production configuration with multiple fallback layers
public static IExchangeRateProvider CreateProductionProvider()
{
    // Layer 1: Primary dynamic provider (live rates)
    var dynamicProvider = new DynamicExchangeRateProvider(
        apiKey: Environment.GetEnvironmentVariable("EXCHANGE_RATE_API_KEY") ?? "",
        apiUrl: "https://api.exchangerate-api.com/v4",
        source: "ExchangeRate-API"
    );

    // Layer 2: Static provider as ultimate fallback
    var staticProvider = new StaticExchangeRateProvider(
        asOfDate: new DateTime(2025, 12, 1),
        source: "Static Fallback Rates"
    );

    // Layer 3: Cached provider wrapping dynamic with static fallback
    var cachedProvider = new CachedExchangeRateProvider(
        innerProvider: dynamicProvider,
        fallbackProvider: staticProvider,
        cacheExpiration: TimeSpan.FromMinutes(30),
        maxStaleness: TimeSpan.FromDays(7)
    );

    return cachedProvider;
}

// Development/testing configuration (no external dependencies)
public static IExchangeRateProvider CreateDevelopmentProvider()
{
    return new StaticExchangeRateProvider(
        asOfDate: DateTime.UtcNow,
        source: "Development Static Rates"
    );
}
```

### Phase 4: Currency Conversion Complexity Management

#### 4.1 Best Practices for Currency Handling

**1. Always Track Rate Metadata**
```csharp
// DON'T: Simple conversion without tracking
var eurAmount = usdAmount.As("EUR"); 

// DO: Track conversion details
var rate = await exchangeProvider.GetExchangeRateAsync("USD", "EUR");
var eurAmount = usdAmount * rate.Rate;
Console.WriteLine($"Converted using {rate.Source} rate from {rate.Timestamp:yyyy-MM-dd HH:mm}");
Console.WriteLine($"Rate: 1 USD = {rate.Rate} EUR (confidence: {rate.Confidence:P0})");
```

**2. Lock Rates for Quotations**
```csharp
// Create a snapshot of current rates for a quotation
public class QuotationRateSnapshot
{
    public DateTime SnapshotDate { get; set; }
    public Dictionary<string, ExchangeRate> Rates { get; set; } = new();
    public string QuotationId { get; set; } = string.Empty;

    public Currency ConvertWithSnapshot(Currency amount, string toCurrency)
    {
        var key = $"{amount.U}:{toCurrency}";
        if (Rates.TryGetValue(key, out var rate))
        {
            // Use locked rate from snapshot
            return amount * rate.Rate;
        }
        throw new InvalidOperationException($"No rate locked for {key} in this quotation");
    }
}
```

**3. Warn on Stale Rates**
```csharp
public Currency ConvertCurrency(Currency amount, string targetCurrency, IExchangeRateProvider provider)
{
    // Check if rates are stale
    if (provider.AreRatesStale(TimeSpan.FromHours(24)))
    {
        Console.WriteLine("⚠️  WARNING: Exchange rates are more than 24 hours old!");
        Console.WriteLine($"    Last update: {provider.GetLastUpdateTime():yyyy-MM-dd HH:mm}");
        Console.WriteLine("    Consider refreshing rates before making financial decisions.");
    }

    var rate = await provider.GetExchangeRateAsync(amount.U, targetCurrency);
    return amount * rate.Rate;
}
```

**4. Handle Cross-Currency Calculations Carefully**
```csharp
// Complex scenario: Sum costs from multiple currencies
public Currency CalculateMultiCurrencyTotal(List<Currency> costs, string targetCurrency)
{
    var total = 0.0;
    var conversionDetails = new List<string>();

    foreach (var cost in costs)
    {
        if (cost.U == targetCurrency)
        {
            total += cost.Value();
        }
        else
        {
            var rate = await exchangeProvider.GetExchangeRateAsync(cost.U, targetCurrency);
            var converted = cost.Value() * rate.Rate;
            total += converted;
            
            conversionDetails.Add(
                $"  {cost.FormatCurrency()} → {converted:F2} {targetCurrency} " +
                $"@ {rate.Rate:F4} (from {rate.Source}, {rate.Timestamp:yyyy-MM-dd})");
        }
    }

    Console.WriteLine("Multi-currency conversion breakdown:");
    conversionDetails.ForEach(Console.WriteLine);
    
    return new Currency(unitGroup).Assign(total, targetCurrency);
}
```

#### 4.2 Common Pitfalls and Solutions

| Pitfall | Problem | Solution |
|---------|---------|----------|
| **Naive Caching** | Using stale rates without awareness | Implement staleness detection and warnings |
| **No Fallback** | System fails when API is down | Always configure fallback providers |
| **Precision Loss** | Rounding errors in chains of conversions | Minimize conversion chains, use high precision |
| **Rate Mixing** | Mixing rates from different times/sources | Track rate metadata, use snapshots for consistency |
| **Cross-Rate Errors** | Direct rates vs calculated cross-rates differ | Document rate calculation method, prefer hub-based |
| **Ignored Confidence** | Treating all rates as equally reliable | Check and display confidence scores |
| **No Audit Trail** | Can't explain historical conversions | Log all conversions with rate metadata |

#### 4.3 Simplified vs. Full-Featured Approaches

**Simplified Approach (Good for most applications):**
- Use static rates updated monthly
- Single currency conversions only
- No historical tracking
- Suitable for: Internal tools, estimates, non-critical calculations

**Full-Featured Approach (Required for financial applications):**
- Dynamic rates with multiple sources
- Complete audit trail with metadata
- Historical rate support
- Rate locking for quotes
- Staleness detection
- Multiple fallback layers
- Suitable for: Accounting, invoicing, financial reporting, compliance

#### 4.4 Example: Complete Currency Conversion with Full Metadata

```csharp
public class CurrencyConversion
{
    public Currency OriginalAmount { get; set; }
    public Currency ConvertedAmount { get; set; }
    public ExchangeRate RateUsed { get; set; }
    public DateTime ConversionTimestamp { get; set; }
    public string Purpose { get; set; } // "Quotation", "Invoice", "Report", etc.
    
    public string GetAuditRecord()
    {
        return $"[{ConversionTimestamp:yyyy-MM-dd HH:mm:ss}] {Purpose}\n" +
               $"  Original: {OriginalAmount.FormatCurrency()}\n" +
               $"  Converted: {ConvertedAmount.FormatCurrency()}\n" +
               $"  Rate: 1 {RateUsed.FromCurrency} = {RateUsed.Rate:F6} {RateUsed.ToCurrency}\n" +
               $"  Source: {RateUsed.Source}\n" +
               $"  Rate Date: {RateUsed.Timestamp:yyyy-MM-dd HH:mm}\n" +
               $"  Rate Type: {RateUsed.Type}\n" +
               $"  Confidence: {RateUsed.Confidence:P1}\n" +
               $"  Cached: {RateUsed.IsCached}";
    }
}

// Usage
var conversion = await PerformTrackedConversion(
    amount: usdCost,
    targetCurrency: "EUR",
    purpose: "Customer Invoice #12345"
);

// Save to audit log
_auditLog.LogConversion(conversion);
Console.WriteLine(conversion.GetAuditRecord());
```

## Usage Examples

### Example 1: Basic Currency Conversion

```csharp
var unitSystem = new UnitSystem(UnitSystemType.SI);

// Create currency values
var usd = unitSystem.CreateUnit<Currency>(100, "USD");
var eur = unitSystem.CreateUnit<Currency>(100, "EUR");

// Convert between currencies
var usdFromEur = eur.As("USD"); // Returns value in USD
Console.WriteLine($"€100 = ${usdFromEur:F2}");
```

### Example 2: Material Cost Calculation

```csharp
var unitSystem = new UnitSystem(UnitSystemType.SI);

// Material properties
var wireLength = unitSystem.CreateUnit<Length>(50, "m");
var wireCost = unitSystem.CreateUnit<CostPerLength>(2.50, "USD/m");

// Calculate total cost
var totalCost = wireCost.CalculateCost(wireLength, unitSystem.GetUnitGroup(UnitFamilyName.Currency));
Console.WriteLine($"Cost for {wireLength.AsString("m")} of wire: {totalCost.FormatCurrency()}");
```

### Example 3: Bulk Material Pricing

```csharp
var unitSystem = new UnitSystem(UnitSystemType.SI);

// Steel pricing
var steelMass = unitSystem.CreateUnit<Mass>(500, "kg");
var steelPrice = unitSystem.CreateUnit<CostPerMass>(3.50, "USD/kg");

// Calculate cost
var steelCost = steelPrice.CalculateCost(steelMass, unitSystem.GetUnitGroup(UnitFamilyName.Currency));
Console.WriteLine($"Cost for {steelMass.AsString("kg")} of steel: {steelCost.FormatCurrency()}");

// Convert to different currency
var eurCost = steelCost.As("EUR");
Console.WriteLine($"Cost in EUR: €{eurCost:F2}");
```

### Example 4: Labor Cost Calculation

```csharp
var unitSystem = new UnitSystem(UnitSystemType.SI);

// Labor rate and time
var hourlyRate = unitSystem.CreateUnit<CostPerTime>(75.0, "USD/hr");
var workTime = unitSystem.CreateUnit<Time>(8.5, "h");

// Calculate labor cost
var laborCost = hourlyRate.CalculateCost(workTime, unitSystem.GetUnitGroup(UnitFamilyName.Currency));
Console.WriteLine($"Labor cost for {workTime.AsString("h")}: {laborCost.FormatCurrency()}");
```

### Example 5: Multi-Currency Bill of Materials

```csharp
var unitSystem = new UnitSystem(UnitSystemType.SI);

// Components from different regions
var componentA = unitSystem.CreateUnit<Currency>(250, "USD");
var componentB = unitSystem.CreateUnit<Currency>(180, "EUR");
var componentC = unitSystem.CreateUnit<Currency>(150, "GBP");

// Convert all to USD for total
var totalUSD = componentA.Value() + 
               componentB.As("USD") + 
               componentC.As("USD");

Console.WriteLine($"Total BOM cost: ${totalUSD:F2}");
```

### Example 6: Discrete Item Cost Calculation

```csharp
var unitSystem = new UnitSystem(UnitSystemType.SI);

// Component pricing
var screwPrice = unitSystem.CreateUnit<CostPerQuantity>(0.15, "USD/ea");
var screwCount = unitSystem.CreateUnit<Quantity>(250, "unit");

// Calculate total cost
var screwCost = screwPrice.CalculateCost(screwCount, unitSystem.GetUnitGroup(UnitFamilyName.Currency));
Console.WriteLine($"Cost for {screwCount.Value()} screws at {screwPrice.AsString("USD/ea")}: {screwCost.FormatCurrency()}");

// Or using direct count
var totalCost = screwPrice.CalculateCost(250, unitSystem.GetUnitGroup(UnitFamilyName.Currency));
Console.WriteLine($"Direct calculation: {totalCost.FormatCurrency()}");
```

### Example 7: Volume Discount Pricing

```csharp
var unitSystem = new UnitSystem(UnitSystemType.SI);

// Standard pricing
var standardPrice = unitSystem.CreateUnit<CostPerQuantity>(5.00, "USD/unit");

// Apply 15% volume discount
var bulkPrice = standardPrice.ApplyVolumeDiscount(15.0);
Console.WriteLine($"Standard: {standardPrice.FormatCurrency()} per unit");
Console.WriteLine($"Bulk (15% off): {bulkPrice.FormatCurrency()} per unit");

// Calculate savings on 1000 units
var quantity = 1000;
var standardTotal = standardPrice.CalculateCost(quantity, unitSystem.GetUnitGroup(UnitFamilyName.Currency));
var bulkTotal = bulkPrice.CalculateCost(quantity, unitSystem.GetUnitGroup(UnitFamilyName.Currency));
var savings = standardTotal - bulkTotal;

Console.WriteLine($"Savings on {quantity} units: {savings.FormatCurrency()}");
```

### Example 8: Cost Per Dozen/Hundred/Thousand

```csharp
var unitSystem = new UnitSystem(UnitSystemType.SI);

// Pricing in bulk quantities
var pricePerDozen = unitSystem.CreateUnit<CostPerQuantity>(24.00, "USD/dozen");
var pricePerHundred = unitSystem.CreateUnit<CostPerQuantity>(180.00, "USD/hundred");

// Convert to per-unit pricing for comparison
var perUnitFromDozen = pricePerDozen.As("USD/unit");
var perUnitFromHundred = pricePerHundred.As("USD/unit");

Console.WriteLine($"Price per dozen ($24.00/dz): ${perUnitFromDozen:F2} per unit");
Console.WriteLine($"Price per hundred ($180.00/C): ${perUnitFromHundred:F2} per unit");

// Calculate cost for specific quantity
var orderQuantity = 350;
var costFromDozen = pricePerDozen.CalculateCost(orderQuantity, unitSystem.GetUnitGroup(UnitFamilyName.Currency));
Console.WriteLine($"Cost for {orderQuantity} units at dozen pricing: {costFromDozen.FormatCurrency()}");
```

### Example 9: Mixed BOM with Multiple Cost Types

```csharp
var unitSystem = new UnitSystem(UnitSystemType.SI);

// Material costs
var aluminumPrice = unitSystem.CreateUnit<CostPerMass>(4.50, "USD/kg");
var aluminumAmount = unitSystem.CreateUnit<Mass>(12.5, "kg");

// Item costs
var connectorPrice = unitSystem.CreateUnit<CostPerQuantity>(3.25, "USD/ea");
var connectorQty = 24;

// Cable costs
var cablePrice = unitSystem.CreateUnit<CostPerLength>(1.75, "USD/ft");
var cableLength = unitSystem.CreateUnit<Length>(150, "ft");

// Labor costs
var laborRate = unitSystem.CreateUnit<CostPerTime>(85.00, "USD/hr");
var laborTime = unitSystem.CreateUnit<Time>(6.5, "h");

// Calculate totals
var currencyGroup = unitSystem.GetUnitGroup(UnitFamilyName.Currency);
var materialCost = aluminumPrice.CalculateCost(aluminumAmount, currencyGroup);
var connectorCost = connectorPrice.CalculateCost(connectorQty, currencyGroup);
var cableCost = cablePrice.CalculateCost(cableLength, currencyGroup);
var laborCost = laborRate.CalculateCost(laborTime, currencyGroup);

var totalProjectCost = materialCost + connectorCost + cableCost + laborCost;

Console.WriteLine("Project Cost Breakdown:");
Console.WriteLine($"  Aluminum: {materialCost.FormatCurrency()}");
Console.WriteLine($"  Connectors: {connectorCost.FormatCurrency()}");
Console.WriteLine($"  Cable: {cableCost.FormatCurrency()}");
Console.WriteLine($"  Labor: {laborCost.FormatCurrency()}");
Console.WriteLine($"  Total: {totalProjectCost.FormatCurrency()}");
```
var totalUSD = componentA.Value() + 
               componentB.As("USD") + 
               componentC.As("USD");

Console.WriteLine($"Total BOM cost: ${totalUSD:F2}");
```

## Implementation Plan: Core Features (Cost Per Item & Cost Per Hour)

This plan focuses on the two essential features with minimal complexity, getting you to working functionality quickly.

### Phase 1: Foundation (Week 1) - Currency Support Only

**Goal:** Add basic currency representation without exchange rates

**Tasks:**
1. Update `UnitFamilyName.cs` enum
   - Add `Currency` enum value
   - Do NOT add other cost types yet

2. Create `Currency.cs` class
   - Basic structure following `Length.cs` pattern
   - Value storage, operators (+, -, *, /)
   - Format methods for display ($, €, £)
   - No exchange rate logic yet

3. Update unit specifications
   - Add USD as base unit to all unit system specs (SI, MKS, FPS, etc.)
   - Add 5-10 major currencies (EUR, GBP, JPY, CAD, AUD, CHF, INR)
   - Use STATIC exchange rates (hard-coded, updated manually)
   - Document that rates are approximate

4. Update `MeasuredValue.cs`
   - Add `[JsonDerivedType(typeof(Currency))]` attribute

**Deliverable:** Can create Currency objects, store amounts, do basic math, display with symbols

**Testing:** 
- Create USD currency values
- Add/subtract currency amounts
- Display formatted currency
- Serialize/deserialize to JSON

**Skip for Phase 1:**
- Exchange rate providers
- Dynamic rates
- Historical rates
- Cross-currency conversions

---

### Phase 2: Cost Per Item (Week 2) - CRITICAL FEATURE

**Goal:** Enable discrete component pricing

**Tasks:**
1. Update `UnitFamilyName.cs` enum
   - Add `CostPerQuantity` enum value

2. Create `CostPerQuantity.cs` class
   - Follow same pattern as Currency
   - `CalculateCost(Quantity quantity)` method
   - `CalculateCost(double itemCount)` overload (simpler)
   - `ApplyVolumeDiscount(double percent)` method
   - Operators for math operations

3. Update ALL 6 unit specifications (IDENTICAL definitions in each)
   - Add base unit: `USD/unit`
   - Add aliases: `USD/ea`, `USD/piece`, `USD/item`
   - Add bulk units: `USD/dozen`, `USD/hundred`, `USD/thousand`
   - Add EUR, GBP variants
   - Files: SI, IPS, FPS, MKS, CGS, mmNs specifications

4. Update `MeasuredValue.cs`
   - Add `[JsonDerivedType(typeof(CostPerQuantity))]`

**Deliverable:** Can price components, calculate totals, apply discounts

**Testing:**
- Price a screw at $0.15/ea, calculate cost for 500 units
- Price by dozen, calculate for any quantity
- Apply 10% volume discount
- Multi-currency item pricing (USD vs EUR)

**Use Cases Enabled:**
- Component BOM costing
- Inventory valuation
- Purchase order calculations
- Bulk pricing scenarios

---

### Phase 3: Cost Per Hour (Week 3) - CRITICAL FEATURE

**Goal:** Enable labor rate and time-based cost calculations

**Tasks:**
1. Update `UnitFamilyName.cs` enum
   - Add `CostPerTime` enum value

2. Create `CostPerTime.cs` class
   - Follow same pattern as CostPerQuantity
   - `CalculateCost(Time duration)` method
   - Handle hourly, daily, weekly, yearly rates
   - Conversion between time units ($/hr ↔ $/day)

3. Update ALL 6 unit specifications (IDENTICAL definitions in each)
   - Add base unit: `USD/s` (seconds for consistency)
   - Add common units: `USD/hr`, `USD/day`, `USD/wk`, `USD/yr`
   - Conversion factors based on time
   - Add EUR, GBP variants
   - Files: SI, IPS, FPS, MKS, CGS, mmNs specifications

4. Update `MeasuredValue.cs`
   - Add `[JsonDerivedType(typeof(CostPerTime))]`

**Deliverable:** Can calculate labor costs, equipment rental, time-based services

**Testing:**
- Engineer at $85/hr works 6.5 hours
- Convert hourly rate to annual salary
- Calculate daily equipment rental
- Multi-currency labor rates

**Use Cases Enabled:**
- Labor cost estimation
- Equipment rental calculations
- Contractor billing
- Project time costing

---

### Phase 4: Integration & Polish (Week 4)

**Goal:** Make the system production-ready

**Tasks:**
1. Comprehensive unit tests
   - All currency operations
   - All CostPerQuantity scenarios
   - All CostPerTime scenarios
   - Edge cases (zero, negative, very large values)
   - Serialization round-trips

2. Integration tests
   - Mixed BOM with items + labor
   - Multi-currency scenarios
   - Real-world project costing examples

3. Documentation
   - API documentation for all three classes
   - Usage examples in README
   - Migration guide for existing code

4. Configuration
   - Exchange rate configuration in appsettings.json
   - Default currency setting
   - Rate update documentation

**Deliverable:** Production-ready core feature set

**Success Criteria:**
- Can price 100+ item BOM in seconds
- Can calculate labor costs for projects
- Can mix USD, EUR, GBP in one calculation
- All tests pass
- Documentation complete

---

### Phase 5 (Version 2.0): System-Dependent Cost Types

**DEFERRED - Not in initial release**

**Reason:** These require system-specific base units and add significant complexity:

**System-Dependent Cost Types:**
- `CostPerLength` - Wire, cable, lumber pricing (USD/m in SI, USD/in in IPS, USD/ft in FPS)
- `CostPerMass` - Raw materials, commodities (USD/kg in SI, USD/lb in IPS/FPS)
- `CostPerVolume` - Liquids, gases (USD/m³ in SI, USD/in³ in IPS, USD/ft³ in FPS)
- `CostPerArea` - Sheet materials, real estate (USD/m² in SI, USD/in² in IPS, USD/ft² in FPS)

**Implementation note:** Each type requires DIFFERENT base units and conversion factors in each of the 6 unit system specifications. See "Architecture Patterns" section for details.

**When to implement:** Only after Version 1.0 is stable and if there's proven demand for these features.

---

### Phase 6 (Future): Advanced Exchange Rates

**Only implement if you need dynamic, live currency rates**

**Tasks:**
1. Exchange rate provider interface
2. Cached provider with fallback
3. Dynamic provider with API integration
4. Historical rate support
5. Rate audit trail

**When to do this:**
- You need up-to-date exchange rates
- You're doing international business with real quotes
- You need audit compliance
- You're doing financial reporting

**When NOT to do this:**
- Internal estimates only
- Single currency operations
- Engineering BOMs (estimates ok)
- Rates updated monthly is fine

---

## Decision Points

### When to Move to Next Phase?

**Move from Phase 1 → 2 when:**
- Currency class works and tests pass
- Can display amounts in multiple currencies
- JSON serialization works

**Move from Phase 2 → 3 when:**
- Can calculate item costs correctly
- Volume discounts work
- Tests pass for all CostPerQuantity scenarios

**Move from Phase 3 → 4 when:**
- Can calculate labor costs correctly
- Time unit conversions work
- Tests pass for all CostPerTime scenarios

**Do Phase 5 only if:**
- You actually need those specific cost types
- Have real use cases for them
- Core features (1-4) are solid

**Do Phase 6 only if:**
- You need live exchange rates
- Working internationally with real money
- Compliance/audit requirements

---

## Version 1.0 Deliverables (System-Independent Types Only)

**Phase 1-4 Implementation:**
- ✅ Currency representation (USD, EUR, GBP, JPY, CAD, AUD, CHF, INR)
- ✅ Cost per item pricing ($/unit, $/ea, $/piece, $/dozen)
- ✅ Cost per hour labor rates ($/hr, $/day, $/year)
- ✅ Basic multi-currency support (static rates, manually configured)
- ✅ Volume discount calculations
- ✅ Time unit conversions
- ✅ Works identically across ALL unit systems (SI, IPS, FPS, MKS, CGS, mmNs)

**This covers 80%+ of real-world engineering cost calculations with minimal complexity.**

**NOT included in Version 1.0:**
- ❌ Cost per length, mass, volume, area (system-dependent)
- ❌ Dynamic exchange rates (API integration)
- ❌ Historical rates
- ❌ Cost per energy

---

## Risk Mitigation

**Biggest Risks:**
1. **Exchange rate staleness** - Mitigate: Document that rates are approximate, update quarterly
2. **Precision issues** - Mitigate: Document double vs decimal tradeoffs, plan migration to decimal
3. **Scope creep** - Mitigate: Stick to core features, resist adding secondary cost types
4. **Over-engineering** - Mitigate: Start with static rates, only add dynamic if truly needed

**Keep It Simple Strategy:**
- Static exchange rates in config file
- Manual updates every 3-6 months
- Clear warnings about rate age
- Focus on per-item and per-hour only
- Add other features ONLY when needed

---

## Architecture Patterns: Following Existing Unit Structure

### Pattern 1: Unit Type Class Structure

**Every unit type follows the same pattern:**

```
[Serializable]
[UnitType(UnitFamilyName.XXX, Description = "...")]
public class XXX : MeasuredValue
{
    // 1. Constructor with UnitGroup injection
    public XXX(UnitGroup unitGroup) : base(unitGroup)
    {
        if (unitGroup.Family != UnitFamilyName.XXX)
            throw new ArgumentException(...);
    }

    // 2. Assign methods
    public XXX Assign(double value, string? units) { ... }
    public XXX Assign(XXX source) { ... }

    // 3. Copy method
    public XXX Copy() { ... }

    // 4. Operators: +, -, *, /, ==, !=, <, >
    
    // 5. Equals and GetHashCode overrides
}
```

**Examples from existing code:**
- **Length.cs** - Base pattern for linear measurements
- **Mass.cs** - Simple unit with standard operations
- **Temperature.cs** - Uses `DerivedUnit` for non-linear conversions (Celsius, Fahrenheit)

**For Currency and Cost types:**
```csharp
// Currency follows Length/Mass pattern exactly
public class Currency : MeasuredValue { ... }

// CostPerQuantity follows Length/Mass pattern exactly
public class CostPerQuantity : MeasuredValue { ... }

// CostPerTime follows Length/Mass pattern exactly
public class CostPerTime : MeasuredValue { ... }
```

### Pattern 2: Unit System Specifications

**Critical Insight:** Currency is INDEPENDENT of unit systems (SI, IPS, FPS)

**Physical units vary by system:**
- **SI:** meters, kilograms, seconds
- **IPS:** inches, pounds, seconds  
- **FPS:** feet, pounds, seconds

**Currency is UNIVERSAL across all systems:**
- USD is USD in SI, IPS, FPS, MKS, CGS
- EUR is EUR in SI, IPS, FPS, MKS, CGS
- Exchange rates don't change based on unit system

**Unit Definition Pattern:**

```csharp
// Each unit system specification has:
public override IReadOnlyList<UnitDefinition> UnitDefinitions { get; } = new List<UnitDefinition>
{
    // Base unit for the family
    UnitDefinition.BaseUnit("base", "name", UnitFamilyName.XXX),
    
    // Linear conversions (multiplication)
    UnitDefinition.LinearUnit("unit", "name", UnitFamilyName.XXX, conversionFactor),
    
    // Non-linear conversions (Temperature uses this)
    UnitDefinition.DerivedUnit("unit", "name", UnitFamilyName.XXX, toBase, fromBase)
};
```

### Pattern 3: Adding Currency to Each Unit System

**Key Rule:** Currency definitions are IDENTICAL in all unit system specifications

**Add to EVERY system (SI, IPS, FPS, MKS, CGS, mmNs):**

```csharp
// SIUnitSystemSpecification.cs
public override IReadOnlyList<UnitDefinition> UnitDefinitions { get; } = new List<UnitDefinition>
{
    // ... existing Length, Mass, Time, etc. ...
    
    // Currency units (USD as base) - SAME IN ALL SYSTEMS
    UnitDefinition.BaseUnit("USD", "US Dollars", UnitFamilyName.Currency),
    UnitDefinition.LinearUnit("EUR", "Euros", UnitFamilyName.Currency, 0.92),
    UnitDefinition.LinearUnit("GBP", "British Pounds", UnitFamilyName.Currency, 0.79),
    UnitDefinition.LinearUnit("JPY", "Japanese Yen", UnitFamilyName.Currency, 145.0),
    // ... etc. Same for all systems
};
```

**Why duplicate?** Each unit system is self-contained. This allows:
- Switching between systems without losing currency support
- Independent updates of exchange rates per system (if needed)
- Consistency with existing architecture

### Pattern 4: Cost-Per-Unit Definitions (System-Dependent)

**Cost units COMBINE currency + physical measurement**

**Problem:** Physical units differ by system
- SI uses meters → CostPerLength base is `USD/m`
- IPS uses inches → CostPerLength base is `USD/in`
- FPS uses feet → CostPerLength base is `USD/ft`

**Solution: Base unit matches the system's base physical unit**

**In SIUnitSystemSpecification.cs:**
```csharp
// Cost per Length (USD per meter - matches SI Length base)
UnitDefinition.BaseUnit("USD/m", "US Dollars per meter", UnitFamilyName.CostPerLength),
UnitDefinition.LinearUnit("USD/ft", "US Dollars per foot", UnitFamilyName.CostPerLength, 3.28084),
UnitDefinition.LinearUnit("EUR/m", "Euros per meter", UnitFamilyName.CostPerLength, 0.92),

// Cost per Mass (USD per kilogram - matches SI Mass base)
UnitDefinition.BaseUnit("USD/kg", "US Dollars per kilogram", UnitFamilyName.CostPerMass),
UnitDefinition.LinearUnit("USD/lb", "US Dollars per pound", UnitFamilyName.CostPerMass, 2.20462),

// Cost per Time (USD per second - matches SI Time base)
UnitDefinition.BaseUnit("USD/s", "US Dollars per second", UnitFamilyName.CostPerTime),
UnitDefinition.LinearUnit("USD/hr", "US Dollars per hour", UnitFamilyName.CostPerTime, 1.0/3600.0),
```

**In IPSUnitSystemSpecification.cs:**
```csharp
// Cost per Length (USD per inch - matches IPS Length base)
UnitDefinition.BaseUnit("USD/in", "US Dollars per inch", UnitFamilyName.CostPerLength),
UnitDefinition.LinearUnit("USD/ft", "US Dollars per foot", UnitFamilyName.CostPerLength, 1.0/12.0),
UnitDefinition.LinearUnit("USD/m", "US Dollars per meter", UnitFamilyName.CostPerLength, 39.3701),

// Cost per Mass (USD per pound - matches IPS Mass base)
UnitDefinition.BaseUnit("USD/lb", "US Dollars per pound", UnitFamilyName.CostPerMass),
UnitDefinition.LinearUnit("USD/kg", "US Dollars per kilogram", UnitFamilyName.CostPerMass, 1.0/2.20462),
```

**In FPSUnitSystemSpecification.cs:**
```csharp
// Cost per Length (USD per foot - matches FPS Length base)
UnitDefinition.BaseUnit("USD/ft", "US Dollars per foot", UnitFamilyName.CostPerLength),
UnitDefinition.LinearUnit("USD/in", "US Dollars per inch", UnitFamilyName.CostPerLength, 1.0/12.0),
UnitDefinition.LinearUnit("USD/m", "US Dollars per meter", UnitFamilyName.CostPerLength, 3.28084),
```

### Pattern 5: Cost Per Quantity (Simple - No System Dependency)

**CostPerQuantity is INDEPENDENT of unit system** (like Currency)

A "unit" or "piece" is the same in SI, IPS, FPS:
- 1 screw = 1 screw (regardless of measurement system)
- 1 connector = 1 connector
- Dozen = 12 items everywhere

**Add to ALL systems identically:**
```csharp
// Cost per Quantity (USD per unit) - SAME IN ALL SYSTEMS
UnitDefinition.BaseUnit("USD/unit", "US Dollars per unit", UnitFamilyName.CostPerQuantity),
UnitDefinition.LinearUnit("USD/ea", "US Dollars each", UnitFamilyName.CostPerQuantity, 1.0),
UnitDefinition.LinearUnit("USD/piece", "US Dollars per piece", UnitFamilyName.CostPerQuantity, 1.0),
UnitDefinition.LinearUnit("USD/dozen", "US Dollars per dozen", UnitFamilyName.CostPerQuantity, 1.0/12.0),
UnitDefinition.LinearUnit("EUR/unit", "Euros per unit", UnitFamilyName.CostPerQuantity, 0.92),
```

### Pattern 6: Cost Per Time (System-Independent but Time-Aware)

**Time units are the same across systems** (seconds, hours, days)

**Add to ALL systems identically:**
```csharp
// Cost per Time (USD per second base) - SAME IN ALL SYSTEMS
UnitDefinition.BaseUnit("USD/s", "US Dollars per second", UnitFamilyName.CostPerTime),
UnitDefinition.LinearUnit("USD/hr", "US Dollars per hour", UnitFamilyName.CostPerTime, 1.0/3600.0),
UnitDefinition.LinearUnit("USD/day", "US Dollars per day", UnitFamilyName.CostPerTime, 1.0/86400.0),
UnitDefinition.LinearUnit("EUR/hr", "Euros per hour", UnitFamilyName.CostPerTime, 0.92/3600.0),
```

### Summary Table: Unit System Dependencies

| Unit Family | System-Dependent? | Base Unit Varies? | Implementation |
|-------------|-------------------|-------------------|----------------|
| **Currency** | ❌ No | USD everywhere | Copy identical definitions to all systems |
| **CostPerQuantity** | ❌ No | USD/unit everywhere | Copy identical definitions to all systems |
| **CostPerTime** | ❌ No | USD/s everywhere | Copy identical definitions to all systems |
| **CostPerLength** | ✅ Yes | USD/m (SI), USD/in (IPS), USD/ft (FPS) | Different base, same concept per system |
| **CostPerMass** | ✅ Yes | USD/kg (SI), USD/lb (IPS/FPS) | Different base, same concept per system |
| **CostPerVolume** | ✅ Yes | USD/m³ (SI), USD/in³ (IPS), USD/ft³ (FPS) | Different base, same concept per system |
| **CostPerArea** | ✅ Yes | USD/m² (SI), USD/in² (IPS), USD/ft² (FPS) | Different base, same concept per system |

### Implementation Checklist (Version 1.0 - System-Independent Only)

**Phase 1: Currency**
- ✅ Add `Currency` to UnitFamilyName enum (once)
- ✅ Create `Currency.cs` class (once)
- ✅ Add IDENTICAL currency definitions to all 6 unit system specs:
  - SIUnitSystemSpecification.cs
  - IPSUnitSystemSpecification.cs
  - FPSUnitSystemSpecification.cs
  - MKSUnitSystemSpecification.cs
  - CGSUnitSystemSpecification.cs
  - mmNsUnitSystemSpecification.cs
- ✅ Update MeasuredValue.cs with `[JsonDerivedType(typeof(Currency))]`

**Phase 2: CostPerQuantity**
- ✅ Add `CostPerQuantity` to UnitFamilyName enum
- ✅ Create `CostPerQuantity.cs` class
- ✅ Add IDENTICAL definitions to all 6 unit system specs (same files as above)
- ✅ Update MeasuredValue.cs with `[JsonDerivedType(typeof(CostPerQuantity))]`

**Phase 3: CostPerTime**
- ✅ Add `CostPerTime` to UnitFamilyName enum
- ✅ Create `CostPerTime.cs` class
- ✅ Add IDENTICAL definitions to all 6 unit system specs (same files as above)
- ✅ Update MeasuredValue.cs with `[JsonDerivedType(typeof(CostPerTime))]`

**Key Simplification:** All three types use IDENTICAL definitions across all unit systems. Just copy-paste the same unit definitions to all 6 specs.

---

### Deferred to Version 2.0 (System-Dependent Types)

**CostPerLength, CostPerMass, CostPerVolume, CostPerArea:**
- ⚠️ Requires DIFFERENT base units per system
- ⚠️ SI: USD/m, USD/kg, USD/m³, USD/m²
- ⚠️ IPS: USD/in, USD/lb, USD/in³, USD/in²
- ⚠️ FPS: USD/ft, USD/lb, USD/ft³, USD/ft²
- ⚠️ Much more complex - only implement if truly needed

### Files to Modify (Version 1.0 - Simplified List)

**One-time edits (2 files):**
1. `UnitSystem/UnitFamilyName.cs` - Add 3 enum values: `Currency`, `CostPerQuantity`, `CostPerTime`
2. `UnitSystem/MeasuredValue.cs` - Add 3 JsonDerivedType attributes

**New files to create (3 files):**
3. `UnitSystem/UnitTypes/Currency.cs`
4. `UnitSystem/UnitTypes/CostPerQuantity.cs`
5. `UnitSystem/UnitTypes/CostPerTime.cs`

**Modify all unit system specifications (6 files - IDENTICAL changes in each):**
6. `UnitSystem/Specifications/SIUnitSystemSpecification.cs`
7. `UnitSystem/Specifications/IPSUnitSystemSpecification.cs`
8. `UnitSystem/Specifications/FPSUnitSystemSpecification.cs`
9. `UnitSystem/Specifications/MKSUnitSystemSpecification.cs`
10. `UnitSystem/Specifications/CGSUnitSystemSpecification.cs`
11. `UnitSystem/Specifications/mmNsUnitSystemSpecification.cs`

**Total: 11 files to modify/create**

**Implementation Time:** 3-4 weeks for complete, tested implementation

**Key Advantage:** All currency and cost definitions are IDENTICAL across the 6 specifications. No system-specific variations needed!

## Migration Path

### Phase 1 (Immediate)
1. Add `Currency` enum values to `UnitFamilyName`
2. Implement `Currency` class
3. Add currency definitions to unit system specifications
4. Update `MeasuredValue` JsonDerivedType attributes

### Phase 2 (Short-term) - HIGH PRIORITY
1. **Implement CostPerQuantity class** (per-item pricing) - CRITICAL
2. **Implement CostPerTime class** (hourly rates) - CRITICAL
3. Implement remaining cost-per-unit classes (CostPerLength, CostPerMass, CostPerVolume, CostPerArea)
4. Add cost unit definitions to specifications
5. Implement calculation helper methods
6. Add unit tests for currency and cost operations

### Phase 3 (Medium-term)
1. Implement exchange rate provider interface
2. Create static exchange rate provider
3. Add configuration system for exchange rates
4. Implement rate update mechanisms

### Phase 4 (Long-term)
1. Implement dynamic exchange rate provider
2. Add API integration for live rates
3. Implement rate caching and persistence
4. Add historical rate support

## Testing Strategy

### Unit Tests Required

1. **Currency Conversion Tests**
   - Basic currency conversion
   - Round-trip conversions
   - Precision handling (fractional cents)
   - Multiple currency chains

2. **Cost Calculation Tests**
   - Cost per length calculations
   - Cost per mass calculations
   - Cost per time calculations
   - Unit conversion in cost calculations

3. **Exchange Rate Tests**
   - Static rate provider accuracy
   - Rate update mechanisms
   - Cache expiration handling
   - Error handling for unknown currencies

4. **Serialization Tests**
   - JSON serialization of currency values
   - JSON serialization of cost-per-unit values
   - Deserialization and reconstruction

### Integration Tests

1. Complete bill of materials calculations
2. Multi-currency project cost estimation
3. Unit system switching with currency values
4. Performance tests with large datasets

## Configuration

### Exchange Rate API Recommendations

| Provider | Free Tier | Historical | Frequency | Best For |
|----------|-----------|------------|-----------|----------|
| **exchangerate-api.com** | 1,500 req/mo | Yes | Daily | Small apps |
| **currencyapi.com** | 300 req/mo | Yes | Hourly | Development |
| **ECB (European Central Bank)** | Unlimited | Yes | Daily | EU focus, free |
| **Federal Reserve** | Unlimited | Yes | Daily | USD focus, free |
| **fixer.io** | 100 req/mo | Yes | Hourly | Legacy apps |
| **xe.com** | Paid only | Yes | Real-time | Enterprise |
| **Open Exchange Rates** | 1,000 req/mo | Limited | Hourly | General purpose |

**Recommendation for Production:**
- **Primary:** Open Exchange Rates or currencyapi.com (reliable, good free tier)
- **Fallback:** ECB or Federal Reserve (free, reliable, government sources)
- **Ultimate Fallback:** Static rates (always available offline)

### Exchange Rate Configuration

**File:** `appsettings.json` (or similar)

```json
{
  "CurrencySettings": {
    "DefaultCurrency": "USD",
    "ExchangeRateProvider": "Cached",
    "PrimaryProvider": {
      "Type": "Dynamic",
      "ApiKey": "${EXCHANGE_RATE_API_KEY}",
      "ApiUrl": "https://api.exchangerate-api.com/v4/latest",
      "Source": "ExchangeRate-API"
    },
    "FallbackProvider": {
      "Type": "Static",
      "RatesAsOfDate": "2025-12-01"
    },
    "CacheSettings": {
      "ExpirationMinutes": 30,
      "MaxStalenessHours": 168
    },
    "RateUpdateIntervalHours": 1,
    "StaleRateWarningHours": 24,
    "StaticRates": {
      "USD": 1.0,
      "EUR": 0.92,
      "GBP": 0.79,
      "JPY": 145.0,
      "CNY": 7.2,
      "CAD": 1.36,
      "AUD": 1.55,
      "CHF": 0.88,
      "INR": 83.0
    }
  }
}
```

## Design Considerations

### 1. Precision

Currency requires high precision to avoid rounding errors in financial calculations:
- Use `decimal` type instead of `double` for financial calculations (future enhancement)
- Maintain at least 4 decimal places for fractional cent accuracy
- Implement banker's rounding for currency values
- **WARNING:** Current implementation uses `double` - acceptable for estimates but not financial reporting

### 2. Exchange Rate Volatility

Exchange rates change constantly and this complexity cannot be eliminated:
- **Reality:** Forex markets trade 24/7, rates change every second
- **Mitigation:** Provide timestamp for rate snapshots
- **Support:** Historical rate queries for audit trail
- **Best Practice:** Rate locking for quotations (valid for specific time period)
- **Documentation:** Always document rate source and update frequency
- **User Education:** Make users aware that rates are approximate and time-sensitive

### 3. Cross-Currency Complexity

Multi-currency operations compound complexity:
- **Hub-and-Spoke Model:** All conversions go through USD base (simplifies but may introduce error)
- **Direct Rates:** Some currency pairs have direct markets (EUR/GBP) - consider supporting
- **Triangular Arbitrage:** Direct rates may differ from calculated cross-rates - document approach
- **Rate Consistency:** Within a transaction, use consistent rate snapshot

### 4. Composite Unit Complexity

Cost-per-unit types combine two dimensions (currency + physical measurement):
- Ensure proper unit conversion in both dimensions
- Example: USD/ft to EUR/m requires both currency AND length conversion
- Handle compound conversions correctly: `(USD/ft) * (EUR/USD) * (ft/m) = EUR/m`
- Provide clear calculation methods
- Validate dimensional consistency

### 5. Performance Considerations

Financial calculations may involve large datasets:
- Cache exchange rates appropriately (but track freshness)
- Optimize conversion calculations (pre-calculate common paths)
- Batch update mechanisms (refresh rates once, use for many conversions)
- Consider decimal vs double performance tradeoffs
- **Real-world:** Most apps need <1000 currency conversions/sec - performance unlikely to be bottleneck

### 6. The "Good Enough" Principle

**Perfect currency conversion is impossible** - embrace pragmatic solutions:

| Application Type | Acceptable Approach | Tolerable Staleness |
|------------------|---------------------|---------------------|
| Rough estimates | Static rates | Months |
| Engineering BOM | Weekly updates | 1 week |
| Customer quotes | Daily rates, locked | 24 hours |
| Invoicing | Daily rates, archived | Must be point-in-time |
| Financial reports | Historical rates | Must be exact |
| Accounting | Point-in-time, audited | Must be exact |

**Key Insight:** Match your complexity to your requirements. Most engineering applications can use static monthly rates. Only financial applications need real-time, audited, historical rates.

## Security Considerations

1. **API Key Protection** - Secure storage of exchange rate API keys
2. **Rate Manipulation** - Validate rates from external sources
3. **Precision Attacks** - Guard against rounding manipulation
4. **Audit Trail** - Log currency conversions for financial applications

## Future Enhancements

### Recommended Priorities

1. **Decimal Type Migration** - Replace double with decimal for currency (HIGH PRIORITY for financial accuracy)
2. **Historical Rates** - Time-based rate queries (needed for audit trails)
3. **Rate Alerts** - Notify on significant rate changes
4. **Financial Functions** - NPV, IRR, amortization
5. **Tax Calculations** - VAT, sales tax integration

### Out of Scope

**Cryptocurrency** - Out of scope for this specification
   - Traditional fiat currencies (USD, EUR, GBP, etc.) only
   - Crypto requires separate architecture due to extreme volatility and complexity
   - Use specialized crypto APIs if needed (completely separate system)

## Appendix A: Supported Currencies (Initial)

| Code | Name | Symbol | Region |
|------|------|--------|--------|
| USD | US Dollar | $ | United States |
| EUR | Euro | € | European Union |
| GBP | British Pound | £ | United Kingdom |
| JPY | Japanese Yen | ¥ | Japan |
| CNY | Chinese Yuan | ¥ | China |
| CAD | Canadian Dollar | C$ | Canada |
| AUD | Australian Dollar | A$ | Australia |
| CHF | Swiss Franc | CHF | Switzerland |
| INR | Indian Rupee | ₹ | India |

## Appendix B: Related Documentation

- `UNIT_FAMILY_AMBIGUITY_ARCHITECTURE.md` - Unit family design patterns
- Unit system specification files in `UnitSystem/Specifications/`
- Exchange rate API documentation (to be added)

## Conclusion

This specification provides a comprehensive framework for adding currency and cost-per-unit calculations to the FoundryRulesAndUnits system. The design maintains consistency with existing patterns while addressing the unique requirements of financial measurements.

Implementation should proceed in phases to allow for testing and validation at each stage. The exchange rate management system provides flexibility for both static offline use and dynamic online rate updates.
