using System;
using System.Collections.Generic;
using FoundryRulesAndUnits.Units;

namespace FoundryRulesAndUnits.Units.Specifications
{
    /// <summary>
    /// mmNs (Millimeter-Newton-Second) Unit System Specification
    /// Base units: millimeters, grams, seconds, Celsius, degrees, newtons
    /// Uses base unit hub architecture - all conversions go through the base unit
    /// Appropriate for precision engineering, CAD, and micro-manufacturing applications
    /// </summary>
    public class mmNsUnitSystemSpecification : UnitSystemSpecificationBase
    {
        public override string SystemName => "mmNs";

        public override string SystemDescription => "mmNs (Millimeter-Newton-Second) Unit System with base units: millimeters, grams, seconds, Celsius, degrees, newtons";

        public override UnitSystemType SystemType => UnitSystemType.mmNs;

        public override IReadOnlyList<UnitDefinition> UnitDefinitions { get; } = new List<UnitDefinition>
        {
            // Length units (millimeters as base) - Small-scale measurements
            UnitDefinition.BaseUnit("mm", "millimeters", UnitFamilyName.Length),
            UnitDefinition.LinearUnit("μm", "micrometers", UnitFamilyName.Length, 0.001),       // 1 μm = 0.001 mm
            UnitDefinition.LinearUnit("nm", "nanometers", UnitFamilyName.Length, 0.000001),     // 1 nm = 0.000001 mm
            UnitDefinition.LinearUnit("cm", "centimeters", UnitFamilyName.Length, 10.0),        // 1 cm = 10 mm
            UnitDefinition.LinearUnit("m", "meters", UnitFamilyName.Length, 1000.0),            // 1 m = 1000 mm
            UnitDefinition.LinearUnit("in", "inches", UnitFamilyName.Length, 25.4),             // 1 in = 25.4 mm
            UnitDefinition.LinearUnit("ft", "feet", UnitFamilyName.Length, 304.8),              // 1 ft = 304.8 mm
            UnitDefinition.LinearUnit("mil", "mils", UnitFamilyName.Length, 0.0254),            // 1 mil = 0.0254 mm

            // Mass units (grams as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("g", "grams", UnitFamilyName.Mass),
            UnitDefinition.LinearUnit("mg", "milligrams", UnitFamilyName.Mass, 0.001),          // 1 mg = 0.001 g
            UnitDefinition.LinearUnit("μg", "micrograms", UnitFamilyName.Mass, 0.000001),       // 1 μg = 0.000001 g
            UnitDefinition.LinearUnit("kg", "kilograms", UnitFamilyName.Mass, 1000.0),          // 1 kg = 1000 g
            UnitDefinition.LinearUnit("lb", "pounds", UnitFamilyName.Mass, 453.592),            // 1 lb = 453.592 g
            UnitDefinition.LinearUnit("oz", "ounces", UnitFamilyName.Mass, 28.3495),            // 1 oz = 28.3495 g

            // Force units (newtons as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("N", "newtons", UnitFamilyName.Force),
            UnitDefinition.LinearUnit("mN", "millinewtons", UnitFamilyName.Force, 0.001),       // 1 mN = 0.001 N
            UnitDefinition.LinearUnit("μN", "micronewtons", UnitFamilyName.Force, 0.000001),    // 1 μN = 0.000001 N
            UnitDefinition.LinearUnit("kN", "kilonewtons", UnitFamilyName.Force, 1000.0),       // 1 kN = 1000 N
            UnitDefinition.LinearUnit("dyne", "dynes", UnitFamilyName.Force, 0.00001),          // 1 dyne = 0.00001 N
            UnitDefinition.LinearUnit("lbf", "pounds-force", UnitFamilyName.Force, 4.44822),    // 1 lbf = 4.44822 N
            UnitDefinition.LinearUnit("gf", "grams-force", UnitFamilyName.Force, 0.00980665),   // 1 gf = 0.009807 N

            // Temperature units (Celsius as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("C", "Celsius", UnitFamilyName.Temperature),
            UnitDefinition.DerivedUnit("K", "Kelvin", UnitFamilyName.Temperature,
                k => k - 273.15,          // K to C: subtract 273.15
                c => c + 273.15),         // C to K: add 273.15
            UnitDefinition.DerivedUnit("F", "Fahrenheit", UnitFamilyName.Temperature,
                f => (f - 32.0) * 5.0/9.0,       // F to C: (F-32)*5/9
                c => c * 9.0/5.0 + 32.0),        // C to F: C*9/5 + 32

            // Angle units (radians as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("rad", "radians", UnitFamilyName.Angle),
            UnitDefinition.LinearUnit("deg", "degrees", UnitFamilyName.Angle, Math.PI / 180.0),  // 1 deg = π/180 rad
            UnitDefinition.LinearUnit("mrad", "milliradians", UnitFamilyName.Angle, 0.001),     // 1 mrad = 0.001 rad

            // Time units (seconds as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("s", "seconds", UnitFamilyName.Time),
            UnitDefinition.LinearUnit("ms", "milliseconds", UnitFamilyName.Time, 0.001),        // 1 ms = 0.001 s
            UnitDefinition.LinearUnit("μs", "microseconds", UnitFamilyName.Time, 0.000001),     // 1 μs = 0.000001 s
            UnitDefinition.LinearUnit("ns", "nanoseconds", UnitFamilyName.Time, 0.000000001),   // 1 ns = 0.000000001 s
            UnitDefinition.LinearUnit("min", "minutes", UnitFamilyName.Time, 60.0),             // 1 min = 60 s
            UnitDefinition.LinearUnit("hr", "hours", UnitFamilyName.Time, 3600.0),              // 1 hr = 3600 s

            // Area units (square millimeters as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("mm2", "square millimeters", UnitFamilyName.Area),
            UnitDefinition.LinearUnit("μm2", "square micrometers", UnitFamilyName.Area, 0.000001), // 1 μm² = 0.000001 mm²
            UnitDefinition.LinearUnit("cm2", "square centimeters", UnitFamilyName.Area, 100.0), // 1 cm² = 100 mm²
            UnitDefinition.LinearUnit("m2", "square meters", UnitFamilyName.Area, 1000000.0),   // 1 m² = 1,000,000 mm²
            UnitDefinition.LinearUnit("in2", "square inches", UnitFamilyName.Area, 645.16),     // 1 in² = 645.16 mm²

            // Volume units (cubic millimeters as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("mm3", "cubic millimeters", UnitFamilyName.Volume),
            UnitDefinition.LinearUnit("μm3", "cubic micrometers", UnitFamilyName.Volume, 0.000000001), // 1 μm³ = 1e-9 mm³
            UnitDefinition.LinearUnit("cm3", "cubic centimeters", UnitFamilyName.Volume, 1000.0), // 1 cm³ = 1000 mm³
            UnitDefinition.LinearUnit("mL", "milliliters", UnitFamilyName.Volume, 1000.0),      // 1 mL = 1000 mm³
            UnitDefinition.LinearUnit("L", "liters", UnitFamilyName.Volume, 1000000.0),         // 1 L = 1,000,000 mm³
            UnitDefinition.LinearUnit("m3", "cubic meters", UnitFamilyName.Volume, 1000000000.0), // 1 m³ = 1e9 mm³
            UnitDefinition.LinearUnit("in3", "cubic inches", UnitFamilyName.Volume, 16387.1),   // 1 in³ = 16387.1 mm³

            // Speed units (millimeters per second as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("mm/s", "millimeters per second", UnitFamilyName.Speed),
            UnitDefinition.LinearUnit("μm/s", "micrometers per second", UnitFamilyName.Speed, 0.001), // 1 μm/s = 0.001 mm/s
            UnitDefinition.LinearUnit("cm/s", "centimeters per second", UnitFamilyName.Speed, 10.0), // 1 cm/s = 10 mm/s
            UnitDefinition.LinearUnit("m/s", "meters per second", UnitFamilyName.Speed, 1000.0),    // 1 m/s = 1000 mm/s
            UnitDefinition.LinearUnit("mm/min", "millimeters per minute", UnitFamilyName.Speed, 1.0/60.0), // 1 mm/min = 1/60 mm/s
            UnitDefinition.LinearUnit("in/s", "inches per second", UnitFamilyName.Speed, 25.4),    // 1 in/s = 25.4 mm/s
            UnitDefinition.LinearUnit("ft/s", "feet per second", UnitFamilyName.Speed, 304.8),     // 1 ft/s = 304.8 mm/s

            // Distance units (kilometers as base) - Large-scale measurements with dual family support
            UnitDefinition.BaseUnit("km", "kilometers", UnitFamilyName.Distance),
            // Dual family support: Accept all Length units but convert to km base
            UnitDefinition.LinearUnit("mm", "millimeters", UnitFamilyName.Distance, 0.000001),  // 1 mm = 0.000001 km
            UnitDefinition.LinearUnit("μm", "micrometers", UnitFamilyName.Distance, 0.000000001), // 1 μm = 1e-9 km
            UnitDefinition.LinearUnit("nm", "nanometers", UnitFamilyName.Distance, 0.000000000001), // 1 nm = 1e-12 km
            UnitDefinition.LinearUnit("cm", "centimeters", UnitFamilyName.Distance, 0.00001),   // 1 cm = 0.00001 km
            UnitDefinition.LinearUnit("m", "meters", UnitFamilyName.Distance, 0.001),           // 1 m = 0.001 km
            UnitDefinition.LinearUnit("in", "inches", UnitFamilyName.Distance, 0.0000254),      // 1 in = 0.0000254 km
            UnitDefinition.LinearUnit("ft", "feet", UnitFamilyName.Distance, 0.0003048),        // 1 ft = 0.0003048 km
            UnitDefinition.LinearUnit("mil", "mils", UnitFamilyName.Distance, 0.0000000254),    // 1 mil = 2.54e-8 km
            // Traditional distance units
            UnitDefinition.LinearUnit("mi", "miles", UnitFamilyName.Distance, 1.609344),        // 1 mi = 1.609344 km
            UnitDefinition.LinearUnit("nmi", "nautical miles", UnitFamilyName.Distance, 1.852), // 1 nmi = 1.852 km

            // Pressure units (kilopascals as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("kPa", "kilopascals", UnitFamilyName.Pressure),
            UnitDefinition.LinearUnit("Pa", "pascals", UnitFamilyName.Pressure, 0.001),         // 1 Pa = 0.001 kPa
            UnitDefinition.LinearUnit("MPa", "megapascals", UnitFamilyName.Pressure, 1000.0),   // 1 MPa = 1000 kPa
            UnitDefinition.LinearUnit("bar", "bars", UnitFamilyName.Pressure, 100.0),           // 1 bar = 100 kPa
            UnitDefinition.LinearUnit("atm", "atmospheres", UnitFamilyName.Pressure, 101.325),  // 1 atm = 101.325 kPa
            UnitDefinition.LinearUnit("psi", "pounds per square inch", UnitFamilyName.Pressure, 6.89476), // 1 psi = 6.895 kPa
            UnitDefinition.LinearUnit("torr", "torr", UnitFamilyName.Pressure, 0.133322),       // 1 torr = 0.1333 kPa

            // Energy units (millijoules as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("mJ", "millijoules", UnitFamilyName.Energy),
            UnitDefinition.LinearUnit("μJ", "microjoules", UnitFamilyName.Energy, 0.001),       // 1 μJ = 0.001 mJ
            UnitDefinition.LinearUnit("nJ", "nanojoules", UnitFamilyName.Energy, 0.000001),     // 1 nJ = 0.000001 mJ
            UnitDefinition.LinearUnit("J", "joules", UnitFamilyName.Energy, 1000.0),            // 1 J = 1000 mJ
            UnitDefinition.LinearUnit("kJ", "kilojoules", UnitFamilyName.Energy, 1000000.0),    // 1 kJ = 1,000,000 mJ
            UnitDefinition.LinearUnit("cal", "calories", UnitFamilyName.Energy, 4184.0),        // 1 cal = 4184 mJ
            UnitDefinition.LinearUnit("eV", "electron volts", UnitFamilyName.Energy, 1.602176634e-16), // 1 eV in mJ

            // Power units (milliwatts as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("mW", "milliwatts", UnitFamilyName.Power),
            UnitDefinition.LinearUnit("μW", "microwatts", UnitFamilyName.Power, 0.001),         // 1 μW = 0.001 mW
            UnitDefinition.LinearUnit("nW", "nanowatts", UnitFamilyName.Power, 0.000001),       // 1 nW = 0.000001 mW
            UnitDefinition.LinearUnit("W", "watts", UnitFamilyName.Power, 1000.0),              // 1 W = 1000 mW
            UnitDefinition.LinearUnit("kW", "kilowatts", UnitFamilyName.Power, 1000000.0),      // 1 kW = 1,000,000 mW
            UnitDefinition.LinearUnit("hp", "horsepower", UnitFamilyName.Power, 745700.0),      // 1 hp = 745,700 mW

            // Frequency units (hertz as base) - same across all systems
            UnitDefinition.BaseUnit("Hz", "hertz", UnitFamilyName.Frequency),
            UnitDefinition.LinearUnit("kHz", "kilohertz", UnitFamilyName.Frequency, 1000.0),    // 1 kHz = 1000 Hz
            UnitDefinition.LinearUnit("MHz", "megahertz", UnitFamilyName.Frequency, 1000000.0), // 1 MHz = 1,000,000 Hz
            UnitDefinition.LinearUnit("GHz", "gigahertz", UnitFamilyName.Frequency, 1000000000.0), // 1 GHz = 1,000,000,000 Hz
            UnitDefinition.LinearUnit("rpm", "revolutions per minute", UnitFamilyName.Frequency, 1.0/60.0), // 1 rpm = 1/60 Hz

            // Voltage units (volts as base) - same across all systems
            UnitDefinition.BaseUnit("V", "volts", UnitFamilyName.Voltage),
            UnitDefinition.LinearUnit("mV", "millivolts", UnitFamilyName.Voltage, 0.001),       // 1 mV = 0.001 V
            UnitDefinition.LinearUnit("μV", "microvolts", UnitFamilyName.Voltage, 0.000001),    // 1 μV = 0.000001 V
            UnitDefinition.LinearUnit("kV", "kilovolts", UnitFamilyName.Voltage, 1000.0),       // 1 kV = 1000 V

            // Current units (amperes as base) - same across all systems
            UnitDefinition.BaseUnit("A", "amperes", UnitFamilyName.Current),
            UnitDefinition.LinearUnit("mA", "milliamperes", UnitFamilyName.Current, 0.001),     // 1 mA = 0.001 A
            UnitDefinition.LinearUnit("μA", "microamperes", UnitFamilyName.Current, 0.000001),  // 1 μA = 0.000001 A
            UnitDefinition.LinearUnit("nA", "nanoamperes", UnitFamilyName.Current, 0.000000001), // 1 nA = 0.000000001 A
            UnitDefinition.LinearUnit("kA", "kiloamperes", UnitFamilyName.Current, 1000.0),     // 1 kA = 1000 A

            // Currency units (USD as base) - System-independent, identical in all unit systems
            // Exchange rates as of December 2025 (approximate, should be updated periodically)
            UnitDefinition.BaseUnit("USD", "US Dollars", UnitFamilyName.Currency),
            UnitDefinition.LinearUnit("EUR", "Euros", UnitFamilyName.Currency, 0.92),           // 1 EUR = 0.92 USD
            UnitDefinition.LinearUnit("GBP", "British Pounds", UnitFamilyName.Currency, 0.79),  // 1 GBP = 0.79 USD
            UnitDefinition.LinearUnit("JPY", "Japanese Yen", UnitFamilyName.Currency, 145.0),   // 1 JPY = 145 USD
            UnitDefinition.LinearUnit("CNY", "Chinese Yuan", UnitFamilyName.Currency, 7.2),     // 1 CNY = 7.2 USD
            UnitDefinition.LinearUnit("CAD", "Canadian Dollars", UnitFamilyName.Currency, 1.36), // 1 CAD = 1.36 USD
            UnitDefinition.LinearUnit("AUD", "Australian Dollars", UnitFamilyName.Currency, 1.55), // 1 AUD = 1.55 USD
            UnitDefinition.LinearUnit("CHF", "Swiss Francs", UnitFamilyName.Currency, 0.88),    // 1 CHF = 0.88 USD
            UnitDefinition.LinearUnit("INR", "Indian Rupees", UnitFamilyName.Currency, 83.0),   // 1 INR = 83 USD
            UnitDefinition.LinearUnit("MXN", "Mexican Pesos", UnitFamilyName.Currency, 17.0),   // 1 MXN = 17 USD
            UnitDefinition.LinearUnit("BRL", "Brazilian Reais", UnitFamilyName.Currency, 5.0),  // 1 BRL = 5 USD
            UnitDefinition.LinearUnit("KRW", "South Korean Won", UnitFamilyName.Currency, 1320.0), // 1 KRW = 1320 USD
            UnitDefinition.LinearUnit("SGD", "Singapore Dollars", UnitFamilyName.Currency, 1.34), // 1 SGD = 1.34 USD
            UnitDefinition.LinearUnit("HKD", "Hong Kong Dollars", UnitFamilyName.Currency, 7.8), // 1 HKD = 7.8 USD
            UnitDefinition.LinearUnit("cent", "cents", UnitFamilyName.Currency, 0.01),          // 1 cent = 0.01 USD

            // Cost per quantity units (USD/unit as base) - System-independent pricing
            UnitDefinition.BaseUnit("USD/unit", "US Dollars per unit", UnitFamilyName.CostPerQuantity),
            UnitDefinition.LinearUnit("USD/ea", "US Dollars per each", UnitFamilyName.CostPerQuantity, 1.0),      // Alias for USD/unit
            UnitDefinition.LinearUnit("USD/piece", "US Dollars per piece", UnitFamilyName.CostPerQuantity, 1.0),  // Alias for USD/unit
            UnitDefinition.LinearUnit("USD/item", "US Dollars per item", UnitFamilyName.CostPerQuantity, 1.0),    // Alias for USD/unit
            UnitDefinition.LinearUnit("USD/dozen", "US Dollars per dozen", UnitFamilyName.CostPerQuantity, 1.0/12.0), // 1 USD/dozen = USD/unit ÷ 12
            UnitDefinition.LinearUnit("USD/hundred", "US Dollars per hundred", UnitFamilyName.CostPerQuantity, 0.01), // 1 USD/hundred = USD/unit ÷ 100
            UnitDefinition.LinearUnit("USD/thousand", "US Dollars per thousand", UnitFamilyName.CostPerQuantity, 0.001), // 1 USD/thousand = USD/unit ÷ 1000
            UnitDefinition.LinearUnit("EUR/unit", "Euros per unit", UnitFamilyName.CostPerQuantity, 0.92),        // 1 EUR/unit = 0.92 USD/unit
            UnitDefinition.LinearUnit("GBP/unit", "British Pounds per unit", UnitFamilyName.CostPerQuantity, 0.79), // 1 GBP/unit = 0.79 USD/unit
            UnitDefinition.LinearUnit("JPY/unit", "Japanese Yen per unit", UnitFamilyName.CostPerQuantity, 145.0), // 1 JPY/unit = 145 USD/unit
            UnitDefinition.LinearUnit("CAD/unit", "Canadian Dollars per unit", UnitFamilyName.CostPerQuantity, 1.36), // 1 CAD/unit = 1.36 USD/unit
            UnitDefinition.LinearUnit("cent/unit", "cents per unit", UnitFamilyName.CostPerQuantity, 0.01),        // 1 cent/unit = 0.01 USD/unit

            // Cost per time units (USD/hr as base) - System-independent labor/service rates
            UnitDefinition.BaseUnit("USD/hr", "US Dollars per hour", UnitFamilyName.CostPerTime),
            UnitDefinition.LinearUnit("USD/h", "US Dollars per hour", UnitFamilyName.CostPerTime, 1.0),          // Alias for USD/hr
            UnitDefinition.LinearUnit("USD/s", "US Dollars per second", UnitFamilyName.CostPerTime, 3600.0),     // 1 USD/s = 3600 USD/hr
            UnitDefinition.LinearUnit("USD/min", "US Dollars per minute", UnitFamilyName.CostPerTime, 60.0),     // 1 USD/min = 60 USD/hr
            UnitDefinition.LinearUnit("USD/day", "US Dollars per day", UnitFamilyName.CostPerTime, 1.0/24.0),    // 1 USD/day = USD/hr ÷ 24
            UnitDefinition.LinearUnit("USD/wk", "US Dollars per week", UnitFamilyName.CostPerTime, 1.0/168.0),   // 1 USD/wk = USD/hr ÷ 168
            UnitDefinition.LinearUnit("USD/mo", "US Dollars per month", UnitFamilyName.CostPerTime, 1.0/730.0),  // 1 USD/mo ≈ USD/hr ÷ 730 (30.4 days avg)
            UnitDefinition.LinearUnit("USD/yr", "US Dollars per year", UnitFamilyName.CostPerTime, 1.0/8760.0),  // 1 USD/yr = USD/hr ÷ 8760
            UnitDefinition.LinearUnit("EUR/hr", "Euros per hour", UnitFamilyName.CostPerTime, 0.92),            // 1 EUR/hr = 0.92 USD/hr
            UnitDefinition.LinearUnit("GBP/hr", "British Pounds per hour", UnitFamilyName.CostPerTime, 0.79),   // 1 GBP/hr = 0.79 USD/hr
            UnitDefinition.LinearUnit("JPY/hr", "Japanese Yen per hour", UnitFamilyName.CostPerTime, 145.0),    // 1 JPY/hr = 145 USD/hr
            UnitDefinition.LinearUnit("CAD/hr", "Canadian Dollars per hour", UnitFamilyName.CostPerTime, 1.36), // 1 CAD/hr = 1.36 USD/hr
            UnitDefinition.LinearUnit("cent/hr", "cents per hour", UnitFamilyName.CostPerTime, 0.01)           // 1 cent/hr = 0.01 USD/hr
        };
    }
}