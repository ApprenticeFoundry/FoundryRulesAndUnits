using System;
using System.Collections.Generic;
using FoundryRulesAndUnits.Units;

namespace FoundryRulesAndUnits.Units.Specifications
{
    /// <summary>
    /// CGS (Centimeter-Gram-Second) Unit System Specification
    /// Base units: centimeters, grams, seconds, Celsius, degrees, dynes
    /// Uses base unit hub architecture - all conversions go through the base unit
    /// Appropriate for laboratory and scientific applications
    /// </summary>
    public class CGSUnitSystemSpecification : UnitSystemSpecificationBase
    {
        public override string SystemName => "CGS";

        public override string SystemDescription => "CGS (Centimeter-Gram-Second) Unit System with base units: centimeters, grams, seconds, Celsius, degrees, dynes";

        public override UnitSystemType SystemType => UnitSystemType.CGS;

        public override IReadOnlyList<UnitDefinition> UnitDefinitions { get; } = new List<UnitDefinition>
        {
            // Length units (centimeters as base) - Small-scale measurements
            UnitDefinition.BaseUnit("cm", "centimeters", UnitFamilyName.Length),
            UnitDefinition.LinearUnit("mm", "millimeters", UnitFamilyName.Length, 0.1),         // 1 mm = 0.1 cm
            UnitDefinition.LinearUnit("μm", "micrometers", UnitFamilyName.Length, 0.0001),      // 1 μm = 0.0001 cm
            UnitDefinition.LinearUnit("nm", "nanometers", UnitFamilyName.Length, 0.0000001),    // 1 nm = 0.0000001 cm
            UnitDefinition.LinearUnit("m", "meters", UnitFamilyName.Length, 100.0),             // 1 m = 100 cm
            UnitDefinition.LinearUnit("in", "inches", UnitFamilyName.Length, 2.54),             // 1 in = 2.54 cm
            UnitDefinition.LinearUnit("ft", "feet", UnitFamilyName.Length, 30.48),              // 1 ft = 30.48 cm
            UnitDefinition.LinearUnit("RU", "rack-units", UnitFamilyName.Length, 4.445),         // 1 RU = 4.445 cm (1.75 in)
            UnitDefinition.LinearUnit("Å", "angstroms", UnitFamilyName.Length, 0.00000001),     // 1 Å = 1e-8 cm

            // Mass units (grams as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("g", "grams", UnitFamilyName.Mass),
            UnitDefinition.LinearUnit("mg", "milligrams", UnitFamilyName.Mass, 0.001),          // 1 mg = 0.001 g
            UnitDefinition.LinearUnit("μg", "micrograms", UnitFamilyName.Mass, 0.000001),       // 1 μg = 0.000001 g
            UnitDefinition.LinearUnit("ng", "nanograms", UnitFamilyName.Mass, 0.000000001),     // 1 ng = 1e-9 g
            UnitDefinition.LinearUnit("kg", "kilograms", UnitFamilyName.Mass, 1000.0),          // 1 kg = 1000 g
            UnitDefinition.LinearUnit("lb", "pounds", UnitFamilyName.Mass, 453.592),            // 1 lb = 453.592 g
            UnitDefinition.LinearUnit("oz", "ounces", UnitFamilyName.Mass, 28.3495),            // 1 oz = 28.3495 g
            UnitDefinition.LinearUnit("u", "atomic mass units", UnitFamilyName.Mass, 1.66054e-24), // 1 u in grams

            // Force units (dynes as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("dyne", "dynes", UnitFamilyName.Force),
            UnitDefinition.LinearUnit("μdyne", "microdyne", UnitFamilyName.Force, 0.000001),    // 1 μdyne = 1e-6 dyne
            UnitDefinition.LinearUnit("N", "newtons", UnitFamilyName.Force, 100000.0),          // 1 N = 100,000 dyne
            UnitDefinition.LinearUnit("kN", "kilonewtons", UnitFamilyName.Force, 100000000.0),  // 1 kN = 1e8 dyne
            UnitDefinition.LinearUnit("lbf", "pounds-force", UnitFamilyName.Force, 444822.0),   // 1 lbf = 444,822 dyne
            UnitDefinition.LinearUnit("gf", "grams-force", UnitFamilyName.Force, 980.665),      // 1 gf = 980.665 dyne
            UnitDefinition.LinearUnit("kgf", "kilograms-force", UnitFamilyName.Force, 980665.0), // 1 kgf = 980,665 dyne

            // Temperature units (Celsius as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("C", "Celsius", UnitFamilyName.Temperature),
            UnitDefinition.DerivedUnit("K", "Kelvin", UnitFamilyName.Temperature,
                k => k - 273.15,          // K to C: subtract 273.15
                c => c + 273.15),         // C to K: add 273.15
            UnitDefinition.DerivedUnit("F", "Fahrenheit", UnitFamilyName.Temperature,
                f => (f - 32.0) * 5.0/9.0,       // F to C: (F-32)*5/9
                c => c * 9.0/5.0 + 32.0),        // C to F: C*9/5 + 32
            // Unambiguous aliases — "C"/"F" collide with Coulombs/Farads (last-registered wins,
            // no collision detection in UnitSystem.cs's BuildUnitLookupCache). C is this
            // system's base unit, so degC is a pure identity pass-through.
            UnitDefinition.DerivedUnit("degC", "Celsius (unambiguous)", UnitFamilyName.Temperature,
                c => c,
                c => c),
            UnitDefinition.DerivedUnit("degF", "Fahrenheit (unambiguous)", UnitFamilyName.Temperature,
                f => (f - 32.0) * 5.0/9.0,
                c => c * 9.0/5.0 + 32.0),

            // Angle units (radians as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("rad", "radians", UnitFamilyName.Angle),
        UnitDefinition.LinearUnit("deg", "degrees", UnitFamilyName.Angle, Math.PI / 180.0, "°"),  // ASCII: deg, Unicode: °
            UnitDefinition.LinearUnit("mrad", "milliradians", UnitFamilyName.Angle, 0.001),     // 1 mrad = 0.001 rad

            // Time units (seconds as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("s", "seconds", UnitFamilyName.Time),
            UnitDefinition.LinearUnit("ms", "milliseconds", UnitFamilyName.Time, 0.001),        // 1 ms = 0.001 s
            UnitDefinition.LinearUnit("μs", "microseconds", UnitFamilyName.Time, 0.000001),     // 1 μs = 0.000001 s
            UnitDefinition.LinearUnit("ns", "nanoseconds", UnitFamilyName.Time, 0.000000001),   // 1 ns = 1e-9 s
            UnitDefinition.LinearUnit("ps", "picoseconds", UnitFamilyName.Time, 0.000000000001), // 1 ps = 1e-12 s
            UnitDefinition.LinearUnit("min", "minutes", UnitFamilyName.Time, 60.0),             // 1 min = 60 s
            UnitDefinition.LinearUnit("hr", "hours", UnitFamilyName.Time, 3600.0),              // 1 hr = 3600 s

            // Area units (square centimeters as base) - using enhanced approach with UnitFamilyName enum!
        UnitDefinition.BaseUnit("cm2", "square centimeters", UnitFamilyName.Area, "cm²"),
        UnitDefinition.LinearUnit("mm2", "square millimeters", UnitFamilyName.Area, 0.01, "mm²"),  // ASCII: mm2, Unicode: mm²
        UnitDefinition.LinearUnit("μm2", "square micrometers", UnitFamilyName.Area, 0.00000001, "μm²"), // ASCII: μm2, Unicode: μm²
        UnitDefinition.LinearUnit("m2", "square meters", UnitFamilyName.Area, 10000.0, "m²"),     // ASCII: m2, Unicode: m²
        UnitDefinition.LinearUnit("in2", "square inches", UnitFamilyName.Area, 6.4516, "in²"),     // ASCII: in2, Unicode: in²
        UnitDefinition.LinearUnit("ft2", "square feet", UnitFamilyName.Area, 929.0304, "ft²"),     // ASCII: ft2, Unicode: ft²
            // Volume units (cubic centimeters as base) - using enhanced approach with UnitFamilyName enum!
        UnitDefinition.BaseUnit("cm3", "cubic centimeters", UnitFamilyName.Volume, "cm³"),
        UnitDefinition.LinearUnit("mL", "milliliters", UnitFamilyName.Volume, 1.0),         // 1 mL = 1 cm³
        UnitDefinition.LinearUnit("mm3", "cubic millimeters", UnitFamilyName.Volume, 0.001, "mm³"), // ASCII: mm3, Unicode: mm³
        UnitDefinition.LinearUnit("L", "liters", UnitFamilyName.Volume, 1000.0),            // 1 L = 1000 cm³
        UnitDefinition.LinearUnit("m3", "cubic meters", UnitFamilyName.Volume, 1000000.0, "m³"),  // ASCII: m3, Unicode: m³
        UnitDefinition.LinearUnit("in3", "cubic inches", UnitFamilyName.Volume, 16.3871, "in³"),   // ASCII: in3, Unicode: in³
        UnitDefinition.LinearUnit("ft3", "cubic feet", UnitFamilyName.Volume, 28316.8, "ft³"),     // ASCII: ft3, Unicode: ft³
            // Speed units (centimeters per second as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("cm/s", "centimeters per second", UnitFamilyName.Speed),
            UnitDefinition.LinearUnit("mm/s", "millimeters per second", UnitFamilyName.Speed, 0.1), // 1 mm/s = 0.1 cm/s
            UnitDefinition.LinearUnit("m/s", "meters per second", UnitFamilyName.Speed, 100.0), // 1 m/s = 100 cm/s
            UnitDefinition.LinearUnit("km/h", "kilometers per hour", UnitFamilyName.Speed, 27.7778), // 1 km/h = 27.78 cm/s
            UnitDefinition.LinearUnit("ft/s", "feet per second", UnitFamilyName.Speed, 30.48),  // 1 ft/s = 30.48 cm/s
            UnitDefinition.LinearUnit("mph", "miles per hour", UnitFamilyName.Speed, 44.704),   // 1 mph = 44.704 cm/s

            // Acceleration units (centimeters per second squared as base) - ASCII canonical with Unicode display
            UnitDefinition.BaseUnit("cm/s2", "centimeters per second squared", UnitFamilyName.Acceleration, "cm/s²"),
            UnitDefinition.LinearUnit("mm/s2", "millimeters per second squared", UnitFamilyName.Acceleration, 0.1, "mm/s²"), // 1 mm/s² = 0.1 cm/s²
            UnitDefinition.LinearUnit("m/s2", "meters per second squared", UnitFamilyName.Acceleration, 100.0, "m/s²"),      // 1 m/s² = 100 cm/s²
            UnitDefinition.LinearUnit("ft/s2", "feet per second squared", UnitFamilyName.Acceleration, 30.48, "ft/s²"),      // 1 ft/s² = 30.48 cm/s²
            UnitDefinition.LinearUnit("in/s2", "inches per second squared", UnitFamilyName.Acceleration, 2.54, "in/s²"),     // 1 in/s² = 2.54 cm/s²
            UnitDefinition.LinearUnit("gee", "standard gravity", UnitFamilyName.Acceleration, 980.665),                      // 1 gee = 980.665 cm/s² ("g" collides with grams)

            // Distance units (kilometers as base) - Large-scale measurements with dual family support
            UnitDefinition.BaseUnit("km", "kilometers", UnitFamilyName.Distance),
            // Dual family support: Accept all Length units but convert to km base
            UnitDefinition.LinearUnit("cm", "centimeters", UnitFamilyName.Distance, 0.00001),   // 1 cm = 0.00001 km
            UnitDefinition.LinearUnit("mm", "millimeters", UnitFamilyName.Distance, 0.000001),  // 1 mm = 0.000001 km
            UnitDefinition.LinearUnit("μm", "micrometers", UnitFamilyName.Distance, 0.000000001), // 1 μm = 1e-9 km
            UnitDefinition.LinearUnit("nm", "nanometers", UnitFamilyName.Distance, 0.000000000001), // 1 nm = 1e-12 km
            UnitDefinition.LinearUnit("m", "meters", UnitFamilyName.Distance, 0.001),            // 1 m = 0.001 km
            UnitDefinition.LinearUnit("in", "inches", UnitFamilyName.Distance, 0.0000254),       // 1 in = 0.0000254 km
            UnitDefinition.LinearUnit("ft", "feet", UnitFamilyName.Distance, 0.0003048),         // 1 ft = 0.0003048 km
            UnitDefinition.LinearUnit("Å", "angstroms", UnitFamilyName.Distance, 0.0000000000001), // 1 Å = 1e-13 km
            // Traditional distance units
            UnitDefinition.LinearUnit("mi", "miles", UnitFamilyName.Distance, 1.609344),        // 1 mi = 1.609344 km
            UnitDefinition.LinearUnit("nmi", "nautical miles", UnitFamilyName.Distance, 1.852), // 1 nmi = 1.852 km

            // Stiffness units (newtons per meter as base) — spring constants, Force ÷ Length (bug 30/31)
            UnitDefinition.BaseUnit("N/m", "newtons per meter", UnitFamilyName.Stiffness),
            UnitDefinition.LinearUnit("kN/m", "kilonewtons per meter", UnitFamilyName.Stiffness, 1000.0),  // 1 kN/m = 1000 N/m
            UnitDefinition.LinearUnit("N/mm", "newtons per millimeter", UnitFamilyName.Stiffness, 1000.0), // 1 N/mm = 1000 N/m

            // Damping units (newton-seconds per meter as base) — damping coefficients, Force ÷ Speed (bug 30/31)
            UnitDefinition.BaseUnit("Ns/m", "newton-seconds per meter", UnitFamilyName.Damping),
            UnitDefinition.LinearUnit("kNs/m", "kilonewton-seconds per meter", UnitFamilyName.Damping, 1000.0), // 1 kNs/m = 1000 Ns/m

            // Pressure units (barye as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("Ba", "barye", UnitFamilyName.Pressure),
            UnitDefinition.LinearUnit("μBa", "microbarye", UnitFamilyName.Pressure, 0.000001),  // 1 μBa = 1e-6 Ba
            UnitDefinition.LinearUnit("Pa", "pascals", UnitFamilyName.Pressure, 10.0),          // 1 Pa = 10 Ba
            UnitDefinition.LinearUnit("kPa", "kilopascals", UnitFamilyName.Pressure, 10000.0),  // 1 kPa = 10,000 Ba
            UnitDefinition.LinearUnit("bar", "bars", UnitFamilyName.Pressure, 1000000.0),       // 1 bar = 1,000,000 Ba
            UnitDefinition.LinearUnit("atm", "atmospheres", UnitFamilyName.Pressure, 1013250.0), // 1 atm = 1,013,250 Ba
            UnitDefinition.LinearUnit("torr", "torr", UnitFamilyName.Pressure, 1333.22),        // 1 torr = 1333.22 Ba
            UnitDefinition.LinearUnit("psi", "pounds per square inch", UnitFamilyName.Pressure, 68947.6), // 1 psi in Ba

            // Energy units (ergs as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("erg", "ergs", UnitFamilyName.Energy),
            UnitDefinition.LinearUnit("J", "joules", UnitFamilyName.Energy, 10000000.0),        // 1 J = 10,000,000 erg
            UnitDefinition.LinearUnit("kJ", "kilojoules", UnitFamilyName.Energy, 10000000000.0), // 1 kJ = 1e10 erg
            UnitDefinition.LinearUnit("cal", "calories", UnitFamilyName.Energy, 41840000.0),     // 1 cal = 41,840,000 erg
            UnitDefinition.LinearUnit("kcal", "kilocalories", UnitFamilyName.Energy, 41840000000.0), // 1 kcal = 4.184e10 erg
            UnitDefinition.LinearUnit("eV", "electron volts", UnitFamilyName.Energy, 1.602176634e-12), // 1 eV in erg
            UnitDefinition.LinearUnit("keV", "kiloelectron volts", UnitFamilyName.Energy, 1.602176634e-9), // 1 keV in erg

            // Power units (ergs per second as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("erg/s", "ergs per second", UnitFamilyName.Power),
            UnitDefinition.LinearUnit("W", "watts", UnitFamilyName.Power, 10000000.0),          // 1 W = 10,000,000 erg/s
            UnitDefinition.LinearUnit("mW", "milliwatts", UnitFamilyName.Power, 10000.0),       // 1 mW = 10,000 erg/s
            UnitDefinition.LinearUnit("kW", "kilowatts", UnitFamilyName.Power, 10000000000.0),  // 1 kW = 1e10 erg/s
            UnitDefinition.LinearUnit("hp", "horsepower", UnitFamilyName.Power, 7457000000.0),  // 1 hp = 7.457e9 erg/s

            // Frequency units (hertz as base) - same across all systems
            UnitDefinition.BaseUnit("Hz", "hertz", UnitFamilyName.Frequency),
            UnitDefinition.LinearUnit("kHz", "kilohertz", UnitFamilyName.Frequency, 1000.0),    // 1 kHz = 1000 Hz
            UnitDefinition.LinearUnit("MHz", "megahertz", UnitFamilyName.Frequency, 1000000.0), // 1 MHz = 1,000,000 Hz
            UnitDefinition.LinearUnit("GHz", "gigahertz", UnitFamilyName.Frequency, 1000000000.0), // 1 GHz = 1e9 Hz
            UnitDefinition.LinearUnit("THz", "terahertz", UnitFamilyName.Frequency, 1000000000000.0), // 1 THz = 1e12 Hz

            // Voltage units (volts as base) - same across all systems
            UnitDefinition.BaseUnit("V", "volts", UnitFamilyName.Voltage),
            UnitDefinition.LinearUnit("mV", "millivolts", UnitFamilyName.Voltage, 0.001),       // 1 mV = 0.001 V
            UnitDefinition.LinearUnit("μV", "microvolts", UnitFamilyName.Voltage, 0.000001),    // 1 μV = 0.000001 V
            UnitDefinition.LinearUnit("kV", "kilovolts", UnitFamilyName.Voltage, 1000.0),       // 1 kV = 1000 V

            // Current units (amperes as base) - same across all systems
            UnitDefinition.BaseUnit("A", "amperes", UnitFamilyName.Current),
            UnitDefinition.LinearUnit("mA", "milliamperes", UnitFamilyName.Current, 0.001),     // 1 mA = 0.001 A
            UnitDefinition.LinearUnit("μA", "microamperes", UnitFamilyName.Current, 0.000001),  // 1 μA = 0.000001 A
            UnitDefinition.LinearUnit("nA", "nanoamperes", UnitFamilyName.Current, 0.000000001), // 1 nA = 1e-9 A
            UnitDefinition.LinearUnit("pA", "picoamperes", UnitFamilyName.Current, 0.000000000001), // 1 pA = 1e-12 A

            // Currency units (USD as base) - System-independent, identical in all unit systems
            // Exchange rates as of January 2026 (approximate, should be updated periodically)
            UnitDefinition.BaseUnit("USD", "US Dollars", UnitFamilyName.Currency, "$"),
            UnitDefinition.LinearUnit("EUR", "Euros", UnitFamilyName.Currency, 0.92, "€"),           // 1 EUR = 0.92 USD
            UnitDefinition.LinearUnit("GBP", "British Pounds", UnitFamilyName.Currency, 0.79, "£"),  // 1 GBP = 0.79 USD
            UnitDefinition.LinearUnit("JPY", "Japanese Yen", UnitFamilyName.Currency, 145.0, "¥"),   // 1 JPY = 145 USD
            UnitDefinition.LinearUnit("CNY", "Chinese Yuan", UnitFamilyName.Currency, 7.2, "¥"),     // 1 CNY = 7.2 USD
            UnitDefinition.LinearUnit("CAD", "Canadian Dollars", UnitFamilyName.Currency, 1.36, "C$"), // 1 CAD = 1.36 USD
            UnitDefinition.LinearUnit("AUD", "Australian Dollars", UnitFamilyName.Currency, 1.55, "A$"), // 1 AUD = 1.55 USD
            UnitDefinition.LinearUnit("CHF", "Swiss Francs", UnitFamilyName.Currency, 0.88, "₣"),    // 1 CHF = 0.88 USD
            UnitDefinition.LinearUnit("INR", "Indian Rupees", UnitFamilyName.Currency, 83.0, "₹"),   // 1 INR = 83 USD
            UnitDefinition.LinearUnit("MXN", "Mexican Pesos", UnitFamilyName.Currency, 17.0, "$"),   // 1 MXN = 17 USD
            UnitDefinition.LinearUnit("BRL", "Brazilian Reais", UnitFamilyName.Currency, 5.0, "R$"),  // 1 BRL = 5 USD
            UnitDefinition.LinearUnit("KRW", "South Korean Won", UnitFamilyName.Currency, 1320.0, "₩"), // 1 KRW = 1320 USD
            UnitDefinition.LinearUnit("SGD", "Singapore Dollars", UnitFamilyName.Currency, 1.34, "S$"), // 1 SGD = 1.34 USD
            UnitDefinition.LinearUnit("HKD", "Hong Kong Dollars", UnitFamilyName.Currency, 7.8, "HK$"), // 1 HKD = 7.8 USD
            UnitDefinition.LinearUnit("cent", "cents", UnitFamilyName.Currency, 0.01, "¢"),          // 1 cent = 0.01 USD

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
            UnitDefinition.LinearUnit("cent/hr", "cents per hour", UnitFamilyName.CostPerTime, 0.01),           // 1 cent/hr = 0.01 USD/hr

            // Torque units (dyne-centimeters as base)
            UnitDefinition.BaseUnit("dyne*cm", "dyne centimeters", UnitFamilyName.Torque, "dyne⋅cm"),
            UnitDefinition.LinearUnit("N*m", "newton meters", UnitFamilyName.Torque, 10000000.0, "N⋅m"),
            UnitDefinition.LinearUnit("mN*m", "millinewton meters", UnitFamilyName.Torque, 10000.0, "mN⋅m"),
            UnitDefinition.LinearUnit("kN*m", "kilonewton meters", UnitFamilyName.Torque, 10000000000.0, "kN⋅m"),
            UnitDefinition.LinearUnit("lb*ft", "pound feet", UnitFamilyName.Torque, 13558179.0, "lb⋅ft"),
            UnitDefinition.LinearUnit("lb*in", "pound inches", UnitFamilyName.Torque, 1129848.0, "lb⋅in"),

            // Inertia units (g·cm² as base)
            UnitDefinition.BaseUnit("g*cm2", "gram square centimeters", UnitFamilyName.Inertia, "g⋅cm²"),
            UnitDefinition.LinearUnit("kg*m2", "kilogram square meters", UnitFamilyName.Inertia, 10000000.0, "kg⋅m²"),
            UnitDefinition.LinearUnit("kg*cm2", "kilogram square centimeters", UnitFamilyName.Inertia, 1000.0, "kg⋅cm²"),
            UnitDefinition.LinearUnit("slug*ft2", "slug square feet", UnitFamilyName.Inertia, 13558179.0, "slug⋅ft²"),
            UnitDefinition.LinearUnit("lb*ft2", "pound square feet", UnitFamilyName.Inertia, 421401.0, "lb⋅ft²"),

            // Angular velocity units (rad/s as base)
            UnitDefinition.BaseUnit("rad/s", "radians per second", UnitFamilyName.AngularVelocity),
            UnitDefinition.LinearUnit("deg/s", "degrees per second", UnitFamilyName.AngularVelocity, 0.0174533, "°/s"),
            UnitDefinition.LinearUnit("rpm", "revolutions per minute", UnitFamilyName.AngularVelocity, 0.10472),
            UnitDefinition.LinearUnit("rps", "revolutions per second", UnitFamilyName.AngularVelocity, 6.28319),
            UnitDefinition.LinearUnit("rev/min", "revolutions per minute", UnitFamilyName.AngularVelocity, 0.10472),

            // Angular acceleration units (rad/s² as base)
            UnitDefinition.BaseUnit("rad/s2", "radians per second squared", UnitFamilyName.AngularAcceleration, "rad/s²"),
            UnitDefinition.LinearUnit("deg/s2", "degrees per second squared", UnitFamilyName.AngularAcceleration, 0.0174533, "°/s²"),
            UnitDefinition.LinearUnit("rpm/s", "rpm per second", UnitFamilyName.AngularAcceleration, 0.10472),
            UnitDefinition.LinearUnit("rev/s2", "revolutions per second squared", UnitFamilyName.AngularAcceleration, 6.28319, "rev/s²"),

            // Inductance units (Henry as base)
            UnitDefinition.BaseUnit("H", "henries", UnitFamilyName.Inductance),
            UnitDefinition.LinearUnit("mH", "millihenries", UnitFamilyName.Inductance, 0.001),
            UnitDefinition.LinearUnit("uH", "microhenries", UnitFamilyName.Inductance, 0.000001, "μH"),
            UnitDefinition.LinearUnit("nH", "nanohenries", UnitFamilyName.Inductance, 0.000000001)
        };
    }
}