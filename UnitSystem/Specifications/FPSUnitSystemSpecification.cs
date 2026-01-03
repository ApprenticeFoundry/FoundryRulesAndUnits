using System;
using System.Collections.Generic;
using FoundryRulesAndUnits.Units;

namespace FoundryRulesAndUnits.Units.Specifications
{
    /// <summary>
    /// FPS (Foot-Pound-Second) Unit System Specification
    /// Base units: feet, pounds (mass), seconds, Fahrenheit, degrees, pounds-force
    /// Uses base unit hub architecture - all conversions go through the base unit
    /// Appropriate for US structural and civil engineering applications
    /// </summary>
    public class FPSUnitSystemSpecification : UnitSystemSpecificationBase
    {
        public override string SystemName => "FPS";

        public override string SystemDescription => "FPS (Foot-Pound-Second) Unit System with base units: feet, pounds, seconds, Fahrenheit, degrees, pounds-force";

        public override UnitSystemType SystemType => UnitSystemType.FPS;

        public override IReadOnlyList<UnitDefinition> UnitDefinitions { get; } = new List<UnitDefinition>
        {
            // Length units (feet as base) - Small-scale measurements
            UnitDefinition.BaseUnit("ft", "feet", UnitFamilyName.Length),
            UnitDefinition.LinearUnit("in", "inches", UnitFamilyName.Length, 1.0/12.0),          // 1 in = 1/12 ft
            UnitDefinition.LinearUnit("RU", "rack-units", UnitFamilyName.Length, 1.75/12.0),     // 1 RU = 1.75/12 ft (1.75 in)
            UnitDefinition.LinearUnit("yd", "yards", UnitFamilyName.Length, 3.0),                // 1 yd = 3 ft
            UnitDefinition.LinearUnit("mil", "mils", UnitFamilyName.Length, 1.0/12000.0),        // 1 mil = 1/12000 ft
            UnitDefinition.LinearUnit("m", "meters", UnitFamilyName.Length, 3.28083989501),      // 1 m = 3.2808 ft
            UnitDefinition.LinearUnit("cm", "centimeters", UnitFamilyName.Length, 0.0328083989501), // 1 cm = 0.03281 ft
            UnitDefinition.LinearUnit("mm", "millimeters", UnitFamilyName.Length, 0.00328083989501), // 1 mm = 0.003281 ft

            // Mass units (pounds as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("lb", "pounds", UnitFamilyName.Mass),
            UnitDefinition.LinearUnit("oz", "ounces", UnitFamilyName.Mass, 1.0/16.0),           // 1 oz = 1/16 lb
            UnitDefinition.LinearUnit("ton", "tons", UnitFamilyName.Mass, 2000.0),              // 1 ton = 2000 lb
            UnitDefinition.LinearUnit("slug", "slugs", UnitFamilyName.Mass, 32.174),            // 1 slug = 32.174 lb
            UnitDefinition.LinearUnit("kg", "kilograms", UnitFamilyName.Mass, 2.20462262185),   // 1 kg = 2.2046 lb
            UnitDefinition.LinearUnit("g", "grams", UnitFamilyName.Mass, 0.00220462262185),     // 1 g = 0.002205 lb

            // Force units (pounds-force as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("lbf", "pounds-force", UnitFamilyName.Force),
            UnitDefinition.LinearUnit("kip", "kips", UnitFamilyName.Force, 1000.0),             // 1 kip = 1000 lbf
            UnitDefinition.LinearUnit("ozf", "ounces-force", UnitFamilyName.Force, 1.0/16.0),   // 1 ozf = 1/16 lbf
            UnitDefinition.LinearUnit("N", "newtons", UnitFamilyName.Force, 0.224808943),       // 1 N = 0.2248 lbf
            UnitDefinition.LinearUnit("kN", "kilonewtons", UnitFamilyName.Force, 224.808943),   // 1 kN = 224.8 lbf

            // Temperature units (Fahrenheit as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("F", "Fahrenheit", UnitFamilyName.Temperature),
            UnitDefinition.DerivedUnit("C", "Celsius", UnitFamilyName.Temperature,
                c => c * 9.0/5.0 + 32.0,         // C to F: C*9/5 + 32
                f => (f - 32.0) * 5.0/9.0),       // F to C: (F-32)*5/9
            UnitDefinition.DerivedUnit("K", "Kelvin", UnitFamilyName.Temperature,
                k => (k - 273.15) * 9.0/5.0 + 32.0,    // K to F: (K-273.15)*9/5 + 32
                f => (f - 32.0) * 5.0/9.0 + 273.15),   // F to K: (F-32)*5/9 + 273.15

            // Angle units (radians as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("rad", "radians", UnitFamilyName.Angle),
        UnitDefinition.LinearUnit("deg", "degrees", UnitFamilyName.Angle, Math.PI / 180.0, "°"),  // ASCII: deg, Unicode: °
            UnitDefinition.LinearUnit("mrad", "milliradians", UnitFamilyName.Angle, 0.001),     // 1 mrad = 0.001 rad

            // Time units (seconds as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("s", "seconds", UnitFamilyName.Time),
            UnitDefinition.LinearUnit("min", "minutes", UnitFamilyName.Time, 60.0),             // 1 min = 60 s
            UnitDefinition.LinearUnit("hr", "hours", UnitFamilyName.Time, 3600.0),              // 1 hr = 3600 s
            UnitDefinition.LinearUnit("day", "days", UnitFamilyName.Time, 86400.0),             // 1 day = 86400 s

            // Area units (square feet as base) - using enhanced approach with UnitFamilyName enum!
        UnitDefinition.BaseUnit("ft2", "square feet", UnitFamilyName.Area, "ft²"),
        UnitDefinition.LinearUnit("in2", "square inches", UnitFamilyName.Area, 1.0/144.0, "in²"),  // ASCII: in2, Unicode: in²
        UnitDefinition.LinearUnit("yd2", "square yards", UnitFamilyName.Area, 9.0, "yd²"),         // ASCII: yd2, Unicode: yd²
        UnitDefinition.LinearUnit("acre", "acres", UnitFamilyName.Area, 43560.0),           // 1 acre = 43,560 ft²
        UnitDefinition.LinearUnit("m2", "square meters", UnitFamilyName.Area, 10.7639, "m²"),     // ASCII: m2, Unicode: m²
            // Volume units (cubic feet as base) - using enhanced approach with UnitFamilyName enum!
        UnitDefinition.BaseUnit("ft3", "cubic feet", UnitFamilyName.Volume, "ft³"),
        UnitDefinition.LinearUnit("in3", "cubic inches", UnitFamilyName.Volume, 1.0/1728.0, "in³"), // ASCII: in3, Unicode: in³
        UnitDefinition.LinearUnit("yd3", "cubic yards", UnitFamilyName.Volume, 27.0, "yd³"),        // ASCII: yd3, Unicode: yd³
        UnitDefinition.LinearUnit("gal", "gallons", UnitFamilyName.Volume, 0.133681),        // 1 gal = 0.1337 ft³
        UnitDefinition.LinearUnit("m3", "cubic meters", UnitFamilyName.Volume, 35.3147, "m³"),     // ASCII: m3, Unicode: m³
            // Speed units (feet per second as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("ft/s", "feet per second", UnitFamilyName.Speed),
            UnitDefinition.LinearUnit("mph", "miles per hour", UnitFamilyName.Speed, 1.46667),  // 1 mph = 1.467 ft/s
            UnitDefinition.LinearUnit("ft/min", "feet per minute", UnitFamilyName.Speed, 1.0/60.0), // 1 ft/min = 1/60 ft/s
            UnitDefinition.LinearUnit("in/s", "inches per second", UnitFamilyName.Speed, 1.0/12.0), // 1 in/s = 1/12 ft/s
            UnitDefinition.LinearUnit("m/s", "meters per second", UnitFamilyName.Speed, 3.28084), // 1 m/s = 3.281 ft/s

            // Distance units (miles as base) - Large-scale measurements with dual family support
            UnitDefinition.BaseUnit("mi", "miles", UnitFamilyName.Distance),
            // Dual family support: Accept all Length units but convert to mi base
            UnitDefinition.LinearUnit("ft", "feet", UnitFamilyName.Distance, 1.0/5280.0),         // 1 ft = 1/5280 mi
            UnitDefinition.LinearUnit("in", "inches", UnitFamilyName.Distance, 1.0/(5280.0*12.0)), // 1 in = 1/63360 mi
            UnitDefinition.LinearUnit("yd", "yards", UnitFamilyName.Distance, 1.0/1760.0),        // 1 yd = 1/1760 mi
            UnitDefinition.LinearUnit("mil", "mils", UnitFamilyName.Distance, 1.0/(5280.0*12000.0)), // 1 mil in mi
            UnitDefinition.LinearUnit("m", "meters", UnitFamilyName.Distance, 0.000621371),       // 1 m = 0.000621 mi
            UnitDefinition.LinearUnit("cm", "centimeters", UnitFamilyName.Distance, 0.00000621371), // 1 cm = 6.214e-6 mi
            UnitDefinition.LinearUnit("mm", "millimeters", UnitFamilyName.Distance, 0.000000621371), // 1 mm = 6.214e-7 mi
            // Traditional distance units
            UnitDefinition.LinearUnit("km", "kilometers", UnitFamilyName.Distance, 0.621371),     // 1 km = 0.6214 mi
            UnitDefinition.LinearUnit("nmi", "nautical miles", UnitFamilyName.Distance, 1.15078), // 1 nmi = 1.15078 mi

            // Pressure units (pounds per square foot as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("psf", "pounds per square foot", UnitFamilyName.Pressure),
            UnitDefinition.LinearUnit("psi", "pounds per square inch", UnitFamilyName.Pressure, 144.0), // 1 psi = 144 psf
            UnitDefinition.LinearUnit("kip/ft2", "kips per square foot", UnitFamilyName.Pressure, 1000.0), // 1 kip/ft² = 1000 psf
            UnitDefinition.LinearUnit("atm", "atmospheres", UnitFamilyName.Pressure, 2116.22),  // 1 atm = 2116.22 psf
            UnitDefinition.LinearUnit("Pa", "pascals", UnitFamilyName.Pressure, 0.0208854),     // 1 Pa = 0.02089 psf
            UnitDefinition.LinearUnit("kPa", "kilopascals", UnitFamilyName.Pressure, 20.8854),  // 1 kPa = 20.885 psf

            // Energy units (foot-pounds as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("ft·lbf", "foot-pounds", UnitFamilyName.Energy),
            UnitDefinition.LinearUnit("in·lbf", "inch-pounds", UnitFamilyName.Energy, 1.0/12.0), // 1 in·lbf = 1/12 ft·lbf
            UnitDefinition.LinearUnit("BTU", "British thermal units", UnitFamilyName.Energy, 778.169), // 1 BTU = 778.169 ft·lbf
            UnitDefinition.LinearUnit("J", "joules", UnitFamilyName.Energy, 0.737562),           // 1 J = 0.7376 ft·lbf
            UnitDefinition.LinearUnit("kWh", "kilowatt-hours", UnitFamilyName.Energy, 2655224.0), // 1 kWh = 2,655,224 ft·lbf

            // Power units (horsepower as base) - using enhanced approach with UnitFamilyName enum!
            UnitDefinition.BaseUnit("hp", "horsepower", UnitFamilyName.Power),
            UnitDefinition.LinearUnit("ft·lbf/s", "foot-pounds per second", UnitFamilyName.Power, 1.0/550.0), // 1 ft·lbf/s = 1/550 hp
            UnitDefinition.LinearUnit("BTU/hr", "BTU per hour", UnitFamilyName.Power, 0.000393), // 1 BTU/hr = 0.000393 hp
            UnitDefinition.LinearUnit("W", "watts", UnitFamilyName.Power, 0.00134102),           // 1 W = 0.001341 hp
            UnitDefinition.LinearUnit("kW", "kilowatts", UnitFamilyName.Power, 1.34102),         // 1 kW = 1.341 hp

            // Frequency units (hertz as base) - same across all systems
            UnitDefinition.BaseUnit("Hz", "hertz", UnitFamilyName.Frequency),
            UnitDefinition.LinearUnit("kHz", "kilohertz", UnitFamilyName.Frequency, 1000.0),    // 1 kHz = 1000 Hz
            UnitDefinition.LinearUnit("rpm", "revolutions per minute", UnitFamilyName.Frequency, 1.0/60.0), // 1 rpm = 1/60 Hz

            // Voltage units (volts as base) - same across all systems
            UnitDefinition.BaseUnit("V", "volts", UnitFamilyName.Voltage),
            UnitDefinition.LinearUnit("mV", "millivolts", UnitFamilyName.Voltage, 0.001),       // 1 mV = 0.001 V
            UnitDefinition.LinearUnit("kV", "kilovolts", UnitFamilyName.Voltage, 1000.0),       // 1 kV = 1000 V

            // Current units (amperes as base) - same across all systems
            UnitDefinition.BaseUnit("A", "amperes", UnitFamilyName.Current),
            UnitDefinition.LinearUnit("mA", "milliamperes", UnitFamilyName.Current, 0.001),     // 1 mA = 0.001 A
            UnitDefinition.LinearUnit("kA", "kiloamperes", UnitFamilyName.Current, 1000.0),     // 1 kA = 1000 A

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
            UnitDefinition.LinearUnit("cent/hr", "cents per hour", UnitFamilyName.CostPerTime, 0.01)           // 1 cent/hr = 0.01 USD/hr
        };
    }
}